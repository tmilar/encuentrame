import {apiUrl} from '../config/apiProperties'
import Service from './Service';
import {AsyncStorage} from 'react-native';

class AccountsService {

  async getAllUsersAccounts(accountData) {
    let accountsUrl = 'accounts';
    let accountsResponse =  await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });
    await AsyncStorage.setItem("Accounts", accountsResponse);
    return accountsResponse;
  }
}

let accountService = new AccountsService();
export default accountService;
