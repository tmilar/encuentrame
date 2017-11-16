import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import FamilyList from "./FamilyList";
import ContactsService from '../service/ContactsService';
import {showToast} from "react-native-notifyer";
import LoadingIndicator from "./LoadingIndicator";

export default class FamilyListContainer extends Component {
  state = {
    contacts: [],
    loading: true
  };

  componentWillMount = async () => {
    await this.fetchContacts();
  };

  fetchContacts = async () => {
    let contacts;
    try {
      contacts = await ContactsService.getAllContacts();
    } catch (e) {
      showToast("¡Ups! Hubo un problema al recuperar tu lista de contactos.");
    }
    this.setState({contacts, loading: false});
  };

  render() {
    return this.state.loading ?
      <View style={{top: 0, height: 500}}>
        <LoadingIndicator/>
      </View>
      :
      <View style={{flex: 1, marginTop: 5}}>
        <FamilyList familyMembers={[...this.state.contacts]}/>
      </View>

  }
}
