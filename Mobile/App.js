import React, {Component} from 'react';
import {RootNavigator} from './app/config/router';
import {View} from "react-native";
import {containers} from './app/style';
import AppLoading from "expo/src/AppLoading";

class App extends Component {

  state = {
    isReady: false
  };

  _loadFontsAsync = async () => {
    await Expo.Font.loadAsync({
      'Roboto': require('native-base/Fonts/Roboto.ttf'),
      'Roboto_medium': require('native-base/Fonts/Roboto_medium.ttf'),
    });
  };

  render() {
    if(!this.state.isReady) {
      return <AppLoading
        startAsync={this._loadFontsAsync}
        onFinish={() => this.setState({ isReady: true })}
        onError={console.warn}
      />
    }

    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <RootNavigator/>
      </View>
    )
  }
}

export default App;
