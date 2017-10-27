import React, {Component} from 'react'
import {View, StyleSheet} from 'react-native'
import Swiper from "react-native-deck-swiper/Swiper";
import EncuentraCard from "../component/EncuentraCard";
import {containers} from '../style';
import SoughtPeopleContainer from "../component/SoughtPeopleContainer";


export default class Find extends Component {

  constructor(props) {
    super(props);
  }

  render() {
    return (
      <SoughtPeopleContainer navigation={this.props.navigation}/>
    )
  }
}
