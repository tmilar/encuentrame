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
    if (!person.estado.estaBien.consultado){
      return "ok";
    }
    if (person.estado.estaBien.respuesta === false){
      return "no"
    }
    if (person.estado.estaBien.respuesta === null){
      if (person.estado.busquedaColaborativa.activa === false){
        //si la busqueda colaborativa esta en false, significara que ya fue cancelanda/cerrada, la persona esta Ok?
        //TODO: cuando se cancela la busqueda colaborativa, deberia indicarse que la persona esta bien a la app central?
        return "ok";
      } else {
        //si la busqueda colaborativa esta en null es que no se ha iniciado, y en true significaria que si. En ambos casos es estado unknown
        return "unknown";
      }
    }
  }
  getStateColor(person) {
    let state = this.analyzeState(person);
    switch(state) {
      case "ok":
        return "green";
        break;
      case "no":
        return "red";
        break;
      default:
        return "white";
    }
  }

  render () {
    return (
      <Card containerStyle={{flex: 1, padding: 5, backgroundColor: this.getStateColor(this.state.personProps)}}>
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
