import Service from './Service';

class AccountsService {

  async getAllUserAccounts() {
    let accountsUrl = 'accounts';

    let accounts =  await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });

    return accounts;
  }
}

let accountService = new AccountsService();
export default accountService;
