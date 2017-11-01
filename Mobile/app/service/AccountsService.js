import Service from './Service';
import ContactsService from '../service/ContactsService';

class AccountsService {

  async getAllUserAccounts() {
    let accountsUrl = 'accounts';

    let accounts = await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });

    return accounts;
  }

  async getUnknownUsersAccounts() {
    let accounts = await this.getAllUserAccounts();
    let contacts = await ContactsService.getAllContacts();

    let isAcctUnknown = (acct) => contacts.every((contact) => contact.User.Id !== acct.Id);
    let unknownPeople = accounts.filter(isAcctUnknown);

    return unknownPeople;
  }
}

let accountService = new AccountsService();
export default accountService;
