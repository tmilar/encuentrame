import React, {Component} from 'react'
import {StyleSheet, View} from "react-native";
import FamilyCard from "./FamilyCard";

const styles = StyleSheet.create({
  familyCard: {
    flex: 0.8
  }
});


const FamilyList = props =>
      <View>
        {props.familyMembers.map((person, i) => {
          return (
            <FamilyCard key = {i} style={styles.familyCard} personProps={person}>
            </FamilyCard>
          )
        })}
      </View>


export default FamilyList;
