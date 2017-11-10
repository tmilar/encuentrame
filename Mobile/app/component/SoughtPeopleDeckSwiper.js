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
import sleep from "../util/sleep";


export default class SoughtPeopleDeckSwiper extends Component {

  static defaultProps = {
    soughtPeople: [],
    navigation: {},
    onIveSeenHimSubmit: () => console.log("Submitting IveSeenHim"),
    onIveSeenHimCancel: () => console.log("Canceling IveSeenHim"),
    onIveNotSeenHim: () => console.log("Submitting IveNotSeenHim")
  };

  state = {
    empty: false
  };

  _deckSwiper;

  handleIveSeenHimSwipe = async personCard => {
    console.debug("[SoughtPeopleDeckSwiper] Swiped right: ", personCard);
    this.props.navigation.navigate("SupplyInfo", {
      soughtPersonId: personCard.soughtPersonId,
      onSubmit: async (suppliedInfo) => {
        console.log("[SoughtPeopleDeckSwiper] SupplyInfo.onSubmit() called. Supplying info to server.", suppliedInfo);
        await this.props.onIveSeenHimSubmit(personCard, suppliedInfo);
        this.checkEmpty();
      },
      onClose: async () => {
        console.log("[SoughtPeopleDeckSwiper] SupplyInfo.onClose() called. Restoring swiped person card.");
        await this.props.onIveSeenHimCancel(personCard);
        this.checkEmpty();
      }
    });
  };

  handleIveNotSeenHimSwipe = async personCard => {
    console.debug("[SoughtPeopleDeckSwiper] Swiped left: ", personCard);
    await this.props.onIveNotSeenHim(personCard);
    // TODO send card to end of list; if second time then remove? Or simply remove?
    await sleep(50);
    this.checkEmpty();
  };

  checkEmpty = () => {
    let empty = (this._deckSwiper && this._deckSwiper._root.state.disabled);
    this.setState({empty});
  };

  _renderEmptySwiper = () => (
    <View style={{height: "100%", alignItems: "center", justifyContent: "center"}}>
      <Text style={{textAlign: "center"}} note>
        {"Ya no queda nadie por buscar. \n¡Gracias por tu aporte!"}
      </Text>
    </View>
  );

  _renderDeckSwiper = () => (
    <View>
      <DeckSwiper
        ref={(c) => this._deckSwiper = c}
        dataSource={this.props.soughtPeople}
        renderEmpty={this._renderEmptySwiper}
        looping={false}
        renderItem={item =>
          <Card style={{elevation: 3}}>
            <CardItem>
              <Left>
                <Thumbnail source={{uri: item.User.imageUri}}/>
                <Body>
                <Text>{`${item.User.FirstName || "[nombre]"} ${item.User.LastName || "[apellido]"}`}</Text>
                <Text note>{`@${item.User.Username}`}</Text>
                </Body>
              </Left>
            </CardItem>
            <CardItem cardBody>
              <Image style={{height: 250, flex: 1}} source={{uri: item.User.imageUri}}/>
            </CardItem>
            <CardItem>
              <Icon name="heart" style={{color: '#ED4A6A'}}/>
              <Text>{`Se lo vio a ${item.Distance} mts tuyos.`}</Text>
            </CardItem>
          </Card>
        }
        onSwipeRight={this.handleIveSeenHimSwipe}
        onSwipeLeft={this.handleIveNotSeenHimSwipe}
      />
    </View>
  );

  handleSwipeLeftButtonPress = async () => {
    this._deckSwiper._root.swipeLeft();
    let personCard = this._deckSwiper._root.state.selectedItem;
    await this.handleIveNotSeenHimSwipe(personCard);
  };

  handleSwipeRightButtonPress = () => {
    this._deckSwiper._root.swipeRight();
    let personCard = this._deckSwiper._root.state.selectedItem;
    this.handleIveSeenHimSwipe(personCard);
  };

  _renderSwipeButtons = () => {
    return !this.state.empty && (
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
          <Button iconLeft onPress={this.handleSwipeLeftButtonPress}>
            <Icon name="arrow-back"/>
            <Text>No lo he visto :(</Text>
          </Button>
          <Button iconRight onPress={this.handleSwipeRightButtonPress}>
            <Text>¡Lo he visto!</Text>
            <Icon name="arrow-forward"/>
          </Button>
        </View>
      );
  };

  render() {
    return (
      <Container>
        {this._renderDeckSwiper()}
        {this._renderSwipeButtons()}
      </Container>
    )
  }
}
