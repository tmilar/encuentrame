import {AsyncStorage} from 'react-native'

class SessionService {

  STORAGE_KEY = 'ENCUENTRAME_SESSION_TOKEN';
  SESSION_TTL = 2 * 60 * 60 * 1000; //2 hours

  /**
   * Set session token id.
   */
  async setSessionToken(id_token) {
    let timestamp = new Date().getTime();
    let session = {
      id: id_token,
      expires: new Date(timestamp + this.SESSION_TTL).getTime()
    };
    return await AsyncStorage.setItem(this.STORAGE_KEY, JSON.stringify(session));
  }

  async isSessionAlive() {
    let sessionJson = await AsyncStorage.getItem(this.STORAGE_KEY);
    let session = JSON.parse(sessionJson);
    let isAlive = false;

    if (session && session.expires) {
      isAlive = new Date() < new Date(session.expires);
    }
    console.log("Session check: ", session, `alive?: ${isAlive}`);
    return isAlive;
  }

  async clearSession() {
    return await AsyncStorage.removeItem(this.STORAGE_KEY);
  }
}

let sessionService = new SessionService();
export default sessionService;
