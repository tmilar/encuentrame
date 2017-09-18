import React, {Component} from 'react';
import {Root} from './app/config/router';
import {View} from "react-native";
import {containers} from './app/style';

class App extends Component {

  async componentWillMount() {
    await Expo.Font.loadAsync({
      'Roboto': require('native-base/Fonts/Roboto.ttf'),
      'Roboto_medium': require('native-base/Fonts/Roboto_medium.ttf'),
    });
  }

  render() {
    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <Root />
      </View>
    )
  }
}

export default App;
