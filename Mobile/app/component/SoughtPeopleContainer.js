import React, {Component} from 'react';
import SoughtPeopleDeckSwiper from './SoughtPeopleDeckSwiper';
import LoadingIndicator from "./LoadingIndicator";
import {soughtPeople} from "../config/soughtPeopleFixture";
import SoughtPeopleService from "../service/SoughtPeopleService";
import SessionService from "../service/SessionService";
import {showToast} from "react-native-notifyer";
import {Alert} from "react-native";

export default class SoughtPeopleContainer extends Component {

  static defaultProps = {
    navigation: {}
  };

  state = {
    soughtPeople: null,
    loadingPeople: true
  };

  REFRESH_INTERVAL = 60 * 1000; // seconds to refresh soughtPeople list.

  componentDidMount = async () => {
    await this.fetchSoughtPeople();
    this.startSoughtPeopleRefresh();
  };

  fetchSoughtPeople = async () => {
    let soughtPeople = await SoughtPeopleService.getSoughtPeople();
    this.setState({soughtPeople});
    this.setState({loadingPeople: false});

    if (await SessionService.isDevSession()) {
      showToast(`Encuentra soughtPeople refreshed! (now: ${soughtPeople.length} people).`,
        {duration: 1500});
    }
  };

  startSoughtPeopleRefresh = async () => {
    this.refreshInterval = setInterval(this.fetchSoughtPeople, this.REFRESH_INTERVAL);
    if (await SessionService.isDevSession()) {
      showToast(`Encuentra soughtPeople periodic refresh, set up: each ${this.REFRESH_INTERVAL / 1000}s.`,
        {duration: 1500});
    }
  };

  componentWillUnmount = () => {
    clearInterval(this.refreshInterval);
  };

  componentWillReceiveProps = async ({emergency}) => {
    console.log(`[SoughtPeopleContainer] ${emergency ? "Emergency!" : "No emergency."}`);

    if(emergency) {
      await this.fetchSoughtPeople();
    }
  };

  /**
   * Send sought person suppliedInfo to server.
   * @returns {Promise.<void>}
   */
  handleIveSeenHimSubmit = async (soughtPerson, suppliedInfo) => {
    showToast("Aportando datos...", {duration: 2000});
    let soughtPersonId = soughtPerson.User.Id;
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
    let soughtPersonId = soughtPerson.User.Id;
    try {
      console.log("Quitando persona...", soughtPerson);
      await SoughtPeopleService.soughtPersonDismiss(soughtPersonId);
    } catch (e) {
      console.error("Error al indicar que no lo has visto: ", e);
      Alert.alert("Error", "¡Ups! Ocurrió un error al indicar que no lo has visto. \n" + (e.message || e));
      return;
    }
  };

  render() {
    if (!this.state.soughtPeople || this.state.loadingPeople) {
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
