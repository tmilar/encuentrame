import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';

class UserService {

  /**
   * Fetch registered users. Using local storage for now.
   */
  async checkCredentials(user) {

    let loginResponse = await fetch(apiUrl + 'authentication/login/', {
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

    if (loginResponse.status !== 200) {
      // TODO refinar control/pase de errores
      throw 'Credenciales invalidas';
    }

    // Login OK. TODO leer 'resultado' y guardar bien el token de la respuesta para la sesion.
    try {
      await SessionService.setSessionToken("DUMMY_TOKEN");
    } catch (e) {
      throw 'Problema al guardar la sesion';
    }

  }

  async registerUser(userData) {

    if (!userData.username || userData.username === ''
      || !userData.password || userData.password === '') {
      throw `Por favor, ingrese un username y contraseñas válidos.`;
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

    if (userRegistrationResult.status !== 200) {
      throw 'Error en el registro...';
    }

    console.log(`Registrado '${userData.username}' exitosamente!'`);
  }
}

let userService = new UserService();
export default userService;
