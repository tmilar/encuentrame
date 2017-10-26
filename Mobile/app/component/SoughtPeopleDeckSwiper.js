import React, {Component} from 'react';
import {Image} from 'react-native';
import {
  Container,
  Header,
  View,
  DeckSwiper,
  Card,
  CardItem,
  Thumbnail,
  Text,
  Left,
  Body,
  Icon,
  Button
} from 'native-base';

const cards = [
  {
    text: 'Luisito Gomez',
    name: 'Visto última vez hace 8 minutos, cerca tuyo.',
    username: '@luisitoverduras',
    image: require('../img/personImgExample.jpg'),
  },
  {
    text: 'Jose Lopez',
    name: 'Visto última vez hace 5 minutos, cerca tuyo.',
    image: require('../img/personImgExample.jpg'),
  },
];

export default class SoughtPeopleDeckSwiper extends Component {

  static defaultProps = {
    soughtPeople: cards,
    onIveSeenHim: () => {},
    onNotSeenHim: () => {}
  };

  _deckSwiper;

  _renderDeckSwiper = () => (
    <View>
      <DeckSwiper
        ref={(c) => this._deckSwiper = c}
        dataSource={this.props.soughtPeople}
        renderEmpty={() =>
          <View style={{alignSelf: "center"}}>
            <Text>Ya no queda nadie por buscar. Gracias!</Text>
          </View>
        }
        renderItem={item =>
          <Card style={{elevation: 3}}>
            <CardItem>
              <Left>
                <Thumbnail source={item.image}/>
                <Body>
                <Text>{item.text}</Text>
                <Text note>{item.username}</Text>
                </Body>
              </Left>
            </CardItem>
            <CardItem cardBody>
              <Image style={{height: 250, flex: 1}} source={item.image}/>
            </CardItem>
            <CardItem>
              <Icon name="heart" style={{color: '#ED4A6A'}}/>
              <Text>{item.name}</Text>
            </CardItem>
          </Card>
        }
      />
    </View>
  );

  _renderSwipeButtons = () => (
    <View style={{
      flexDirection: "row",
      flex: 1,
      position: "absolute",
      bottom: 30,
      left: 0,
      right: 0,
      justifyContent: 'space-between',
      padding: 15
    }}>
      <Button iconLeft onPress={() => this._deckSwiper._root.swipeLeft()}>
        <Icon name="arrow-back"/>
        <Text>No lo he visto :(</Text>
      </Button>
      <Button iconRight onPress={() => this._deckSwiper._root.swipeRight()}>
        <Text>¡Lo he visto!</Text>
        <Icon name="arrow-forward"/>
      </Button>
    </View>
  );

  render() {
    return (
      <Container>
        {/*<Header />*/}
        {this._renderDeckSwiper()}
        {this._renderSwipeButtons()}
      </Container>
    )
  }
}
