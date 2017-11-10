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
    soughtPeople: null
  };

  componentDidMount = async () => {
    // TODO get ACTUAL soughtPeople fixtures when backend is working.
    let soughtPeople = await SoughtPeopleService.getSoughtPeople();
    let debuggingPeople = soughtPeople.map(p => ({
      ...(p.User), Distance: p.Distance
    }));
    console.table(debuggingPeople);
    this.setState({soughtPeople});
  };

  /**
   * Send sought person suppliedInfo to server.
   * @returns {Promise.<void>}
   */
  handleIveSeenHimSubmit = async (soughtPerson, suppliedInfo) => {
    showToast("Aportando datos...", {duration: 2000});
    let {soughtPersonId} = soughtPerson;
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

  handleIveSeenHimCancel = (soughtPerson) => {
  };

  handleIveNotSeenHim = async (soughtPerson) => {
    let {soughtPersonId} = soughtPerson;
    try {
      console.log("Quitando persona...", soughtPerson);
      await SoughtPeopleService.soughtPersonDismiss(soughtPersonId);
    } catch (e) {
      console.error("Error al indicar que no lo has visto: ", e);
      this._pushSoughtPerson(soughtPerson);
      Alert.alert("Error", "¡Ups! Ocurrió un error al indicar que no lo has visto. \n" + (e.message || e));
      return;
    }
  };

  render() {
    if (!this.state.soughtPeople) {
      return <LoadingIndicator text={"Cargando..."}/>
    }

    return <SoughtPeopleDeckSwiper
      soughtPeople={this.state.soughtPeople}
      navigation={this.props.navigation}
      onIveSeenHimSubmit={this.handleIveSeenHimSubmit}
      onIveSeenHimCancel={this.handleIveSeenHimCancel}
      onIveNotSeenHim={this.handleIveNotSeenHim}
    />;
  }
}
