import React, {Component} from 'react';
import {
  Button,
  FlatList, Image, ListView, ScrollView, StyleSheet, Text, TextInput, TouchableHighlight, TouchableNativeFeedback, View
} from 'react-native';
import {text} from '../style';
import Search from '../../lib/react-native-search-box';
import ContactsService from '../service/ContactsService';
import {Alert} from "react-native";
import LoadingIndicator from "../component/LoadingIndicator";
import AccountsService from '../service/AccountsService';
import {showToast} from "react-native-notifyer";


export default class NewContact extends Component {

  state = {
    loading: true,
    filteredAccounts: [],
    searchingContact: ""
  };

  static navigationOptions = {
    title: 'Agregar Contacto'
  };

  datasource = new ListView.DataSource({
    rowHasChanged: (r1, r2) => r1 !== r2,
  });

  /**
   * store accounts for username.
   * @type {Array}
   */
  accounts = [];

  constructor(props) {
    super(props);
  };

  componentWillMount = async () => {
    this.accounts = await AccountsService.getUnknownUsersAccounts();
    this.setState({filteredAccounts: this.datasource.cloneWithRows(this.accounts)});
    this.setState({loading: false});
  };

  _pressRow = async (account, sectionID, rowID) => {
    Alert.alert(
      'Confirma',
      '¿Agregar contacto?',
      [{
        text: 'Cancelar',
        style: 'cancel',
        onPress: () => {
          console.debug("Add contact canceled.")
        }
      }, {
        text: 'Confirmar',
        onPress: () => this._addContactRequest(account, sectionID, rowID)
      }],
      {cancelable: false}
    )
  };

  _addContactRequest = async (account, sectionID, rowID) => {
    this.setState({loading: true});
    await ContactsService.newContactRequest(account.Id);
    this.setState({loading: false});
    showToast("¡Solicitud enviada a " + account.Username + "!", {duration: 5000});
    this._goBack();
  };

  _goBack = () => {
    this.props.navigation.goBack(null);
  };

  searchingContactTextChanged = (searchingContact = "") => {
    this.setState({searchingContact});
    let filteredAccounts = this.accounts.filter((acct) => acct.Username.indexOf(searchingContact) >= 0);
    this.setState({"filteredAccounts": this.datasource.cloneWithRows(filteredAccounts)});
  };

  renderRow = (account, sectionID: number, rowID: number) => {
    return (
      <TouchableNativeFeedback style={{flex: 1}} onPress={() => this._pressRow(account, sectionID, rowID)}
                               background={TouchableNativeFeedback.SelectableBackground()}>
        <View style={{
          flex: 1,
          width: 400,
          flexDirection: 'row',
          justifyContent: 'space-between',
          borderBottomWidth: 1,
          borderBottomColor: 'grey'
        }}>
          <View style={{justifyContent: 'space-around', width: 120, height: 100}}>
            <Text>{account.Username}</Text>
          </View>
          <View style={{justifyContent: 'space-around', width: 120, height: 100}}>
            <Image source={{uri: account.imageUri}} style={{width: 75, height: 75}}/>
          </View>
          <View style={{justifyContent: 'space-around', width: 100, height: 100}}>
            <View style={{
              borderRadius: 40,
              justifyContent: 'space-around',
              width: 50,
              height: 50,
              backgroundColor: '#3DB097',
              borderWidth: 1,
              borderColor: 'white'
            }}>
              <Text style={{textAlign: 'center', color: 'white', fontSize: 45}}>+</Text>
            </View>
          </View>

        </View>
      </TouchableNativeFeedback>
    )
  };


  render() {
    return (
      <View style={{flex: 1}}>
        <Search
          onChangeText={this.searchingContactTextChanged}
          onCancel={this.searchingContactTextChanged}
          onDelete={this.searchingContactTextChanged}
          ref="search_box"
          cancelTitle={"Cancelar"}
          placeholder={"Buscar por nombre"}
          cancelButtonStyle={{width: 80}}
          cancelButtonWidth={80}
          searchIconCollapsedMargin={70}
          placeholderCollapsedMargin={55}
          positionRightDelete={80}
        />
        { this.state.loading ? <LoadingIndicator/> :
          <ScrollView scrollsToTop={false} style={{flex: 1}}>
            <View style={{flex: 1, flexDirection: 'column', justifyContent: 'flex-start', alignItems: "center"}}>
              <ListView style={{flex: 1}}
                        dataSource={this.state.filteredAccounts}
                        renderRow={this.renderRow}
                        enableEmptySections={true}
              />
            </View>
          </ScrollView>
        }
      </View>
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
