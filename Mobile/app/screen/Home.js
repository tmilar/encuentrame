import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput, ScrollView} from 'react-native'
import SessionService from '../service/SessionService';
import NewsListContainer from "../component/NewsListContainer";
import {news} from "../config/data";

import {Icon} from 'react-native-elements';
import ActionButton from "react-native-action-button";

export default class Home extends Component {

  async componentWillMount() {
    let sessionId = await SessionService.getSessionToken();
    if (!sessionId) {
      const {navigate} = this.props.navigation;
      navigate('Login');
    }
  }

  render() {
    return (
      <View style={{flex: 1}}>
        <ScrollView style={{flex: 1.8}}>
          {/* TODO add styles.container for scroll view?*/}
          <Text style={styles.paragraph}>
            Bienvenido al Home!
          </Text>
          <NewsListContainer news={news}/>
        </ScrollView>
        <ActionButton style={{flex: 0.2}}
                      buttonColor="rgba(231,76,60,1)"
                      onPress={() => Alert.alert("Seguime", "Seguimiento activado \uD83D\uDE00")}
                      icon={(<Icon name="person-pin-circle" color="white" size={26}/>)}>
        </ActionButton>
      </View>
    )
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  paragraph: {
    margin: 24,
    fontSize: 18,
    textAlign: 'center',
    color: '#34495e'
  }
});
