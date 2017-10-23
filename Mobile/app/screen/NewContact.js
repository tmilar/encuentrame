import React, {Component} from 'react';
import {
  FlatList, ListView, ScrollView, StyleSheet, Text, TextInput, TouchableHighlight, View} from 'react-native';
import {text} from '../style';
import AccountsService from '../service/AccountsService';


export default class NewContact extends Component {
  datasource = new ListView.DataSource({
    rowHasChanged: (r1, r2) => r1 !== r2,
  });
  state = {
    accounts: this.datasource.cloneWithRows([]),
    searchingContact: ""
  };

  componentWillMount = async () => {
    let accounts = await AccountsService.getAllUsersAccounts();
    this.setState({ "accounts": this.datasource.cloneWithRows(accounts) });
  };

  _pressRow = async (rowID) =>  {
    debugger;
    console.log("clicked" + rowID)
  };

  renderRow(account) {
    return (
      <View>
        <TouchableHighlight onPress={this._pressRow}>
          <View>
            <Text>{account.Username}</Text>
          </View>
        </TouchableHighlight>
      </View>
    )
  }

  render() {
    if (this.state.accounts.length == 0 )
      return null;
    return (
      <ScrollView scrollsToTop={false}>
        <ListView
          dataSource={this.state.accounts}
          renderRow={this.renderRow}
        />
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
