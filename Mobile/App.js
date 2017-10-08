import React, {Component} from 'react';
import {RootNavigator} from './app/config/router';
import {View} from "react-native";
import {containers} from './app/style';
import SessionService from './app/service/SessionService';
import PushNotificationsService from './app/service/PushNotificationsService';
import LoadingIndicator from './app/component/LoadingIndicator';

class App extends Component {


  componentDidMount = async () => {
    await PushNotificationsService.registerDevice();
  };

  render() {
    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <RootNavigator/>
      </View>
    )
  }
}

export default App;
