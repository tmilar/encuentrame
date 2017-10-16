import React, {Component} from 'react';
import SessionService from '../service/SessionService';
import LoadingIndicator from "../component/LoadingIndicator";
import PushNotificationsService from '../service/PushNotificationsService';
import {NavigationActions} from "react-navigation";

/**
 * Dummy screen that is aware of navigation.
 * Will dispatch navigation actions based on session or notifications.
 *
 */
export default class RootDispatcher extends Component {

  componentWillMount = async () => {
    let sessionAlive = await SessionService.isSessionAlive();
    console.log("[RootDispatcher] session alive?", sessionAlive);

    let nextRouteName = sessionAlive ? "PostLogin" : "PreLogin";

    const resetAction = NavigationActions.reset({
      index: 0,
      actions: [
        NavigationActions.navigate({routeName: nextRouteName})
      ]
    });
    console.debug(`[RootDispatcher] Dispatching Reset Action, navigating to '${nextRouteName}'. `);

    this.props.navigation.dispatch(resetAction);
  };

  componentDidMount = async () => {
    // setup navigation-aware notifications dispatcher.
    await PushNotificationsService.setupNotificationsDispatcher(this.props.navigation);
  };

  render() {
    return (
      <LoadingIndicator size="large"/>
    )
  }
}
