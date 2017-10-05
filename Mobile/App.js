import React, {Component} from 'react';
import {Root} from './app/config/router';
import {View} from "react-native";
import {containers} from './app/style';
import PositionTrackingService from "./app/service/PositionTrackingService";

class App extends Component {

  constructor(props) {
    super(props);

    PositionTrackingService.setupPeriodicPositionReport();
  };

  render() {
    return (
      <View style={[{flex: 1}, containers.statusBar]}>
        <Root />
      </View>
    )
  }
}

export default App;
