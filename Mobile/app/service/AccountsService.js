import {apiUrl} from '../config/apiProperties'
import Service from './Service';
import {AsyncStorage} from 'react-native';

class AccountsService {

  async getAllUsersAccounts() {
    let accountsJson, accounts;
    accountsJson = await AsyncStorage.getItem("Accounts");
    accounts = JSON.parse(accountsJson);
    if (accounts){
      return accounts;
    }
    let accountsUrl = 'accounts';
    accounts =  await Service.sendRequest(accountsUrl, {
      method: 'GET'
    });
    await AsyncStorage.setItem("Accounts", JSON.stringify(accounts));
    return accounts;
  }
}

let accountService = new AccountsService();
export default accountService;
