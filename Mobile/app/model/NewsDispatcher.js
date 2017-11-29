import React from 'react';
import NewsService from '../service/NewsService';
import {showToast} from "react-native-notifyer";
import {Alert} from "react-native";
import {Ionicons, EvilIcons, FontAwesome, MaterialCommunityIcons} from '@expo/vector-icons';
import {prettyDate} from "../util/prettyDate";

const NewsTypes = {
  "Areyouok.Ask": {
    dispatch: (navigation, newsData, newsId) => {
      console.log(`[PushNotificationService] Navigating to 'AreYouOk' screen.`);
      navigation.navigate("AreYouOk", {newsId});
    },
    display: {
      text: (data, response) => {
        let responded = "";
        if (response !== undefined) {
          responded = `. Respondiste que ${response.replied ? 'sí' : 'no'}`
        }
        return `Te preguntaron si estabas bien${responded}`;
      },
      icon: () => <EvilIcons name={'question'} style={{color: 'orange'}} size={30}/>
    },
    hasAction: true
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
      text: ({Ok, TargetUserId}) => `Usuario ${TargetUserId} indicó que ${Ok ? "está bien" : "necesita ayuda"}`,
      icon: ({Ok, TargetUserId}) => <Ionicons name={ Ok ? 'md-happy' : 'ios-sad'} style={{color: Ok ? 'green' : 'red'}}
                                              size={30}/>
    },
    hasAction: false
  },
  "Contact.Request": {
    dispatch: (navigation, newsData, newsId) => {
      let params = {
        contactRequestUserId: newsData.UserId,
        contactRequestUsername: newsData.Username,
        newsId: newsId
      };
      console.log(`[PushNotificationService] Navigating to 'ContactRequest' screen.`, params);
      navigation.navigate("ContactRequest", params);
    },
    display: {
      text: (data, yourResponse) => {
        let response = `${data.Username} te ha enviado una solicitud de contacto`;
        if (yourResponse !== undefined) {
          response = `${yourResponse.replied ? 'Aceptaste' : 'Rechazaste'} la solicitud de contacto de ${data.Username}.`
        }
        return response;
      },
      icon: () => <Ionicons name={'md-contacts'} style={{color: 'black'}} size={30}/>
    },
    hasAction: true
  },
  "Contact.Confirm": {
    dispatch: (navigation, {Username}) => {
      Alert.alert(
        "Te respondieron",
        `${Username} ha aceptado tu solicitud de contacto.`
      );
    },
    display: {
      text: ({Username}) => `${Username} ha aceptado tu solicitud de contacto`,
      icon: () => <MaterialCommunityIcons name={'account-check'} style={{color: 'black'}} size={30}/>
    },
    hasAction: false
  },
  "Event.StartCollaborativeSearch": {
    dispatch: (navigation) => {
      showToast("¡Emergencia! Por favor, ayúdanos a encontrar a estas personas.", {duration: 2500});
      navigation.navigate("Find", {emergency: true});
    },
    display: {
      text: `Se ha notificado de un incidente en el evento`,
      icon: () => <FontAwesome name={'warning'} style={{color: 'red'}} size={30}/>
    },
    hasAction: true
  },
  "default": {
    display: {
      icon: () => <Ionicons name={'md-notifications'} style={{color: 'black'}} size={30}/>
    }
  }
};

class NewsDispatcher {

  setup({navigation}) {
    this.navigation = navigation;
  }

  _getTextMessage = (type, data, resolution) => {
    let {text} = NewsTypes[type].display;
    return typeof text === 'string' ? text : text(data, resolution)
  };

  _saveNews = async (type, data) => {
    return await NewsService.saveNews({type, data})
  };

  handleNotification = async ({type, data}) => {
    if (type === "position")
      return;
    let newsData = await this._saveNews(type, data);
    NewsTypes[type].dispatch(this.navigation, newsData.data, newsData.id);
  };

  hasAction = ({type, resolution}) => {
    return !(resolution || !NewsTypes[type].hasAction || type === "position");
  };

  handleNewsAction = ({type, data, id, resolution}) => {
    if (!this.hasAction({type, resolution}))
      return;
    NewsTypes[type].dispatch(this.navigation, data, id);
  };

  getNewsData = ({type, time, data, id, resolution}) => {
    const message = this._getTextMessage(type, data, resolution);
    let Icon = NewsTypes[type].display.icon(data, resolution);
    let date = prettyDate(new Date(time));

    return {message, Icon, date, id}
  };

  resolveNews = async (newsId, resolution) => {
    return await NewsService.updateNewsResolution(newsId, resolution);
  };

  removeNews = async ({id}) => {
    return await NewsService.dismissNewsById(id);
  };
}

const newsDispatcher = new NewsDispatcher();
export default newsDispatcher;
