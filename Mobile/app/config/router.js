import React from 'react'

import {StackNavigator, TabNavigator} from 'react-navigation';
import Login from '../screen/Login';
import Register from '../screen/Register';
import {Icon} from 'react-native-elements';
import Home from "../screen/Home";
import Find from "../screen/Find";
import Me from "../screen/Me";

export const Tabs = TabNavigator({
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
  Me: {
    screen: Me,
    navigationOptions: {
      tabBarLabel: 'Perfil',
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

export const Root = StackNavigator({
  Login: {screen: Login, navigationOptions: {header: null}},
  Register: {screen: Register},
  PostLogin: {screen: Tabs, navigationOptions: {header: null}}
});
