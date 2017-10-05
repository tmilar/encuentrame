import React, {Component} from 'react'
import {Text, View, Alert, ScrollView} from 'react-native'
import SessionService from '../service/SessionService';
import NewsListContainer from "../component/NewsListContainer";
import {text} from '../style';
import PositionTrackingService from '../service/PositionTrackingService';

import {Icon} from 'react-native-elements';
import ActionButton from "react-native-action-button";

export default class Home extends Component {

  constructor(props) {
    super(props);
    this.onPressTitle = this.onPressTitle.bind(this);
  }

  async componentWillMount() {
    let sessionAlive = await SessionService.isSessionAlive();
    if (!sessionAlive) {
      const {navigate} = this.props.navigation;
      navigate('Login');
    }
  }

  componentDidMount = () => {
    PositionTrackingService.setupPositionTracking();
  };


  onPressTitle() {
    const {navigate} = this.props.navigation;
    navigate('AreYouOk');
  }

  render() {
    return (
      <View style={{flex: 1}}>
        <ScrollView style={{flex: 1.8}}>
          <Text style={text.p} onPress={this.onPressTitle}>
            Home
          </Text>
          <NewsListContainer/>
        </ScrollView>
        {/*<ActionButton style={{flex: 0.2}}
                      buttonColor="rgba(231,76,60,1)"
                      onPress={() => Alert.alert("Seguime", "Seguimiento activado \uD83D\uDE00")}
                      icon={(<Icon name="person-pin-circle" color="white" size={26}/>)}>
        </ActionButton>*/}
      </View>
    )
  }
}
