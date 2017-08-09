import React from 'react';
import {Card, Icon} from 'react-native-elements';
import {Text, View} from "react-native";

const NewsList = props =>
  <View>
    {props.news.map((n, i) => {
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
    })}
  </View>
;

export default NewsList;
