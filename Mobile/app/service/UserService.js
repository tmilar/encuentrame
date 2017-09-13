import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';

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
    return await fetch(apiUrl + 'authentication/login/', {
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
}

let userService = new UserService();
export default userService;
