import React, {Component} from 'react';
import {Root} from './app/config/router';
import {View} from "react-native";
import {containers} from './app/style';

class App extends Component {


  render() {
    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <Root />
      </View>
    )
  }
}

export default App;
