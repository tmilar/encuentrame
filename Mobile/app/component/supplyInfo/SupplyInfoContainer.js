import React, {Component} from 'react'
import TabProgressTracker from "./TabProgressTracker";
import {View, StyleSheet} from "react-native";

export default class SupplyInfoContainer extends Component {

  state = {
    whenAnswer: null,
    personStateAnswer: null,
    whereAnswer: null
  };

  static navigationOptions = {
    title: "Por favor, ayuda aportando datos"
  };

  /**
   * Questions to show for supply info.
   * //TODO : bind answer values/structure to latest server API (after v0.92).
   *
   * @type {[*]}
   */
  questions = [
    {
      text: "¿Cuándo?",
      answers: [{
        text: "Recién",
      }, {
        text: "Hace un rato\n(10 a 30 minutos)"
      }, {
        text: "Hace bastante\n(más de media hora)"
      }],
      onAnswer: (option) => {
        this.setState({whenAnswer: option});
      }
    }, {
      text: "¿Bien o Mal?",
      answers: [{
        text: "Bien \ud83d\udc4d"
      }, {
        text: "Necesitaba Ayuda \u{26A0} \u{1F198}"
      }],
      onAnswer: (option) => {
        this.setState({personStateAnswer: option});
      }
    }, {
      text: "¿Dónde?",
      answers: [{
        text: "Cerca mio (indicar en el mapa)"
      }, {
        text: "No me acuerdo"
      }],
      onAnswer: (option) => {
        this.setState({whereAnswer: option});
      }
    }
  ];

  render() {
    return <View style={styles.container}>
      <TabProgressTracker/>
    </View>;
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#D2D100', // '#D2D100', //#ecf0f1',
  }
});
