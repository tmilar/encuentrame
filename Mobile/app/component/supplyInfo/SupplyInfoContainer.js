import React, {Component} from 'react'
import TabProgressTracker from "./TabProgressTracker";
import {View, StyleSheet} from "react-native";

export default class SupplyInfoContainer extends Component {

  state = {};

  static navigationOptions = {
    title: "Por favor, ayuda aportando datos"
  };

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
