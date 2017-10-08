import React, {Component} from 'react';
import SessionService from '../service/SessionService';
import LoadingIndicator from "../component/LoadingIndicator";

/**
 * Dummy screen that is aware of navigation.
 * Will dispatch navigation actions based on session or notifications.
 *
 */
export default class RootDispatcher extends Component {

  componentWillMount = async () => {
    let sessionAlive = await SessionService.isSessionAlive();
    console.log("[RootDispatcher] session alive?", sessionAlive);

    if (sessionAlive) {
      this.props.navigation.navigate("PostLogin");
    } else {
      this.props.navigation.navigate("PreLogin");
    }
  };

  render() {
    return (
      <LoadingIndicator size="large"/>
    )
  }
}
