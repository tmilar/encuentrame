import Service from './Service';
import {Notifications} from 'expo';
import GeolocationService from "./GeolocationService";

class PositionTrackingService {

  static POSITION_SET_INTERVAL = 5 * 60 * 1000; // 5 MINUTES
  static POSITION_SET_INTERVAL_DELTA = 60 * 1000; // +/- 1 MINUTE (60 seconds)

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
  setupPeriodicPositionReport = async () => {
    this.cleanScheduledBackgroundNotifications();

    await GeolocationService.requireLocationPermission();

    this.registerPositionNotificationListener();

    this.scheduleRecurrentBackgroundNotification();
  };

  /**
   * Register Position Notification Listener.
   *
   * This will listen and handle the 'position' local notification,
   * dismiss it ASAP,
   * and proceed to process it for position tracking.
   */
  registerPositionNotificationListener = () => {

    let positionNotificationListener = async (notification) => {
      // Try to remove annoying popup notification.
      let notificationId = notification.notificationId;
      Notifications.dismissNotificationAsync(notificationId);

      // ignore invalid notification
      if (!this._checkLocalNotification(notification)) {
        return;
      }

      try {
        await this.handleLocalPositionNotification(notification);
      } catch (e) {
        console.error("Problem handling local position notification. ", e);
      }
    };

    Notifications.addListener(positionNotificationListener);
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
   */
  scheduleRecurrentBackgroundNotification = () => {

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
      // intervalMs: 5*60*1000 //TODO requires Expo upgrade v21
    };

    console.log("[PositionTrackingService] Tracking started via local periodic background notifications. ", beginDate);
    Notifications.scheduleLocalNotificationAsync(localNotificationBody, schedulingOptions);
  };

  cleanScheduledBackgroundNotifications = () => {
    Notifications.cancelAllScheduledNotificationsAsync();
  };

  postCurrentPosition = () => {
    let deviceLocation = GeolocationService.getDeviceLocation({enableHighAccuracy: true});

    let currentPositionBody = {
      "Latitude": deviceLocation.latitude,
      "Longitude": deviceLocation.longitude
    };
    console.log("[PositionTrackingService] Posting position: ", currentPositionBody);

    return Service.sendRequest("/position/set", {
      method: "POST",
      body: JSON.stringify(currentPositionBody)
    });
  };

  handleLocalPositionNotification = async (notification) => {

    let now = new Date();
    let started = new Date(notification.data.created);

    let elapsedTime = now.getTime() - started.getTime();

    let totalIntervalTime = elapsedTime / PositionTrackingService.POSITION_SET_INTERVAL;

    let relativeIntervalTime = totalIntervalTime - Math.floor(totalIntervalTime);
    let relativeErrorDelta = PositionTrackingService.POSITION_SET_INTERVAL_DELTA / PositionTrackingService.POSITION_SET_INTERVAL;

    let shouldPostPosition = relativeIntervalTime < relativeErrorDelta;

    let remainingPositionTimeMs = Math.round((1 - relativeIntervalTime) * PositionTrackingService.POSITION_SET_INTERVAL);

    let debugData = {
      cycle: Math.floor(totalIntervalTime),
      totalIntervalTime: totalIntervalTime,
      relativeIntervalDelta: relativeIntervalTime,
      remainingForNextMs: remainingPositionTimeMs
    };

    console.log(now,
      'Notification received. ',
      shouldPostPosition ?
        `Posting device position to server` :
        `Not posting position to server yet. (will do in about: ${remainingPositionTimeMs} ms.)`,
      debugData,
      notification);

    if (!shouldPostPosition) {
      return;
    }

    try {
      await this.postCurrentPosition();
    } catch (e) {
      console.log("Problem when sending position to server. ", e);
      // TODO display on screen, a simple Toast error message
    }
  }
}

const positionTrackingServiceInstance = new PositionTrackingService();
export default positionTrackingServiceInstance;
