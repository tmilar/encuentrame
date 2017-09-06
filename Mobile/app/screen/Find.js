import React, {Component} from 'react'
import {View, StyleSheet} from 'react-native'
import Swiper from "react-native-deck-swiper/Swiper";
import EncuentraCard from "../component/EncuentraCard";
import containers from '../style/containers';


export default class Find extends Component {
  renderCard(card) {
    return <EncuentraCard style={styles.encuentraCard}/>
  }

  onSwipedHandler(cardIndex) {
    console.log(cardIndex)
  }

  onSwipedAllHandler() {
    console.log('onSwipedAll')
  }

  render() {
    return (
      <View style={styles.container}>
        <Swiper cards={['DO', 'MORE', 'OF', 'WHAT', 'MAKES', 'YOU', 'HAPPY']}
                renderCard={this.renderCard}
                onSwiped={this.onSwipedHandler}
                onSwipedAll={this.onSwipedAllHandler}
                cardIndex={0}
                backgroundColor={'#4F10E9'}
                style={styles.swiper}>
        </Swiper>
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
  swiper: {},
  paragraph: {
    margin: 24,
    fontSize: 18,
    textAlign: 'center',
    color: '#34495e'
  },
  encuentraCard: {},
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
