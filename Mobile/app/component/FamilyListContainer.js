import React, {Component} from 'react';
import {Text, View, StyleSheet} from 'react-native'
import FamilyList from "./FamilyList";

class FamilyListContainer extends Component {
  state = {
    familyMembers: []
  };

  constructor(props) {
    super(props);
    this.state = {
      familyMembers: this.props.familyMembers
    }
  }

  render() {
    return <View style={{flex: 1}}>
            <Text style={styles.paragraph}>
              Familia
            </Text>
            <FamilyList familyMembers={this.state.familyMembers}/>
          </View>
   ;
  }
}
const styles = StyleSheet.create({
  paragraph: {
    margin: 24,
    fontSize: 18,
    textAlign: 'center',
    color: '#34495e'
  }
});

export default FamilyListContainer;
