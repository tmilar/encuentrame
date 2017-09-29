import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';


/**
 * Manage base session headers for all services after user has logged in
 */
class Service {

  async sendRequest(url, requestData) {
    url = apiUrl + url;
    let userId = await SessionService.getSessionUserId();
    let token = await SessionService.getSessionToken();
    if (!requestData.headers){
      requestData.headers = {};
    }
    Object.assign(requestData.headers, {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'token': token,
      'user': userId
    });
    return await fetch(url, requestData);
  }
  async parseResponse(rawResponse) {
    try {
      return await rawResponse.json();
    } catch (e) {
      console.error("Invalid server raw response", e);
      throw 'Ocurrió un problema en la comunicación con el servidor.'
    }
  }

}

let baseService = new Service();
export default baseService;
