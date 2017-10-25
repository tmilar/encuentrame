import React, {Component} from 'react'
import {View, ScrollView, TextInput, Button} from 'react-native'
import FamilyListContainer from "../component/FamilyListContainer";
import {text} from '../style';
import SessionService from "../service/SessionService";
import AreYouOkService from "../service/AreYouOkService";
import AccountsService from '../service/AccountsService';

import {Alert} from "react-native";


export default class FriendsAndFamily extends Component {

  state = {
    targetUserId: "",
    areYouOkSelectorVisible: false
  };

  _handleUserIdChange = (inputValue) => {
    this.setState({targetUserId: inputValue})
  };

  componentWillMount = async () => {
    // Load in shared storage all user accounts
    await AccountsService.getAllUsersAccounts();

    // show AreYouOk selector if user is Dev.
    let areYouOkSelectorVisible = await SessionService.isDevSession();
    console.log("Are you ok visibile? " + areYouOkSelectorVisible);
    this.setState({areYouOkSelectorVisible});
  };

  _handleEstasBienButtonPress = async () => {
    let targetUserId = this.state.targetUserId;
    if (!targetUserId || isNaN(targetUserId)) {
      let errMsg = `Debe ingresar un user id valido: ${targetUserId}.`;
      Alert.alert("Error", errMsg);
      return;
    }

    try {
      await AreYouOkService.ask({id: targetUserId});
    } catch (e) {
      let errMsg = `Problema al preguntar Estas Bien al user: ${targetUserId}. `;
      console.log(errMsg, e);
      Alert.alert("Error", errMsg);
      return;
    }
    Alert.alert("Aviso", `Le preguntaste Estas Bien? al user ${targetUserId}`);
  };

  renderEstasBienSelector() {
    return this.state.areYouOkSelectorVisible &&
      (<View style={{flexDirection: "row"}}>
        <TextInput
          placeholder="User Id"
          style={{flex: 1}}
          onChangeText={this._handleUserIdChange}
        />
        <Button
          title="Estas Bien?"
          style={{flex: 1}}
          onPress={this._handleEstasBienButtonPress}
        />
      </View>)
  }

  render() {
    return (
      <View style={{flex: 1}}>
        <ScrollView style={{flex: 1.8}}>
          <FamilyListContainer/>
          {
            this.renderEstasBienSelector()
          }
        </ScrollView>
      </View>
    )
  }
}
