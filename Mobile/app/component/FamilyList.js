import React, {Component} from 'react'
import {StyleSheet, View} from "react-native";
import FamilyCard from "./FamilyCard";

export default class Home extends Component {
  constructor(props) {
    super(props);
    this.state = {
      familyMembers: this.props.familyMembers
    }
  }


  render() {
    return (
      <View>
        {this.props.familyMembers.map((person, i) => {
          return (
            <FamilyCard key = {i} style={styles.familyCard} personProps={person}>
            </FamilyCard>
          )
        })}
      </View>
    )
  }
}

const styles = StyleSheet.create({
  familyCard: {
    flex: 0.8
  }
});
