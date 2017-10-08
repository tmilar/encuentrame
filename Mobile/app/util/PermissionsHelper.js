import {Permissions} from 'expo';

class PermissionsHelper {
  static askPermission = async (permissionName, permissionI18n, timeInterval = 3000) => {
    let response = await Permissions.askAsync(Permissions[permissionName]);

    if (response.status !== 'granted') {
      Alert.alert(
        "Ocurrió un problema.",
        `El permiso de ${permissionI18n} es necesario para el uso de esta app!`
      );
      await this._sleep(timeInterval);
      await this.askPermission();
    }
  };

  /**
   * Auxiliar function to have a delay in ms, compatible with async/await.
   *
   * @param time
   * @returns {Promise}
   * @private
   */
  static _sleep = (time) => {
    return new Promise((resolve) => setTimeout(resolve, time));
  };
}

export default PermissionsHelper;
