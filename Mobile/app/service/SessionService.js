var STORAGE_KEY = 'id_token';
import {AsyncStorage} from 'react-native'

class SessionService {

  async _onValueChange(item, selectedValue) {
    await AsyncStorage.setItem(item, selectedValue);
  }
  constructor() {

  }

  /**
   * Set session token id.
   */
  async setSessionToken(id_token) {
    await this._onValueChange(STORAGE_KEY, id_token);
  }

  async getSessionToken() {
    try {
      var token = await AsyncStorage.getItem(STORAGE_KEY);
      return token;
    } catch (e) {
      let errMsg = "Session id set error:";
      console.log(errMsg, e);
      throw new Error(errMsg, e);
    }
  }
}

let sessionService = new SessionService();
export default sessionService;
