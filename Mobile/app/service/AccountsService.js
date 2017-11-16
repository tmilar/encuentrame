import Service from './Service';
import ContactsService from '../service/ContactsService';
import SessionService from './SessionService';
import {apiUrl} from '../config/apiProperties'

class AccountsService {

  async getAllUserAccounts() {
    let accountsUrl = 'accounts';

    let accounts = await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });
    accounts = accounts.map( (acct) =>  Object.assign(acct,{imageUri: this.getAccountImageUriById(acct.Id)}) );

    return accounts;
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

  async getLoggedUserAccount() {
    let myAccountUrl = 'account/get';
    let myAccount = await Service.sendRequest(myAccountUrl, {
      method: 'GET'
    });
    return myAccount;
  }

  async updateAccount(modifiedProfile) {
    let myAccountUpdateUrl = 'account/update';
    await Service.sendRequest(myAccountUpdateUrl, {
      method: 'POST',
      body: JSON.stringify(modifiedProfile)
    });
  }
}

let accountService = new AccountsService();
export default accountService;
