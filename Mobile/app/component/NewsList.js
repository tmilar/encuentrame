import React, {Component} from 'react';
import {Card} from 'react-native-elements';
import {Alert, ListView, ScrollView, Text, TouchableHighlight, TouchableNativeFeedback, View} from "react-native";
import NewsDispatcher from '../model/NewsDispatcher';
import Swipeout from "react-native-swipeout";
import Ionicons from "@expo/vector-icons/Ionicons";
import LoadingIndicator from "./LoadingIndicator";
import {showToast} from "react-native-notifyer";

export default class NewsList extends Component {

  datasource = new ListView.DataSource({
    rowHasChanged: (r1, r2) => r1 !== r2,
  });

  state = {
    loading: false
  };


  renderEmptyNewsListMessage = () => {
    return <View style={{flex: 1, alignItems: "center", justifyContent: "center"}}>
      <Text style={{textAlign: "center"}} note>
        {"No hay novedades."}
      </Text>
    </View>;
  };

  _handleNewsPress = (news) => {
    NewsDispatcher.handleNewsAction(news);
  };

  _handleDeleteNews = async (news) => {
    Alert.alert(
      'Confirma',
      '¿Eliminar noticia?',
      [
        {
          text: 'Cancelar', onPress: () => {
        }, style: 'cancel'
        },
        {
          text: 'Confirmar', onPress: async () => {
          await this._deleteNews(news)
        }
        },
      ],
      {cancelable: false}
    )
  };

  _deleteNews = async (news) => {
    try {
      this.setState({loading: true});
      await NewsDispatcher.removeNews(news);
    } catch (e) {
      console.log("Error deleting news from local storage: ", e);
      Alert.alert(
        'Ocurrió un problema al eliminar la noticia. ',
        e.message || e
      );
    } finally {
      this.setState({loading: false});
    }
  };


  renderNewsListItem = (newsItem) => {
    let {message, Icon, date} = NewsDispatcher.getNewsData(newsItem);
    let hasAction = NewsDispatcher.hasAction(newsItem);

    let DeleteButton = <View style={{flex: 1, justifyContent: "space-around", alignItems: "center"}}>
      <Text style={{
        justifyContent: "space-around",
        alignItems: "center",
        fontSize: 12,
        fontWeight: 'bold',
        color: "white"
      }}>
        Borrar
      </Text>
    </View>;

    let deleteBtn = hasAction ? [] : [{
      text: 'Eliminar',
      width: 60,
      styleButton: {width: 60, height: 50},
      component: DeleteButton,
      backgroundColor: 'red',
      underlayColor: 'rgba(0, 0, 0, 1, 0.6)',
      onPress: async () => {
        await this._handleDeleteNews(newsItem)
      }
    }];

    let ActionButton = hasAction ?
      <TouchableNativeFeedback style={{justifyContent: "space-around", alignItems: 'center'}}
                               onPress={() => this._handleNewsPress(newsItem)}
                               background={TouchableNativeFeedback.SelectableBackground()}>
        <View style={{justifyContent: "space-around", alignItems: 'center', width: 60, height: 80}}>
          <Ionicons name={'ios-arrow-forward'} style={{color: 'black'}} size={30}/>
        </View>
      </TouchableNativeFeedback>
      :
      <View style={{width: 60}}/>;

    return <View style={{flex: 1, height: 80, backgroundColor: 'white', marginBottom: 10}}>
      <Swipeout right={deleteBtn} backgroundColor='transparent'>
        <View style={{
          flexDirection: 'row',
          height: 80,
          justifyContent: "space-around"
        }}>
          <View style={{justifyContent: "space-around", width: 40, alignItems: 'center'}}>
            {Icon}
          </View>
          <View style={{justifyContent: "space-around", width: 260, marginLeft: 10}}>
            <Text style={{fontSize: 12, fontWeight: 'bold'}}>
              {`${message}.  ${date}`}
            </Text>
          </View>
          {ActionButton}
        </View>
      </Swipeout>
    </View>;
  };

  render() {
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }
    if (this.props.news.length === 0) {
      return this.renderEmptyNewsListMessage();
    }
    this.news = this.datasource.cloneWithRows(this.props.news);
    return <View style={{flex: 1}}>
      <ScrollView scrollsToTop={false} style={{flex: 1}}>
        <View style={{flex: 1, flexDirection: 'column', justifyContent: 'flex-start', alignItems: "center"}}>
          <ListView style={{flex: 1}}
                    dataSource={this.news}
                    renderRow={this.renderNewsListItem}
                    enableEmptySections={true}
          />
        </View>
      </ScrollView>
    </View>;
  }
}
