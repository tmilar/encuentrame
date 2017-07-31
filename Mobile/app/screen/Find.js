import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput} from 'react-native'

export default class Find extends Component {
  render() {
    return (
      <View style={styles.container}>
        <Text style={styles.paragraph}>
          Bienvenido al Encuentra!
        </Text>
      </View>
    )
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  paragraph: {
    margin: 24,
    fontSize: 18,
    textAlign: 'center',
    color: '#34495e'
  }
});
