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

  async _handleEstasBienButtonPress() {
    let targetUserId = this.personProps.Id;
    await AreYouOkService.ask({id: targetUserId});
    Alert.alert("Aviso", `Le preguntaste ¿Estás Bien? al user ${targetUserId}`);
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

  _getRowAction() {
    const contactActions = <TouchableHighlight style={{justifyContent: 'space-around',width: 80, height: 80}} onPress={this._handleEstasBienButtonPress}>
      <View style={{justifyContent: 'space-around',alignItems: 'center',width: 80, height: 80}}>
        <View style={{borderRadius: 40, alignItems: 'center', justifyContent: 'space-around',width: 50, height: 50 , backgroundColor: '#3DB097', borderWidth: 1, borderColor: 'white'}}>
          <Icon name="location-searching" size={35} color='white'/>
        </View>
      </View>
    </TouchableHighlight>;

    const pendingContactMessage = <View style={{justifyContent: 'space-around',width: 80, height: 80, }}>
      <View style={{justifyContent: 'space-around',width: 80, height: 60 }}>
        <Text style={{textAlign: 'center', fontSize: 12}}>Solicitud enviada</Text>
      </View>
    </View>;

    if (this.pending) {
      return pendingContactMessage;
    }
    return contactActions;
  }

  render() {
    //let bgColor = this.getStateColor(personProps);
    return (
      <Card styles={{flex: 1, card: {}}}>
        <View style={{
          flexDirection: 'row',
          height: 90,
          flex: 1
        }}>
          <View style={{justifyContent: 'space-around',width: 60, height: 90 }}>
            <Text style={{textAlign: 'left', fontWeight: 'bold', fontSize: 12}}>{this.personProps.Username}</Text>
          </View>
          <View style={{justifyContent: 'center', alignItems: 'center', width: 200, height: 90}}>
            <Image source={{ uri: this.personProps.imageUri }} style={{justifyContent: 'space-around', width: 75, height: 75 }} />
          </View>
          {this._getRowAction()}
        </View>
      </Card>
    );
  }
}
