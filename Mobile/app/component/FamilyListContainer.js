import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import FamilyList from "./FamilyList";
import {familyMembers} from "../config/familyFixture";
import {text} from '../style';
import ContactsService from '../service/ContactsService';

export default class FamilyListContainer extends Component {
  state = {
    contacts: []
  };

  componentWillMount = async () => {
    let contacts = await this.fetchContacts().filter((contact) => {return !contact.Pending;} );;
    this.setState({contacts});
  };

  fetchContacts = async () =>  {
    let contacts = await ContactsService.getAllContacts();
    return contacts;
  }

  render() {
    return <View style={{flex: 1}}>
            <Text style={text.p}>
              Contactos
            </Text>
            <FamilyList familyMembers={this.state.contacts}/>
          </View>
   ;
  }
}
