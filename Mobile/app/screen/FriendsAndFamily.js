import React, {Component} from 'react'
import {View, StyleSheet, ScrollView} from 'react-native'
import FamilyListContainer from "../component/FamilyListContainer";


export default class FriendsAndFamily extends Component {

  render() {
    return (
      <View style={{flex: 1}}>
        <ScrollView style={{flex: 1.8}}>
          <FamilyListContainer/>
        </ScrollView>
      </View>
    )
  }
}
