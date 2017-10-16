import React, {Component} from 'react';
import {RootNavigator} from './app/config/router';
import {View} from "react-native";
import {containers} from './app/style';

class App extends Component {


  render() {
    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <RootNavigator/>
      </View>
    )
  }
}

export default App;
