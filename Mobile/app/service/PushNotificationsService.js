import {Permissions, Notifications} from 'expo';
import Service from './Service';
import {AsyncStorage} from 'react-native';
import PermissionsHelper from '../util/PermissionsHelper';
import {Alert} from "react-native";

// import {Alert} from "react-native";
import {showToast} from 'react-native-notifyer';
import SessionService from './SessionService';
// import NewsService from './NewsService';
import NewsDispatcher from "../model/NewsDispatcher";

class PushNotificationsService {

  /**
   * Register Exponent push token to server for user.
   * Store token locally to prevent duplicated requests.
   *
   * @param user
   * @returns {Promise.<void>}
   */
  registerDevice = async (user) => {

    await this._requireNotificationsPermission();

    // Get the token that uniquely identifies this device
    let token = await Notifications.getExpoPushTokenAsync();

    if (await this._checkRegistered(user)) {
      console.log(`[PushNotificationsService] Device already registered. Token: ${token}`);
      return;
    }

    // register token to server.
    await this._registerDeviceRequest(token, user);

    // Remember stored token in local storage.
    await this._saveRegistered(token, user);
  };

  _registerDeviceRequest = async (token, {username}) => {
    const url = 'Account/devices';
    console.log(`[PushNotificationsService] Registering device... ${JSON.stringify({token, username})}`);

    try {
      await Service.sendRequest(url, {
        method: 'POST',
        body: JSON.stringify({
          Token: token
        })
      });
      console.log(`[PushNotificationsService] Register device token for push OK! ${JSON.stringify({token, username})}`);
      if (await SessionService.isDevSession()) {
        showToast(`POST ${url}. \nRegistro con el servidor OK! \nToken: ${token}`);
      }
    } catch (e) {
      throw 'Problema al registrar el dispositivo. ' + (e.message || e);
    }
  };

  _saveRegistered = async (token, {username}) => {
    await AsyncStorage.setItem(`ENCUENTRAME_NOTIFICATIONS_TOKEN_${username}`, token);
    console.log(`[PushNotificationsService] Device registered for push notifications. Token: ${token}`);
  };

  /**
   * Check if device is already registered.
   *
   * @returns {Promise.<boolean>}
   * @private
   */
  _checkRegistered = async ({username}) => {
    return false; // TEMPORARILY ignore 'already registered' state. TODO delete this line before merge.
    let token = await AsyncStorage.getItem(`ENCUENTRAME_NOTIFICATIONS_TOKEN_${username}`);
    return !!token;
  };

  _requireNotificationsPermission = async () => {

    // Android remote notification permissions are granted during the app
    // install, so this will only ask on iOS
    let finalStatus = await PermissionsHelper.askIfNotGranted("NOTIFICATIONS", "notificaciones");

    // Stop here if the user did not grant permissions
    if (finalStatus !== 'granted') {
      throw new Error("Error: el permiso de notificaciones no fue concedido.");
    }
  };

  /**
   * Register remote notifications listener,
   * and configure to dispatch based on type to corresponding action/navigation.
   *
   * @param navigation
   * @returns {Promise.<void>}
   */
  setupNotificationsDispatcher = async (navigation) => {
    NewsDispatcher.setup({navigation});
    Notifications.addListener(async (notification) => {
      try {
        if (!this._validRemoteNotification(notification)) {
          return;
        }
        console.debug("[PushNotificationService] Received remote notification: ", notification);
        let {data} = notification;
        let type = data.type || data.Type;
        await NewsDispatcher.handleNotification({type, data});
      } catch (e) {
        console.error("Error al manejar push notification: ", e);
        Alert.alert("Error", "¡Ups! Hubo un error al manejar una push del server. \n" + (e.message || e));
      }
    });
  };

  _validRemoteNotification = (notification) => {
    let {remote, data} = notification;

    if (!remote) {
      console.debug("[PushNotificationsService] Received local notification (not remote). Ignoring. ", notification);
      return;
    }

    if (!data) {
      console.debug("[PushNotificationsService] Trying to parse notification body (data) which was null. ", notification);
      return false;
    }

    if (!data.type && !data.Type) {
      console.debug("[PushNotificationsService] Notification does not contain 'type' field. Ignoring.", notification);
      return false;
    }

    if (data.Type && data.Type === "position"){
      console.debug("[PushNotificationsService] Notification of type position is not a valid notification. Ignoring");
      return false;
    }

    return true;
  };
}

const pushNotificationsService = new PushNotificationsService();
export default pushNotificationsService;
