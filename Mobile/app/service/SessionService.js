import {AsyncStorage} from 'react-native'

class SessionService {

  STORAGE_KEY = 'ENCUENTRAME_SESSION_TOKEN';
  SESSION_TTL = 2 * 60 * 60 * 1000; //2 hours

  /**
   * Set session token id.
   */
  async setSession(sessionData) {
    this._validateSessionData();

    let timestamp = new Date().getTime();
    let session = {
      id: sessionData.token,
      userId: sessionData.userId,
      expires: new Date(timestamp + this.SESSION_TTL).getTime()
    };
    return await AsyncStorage.setItem(this.STORAGE_KEY, JSON.stringify(session));
  }

  async isSessionAlive() {
    let sessionJson = await AsyncStorage.getItem(this.STORAGE_KEY);
    let isAlive = false;

    if(!sessionJson) {
      console.log("Session check: [No session found]", `alive?: ${isAlive}`);
      return isAlive;
    }

    let session = JSON.parse(sessionJson);

    if (session && session.expires) {
      isAlive = new Date() < new Date(session.expires);
    }
    console.log("Session check: ", session, `alive?: ${isAlive} (expires: ${new Date(session.expires)}`);
    return isAlive;
  }

  async clearSession() {
    return await AsyncStorage.removeItem(this.STORAGE_KEY);
  }

  _validateSessionData(data) {
    if (!data) {
      throw 'Session data is empty!';
    }
    if(!data.tokenId) {
      throw 'Session tokenId is missing!';
    }
    if(!data.userId) {
      throw 'User Id is missing!';
    }
  }
}

let sessionService = new SessionService();
export default sessionService;
