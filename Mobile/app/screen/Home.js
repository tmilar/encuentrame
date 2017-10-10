import React, {Component} from 'react'
import {Text, View, Alert, ScrollView} from 'react-native'
import SessionService from '../service/SessionService';
import NewsListContainer from "../component/NewsListContainer";
import {text} from '../style';
import PositionTrackingService from '../service/PositionTrackingService';

import {showToast} from 'react-native-notifyer';
import {Icon} from 'react-native-elements';
import ActionButton from "react-native-action-button";

export default class Home extends Component {

  state = {
    trackingEnabled: false
  };

  constructor(props) {
    super(props);
    this.onPressTitle = this.onPressTitle.bind(this);
  }

  async componentWillMount() {
    let sessionAlive = await SessionService.isSessionAlive();
    if (!sessionAlive) {
      showToast("La sesión ha caducado. Por favor, vuelva a iniciar sesión.");
      this.props.navigation.navigate('Logout');
    }
  }

  componentDidMount = async () => {
    await this._refreshPositionTracking();
  };

  _refreshPositionTracking = async () => {
    let trackingEnabled = await PositionTrackingService.refreshPositionTracking();
    this.setState({trackingEnabled});
  };

  onPressTitle() {
    const {navigate} = this.props.navigation;
    navigate('AreYouOk');
  }

  onPressTrackToggle = async () => {
    let prevTrackingEnabled = this.state.trackingEnabled;
    let alertMsg;

    let trackingEnabled = await PositionTrackingService.togglePositionTracking();

    if (prevTrackingEnabled === trackingEnabled) {
      alertMsg = `Ocurrió un problema, no se pudo ${prevTrackingEnabled ? "desactivar" : "activar"} el Seguimiento.`;
    } else if (trackingEnabled) {
      alertMsg = "Segumiento desactivado. \nRecuerda activarlo cuando quieras que te podamos cuidar!";
    } else {
      alertMsg = "Seguimiento activado \uD83D\uDE00";
    }

    this.setState({trackingEnabled});
    Alert.alert("Seguime", alertMsg);
  };

  render() {
    let enabledGreenColor = "rgba(76,231,60,1)";
    let disabledRedColor = "rgba(231,76,60,1)";

    return (
      <View style={{flex: 1}}>
        <ScrollView style={{flex: 1.8}}>
          <Text style={text.p} onPress={this.onPressTitle}>
            Home
          </Text>
          <NewsListContainer/>
        </ScrollView>
        <ActionButton style={{flex: 0.2}}
                      buttonColor={this.state.trackingEnabled ? enabledGreenColor : disabledRedColor}
                      onPress={this.onPressTrackToggle}
                      icon={(<Icon name="person-pin-circle" color="white" size={26}/>)}
        />
      </View>
    )
  }
}
