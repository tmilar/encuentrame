import Service from './Service';
import {Notifications} from 'expo';

class PositionTrackingService {

  /**
   * Setup periodic GPS Position report.
   * This method should be called once at App initialization phase.
   *
   * Will schedule minutely, 'dummy' local notifications,
   * then handle them in background
   * to publish device GPS position to server.
   *
   */
  setupPeriodicPositionReport = () => {
    this.cleanScheduledBackgroundNotifications();

    Notifications.addListener(this.positionNotificationListener);

    this.scheduleRecurrentBackgroundNotification();
  };

  positionNotificationListener = (notification) => {
    // Try to remove annoying popup notification.
    let notificationId = notification.notificationId;
    Notifications.dismissNotificationAsync(notificationId);

    if (!this._checkLocalNotification(notification)) {
      return; // ignore invalid notification
    }

    this.handleLocalPositionNotification(notification);
    // Notifications.dismissAllNotificationsAsync();
  };

  _checkLocalNotification = (notification) => {
    if (!notification.body) {
      console.debug("Trying to parse notification body which was null. ", notification);
      return false;
    }

    let data = JSON.parse(notification.body);

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
    const localNotificationBody = {
      title: 'Encuentrame',
      body: 'Enviando datos...',
      data: JSON.stringify({created: new Date().getTime(), type: "position"}),
      ios: {sound: false},
      android: {sound: false, sticky: false, vibrate: false, priority: "min"}
    };

    let t = new Date();
    t.setSeconds(t.getSeconds() + 3);

    const schedulingOptions = {
      time: t,
      repeat: 'minute',
      // intervalMs: 5*60*1000 //TODO requires Expo upgrade v21
    };

    console.log("Scheduling local notification with options: ", schedulingOptions);
    Notifications.scheduleLocalNotificationAsync(this.localNotificationBody, schedulingOptions);
  };

  cleanScheduledBackgroundNotifications = () => {
    Notifications.cancelAllScheduledNotificationsAsync();
  };

  postCurrentPosition = () => {
    // TODO Get actual device position from GPS service.
    let currentPositionBody = {
      "Latitude": "1.0000",
      "Longitude": "1.0000"
    };

    return Service.sendRequest("/position/set", {
      method: "POST",
      body: currentPositionBody
    });
  };

  handleLocalPositionNotification = async (notification) => {

    let now = new Date();
    console.log(new Date(), 'Notification received... ', notification);

    // TODO only post current position once each 5 minutes. Not once per minute.
    try {
      await this.postCurrentPosition();
    } catch (e) {
      console.log("Problem when sending position to server. ", e);
      // TODO display a simple Toast with error message
    }
  }
}
