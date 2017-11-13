import React from 'react';
import {Card} from 'react-native-elements';
import {Text, View} from "react-native";
import { Ionicons, EvilIcons, FontAwesome, MaterialCommunityIcons } from '@expo/vector-icons';

getIcon = (iconData) => {
  switch(iconData.tagName) {
    case 'EvilIcons':
      return <EvilIcons name={iconData.iconName} style={{ color: iconData.color || 'black'}} size={40}/>;
      break;
    case 'Ionicons':
      return <Ionicons name={iconData.iconName} style={{ color: iconData.color || 'black' }} size={40}/>;
      break;
    case 'MaterialCommunityIcons':
      return <MaterialCommunityIcons name={iconData.iconName} style={{ color: iconData.color || 'black' }} size={40}/>;
      break;
    case 'FontAwesome':
      return <FontAwesome name={iconData.iconName} size={40} style={{ color: iconData.color || 'black' }}/>;
      break;
    default:
      return <MaterialCommunityIcons name='alarm-light' style={{ color: 'black' }} size={40}/>;
  }
};

const NewsList = props =>
  <View>
    {props.news.map((n, i) => {
      return (
        <Card key={i} containerStyle={{padding: 5}}>
          <View style={{
            flexDirection: 'row',
            height: 60,
            justifyContent: "space-around"
          }}>
            <View style={{justifyContent: "space-around", width: 40, alignItems: 'center'}}>
              {getIcon(n.icon)}
            </View>
            <View style={{justifyContent: "space-around", width: 250}}>
              <Text style={{fontSize: 16, fontWeight: 'bold'}}>
                {n.message}
              </Text>
            </View>
          </View>
        </Card>
      )
    })}
  </View>

export default NewsList;
