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

    this._requireNotificationsPermission();

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
    const {status: existingStatus} = await Permissions.getAsync(Permissions.NOTIFICATIONS);

    let finalStatus = existingStatus;

    // only ask if permissions have not already been determined, because
    // iOS won't necessarily prompt the user a second time.
    if (existingStatus !== 'granted') {
      // Android remote notification permissions are granted during the app
      // install, so this will only ask on iOS
      const {status} = await PermissionsHelper.askPermission("NOTIFICATIONS", "notificaciones");
      finalStatus = status;
    }

    // Stop here if the user did not grant permissions
    if (finalStatus !== 'granted') {
      throw new Error("Error: el permiso de notificaciones no fue concedido.");
    }
  }
}

const pushNotificationsService = new PushNotificationsService();
export default pushNotificationsService;
