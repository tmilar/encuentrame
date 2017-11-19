import Service from './Service';
import {Notifications} from 'expo';
import GeolocationService from "./GeolocationService";
import SessionService from "./SessionService";
import {hide, showToast} from 'react-native-notifyer';
import {AsyncStorage} from 'react-native';
import formatDateForBackend from "../util/formatDateForBackend";

class PositionTrackingService {

  static POSITION_SET_INTERVAL = 1 * 60 * 1000; // 1 MINUTE
  static POSITION_SET_INTERVAL_DELTA = 60 * 1000; // +/- 1 MINUTE (60 seconds)

  static INITIAL_TRACKING_ENABLED = true;

  /**
   * Reference to local notifications listener. Useful for activation/deactivation.
   */
  _localNotificationsListener;

  /**
   * Setup periodic GPS Position report.
   * This method should be called once at App initialization phase.
   *
   * [Warning] Will clean ALL scheduled background local notifications.
   *
   * Will schedule minutely, 'dummy' local notifications,
   * then handle them in background
   * to publish device GPS position to server.
   *
   */
  startPositionTracking = async () => {
    this._cleanScheduledBackgroundNotifications();

    await GeolocationService.requireLocationPermission();

    if (await SessionService.isDevSession()) {
      showToast("Location Permission is enabled successfully.");
    }

    this._registerPositionNotificationListener();

    this._scheduleRecurrentBackgroundNotification();

    await this._updateEnabled(true);
  };

  /**
   * Refresh position tracking.
   * If first time refresh, will set it to config INITIAL_TRACKING_ENABLED value.
   *
   * @returns {Promise.<boolean>}
   *    true if was enabled,
   *    false if was disabled.
   */
  refreshPositionTracking = async () => {
    let enabled = await this.checkEnabled();
    let firstTimeCheck = enabled === undefined;

    if (firstTimeCheck) {
      enabled = PositionTrackingService.INITIAL_TRACKING_ENABLED;
      console.debug("[PositionTrackingService] " +
        `First time refresh - ${enabled ? "enabling" : "disabling"} tracking (based on INITIAL_TRACKING_ENABLED value)`);
    }

    if (enabled) {
      await this.startPositionTracking();
    }

    return await this.checkEnabled();
  };

  /**
   * Check Tracking enabled, for user feedback purposes.
   * If user has never enabled/disabled, it will return undefined.
   *
   * @returns {Promise.<boolean|undefined>}
   */
  checkEnabled = async () => {
    let enabledStr = await AsyncStorage.getItem("ENCUENTRAME_TRACKING_ENABLED");
    return enabledStr ? enabledStr === "true" : undefined;
  };

  /**
   * Stop tracking device position.
   *
   * @returns {Promise.<void>}
   */
  stopPositionTracking = async () => {
    this._localNotificationsListener.remove();
    this._localNotificationsListener = null;

    this._cleanScheduledBackgroundNotifications();
    await this._updateEnabled(false);
  };

  _updateEnabled = async (enabled) => {
    await AsyncStorage.setItem("ENCUENTRAME_TRACKING_ENABLED", enabled.toString());
  };


  /**
   * Register Position Notification Listener.
   *
   * This will listen and handle the 'position' local notification,
   * dismiss it ASAP,
   * and proceed to process it for position tracking.
   *
   * @private
   */
  _registerPositionNotificationListener = () => {

    let positionNotificationListener = async (notification) => {
      // Try to remove annoying popup notification.
      let notificationId = notification.notificationId;
      Notifications.dismissNotificationAsync(notificationId);

      // ignore invalid notification
      if (!this._checkLocalNotification(notification)) {
        return;
      }

      try {
        await this._handleLocalPositionNotification(notification);
      } catch (e) {
        console.error("Problem handling local position notification. ", e);
      }
    };

    this._localNotificationsListener = Notifications.addListener(positionNotificationListener);
  };

  _checkLocalNotification = (notification) => {
    if (!notification.data) {
      console.debug("Trying to parse notification body (data) which was null. ", notification);
      return false;
    }

    let data = notification.data;

    if (!data || !data.type || data.type !== "position") {
      console.debug("Notification is not 'position' type. Ignoring.", notification);
      return false;
    }
    return true;
  };

