import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import FamilyList from "./FamilyList";
import ContactsService from '../service/ContactsService';

export default class FamilyListContainer extends Component {
  state = {
    contacts: []
  };

  componentWillMount = async () => {
    let contacts = await this.fetchContacts();
    this.setState({contacts});
  };

  fetchContacts = async () => {
    let contacts = await ContactsService.getAllContacts();
    return contacts;
  };

  render() {
    return <View style={{flex: 1, marginTop: 5}}>
      <FamilyList familyMembers={this.state.contacts}/>
    </View>
  }
}
