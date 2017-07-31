import {AsyncStorage} from 'react-native'

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
    let found = await this.findByEmail(user.email);

    if (!found) {
      throw `El usuario '${user.email}' no esta registrado.`;
    }

    // Check password
    if (this.checkUserPassword(found, user.password)) {
      throw `La contraseña ingresada para '${user.email}' es incorrecta.`;
    }

    return found;
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

    let existingUser = await this.findByEmail(userData.email);

    if(existingUser) {
      throw `El usuario '${userData.email}' ya existe.!`;
    }

    let storedUsers = await this.findAll();

    storedUsers = [...storedUsers, userData];

    console.log(`Registrado '${userData.email}' exitosamente! Users total: ${storedUsers.length}`);
    return await AsyncStorage.setItem("users", JSON.stringify(storedUsers));
  }
}

let userService = new UserService();
export default userService;