  /**
   * Schedule recurrent local notification, so the Expo app gets the chance
   * to do stuff even when in background.
   *
   * This is a workaround, not a production-ready solution.
   *
   * @private
   */
  _scheduleRecurrentBackgroundNotification = () => {

    let beginDate = new Date();
    beginDate.setSeconds(beginDate.getSeconds() + 3);

    const localNotificationBody = {
      title: 'Encuentrame',
      body: 'Enviando datos...',
      data: {created: beginDate, type: "position"},
      ios: {sound: false},
      android: {sound: false, sticky: false, vibrate: false, priority: "min"}
    };

    const schedulingOptions = {
      time: beginDate,
      repeat: 'minute',
      // intervalMs: 15000 // 15 seconds. //TODO requires Expo upgrade to v22 or 23...
    };

    console.log("[PositionTrackingService] Tracking started via local periodic background notifications. ", beginDate);
    Notifications.scheduleLocalNotificationAsync(localNotificationBody, schedulingOptions);
  };

  _cleanScheduledBackgroundNotifications = () => {
    Notifications.cancelAllScheduledNotificationsAsync();
  };

  _postCurrentDevicePosition = async () => {
    let currentPosition = await this._getCurrentDevicePosition();

    console.log("[PositionTrackingService] Posting position: ", currentPosition);

    if (await SessionService.isDevSession()) {
      showToast("Posting position: " + JSON.stringify(currentPosition));
    }

    try {
      await this._postPosition(currentPosition.body);
    } catch (e) {
      console.log(`[PositionTrackingService] Error when posting #${currentPosition.index} position to server. Will retry next time. `, currentPosition.body);
      this.pendingPositions = [...(this.pendingPositions || []), currentPosition];
      throw e;
    }
  };

  _postPendingDevicePositions = async () => {
    if (!(this.pendingPositions && this.pendingPositions.length)) {
      return;
    }

    let logMsg = `[PositionTrackingService] Posting ${this.pendingPositions.length} pending positions. `;
    console.log(logMsg, this.pendingPositions);

    if (await SessionService.isDevSession()) {
      showToast(logMsg);
    }

    let errors = [];
    let newPendingPositions = [];

    this.pendingPositions.forEach(async pos => {
      try {
        await this._postPosition(pos.body);
      } catch (e) {
        errors.push({error: e, position: pos});
        newPendingPositions.push(pos);
      }
    });

    this.pendingPositions = newPendingPositions;

    if (errors.length) {
      let errMsg = `Hubo un problema al informar ${errors.length} de las #${this.pendingPositions.length} posiciones pendientes...`;
      console.log(`[PositionTrackingService] ${errMsg}. Errors info: `, errors);
      throw errMsg;
    }
  };

  _getCurrentDevicePosition = async () => {
    this.gpsPositionIndex = (this.gpsPositionIndex || 0) + 1;

    if (await SessionService.isDevSession()) {
      showToast(`Requesting device position #${this.gpsPositionIndex}...`);
    }

    let deviceLocation = await GeolocationService.getDeviceLocation({enableHighAccuracy: true});

    let locationDate = formatDateForBackend(new Date());

    let currentPositionBody = {
      "Latitude": deviceLocation.latitude,
      "Longitude": deviceLocation.longitude,
      "Creation": locationDate,
    };

    let currentPosition = {
      body: currentPositionBody,
      index: this.gpsPositionIndex
    };

    return currentPosition;
  };

  _postPosition = async (devicePositionBody) => {
    return await Service.sendRequest("Position/set", {
      method: "POST",
      body: JSON.stringify(devicePositionBody)
    });
  };

  _handleLocalPositionNotification = async (notification) => {

    let now = new Date();

    console.log(now,
      'Local "position" notification received. Posting position to server.',
      notification);

    try {
      await this._postPendingDevicePositions();
    } catch (e) {
      console.log("Problem when sending pending positions to server. ", e);
      showToast("Problema en la comunicación con el servidor: " + (e.message || e));
    }

    try {
      await this._postCurrentDevicePosition();
    } catch (e) {
      console.log("Problem when sending position to server. ", e);
      showToast("Problema en la comunicación con el servidor: " + (e.message || e));
    }
  };

  /**
   * Switch tracking enabled on/off.
   *
   * @returns {Promise.<boolean>} enabled?
   */
  togglePositionTracking = async () => {
    let enabled = await this.checkEnabled();
    if (enabled) {
      await this.stopPositionTracking();
    } else {
      await this.startPositionTracking();
    }
    enabled = await this.checkEnabled();
    return enabled;
  }
}

const positionTrackingServiceInstance = new PositionTrackingService();
export default positionTrackingServiceInstance;
