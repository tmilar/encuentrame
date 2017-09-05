import React, {Component} from 'react';
import {Root} from './app/config/router';
import {View} from "react-native";
import {Constants} from 'expo';

class App extends Component {
  render() {
    return (
      <View style={{flex: 1, marginTop: Constants.statusBarHeight}}>
        <Root />
      </View>
    )
  }
}

export default App;
