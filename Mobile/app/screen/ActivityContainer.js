import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import Activity from "./Activity";

export default class ActivityContainer extends Component {
  state = {
    key: Math.random()
  };

  activityCreated = () =>  {
    this.setRandomKeyUpdate();
  };
  activityDeleted = () =>  {
    this.setRandomKeyUpdate();
  };

  setRandomKeyUpdate = () =>  {
    this.setState({key: Math.random()});
  };

  _goBack = () => {
    this.props.navigation.goBack(null);
  };

  shouldComponentUpdate = (nextProps, nextState) => {
    return this.state.key !== nextState.key;
  };

  render() {
    return <View style={{flex: 1}}>
            <Activity key={this.state.key} goBack={this._goBack} activityCreated={this.activityCreated} activityDeleted={this.activityDeleted}/>
          </View>
   ;
  }
}
