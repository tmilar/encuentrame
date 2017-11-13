import React from 'react';
import {Card} from 'react-native-elements';
import {Text, View} from "react-native";
import {prettyDate} from "../util/prettyDate";
import {getIcon} from "./NewsIcons";

const NewsList = props => {
  if (props.news.length === 0)
    return <View style={{flex: 1, alignItems: "center", justifyContent: "center"}}>
      <Text style={{textAlign: "center"}} note>
        {"No hay novedades."}
      </Text>
    </View>;
  return <View style={{flex: 1}}>
      {props.news.map((n, i) => {
        return (
          <Card key={i} containerStyle={{padding: 5}}>
            <View style={{
              flexDirection: 'row',
              height: 60,
              justifyContent: "space-around"
            }}>
              <View style={{justifyContent: "space-around", width: 40, alignItems: 'center'}}>
                {getIcon(n)}
              </View>
              <View style={{justifyContent: "space-around", width: 250}}>
                <Text style={{fontSize: 14, fontWeight: 'bold'}}>
                  {n.message}. {prettyDate(new Date(n.time))}
                </Text>
              </View>
            </View>
          </Card>
        )
      })}
    </View>;
}

export default NewsList;
