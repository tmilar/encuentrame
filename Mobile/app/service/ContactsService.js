import {apiUrl} from '../config/apiProperties'
import Service from './Service';
import {AsyncStorage} from 'react-native';

class ContactsService {

  async getAllContacts() {
    let contactsUrl = 'contacts';
    let contactsResponse =  await Service.sendRequest(contactsUrl, {
      method: 'GET'
    });
    return contactsResponse;
  }

  newContactRequest = async (userId) => {
    const url = "Contact/request/" + userId;
    return await Service.sendRequest(url, {
      method: "POST",
      body: JSON.stringify({
      })
    });
  };

  reply = async (contactRequestUserId, response) => {
    const url = "Contact/confirm/"  +  contactRequestUserId;
    return await Service.sendRequest(url, {
      method: "POST",
      body: JSON.stringify({
        confirm: response
      })
    });
  };
}

let contactsService = new ContactsService();
export default contactsService;
