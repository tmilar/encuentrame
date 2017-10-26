import React, {Component} from 'react';
import SoughtPeopleDeckSwiper from './SoughtPeopleDeckSwiper';
import LoadingIndicator from "./LoadingIndicator";
import {soughtPeople} from "../config/soughtPeopleFixture";


export default class SoughtPeopleContainer extends Component {

  soughtPeople = [];

  componentWillMount = async () => {
    // TODO remove soughtPeople fixtures when backend is working.
    this.soughtPeople = soughtPeople;
  };

  render() {
    if (!this.soughtPeople) {
      return <LoadingIndicator/>
    }

    return <SoughtPeopleDeckSwiper soughtPeople={this.soughtPeople}/>;
  }

}
