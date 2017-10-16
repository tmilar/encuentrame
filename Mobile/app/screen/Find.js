import React, {Component} from 'react'
import {View, StyleSheet} from 'react-native'
import Swiper from "react-native-deck-swiper/Swiper";
import EncuentraCard from "../component/EncuentraCard";
import {containers} from '../style';


export default class Find extends Component {

  constructor(props) {
    super(props);
    this.renderCard = this.renderCard.bind(this);
    this.onSwipedHandler = this.onSwipedHandler.bind(this);
  }

  renderCard(card) {
    return <EncuentraCard style={styles.encuentraCard} navigation=""/>
  }

  onSwipedHandler(cardIndex) {
    console.log(cardIndex);
    const {navigate} = this.props.navigation;
    navigate('SupplyInfo');
  }

  onSwipedAllHandler() {
    console.log('onSwipedAll')
  }

  render() {
    return (
      <View style={containers.container}>
        <Swiper cards={['DO', 'MORE', 'OF', 'WHAT', 'MAKES', 'YOU', 'HAPPY']}
                renderCard={this.renderCard}
                onSwiped={this.onSwipedHandler}
                onSwipedAll={this.onSwipedAllHandler}
                cardIndex={0}
                backgroundColor={'#E9E9EF'}
                style={styles.swiper}>
        </Swiper>
      </View>
    )
  }
}
const styles = StyleSheet.create({
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
