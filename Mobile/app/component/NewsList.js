import React from 'react';
import {Card} from 'react-native-elements';
import {Text, View} from "react-native";
import NewsDispatcher from '../model/NewsDispatcher';

const EmptyNewsListMessage = () => <View style={{flex: 1, alignItems: "center", justifyContent: "center"}}>
  <Text style={{textAlign: "center"}} note>
    {"No hay novedades."}
  </Text>
</View>;

const NewsItem = ({message, Icon, date}) =>
  <View style={{
    flexDirection: 'row',
    height: 60,
    justifyContent: "space-around"
  }}>
    <View style={{justifyContent: "space-around", width: 40, alignItems: 'center'}}>
      {Icon}
    </View>
    <View style={{justifyContent: "space-around", width: 250}}>
      <Text style={{fontSize: 16, fontWeight: 'bold'}}>
        {`${message}.  ${date}`}
      </Text>
    </View>
  </View>;

const NewsList = props =>
  <View>
    {
      props.news.length === 0 ?
        <EmptyNewsListMessage/>
        :
        props.news.map((n, i) =>
          <Card key={i} containerStyle={{padding: 5}}>
            <NewsItem {...NewsDispatcher.getDisplayData(n)}/>
          </Card>
        )
    }
  </View>;

export default NewsList;
