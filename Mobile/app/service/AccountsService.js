import Service from './Service';
import ContactsService from '../service/ContactsService';

class AccountsService {

  async getAllUserAccounts() {
    let accountsUrl = 'accounts';

    let accounts =  await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });

    return accounts;
  }

  async getUnknownUsersAccounts() {
    let accounts = await this.getAllUserAccounts();
    let contacts = await ContactsService.getAllContacts();
    let unknownPeople = [];
    accounts.forEach(function(acct){
      let isUnknown = true;
      contacts.forEach(function(contact){
        if (acct.Id == contact.User.Id){
          isUnknown =  false;
        }
      });
      if (isUnknown)
        unknownPeople.push(acct);
    });
    return unknownPeople;
  }
}

let accountService = new AccountsService();
export default accountService;
