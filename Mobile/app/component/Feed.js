import React, {Component} from 'react';
import {Avatar, Button, Card, Icon, List, ListItem} from 'react-native-elements';
import {Image, Text, View, StyleSheet} from "react-native";

class Feed extends Component {
  state = {
    news: []
  };

  constructor(props) {
    super(props);
    this.state = {
      news: this.props.news
    }
  }

  render() {
    const newsItems = this.state.news.map((n, i) => {
      return (
        <Card key={i} containerStyle={{padding: 5}}>
          <View style={{
            flexDirection: 'row',
            height: 60,
          }}>
            <View
              style={{
                flex: 1,
                justifyContent: 'center',
                alignItems: 'center'
              }}>
              <Icon name={n.icon} size={50} color='#43484d'/>
            </View>
            <View style={{flex: 3, padding: 5}}>
              <Text style={{fontSize: 16}}>
                { !!n.message.started_by.length &&
                <Text style={{fontWeight: 'bold'}}>{n.message.started_by + " "}</Text>}
                {n.message.action}
              </Text>
            </View>
          </View>
        </Card>
      )
    });

    const newsListView = (
      <View>
        {newsItems}
      </View>
    );

    return newsListView;
  }
}


export default Feed;
