var STORAGE_KEY = 'id_token';
import {AsyncStorage} from 'react-native'

class SessionService {

  async _onValueChange(item, selectedValue) {
    try {
      await AsyncStorage.setItem(item, selectedValue);
    } catch (error) {
      console.log('AsyncStorage error: ' + error.message);
    }
  }
  constructor() {

  }

  /**
   * Set session token id.
   */
  async setSessionToken(id_token) {
    try {
      await this._onValueChange(STORAGE_KEY, id_token);
    } catch (e) {
      console.log("Session id set error:", e);
      Alert.alert(
        "Error de sesion",
        `Hubo un problema: ${e}`
      );
      return;
    }
  }

  async getSessionToken() {
    try {
      var token = await AsyncStorage.getItem(STORAGE_KEY);
      return token;
    } catch (e) {
      console.log("Session id set error:", e);
      Alert.alert(
        "Error de sesion",
        `Hubo un problema: ${e}`
      );
      return;
    }
  }
}

let sessionService = new SessionService();
export default sessionService;
