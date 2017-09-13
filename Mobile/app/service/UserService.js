import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';

class UserService {

  /**
   * Check registered user credentials against api
   */
  async checkCredentials(user) {

    let rawResponse = await fetch(apiUrl + 'authentication/login/', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        "Username": user.username,
        "Password": user.password
      })
    });

    // Check response status
    let status = rawResponse.status;
    if (status < 200 || status > 300) {
      console.debug(rawResponse);
      this._manageLoginErrors(status);
    }

    // Parse response data
    let loginResponse;

    try {
      loginResponse = await rawResponse.json();
    } catch (e) {
      console.error("Invalid server login raw response", e);
      throw 'Ocurrió un problema en la comunicación con el servidor.'
    }

    // Login OK. Guardamos el token y el userId en la sesion.
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
