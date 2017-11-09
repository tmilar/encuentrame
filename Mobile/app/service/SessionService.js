import {AsyncStorage} from 'react-native'

class SessionService {

  STORAGE_KEY = 'ENCUENTRAME_SESSION_TOKEN';
  SESSION_TTL = 2 * 60 * 60 * 1000; //2 hours

  /**
   * Set session token id.
   */
  async setSession(sessionData) {
    this._validateSessionData(sessionData);

    let timestamp = new Date().getTime();
    let session = {
      token: sessionData.token,
      userId: sessionData.userId,
      expires: new Date(timestamp + this.SESSION_TTL).getTime(),
      username: sessionData.username
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

  async getSessionParam(paramName) {
    let sessionJson = await AsyncStorage.getItem(this.STORAGE_KEY);
    let param = null;
    if(!sessionJson) {
      console.log(`No hay sesion guardada! No se pudo encontrar el parametro: ${paramName}`);
      return;
    }

    let session = JSON.parse(sessionJson);

    if (session && session[paramName]) {
      param = session[paramName];
    }
    console.log("Session " + paramName + ": ", param);
    return param;
  }

  async getSessionToken() {
    return this.getSessionParam("token");
  }

  async getSessionUserId() {
    return this.getSessionParam("userId");
  }

  async clearSession() {
    return await AsyncStorage.removeItem(this.STORAGE_KEY);
  }

  _validateSessionData(data) {
    if (!data) {
      throw 'Session data is empty!';
    }
    if(!data.token) {
      throw 'Session tokenId is missing!';
    }
    if(!data.userId) {
      throw 'User Id is missing!';
    }
  }

  isDevSession = async () => {
    let username = await this.getSessionParam("username");
    if(!username) {
      return false;
    }
    return username.includes("test");
  }
}

let sessionService = new SessionService();
export default sessionService;
