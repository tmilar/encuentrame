import React from 'react';
import NewsService from '../service/NewsService';
import {showToast} from "react-native-notifyer";
import {Alert} from "react-native";
import {Ionicons, EvilIcons, FontAwesome, MaterialCommunityIcons} from '@expo/vector-icons';

// {onReceive {type, data, navigate}, displayComponent {text, iconComponent}}

const NewsTypes = {
  "Areyouok.Ask": {
    dispatch: (navigation) => {
      console.log(`[PushNotificationService] Navigating to 'AreYouOk' screen.`);
      navigation.navigate("AreYouOk");
    },
    display: {
      text: `Te preguntaron si estabas bien.`,
      icon: () => <EvilIcons name={'question'} style={{color: 'orange'}} size={40}/>
    },
  },
  "Areyouok.Reply": {
    dispatch: (navigation, {Ok, TargetUserId}) => {
      if (Ok === undefined || TargetUserId === undefined) {
        throw 'Javi probablemente cambio la data a "data.ok" y "data.targetUserId" para la push..'
      }
      console.log(`[PushNotificationService] Showing Areyouok response.`);
      Alert.alert(
        "Te respondieron: Estás Bien?",
        `{usuario ${TargetUserId}} indicó que ${Ok ? " está bien. " : " necesita ayuda."}`
      );
    },
    display: {
      text: ({Ok, TargetUserId}) => `{usuario ${TargetUserId}} indico que ${Ok ? " está bien. " : " necesita ayuda."}`,
      icon: () => <Ionicons name={'md-happy'} style={{color: 'green'}} size={40}/>
    },
  },
  "Contact.Request": {
    dispatch: (navigation, {UserId, Username}) => {
      let contactRequestParams = {
        contactRequestUserId: UserId,
        contactRequestUsername: Username
      };
      console.log(`[PushNotificationService] Navigating to 'ContactRequest' screen.`, contactRequestParams);
      navigation.navigate("ContactRequest", contactRequestParams);
    },
    display: {
      text: ({Username}) => `${Username} te ha enviado una solicitud de contacto.`,
      icon: () => <Ionicons name={'md-contacts'} style={{color: 'black'}} size={40}/>
    },
  },
  "Contact.Confirm": {
    dispatch: (navigation, {Username}) => {
      Alert.alert(
        "Te respondieron",
        `${Username} ha aceptado tu solicitud de contacto.`
      );
    },
    display: {
      text: ({Username}) => `${Username} ha aceptado tu solicitud de contacto.`,
      icon: () => <MaterialCommunityIcons name={'account-check'} style={{color: 'black'}} size={40}/>
    }
  },
  "Event/StartCollaborativeSearch": {
    dispatch: (navigation) => {
      showToast("¡Emergencia! Ayúdanos a encontrar a algunas personas.", {duration: 2500});
      navigation.navigate("Find", {emergency: true});
    },
    display: {
      text: `Se ha notificado de una emergencia.`,
      icon: () => <FontAwesome name={'warning'} style={{color: 'red'}} size={40}/>
    }
  },
  "default": {
    display: {
      icon: () => <Ionicons name={'md-notifications'} style={{color: 'black'}} size={40}/>
    }
  }
};

class NewsDispatcher {

  setup({navigation}) {
    this.navigation = navigation;
  }

  _getTextMessage = (type, data) => {
    let {text} = NewsTypes[type].display;
    return typeof text === 'string' ? text : text(data)
  };

  _saveNews = async (type, data) => {
    const message = this._getTextMessage(type, data);
    await NewsService.saveNews({type, message})
  };

  handleNotification = async ({type, data}) => {
    await this._saveNews(type, data);
    NewsTypes[type].dispatch(this.navigation, data);
  };

  getDisplayData = ({type, data}) => {
    let text = this._getTextMessage(type, data);
    let Icon = NewsTypes[type].display.icon;

    return {text, Icon}
  };
}

const newsDispatcher = new NewsDispatcher();
export default newsDispatcher;
