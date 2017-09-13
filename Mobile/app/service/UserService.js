import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';
import fetchMock from 'fetch-mock';

class UserService {

  /**
   * Check registered user credentials against api
   */
  async checkCredentials(user) {

    // request server login
    let rawResponse = await this.postLoginRequest(user);

    // Check response status for errors
    this.checkResponseStatus(rawResponse);

    // Parse response data
    let loginData = await this.parseLoginResponse(rawResponse);

    // Login OK! Guardamos el token y el userId en la sesion.
    await this.storeLoginSession(loginData);

  }

  async postLoginRequest(userData) {
    let loginUrl = apiUrl + 'authentication/login/';

    this.checkTestUser(userData, loginUrl);

    return await fetch(loginUrl, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
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

    fetchMock.once(loginUrl, mockLoginResponse);
  }

  checkResponseStatus(rawResponse) {
    let status = rawResponse.status;
    if (status < 200 || status > 300) {
      console.debug(rawResponse);
      this._manageLoginErrors(status);
    }
  }

  async parseLoginResponse(rawResponse) {
    try {
      return await rawResponse.json();
    } catch (e) {
      console.error("Invalid server login raw response", e);
      throw 'Ocurrió un problema en la comunicación con el servidor.'
    }
  }

  async storeLoginSession(loginResponse) {
    try {
      await SessionService.setSession({token: loginResponse.Token, userId: loginResponse.UserId});
    } catch (e) {
      console.error(e);
      throw 'Problema al guardar la sesion.';
    }
  }

  _manageLoginErrors(loginResponseStatus){
    if (loginResponseStatus === 403) {
      throw 'El servidor no está disponible. Por favor vuelva a intentar más tarde :(';
    }

    if (loginResponseStatus === 401 || loginResponseStatus === 400) {
      throw 'El usuario o la contraseña son inválidas (' + loginResponseStatus + ').';
    }

    throw 'Ha ocurrido un error. (status: ' + loginResponseStatus + ').';
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
