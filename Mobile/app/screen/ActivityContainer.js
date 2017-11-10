import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import Activity from "./Activity";

export default class ActivityContainer extends Component {
  state = {
    activityCreated: false,
    activityDeleted: false
  };

  activityCreated = () =>  {
    this.setState({activityCreated: true});
    this.setState({activityDeleted: false});
  };

  activityDeleted = () =>  {
    this.setState({activityCreated: false});
    this.setState({activityDeleted: true});
  };

  _goBack = () => {
    this.props.navigation.goBack(null);
  };

  render() {
    return <View style={{flex: 1}}>
            <Activity key={Math.random()} goBack={this._goBack} activityCreated={this.activityCreated} activityDeleted={this.activityDeleted}/>
          </View>
   ;
  }
}
