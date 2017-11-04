import Service from './Service';
import ContactsService from '../service/ContactsService';
import {apiUrl} from '../config/apiProperties'

class AccountsService {

  async getAllUserAccounts() {
    let accountsUrl = 'accounts';

    let accounts = await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });
    let that = this;
    accounts = accounts.map( function(acct) { return Object.assign(acct,{imageUri: that.getAccountImageById(acct.Id)}); } );

    return accounts;
  }

  async getUnknownUsersAccounts() {
    let accounts = await this.getAllUserAccounts();
    let contacts = await ContactsService.getAllContacts();

    let isAcctUnknown = (acct) => contacts.every((contact) => contact.User.Id !== acct.Id);
    let unknownPeople = accounts.filter(isAcctUnknown);

    return unknownPeople;
  }

  getAccountImageById(id) {
    let userImgUrl = 'account/getImage/' + id + "?rand=" + Math.random().toString();
    return apiUrl + userImgUrl;

  }
}

let accountService = new AccountsService();
export default accountService;
