import React from 'react';
import {Card} from 'react-native-elements';
import {Text, View} from "react-native";
import {prettyDate} from "../util/prettyDate";
import {Ionicons, EvilIcons, FontAwesome, MaterialCommunityIcons} from '@expo/vector-icons';

const getIcon = (type) => {
  switch(type) {
    case 'Areyouok.Ask':
      return <EvilIcons name='question' style={{ color: 'orange'}} size={40}/>;
      break;
    case 'Areyouok.Reply':
      return <Ionicons name='md-happy' style={{ color: 'green' }} size={40}/>;
      break;
    case 'Contact.Request':
      return <Ionicons name='md-contacts' style={{ color: 'green' }} size={40}/>;
      break;
    case 'Contact.Confirm':
      return <MaterialCommunityIcons name='account-check' style={{ color: 'black' }} size={40}/>;
      break;
    case 'Event/StartCollaborativeSearch':
      return <FontAwesome name='warning' size={40} style={{ color: 'red' }}/>;
      break;
    default:
      return <MaterialCommunityIcons name='alarm-light' style={{ color: 'black' }} size={40}/>;
  }
};

const EmptyNewsListMessage = () => <View style={{flex: 1, alignItems: "center", justifyContent: "center"}}>
  <Text style={{textAlign: "center"}} note>
    {"No hay novedades."}
  </Text>
</View>;

const NewsItem = ({type, message, time}) =>
  <View style={{
    flexDirection: 'row',
    height: 60,
    justifyContent: "space-around"
  }}>
    <View style={{justifyContent: "space-around", width: 40, alignItems: 'center'}}>
      {getIcon(type)}
    </View>
    <View style={{justifyContent: "space-around", width: 250}}>
      <Text style={{fontSize: 16, fontWeight: 'bold'}}>
        {message}.  {prettyDate(new Date(time))}
      </Text>
    </View>
  </View>

const NewsList = props =>
  <View>
    {
      props.news.length === 0 ?
        <EmptyNewsListMessage/>
        :
        props.news.map((n, i) =>
          <Card key={i} containerStyle={{padding: 5}}>
            <NewsItem {...n}/>
          </Card>
        )
    }
  </View>;

export default NewsList;
