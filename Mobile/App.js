import React, {Component} from 'react';
import {View, ActivityIndicator} from "react-native";
import {RootNavigator} from './app/config/router';
import {containers} from './app/style';
import SessionService from './app/service/SessionService';

class App extends Component {

  async componentWillMount() {
    let sessionAlive = await SessionService.isSessionAlive();
    console.log("[App] session alive?", sessionAlive);
    this.setState({sessionAlive});
  };

  render = () => {
    if(!this.state || this.state.sessionAlive === undefined) {
      return (<ActivityIndicator size="small"/>);
    }

    const Navigator = RootNavigator(this.state.sessionAlive);

    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <Navigator/>
      </View>
    )
  }
}

export default App;
