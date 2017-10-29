import React, {Component} from 'react';
import {Alert, Text, Button, View, Image, TouchableHighlight} from 'react-native'
import {Icon} from 'react-native-elements';
import AreYouOkService from "../service/AreYouOkService";
import {
  Card
} from 'react-native-card-view';

export default class FamilyCard extends Component {
  constructor(props) {
    super(props);
    this.personProps = this.props.personProps.User;
    this.pending = this.props.personProps.Pending;

    this.analyzeState = this.analyzeState.bind(this);
    this.getStateColor = this.getStateColor.bind(this);
    this._handleEstasBienButtonPress = this._handleEstasBienButtonPress.bind(this);

  }

  analyzeState(person) {
    if (person.estado.greenFlag.state) {
      return "ok";
    }
    if (!person.estado.estaBien.consultado) {
      return "initial";
    }
    if (person.estado.estaBien.respuesta === false) {
      return "critical"
    }
    if (person.estado.estaBien.respuesta === null) {
      //TODO: De acuerdo al tiempo que paso desde que se disparo la pregunta, sera pending/amarillo o alert/naranja
      return "alert";
    }
  }
  async _handleEstasBienButtonPress (){
    let targetUserId = this.personProps.Id;
    await AreYouOkService.ask({id: targetUserId});
    Alert.alert("Aviso", `Le preguntaste Estas Bien? al user ${targetUserId}`);
  }

  getStateColor(person) {
    let state = this.analyzeState(person);
    switch (state) {
      case "ok":
        return "green";
        break;
      case "critical":
        return "red";
        break;
      case "initial":
        return "white";
        break;
      case "pending":
        return "yellow";
        break;
      case "alert":
        return "orange";
        break;
      default:
        return "white";
    }
  }
  _getRowAction(){
    if (this.pending){
      return <View
        style={{
          flex: 1,
          justifyContent: 'center',
          alignItems: 'center'
        }}>
        <Text style={{flex: 1, fontWeight: 'bold'}}>Solicitud pendiente</Text>
      </View>
    } else {
      return <TouchableHighlight onPress={this._handleEstasBienButtonPress}>
        <View>
          <Icon name="help" size={50} color='#43484d'/>
        </View>
      </TouchableHighlight>
    }
  }

  render() {
    //let bgColor = this.getStateColor(personProps);
    return (
      <Card styles={{card: {}}}>
        <View style={{
          flexDirection: 'row',
          height: 90,
        }}>
          <Text style={{flex: 1.5, fontWeight: 'bold'}}>{this.personProps.Username}</Text>
          <View
            style={{
              flex: 1,
              justifyContent: 'center',
              alignItems: 'center'
            }}>
            <Image source={require('../img/personImgExample.jpg')} size={5}/>
          </View>
          {this._getRowAction()}
        </View>
      </Card>
    );
  }
}
