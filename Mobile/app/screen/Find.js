import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput} from 'react-native'
import Swiper from "react-native-deck-swiper/Swiper";
import EncuentraCard from "../component/EncuentraCard";


export default class Me extends Component {
  render() {
    return (
      <View style={styles.container}>
        <Swiper style={styles.swiper}
          cards={['DO', 'MORE', 'OF', 'WHAT', 'MAKES', 'YOU', 'HAPPY']}
          renderCard={(card) => {
            return (
              <EncuentraCard style={styles.encuentraCard}>
              </EncuentraCard>
            )
          }}
          onSwiped={(cardIndex) => {console.log(cardIndex)}}
          onSwipedAll={() => {console.log('onSwipedAll')}}
          cardIndex={0}
          backgroundColor={'#4FD0E9'}>
          <Button
            onPress={() => {console.log('oulala')}}
            title="Press me">
            You can press me
          </Button>
        </Swiper>
        <View style={{flex:0.2}}></View>
      </View>
    )
  }
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  swiper: {
    flex: 0.8
  },
  paragraph: {
    margin: 24,
    fontSize: 18,
    textAlign: 'center',
    color: '#34495e'
  },
  encuentraCard: {
    flex: 0.8
  },
  card: {
    flex: 1,
    borderRadius: 4,
    borderWidth: 2,
    borderColor: '#E8E8E8',
    justifyContent: 'center',
    backgroundColor: 'white',
  },
  text: {
    textAlign: 'center',
    fontSize: 50,
    backgroundColor: 'transparent'
  }
});
