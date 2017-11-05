import {apiUrl} from '../config/apiProperties'
import Service from './Service';
import AccountsService from '../service/AccountsService';
import {AsyncStorage} from 'react-native';

class ContactsService {

  async getAllContacts() {
    let contactsUrl = 'contacts';
    let contactsResponse =  await Service.sendRequest(contactsUrl, {
      method: 'GET'
    });
    contactsResponse.forEach( function(cont) {
      let user = cont.User;
      user = Object.assign(user,{imageUri: AccountsService.getAccountImageUriById(user.Id)});
      cont.User = user;
    });
    return contactsResponse;
  }

  newContactRequest = async (userId) => {
    const url = "Contact/request/" + userId;
    //el backend no funciona bien si le mandas un POST sin body... por eso se envia
    return await Service.sendRequest(url, {
      method: "POST",
      body: JSON.stringify({
      })
    });
  };

  reply = async (contactRequestUserId, response) => {
    let replyUrl = `Contact/${response ? 'confirm' : 'reject'}/${contactRequestUserId}`;
    return await Service.sendRequest(replyUrl, {
      method: "POST"
    });
  };
}

let contactsService = new ContactsService();
export default contactsService;
