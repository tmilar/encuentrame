import React from 'react'

import {StackNavigator, TabNavigator, NavigationActions, DrawerNavigator} from 'react-navigation';
import {TouchableHighlight, View} from "react-native";
import {Icon} from 'react-native-elements';

import Login from '../screen/Login';
import Register from '../screen/Register';
import Home from "../screen/Home";
import AreYouOk from "../screen/AreYouOk";
import ContactRequest from "../screen/ContactRequest";
import Find from "../screen/Find";
import FriendsAndFamily from "../screen/FriendsAndFamily";
import ActivityContainer from "../screen/ActivityContainer";
import NewContact from "../screen/NewContact";
import UserProfile from "../screen/UserProfile";
import RootDispatcher from "../screen/RootDispatcher";
import SupplyInfoContainer from "../component/supplyInfo/SupplyInfoContainer";

import UserService from '../service/UserService';
import {hideLoading, showLoading, showToast} from "react-native-notifyer";

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
  ActivityContainer: {
    screen: ActivityContainer,
    navigationOptions: {
      tabBarLabel: 'Eventos',
      tabBarIcon: ({tintColor}) => <Icon name="account-circle" size={23} color={tintColor}/>
    },
  },
}, {
  tabBarOptions: {
    activeTintColor: 'white',
    animationEnabled: true,
    showIcon: true,
    inactiveBackgroundColor: '#3CB393',
    inactiveTintColor: '#063450',
    style: {
      backgroundColor: '#3CB393'
    }
  },
  tabBarPosition: 'bottom',
  swipeEnabled: false
});

const AuthStack = StackNavigator({
  Login: {screen: Login, navigationOptions: {header: null}},
  Register: {screen: Register}
});


let drawerOpened = false;
const drawerToggle = (navigation) => {
  let toggle = drawerOpened ? 'DrawerClose' : 'DrawerOpen';
  navigation.navigate(toggle);
  drawerOpened = !drawerOpened;
};

const EncuentrameHeaderOptions = ({navigation}) => ({
  headerTitle: "Encuentrame",
  headerLeft: <TouchableHighlight onPress={() => drawerToggle(navigation)}>
    <View>
      <Icon name="menu" size={25}/>
    </View>
  </TouchableHighlight>,
  headerRight: <TouchableHighlight onPress={() => navigation.navigate('FriendsAndFamily')}>
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
  componentDidMount = async () => {
    // POST logout request. Don't await, since the reply doesn't matter.
    showLoading("Cerrando sesión");
    try {
      await UserService.postLogoutRequest();
      console.log("Logout OK!");
      hideLoading();
    } catch (e) {
      hideLoading();
      console.error("Error al enviar los datos", e);
      showToast("Error al cerrar sesión", {duration: 2000});
    }


    const resetAction = NavigationActions.reset({
      index: 0,
      actions: [
        NavigationActions.navigate({routeName: 'PreLogin', params: {logout: true}})
      ]
    });
    console.debug("[LogoutActionScreen] Dispatching Reset Action, navigate to 'PreLogin'. ", resetAction);
    this.props.navigation.dispatch(resetAction);

  };

  render() {
    return <View/>
  }
}

const AppNavigator = DrawerNavigator({
    Home: {
      path: '/home',
      screen: Tabs,
      // navigationOptions: EncuentrameHeader
    },
    UserProfile: {
      path: '/userProfile',
      screen: UserProfile,
      navigationOptions: {
        drawerLabel: "Mi Perfil",
        drawerIcon: ({tintColor}) => (
          <Icon name="accessibility" size={24}/>
        )
      }
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
  ContactRequest: {screen: ContactRequest, navigationOptions: {header: null}},
  FriendsAndFamily: {screen: FriendsAndFamily},
  SupplyInfo: {screen: SupplyInfoContainer},
  NewContact: {screen: NewContact}
}, {
  initialRouteName: 'Root'
});

export const RootNavigator = BaseStack;
