import React, {Component} from 'react';
import SoughtPeopleDeckSwiper from './SoughtPeopleDeckSwiper';
import LoadingIndicator from "./LoadingIndicator";
import {soughtPeople} from "../config/soughtPeopleFixture";
import SoughtPeopleService from "../service/SoughtPeopleService";

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

  _onSupplySoughtPersonInfo = async (soughtPersonId, info) => {
    await SoughtPeopleService.soughtPersonSupplyInfo(soughtPersonId, info);
  };

  _onDismissSoughtPerson = async (soughtPersonId) => {
    await SoughtPeopleService.soughtPersonDismiss(soughtPersonId);
  };

  render() {
    if (!this.state.soughtPeople || !this.state.soughtPeople.length) {
      return <LoadingIndicator/>
    }

    return <SoughtPeopleDeckSwiper
      soughtPeople={this.state.soughtPeople}
      onSupplySoughtPersonInfo={this._onSupplySoughtPersonInfo}
      onDismissSoughtPerson={this._onDismissSoughtPerson}
      navigation={this.props.navigation}
    />;
  }

}
