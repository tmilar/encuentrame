import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';
import fetchMock from 'fetch-mock';
import PushNotificationsService from '../service/PushNotificationsService';
import Service from './Service';

class UserService {

  /**
   * Login {username,password}
   * Then register device for push notifications.
   *
   * @param user
   * @returns {Promise.<void>}
   */
  doLogin = async (user) => {
    await this.checkCredentials(user);
    await PushNotificationsService.registerDevice(user);
  };

  /**
   * Check registered user credentials against api
   */
  async checkCredentials(user) {

    // request server login
    let loginResponse = await this.tryUserLogin(user);

    // Login OK! Guardamos el token y el userId en la sesion.
    await this.storeLoginSession(loginResponse, user.username);

  }

  tryUserLogin = async (user) => {
    let loginResponse;
    try {
      loginResponse = await this.postLoginRequest(user);
    } catch (e) {
      let errBody = {message: e.message || e, status: e.status || undefined};
      if (errBody.status === 401) {
        errBody.message = 'El usuario o la contraseña son inválidas. Por favor, verifique sus credenciales.';
      }
      console.log("Some error occured when doing postLoginRequest(): ", errBody);
      throw errBody;
    }
    return loginResponse;
  };

  async postLoginRequest(userData) {
    let loginUrl = 'authentication/login/';

    this.checkTestUser(userData, loginUrl);

    return await Service.sendRequest(loginUrl, {
      method: 'POST',
      body: JSON.stringify({
        "Username": userData.username,
        "Password": userData.password
      })
    });
  }

  /**
   * Only DEV env: if credentials belong to 'test' user,
   * mock server login response to allow
   * easy access to the app.
   *
   * @param userData
   * @param loginUrl
   */
  checkTestUser(userData, loginUrl) {
    if (!(__DEV__ && this.isTestUser(userData))) {
      return;
    }
    let mockLoginResponse = {
      Token: "1",
      UserId: "1"
    };

    fetchMock.once(apiUrl + loginUrl, mockLoginResponse);
  }


  async storeLoginSession(loginResponse, username) {
    try {
      await SessionService.setSession({token: loginResponse.Token, userId: loginResponse.UserId, username: username});
    } catch (e) {
      console.error(e);
      throw 'Ocurrió un problema al guardar la sesión.';
    }
  }

  async registerUser(userData) {

    if (!userData.username || userData.username === ''
      || !userData.password || userData.password === '') {
      throw `Por favor, ingrese un Usuario y Contraseña válidos.`;
    }

    let userRegistrationResult = await fetch(apiUrl + 'account/create', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        "Username": userData.username,
        "Password": userData.password,
        "Email": userData.email
      })
    });

    let status = userRegistrationResult.status;
    if (status < 200 || status >= 300) {
      console.debug(userRegistrationResult);
      throw 'Error en el registro. (status: ' + status + ').';
    }

    console.log(`Registrado '${userData.username}' exitosamente!'`);
  }

  isTestUser(userData) {
    const testUser = {
      username: "test",
      password: "test"
    };

    return userData &&
      userData.username === testUser.username &&
      userData.password === testUser.password
  }
}

let userService = new UserService();
export default userService;
