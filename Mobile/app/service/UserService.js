import {AsyncStorage} from 'react-native'
import {apiUrl } from '../config/apiProperties'

class UserService {

  constructor() {
    this.initialUsers = [{
      email: 'admin',
      password: 'admin'
    }];
  }

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
        "Username": 'javier.wamba',
        "Password": '123'
      })
    }).catch((error) => {
      console.error(error);
      console.log(error.json(), responseJson);
      return false;
    });
    if (resultado.status === 200){
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

  /**
   * Naive implementation to Vslidate user credentials against input password.
   *
   * @param user
   * @param password
   * @returns {boolean}
   */
  checkUserPassword(user, password) {
    return user.password !== password;
  }

  async findByEmail(userEmail) {
    let allUsers = await this.findAll();

    return allUsers.find(u => {
      return u.email === userEmail;
    });
  }

  async findAll() {

    let users;
    let storedUsersJson = await AsyncStorage.getItem("users");

    try {
      users = JSON.parse(storedUsersJson) || [];
    } catch (e) {
      throw new Error("Problem parsing users!", e);
    }

    let allUsers = users;

    if(this.initialUsers && this.initialUsers.length) {
      // synchronize (first time only)
      allUsers = [...this.initialUsers, ...users];
      delete this.initialUsers;
      await AsyncStorage.setItem("users", JSON.stringify(allUsers));
    }

    return allUsers;
  }

  async registerUser(userData) {

    if (!userData.email || userData.email === ''
      || !userData.password || userData.password === '') {
      throw `Por favor, ingrese un email y contraseñas válidos.`;
    }

    let userRegistrationResult = await fetch(apiUrl + 'account/create', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        "Username": userData.userName,
        "Password": userData.password,
        "Email": userData.email
      })
    }).catch((error) => {
      console.error(error);
      console.log(error.json(), responseJson);
      return false;
    });
    if (userRegistrationResult.status === 200){
      return {
        ok: true
      };
    } else {
      return {
        ok: false,
        resultado: userRegistrationResult
      };
    }

    console.log(`Registrado '${userData.email}' exitosamente!'`);
    return await AsyncStorage.setItem("user", JSON.stringify(userJson));
  }
}

let userService = new UserService();
export default userService;
