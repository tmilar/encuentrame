import {Permissions, Notifications} from 'expo';
import Service from './Service';
import {AsyncStorage} from 'react-native';
import PermissionsHelper from '../util/PermissionsHelper';
import {Alert} from "react-native";

class PushNotificationsService {

  /**
   * Register Exponent push token to server.
   * Store token locally to prevent duplicated requests.
   *
   * @returns {Promise.<void>}
   */
  registerDevice = async () => {

    if(await this._checkRegistered()) {
      console.log("[PushNotificationsService] Device already registered.");
      return;
    }

    await this._requireNotificationsPermission();

    // Get the token that uniquely identifies this device
    let token = await Notifications.getExponentPushTokenAsync();

    // register token to server.
    this._registerDeviceRequest(token);

    // Remember stored token in local storage.
    await this._saveRegistered(token);
  };

  _registerDeviceRequest(token) {
    const url = 'Account/devices';

    // POST the token to your backend server from where you can retrieve it to send push notifications.
    try {
      Alert.alert(`POST ${url}`, `Registro con el servidor OK! \nToken: ${token}`);
      // TODO hacer la request real al backend..
      // await Service.sendRequest(url, {
      //   method: 'POST',
      //   body: JSON.stringify({
      //     pushToken: token
      //   }),
      // });
    } catch (e) {
      throw 'Problema al registrar el dispositivo. ' + (e.message || e);
    }
  }

  _saveRegistered = async (token) => {
    await AsyncStorage.setItem("ENCUENTRAME_NOTIFICATIONS_TOKEN", token);
    console.log("[PushNotificationsService] Device registered for push notifications. Token: ", token);
  };

  /**
   * Check if device is already registered.
   *
   * @returns {Promise.<boolean>}
   * @private
   */
  _checkRegistered = async () => {
    let token = await AsyncStorage.getItem("ENCUENTRAME_NOTIFICATIONS_TOKEN");
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
    Notifications.addListener((notification) => {
      if(!this._validRemoteNotification(notification)) {
        return;
      }

      console.debug("[PushNotificationService] Received notification: ", notification);
      if(notification.data.type === "estasbien") {
        console.log("[PushNotificationService] Notification 'estasbien'! Navigating to 'AreYouOk' screen.");
        navigation.navigate("AreYouOk");
      }

      //TODO handle/switch over other types of notifications. Move this logic to a different service?
    });
  };

  _validRemoteNotification = (notification) => {
    let data = notification.data;

    if (!data) {
      console.debug("[PushNotificationsService] Trying to parse notification body (data) which was null. ", notification);
      return false;
    }

    if (!data.type) {
      console.debug("[PushNotificationsService] Notification is not 'estasbien' type. Ignoring.", notification);
      return false;
    }

    // TODO check remote here?

    return true;
  };

}

const pushNotificationsService = new PushNotificationsService();
export default pushNotificationsService;
