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
    empty: this.props.soughtPeople.length === 0
  };

  _deckSwiper;

  handleIveSeenHimSwipe = async personCard => {
    console.debug("[SoughtPeopleDeckSwiper] Swiped right: ", personCard);
    this.props.navigation.navigate("SupplyInfo", {
      soughtPersonId: personCard.User.Id,
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

  componentWillReceiveProps = (newProps) => {
    console.debug("[SoughtPeopleDeckSwiper] componentWillReceiveProps: ", newProps);
    this.checkEmpty(newProps.soughtPeople);
    // TODO prevent update if newProps are the same people?
  };

  handleIveNotSeenHimSwipe = async personCard => {
    console.debug("[SoughtPeopleDeckSwiper] Swiped left: ", personCard);
    await this.props.onIveNotSeenHim(personCard);
    // TODO send card to end of list; if second time then remove? Or simply remove?
    await sleep(50);
    this.checkEmpty();
  };

  /**
   * Check empty and update state if different as before.
   * Either: new data present and before was empty, or there was data before and not anymore.
   *
   * @param soughtPeople
   * @returns {boolean}
   */
  checkEmpty = (soughtPeople = this.props.soughtPeople) => {
    let swiperEmpty = () => this._deckSwiper && this._deckSwiper._root.state.disabled;
    let peopleData = () => soughtPeople && soughtPeople.length;
    let isEmptyValue = swiperEmpty() || !peopleData();
    let debugEmpty = (extraMsg) => {
      let checkObj = {swiperEmpty: swiperEmpty(), peopleData: peopleData(), isEmptyValue};
      console.debug(
        `[SoughtPeopleDeckSwiper] _isEmpty check: ${JSON.stringify(checkObj)}${extraMsg}`
      );
    };
    debugEmpty();
    if (peopleData() && swiperEmpty()) {
      // we have people, but the swiper still didn't update. force an update.
      debugEmpty("people present, but swiper empty. Forcing update.");
      /*  TODO this logic works fine. But the forceUpdate() doesn't trigger the re-fresh of the swiper,
       * therefore the disabled is still false.
       * maybe, find a different way to force the swiper update, like using a clone of the datasource
       * or something like forcing unmount-remount....
       */
      this.forceUpdate();
      debugEmpty("after forcing...");
    }

    this.setState({isEmptyValue});

    return isEmptyValue;
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
                <Text>{`${item.User.FirstName || "[FirstName]"} ${item.User.LastName || "[LastName]"}`}</Text>
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
