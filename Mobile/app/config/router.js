import React from 'react'

import {DrawerNavigator, StackNavigator, TabNavigator} from 'react-navigation';
import Login from '../screen/Login';
import Register from '../screen/Register';
import {Icon} from 'react-native-elements';
import Home from "../screen/Home";
import AreYouOk from "../screen/AreYouOk";
import Find from "../screen/Find";
import FriendsAndFamily from "../screen/FriendsAndFamily";
import NewActivity from "../screen/NewActivity";
import {TouchableHighlight, View} from "react-native";


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


const EncuentrameHeader = ({navigation}) => ({
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

const AppNavigator = DrawerNavigator({
    AppTabs: {
      path: '/home',
      screen: Tabs,
      // navigationOptions: EncuentrameHeader
    },
    Logout: {
      path: '/login',
      screen: AuthStack,
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
  // TODO when navigating PreLogin - from inside app - delete the session as "Logout".
  PreLogin: {
    screen: AuthStack,
    navigationOptions: {header: null}
  },
  PostLogin: {
    screen: AppNavigator,
    navigationOptions: EncuentrameHeader
  },
  AreYouOk: {screen: AreYouOk, navigationOptions: {header: null}},
  NewActivity: {screen: NewActivity, navigationOptions: {header: null}}
});

export const Root = BaseStack;
