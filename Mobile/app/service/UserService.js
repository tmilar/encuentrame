import {apiUrl} from '../config/apiProperties'

class UserService {

  /**
   * Fetch registered users. Using local storage for now.
   */
  async checkCredentials(user) {

    let resultado = await fetch(apiUrl + 'authentication/login/', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        "Username": user.username,
        "Password": user.password
      })
    }).catch((error) => {
      console.error(error);
      console.log(error.json(), responseJson);
      return false;
    });
    if (resultado.status === 200) {
      return {
        ok: true
      };
    } else {
      return {
        ok: false,
        resultado: 'Credenciales incorrectas!'
      };
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
