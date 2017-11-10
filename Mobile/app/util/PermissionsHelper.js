import {Permissions} from 'expo';
import {Alert} from 'react-native';
import sleep from "./sleep";

class PermissionsHelper {
  static askPermission = async (permissionName, permissionI18n, timeInterval = 3000) => {
    let response = await Permissions.askAsync(Permissions[permissionName]);

    if (response.status !== 'granted') {
      Alert.alert(
        "Ocurrió un problema.",
        `¡El permiso de ${permissionI18n} es necesario para el uso de esta app!`
      );
      await sleep(timeInterval);
      await this.askPermission();
    }
  };

  static askIfNotGranted = async (permission, permissionI18n) => {
    const {status: existingStatus} = await Permissions.getAsync(Permissions[permission]);

    let finalStatus = existingStatus;

    // only ask if permissions have not already been determined, because
    // iOS won't necessarily prompt the user a second time.
    if (existingStatus !== 'granted') {
      // Android remote notification permissions are granted during the app
      // install, so this will only ask on iOS
      const {status} = await PermissionsHelper.askPermission(permission, permissionI18n);
      finalStatus = status;
    }
    return finalStatus;
  };
}

export default PermissionsHelper;
