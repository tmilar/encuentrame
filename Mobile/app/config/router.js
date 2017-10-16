import React from 'react'

import {StackNavigator, TabNavigator, NavigationActions, DrawerNavigator} from 'react-navigation';
import {TouchableHighlight, View} from "react-native";
import {Icon} from 'react-native-elements';

import Login from '../screen/Login';
import Register from '../screen/Register';
import Home from "../screen/Home";
import AreYouOk from "../screen/AreYouOk";
import Find from "../screen/Find";
import FriendsAndFamily from "../screen/FriendsAndFamily";
import NewActivity from "../screen/NewActivity";
import SupplyInfo from "../screen/SupplyInfo";
import RootDispatcher from "../screen/RootDispatcher";


const Tabs = TabNavigator({
  Home: {
    screen: Home,
    navigationOptions: {
      tabBarLabel: 'Home',
      tabBarIcon: ({tintColor}) => <Icon name="home" size={23} color={tintColor}/>
    },
  },
  Find: {
    screen: Find,
    navigationOptions: {
      tabBarLabel: 'Encuentra',
      tabBarIcon: ({tintColor}) => <Icon name="search" size={23} color={tintColor}/>
    }
  },
  FriendsAndFamily: {
    screen: FriendsAndFamily,
    navigationOptions: {
      tabBarLabel: 'Familia/Amigos',
      tabBarIcon: ({tintColor}) => <Icon name="account-circle" size={23} color={tintColor}/>
    },
  },
}, {
  tabBarOptions: {
    activeTintColor: '#2962FF',
    animationEnabled: true,
    showIcon: true,
  },
  tabBarPosition: 'bottom'
});
Tabs.navigationOptions = {
  drawerLabel: 'Encuentrame',
  drawerIcon: ({tintColor}) => (
    <Icon name="drafts" size={24} color={tintColor}/>
  ),
};

const AuthStack = StackNavigator({
  Login: {screen: Login, navigationOptions: {header: null}},
  Register: {screen: Register}
});


const EncuentrameHeaderOptions = ({navigation}) => ({
  headerTitle: "Encuentrame",
  headerLeft: <TouchableHighlight onPress={() => navigation.navigate('DrawerOpen')}>
    <View>
      <Icon name="menu" size={25}/>
    </View>
  </TouchableHighlight>,
  headerRight: <TouchableHighlight onPress={() => navigation.navigate('NewActivity')}>
    <View>
      <Icon name="people" size={25}/>
    </View>
  </TouchableHighlight>
});

/**
 * Dummy screen to reset navigator back to Login screen.
 * Then Login screen will take care of resetting session.
 */
class LogoutActionScreen extends React.Component {
  componentDidMount() {
    const resetAction = NavigationActions.reset({
      index: 0,
      actions: [
        NavigationActions.navigate({routeName: 'PreLogin', params: {logout: true}})
      ]
    });
    console.debug("[LogoutActionScreen] Dispatching Reset Action, navigate to 'PreLogin'. ", resetAction);
    this.props.navigation.dispatch(resetAction);
  }

  render() {
    return <View/>
  }
}

const AppNavigator = DrawerNavigator({
    AppTabs: {
      path: '/home',
      screen: Tabs,
      // navigationOptions: EncuentrameHeader
    },
    Logout: {
      path: '/logout',
      screen: LogoutActionScreen,
      navigationOptions: {
        drawerLabel: "Logout",
        drawerIcon: ({tintColor}) => (
          <Icon name="drafts" size={24}/>
        )
      }
    },
  }, {
    // initialRouteName: 'AppTabs',
    contentOptions: {
      activeTintColor: '#2962FF'
    },
  }
);

const BaseStack = StackNavigator({
  Root: {
    screen: RootDispatcher,
    navigationOptions: {header: null}
  },
  PreLogin: {
    screen: AuthStack,
    navigationOptions: {header: null}
  },
  PostLogin: {
    screen: AppNavigator,
    navigationOptions: EncuentrameHeaderOptions
  },
  AreYouOk: {screen: AreYouOk, navigationOptions: {header: null}},
  NewActivity: {screen: NewActivity, navigationOptions: {header: null}},
  SupplyInfo: {screen: SupplyInfo, navigationOptions: {header: null}}
},{
  initialRouteName: 'Root'
});

export const RootNavigator = BaseStack;
