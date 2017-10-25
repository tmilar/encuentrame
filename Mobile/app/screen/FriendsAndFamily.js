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

  /**
   * Store all user account references for auto-complete functionality
   *
   * @type {Array}
   */
  userAccounts = [];

  _handleUserIdChange = (inputValue) => {
    this.setState({targetUserId: inputValue})
  };

  componentWillMount = async () => {
    // Load in shared storage all user accounts
    this.userAccounts = await AccountsService.getAllUserAccounts();

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

  _handleNuevoContactoButtonpress = async () => {
    const {navigate} = this.props.navigation;
    navigate('NewContact', {accounts: this.userAccounts});
  };

  render() {
    return (
      <View style={{flex: 1}}>
        <ScrollView style={{flex: 5.5}}>
          <FamilyListContainer/>
          {
            this.renderEstasBienSelector()
          }
        </ScrollView>
        <View style={{ flex: 1, justifyContent: "space-around" }}>
          <Button
            style={{width: 100, height: 50}}
            title="Agregar contacto"
            onPress={this._handleNuevoContactoButtonpress}
          />
        </View>
      </View>
    )
  }
}
