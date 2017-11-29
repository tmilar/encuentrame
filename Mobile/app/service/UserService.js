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
    let loginTokensResponse;
    try {
      loginTokensResponse = await this.postLoginRequest(user);
    } catch (e) {
      let errBody = {message: e.message || e, status: e.status || undefined};
      if (errBody.status === 401) {
        errBody.message = 'El usuario o la contraseña son inválidas. Por favor, verifique sus credenciales.';
      }
      console.log("Some error occured when doing postLoginRequest(): ", errBody);
      throw errBody;
    }
    return loginTokensResponse;
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

  postLogoutRequest = async () => {
    let logoutUrl = 'authentication/logout';

    return await Service.sendRequest(logoutUrl, {
      method: 'POST'
    });
  };

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

  isValidEmail = (email) => {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
  };

  async registerUser(userData) {
    let formErrMsg = "";
    if (!userData.username || userData.username === '') {
      formErrMsg += `\nPor favor, ingrese un nombre de usuario válido.`;
    }

    if (!userData.email || userData.email === '' || !this.isValidEmail(userData.email)) {
      formErrMsg += `\nPor favor, ingrese una dirección de Email válida.`;
    }

    if (!userData.password || userData.password === '') {
      formErrMsg += `\nPor favor, ingrese una Contraseña válida.`;
    }

    if (formErrMsg) {
      throw formErrMsg;
    }


    await this.tryRegisterUserRequest(userData);

    console.log(`¡Registrado '${userData.username}' exitosamente!'`);
  }

  async tryRegisterUserRequest(userData) {
    let registerUrl = 'account/create';
    try {
      await Service.sendRequest(registerUrl, {
        method: 'POST',
        body: {
          "Username": userData.username,
          "Password": userData.password,
          "Email": userData.email
        }
      });
    } catch (e) {
      let errBody = {message: e.message || e, status: e.status || undefined};
      if (errBody.status === 400) {
        errBody.originalServerMessage = errBody.message;
        errBody.message = `\nEl nombre de usuario "${userData.username}" ya existe.`;
      }
      console.log("Some error occured when doing registerUser(): ", errBody);
      throw errBody;
    }
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

  async uploadUserProfileImage(formData) {
    let uploadUserImageUrl = 'account/uploadImage';
    let uploadUserImageResponse = await Service.sendMultipartFormDataRequest(uploadUserImageUrl, {
      method: 'POST',
      body: formData
    });
    return uploadUserImageResponse;
  }

  async getLoggedUserImgUrl() {
    let userId = await SessionService.getSessionUserId();
    let userImgUrl = 'account/getImage/' + userId;
    return apiUrl + userImgUrl;
  };

}

let userService = new UserService();
export default userService;
