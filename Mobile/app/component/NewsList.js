import React, {Component} from 'react';
import {Card} from 'react-native-elements';
import {ListView, ScrollView, Text, TouchableHighlight, TouchableNativeFeedback, View} from "react-native";
import NewsDispatcher from '../model/NewsDispatcher';

export default class NewsList extends Component {

  datasource = new ListView.DataSource({
    rowHasChanged: (r1, r2) => r1 !== r2,
  });


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

  renderNewsListItem = (news) => {
    let {message, Icon, date} = NewsDispatcher.getNewsData(news);
    let myView = <TouchableNativeFeedback style={{flex: 1}} onPress={() => this._handleNewsPress(news)}
                               background={TouchableNativeFeedback.SelectableBackground()}>
        <Card containerStyle={{padding: 5}}>
          <View style={{
            flex: 1,
            flexDirection: 'row',
            height: 60,
            justifyContent: "space-around"
          }}>
            <View style={{justifyContent: "space-around", width: 60, alignItems: 'center'}}>
              {Icon}
            </View>
            <View style={{justifyContent: "space-around", width: 300}}>
              <Text style={{fontSize: 14, fontWeight: 'bold'}}>
                {`${message}.  ${date}`}
              </Text>
            </View>
          </View>
        </Card>
      </TouchableNativeFeedback>;

    return myView;
  };

  render() {
    if (this.props.news.length === 0){
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
