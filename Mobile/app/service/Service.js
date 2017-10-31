import {apiUrl} from '../config/apiProperties'
import SessionService from './SessionService';
import isJSON from '../util/isJSON';

/**
 * Manage base session headers for all services after user has logged in
 */
class Service {

  async sendRequest(url, requestData, returnRawResponse) {
    url = apiUrl + url;
    let userId = await SessionService.getSessionUserId();
    let token = await SessionService.getSessionToken();

    requestData = requestData || {method: "GET"};

    if (!requestData.headers) {
      requestData.headers = {};
    }
    if (requestData.method == 'POST' && (!requestData.body)){
      requestData.body = JSON.stringify({
      });
    }
    if (!requestData.headers['Content-Type']){
      Object.assign(requestData.headers, {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      });
    }
    Object.assign(requestData.headers, {
      'token': token,
      'user': userId
    });

    let rawResponse = await fetch(url, requestData);
    this.checkResponseStatus(rawResponse);
    if (returnRawResponse)
      return rawResponse;
    let finalResponse = await this.parseResponse(rawResponse);
    return finalResponse;
  }

  async parseResponse(rawResponse) {
    try {
      return await this._parseJSON(rawResponse);
    } catch (e) {
      console.error("Invalid server raw response", e);
      throw 'Ocurrió un problema en la comunicación con el servidor.'
    }
  }

  /**
   * Parse response body to always return a valid JS object,
   * even if body is null or empty ("{}").
   *
   * @param response
   * @returns {Promise.<{}>}
   * @private
   */
  _parseJSON = async (response) => {
    let text = await response.text();
    let parsed = isJSON(text) ? JSON.parse(text) : {};
    return parsed;
  };


  checkResponseStatus(rawResponse) {
    let status = rawResponse.status;
    if (status < 200 || status >= 300) {
      console.debug(rawResponse);
      if (status === 403) {
        throw 'El servidor no está disponible. Por favor vuelva a intentar más tarde :(';
      }

      if (status === 401 || status === 400) {
        throw 'La sesión ha caducado. Por favor, vuelva a iniciar sesión. (' + status + ').';
      }
      debugger;
      throw 'Ha ocurrido un error. (status: ' + status + ').';

    }
  }

}

let baseService = new Service();
export default baseService;
