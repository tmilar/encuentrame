import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import FamilyList from "./FamilyList";
import {familyMembers} from "../config/familyFixture";
import {text} from '../style';

class FamilyListContainer extends Component {
  state = {
    familyMembers: []
  };

  async componentWillMount() {
    let familyMembers = await this.fetchFamilyMembers();
    this.setState({familyMembers});
  }

  fetchFamilyMembers() {
    // TODO fetch familyMembers from actual remote API
    return familyMembers;
  }

  render() {
    return <View style={{flex: 1}}>
            <Text style={text.p}>
              Contactos
            </Text>
            <FamilyList familyMembers={this.state.familyMembers}/>
          </View>
   ;
  }
}

export default FamilyListContainer;
