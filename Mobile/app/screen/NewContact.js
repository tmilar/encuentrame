import React, {Component} from 'react';
import {
  Button,
  FlatList, ListView, ScrollView, StyleSheet, Text, TextInput, TouchableHighlight, View
} from 'react-native';
import {text} from '../style';
import ContactsService from '../service/ContactsService';
import {Alert} from "react-native";
import LoadingIndicator from "../component/LoadingIndicator";
import AccountsService from '../service/AccountsService';


export default class NewContact extends Component {

  datasource = new ListView.DataSource({
    rowHasChanged: (r1, r2) => r1 !== r2,
  });

  state = {
    loading: true,
    filteredAccounts: this.datasource.cloneWithRows([]),
    searchingContact: ""
  };

  /**
   * store accounts for username.
   * @type {Array}
   */
  accounts = [];

  constructor(props) {
    super(props);
  };

  componentWillMount = async () => {
    let accounts = await AccountsService.getUnknownUsersAccounts();
    this.accounts = accounts || [];
    this.setState({ filteredAccounts: this.datasource.cloneWithRows(this.accounts) });
    this.setState({ loading: false });
  };

   _pressRow = async (account, sectionID, rowID) =>  {
    let requestContact = await ContactsService.newContactRequest(account.Id);
    Alert.alert("Solicitud enviada!", "Solicitud enviada con exito a " + account.Username);
    this._goBack();
  };

  _goBack = () => {
    this.props.navigation.goBack(null);
  };

  searchingContactTextChanged = (searchingContact) => {
    this.setState({searchingContact});
    let filteredAccounts = this.accounts.filter((acct) => acct.Username.indexOf(searchingContact) >= 0);
    this.setState({ "filteredAccounts": this.datasource.cloneWithRows(filteredAccounts) });
  };

  renderRow = (account: string, sectionID: number, rowID: number) => {
    return (
      <TouchableHighlight style={{flex: 1, height: 30 }} onPress={() => this._pressRow(account, sectionID, rowID)}>
        <View>
          <Text>{account.Username} - Agregar</Text>
        </View>
      </TouchableHighlight>
    )
  };


  render() {
    if (this.state.loading )
      return <LoadingIndicator/>;
    return (
      <ScrollView scrollsToTop={false} style={{marginTop: 50, flex: 1 }}>
        <View style={{flex: 1, justifyContent: 'flex-start', borderBottomColor: '#47315a', borderBottomWidth: 1 }}>
          <TextInput
            value={this.state.searchingContact}
            placeholder="Buscar por nombre"
            ref="searchingContact"
            selectTextOnFocus
            onChangeText={this.searchingContactTextChanged}
            underlineColorAndroid='transparent'
          />
        </View>
        <View style={{flex: 9, height: 200, flexDirection: 'column', justifyContent: 'flex-start', alignItems: "center", borderBottomColor: '#47315a', borderBottomWidth: 1 }}>
          <ListView style={{flex: 1}}
            dataSource={this.state.filteredAccounts}
            renderRow={this.renderRow}
          />
        </View>
        <View style={{flex: 1}}>
          <View style={{flexDirection: 'row', justifyContent: 'space-around'}}>
            <Button
              title="Volver"
              color='#ff5c5c'
              onPress={this._goBack}
            />

          </View>
        </View>

      </ScrollView>
    )
  }
}

const styles = StyleSheet.create({
  message: {
    flex: 5,
    height: 3,
    alignItems: 'center',
    justifyContent: 'center',
  },
  container: {
    flex: 1,
    paddingTop: 22
  },
  item: {
    padding: 10,
    fontSize: 18,
    height: 44,
  },
  style_row_view: {
    flex: 1,
    flexDirection: 'row',
    height: 57,
    backgroundColor: '#FFFFFF',
  },
  style_text: {
    flex: 1,
    marginLeft: 12,
    fontSize: 16,
    color: '#333333',
    alignSelf: 'center',
  },
  style_separator: {
    borderBottomWidth: 1,
    borderBottomColor: 'red'
  }
});
