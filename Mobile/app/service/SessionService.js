import {AsyncStorage} from 'react-native'

class SessionService {

  STORAGE_KEY = 'ENCUENTRAME_SESSION_TOKEN';

  /**
   * Set session token id.
   */
  async setSessionToken(id_token) {
    return await AsyncStorage.setItem(this.STORAGE_KEY, id_token);
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
