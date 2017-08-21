import React, {Component} from 'react';
import {Text, Button, View, Image} from 'react-native'
import {
  StyleSheet
} from 'react-native';

import {
  Card,
  CardImage,
  CardTitle,
  CardContent,
  CardAction
} from 'react-native-card-view';

export default class FamilyCard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      personProps: this.props.personProps
    }
    this.analyzeState = this.analyzeState.bind(this);
    this.getStateColor = this.getStateColor.bind(this);
  }
  analyzeState(person) {
    if (person.estado.greenFlag.state){
      return "ok";
    }
    if (!person.estado.estaBien.consultado){
      return "initial";
    }
    if (person.estado.estaBien.respuesta === false){
      return "critical"
    }
    if (person.estado.estaBien.respuesta === null){
      //TODO: De acuerdo al tiempo que paso desde que se disparo la pregunta, sera pending/amarillo o alert/naranja
      return "alert";
    }
  }
  getStateColor(person) {
    let state = this.analyzeState(person);
    switch(state) {
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

  render () {
    return (
      <Card styles={{ card: { backgroundColor: this.getStateColor(this.state.personProps) }}}>
        <View style={{
          flexDirection: 'row',
          height: 60,
        }}>
          <Text style={{flex: 1,fontWeight: 'bold'}}>{this.state.personProps.name}</Text>
          <View
            style={{
              flex: 1,
              justifyContent: 'center',
              alignItems: 'center'
            }}>
            <Image source={require('../img/personImgExample.jpg')} size={50} />
          </View>

        </View>
      </Card>
    );
  }
}

const styles = StyleSheet.create({

});
