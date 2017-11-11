import Service from './Service';
import SessionService from './SessionService';
import {apiUrl} from '../config/apiProperties';
import {AsyncStorage} from 'react-native';

class NewsService {

  async setSession(sessionData) {
    this._validateSessionData(sessionData);

    let timestamp = new Date().getTime();
    let session = {
      token: sessionData.token,
      userId: sessionData.userId,
      expires: new Date(timestamp + this.SESSION_TTL).getTime(),
      username: sessionData.username
    };
    return await AsyncStorage.setItem(this.STORAGE_KEY, JSON.stringify(session));
  }

  async getUnknownUsersAccounts() {
    let userId = await SessionService.getSessionUserId();
    let accounts = await this.getAllUserAccounts();
    let contacts = await ContactsService.getAllContacts();

    let isAcctUnknown = (acct) => {return acct.Id !== userId && contacts.every((contact) => contact.User.Id !== acct.Id)};
    let unknownPeople = accounts.filter(isAcctUnknown);

    return unknownPeople;
  }

  getAccountImageUriById(id) {
    let userImgUrl = 'account/getImage/' + id + "?rand=" + Math.random().toString();
    return apiUrl + userImgUrl;

  }
}

let newsService = new NewsService();
export default newsService;
