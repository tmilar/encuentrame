import React, {Component} from 'react'
import {View, ScrollView, TextInput, Button} from 'react-native'
import FamilyListContainer from "../component/FamilyListContainer";
import {text} from '../style';
import SessionService from "../service/SessionService";
import AreYouOkService from "../service/AreYouOkService";


import {Alert} from "react-native";


export default class FriendsAndFamily extends Component {

  state = {
    targetUserId: "",
    areYouOkSelectorVisible: false
  };

  static navigationOptions = {
    title: "Mis contactos"
  };

  _handleUserIdChange = (inputValue) => {
    this.setState({targetUserId: inputValue})
  };

  componentWillMount = async () => {
    // show AreYouOk selector if user is Dev.
    let areYouOkSelectorVisible = await SessionService.isDevSession();
    console.log("[FriendsAndFamily] Are you ok selector visibile? " + areYouOkSelectorVisible);
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
      let errMsg = `Problema al preguntar ¿Estás Bien al user: ${targetUserId}. `;
      console.log(errMsg, e);
      Alert.alert("Error", errMsg);
      return;
    }
    Alert.alert("Aviso", `Le preguntaste ¿Estás Bien? al user ${targetUserId}`);
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
          title="¿Estás Bien?"
          style={{flex: 1}}
          onPress={this._handleEstasBienButtonPress}
        />
      </View>)
  }

  _handleNuevoContactoButtonpress = async () => {
    const {navigate} = this.props.navigation;
    navigate('NewContact');
  };

  render() {
    return (
      <View style={{flex: 1}}>
        <View style={{flex: 1, justifyContent: "space-around"}}>
          <ScrollView style={{flex: 1}}>
            <FamilyListContainer/>
          </ScrollView>
          {this.renderEstasBienSelector()}
        </View>
        <View style={{margin: 5, marginBottom: 10}}>
          <Button
            style={{width: 100, height: 50}}
            title="Agregar contacto"
            onPress={this._handleNuevoContactoButtonpress}
            color="#063450"
          />
        </View>
      </View>
    )
  }
}
