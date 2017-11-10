import React, {Component} from 'react';
import SoughtPeopleDeckSwiper from './SoughtPeopleDeckSwiper';
import LoadingIndicator from "./LoadingIndicator";
import {soughtPeople} from "../config/soughtPeopleFixture";
import SoughtPeopleService from "../service/SoughtPeopleService";
import {showToast} from "react-native-notifyer";
import {Alert} from "react-native";

export default class SoughtPeopleContainer extends Component {

  static defaultProps = {
    navigation: {}
  };

  state = {
    soughtPeople: []
  };

  componentWillMount = async () => {
    // let soughtPeople = await SoughtPeopleService.getSoughtPeople();
    // this.state.soughtPeople = soughtPeople || soughtPeopleFixture;
    // TODO remove soughtPeople fixtures when backend is working.
    this.state.soughtPeople = soughtPeople;
  };

  /**
   * Send sought person suppliedInfo to server.
   * @returns {Promise.<void>}
   */
  handleIveSeenHim = async (soughtPersonId, suppliedInfo) => {
    showToast("Aportando datos...", {duration: 2000});

    try {
      console.log("Aportando datos...", soughtPersonId, suppliedInfo);
      await SoughtPeopleService.soughtPersonSupplyInfo(soughtPersonId, suppliedInfo);
    } catch (e) {
      console.error("Error al aportar datos: ", e);
      Alert.alert("Error", "¡Ups! Ocurrió un error al aportar los datos de la persona. \n" + (e.message || e));
      return;
    }

    showToast("¡Gracias por tu ayuda!", {duration: 1000});
  };

  handleIveNotSeenHim = async (soughtPersonId) => {
    try {
      console.log("Quitando persona...", soughtPersonId);
      await SoughtPeopleService.soughtPersonDismiss(soughtPersonId);
    } catch (e) {
      console.error("Error al indicar que no lo has visto: ", e);
      Alert.alert("Error", "¡Ups! Ocurrió un error al indicar que no lo has visto. \n" + (e.message || e));
      return;
    }
  };

  render() {
    if (!this.state.soughtPeople || !this.state.soughtPeople.length) {
      return <LoadingIndicator/>
    }

    return <SoughtPeopleDeckSwiper
      soughtPeople={this.state.soughtPeople}
      navigation={this.props.navigation}
      onIveSeenHimSubmit={this.handleIveSeenHim}
      onIveNotSeenHim={this.handleIveNotSeenHim}
    />;
  }

}
