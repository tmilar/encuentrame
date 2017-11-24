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
import {deepEquals} from "../util/deepEquals";


export default class SoughtPeopleDeckSwiper extends Component {

  static defaultProps = {
    soughtPeople: [],
    navigation: {},
    onIveSeenHimSubmit: () => console.log("Submitting IveSeenHim"),
    onIveSeenHimCancel: () => console.log("Canceling IveSeenHim"),
    onIveNotSeenHim: () => console.log("Submitting IveNotSeenHim")
  };

  state = {
    empty: this.props.soughtPeople.length === 0,
    swiperRandomKey: 0
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
        console.log("[SoughtPeopleDeckSwiper] SupplyInfo.onClose() called. (//TODO: should restore swiped person card, back?)");
        await this.props.onIveSeenHimCancel(personCard);
        this.checkEmpty();
      }
    });
  };

  /**
   * Check if new soughtPeople prop received, is different from before -> remount.
   * Also Update this.state.empty based on soughtPeople.length
   *
   * @param soughtPeople
   */
  componentWillReceiveProps = ({soughtPeople}) => {
    console.debug("[SoughtPeopleDeckSwiper] componentWillReceiveProps. soughtPeople: ", soughtPeople);

    let swiperDisabled = () => this._deckSwiper && this._deckSwiper._root.state.disabled;
    let peopleData = () => soughtPeople && soughtPeople.length > 0;
    let peopleDataIsNew = () => peopleData() && !deepEquals(soughtPeople, this.props.soughtPeople);

    // remount swiper, if got new and DIFFERENT data.
    if (swiperDisabled() && peopleData()) {
      // swiper was disabled, but got new data. Remount whole thing...
      console.debug(`[SoughtPeopleDeckSwiper] swiper WAS disabled / but got ${soughtPeople.length} people. remounting swiper.`);
      this.setState({swiperRandomKey: Math.random().toString()})
    } else if (peopleDataIsNew()) {
      // Got new and different data. Remount whole thing...
      console.debug(`[SoughtPeopleDeckSwiper] got data and is different (dataIsNew). remounting swiper. `);
      this.setState({swiperRandomKey: Math.random().toString()})
    }

    // Update empty.
    this.setState({empty: !peopleData()});
  };

  handleIveNotSeenHimSwipe = async personCard => {
    console.debug("[SoughtPeopleDeckSwiper] Swiped left: ", personCard);
    await this.props.onIveNotSeenHim(personCard);
    // TODO send card to end of list; if second time then remove? Or simply remove?
    this.checkEmpty();
  };

  /**
   * Check empty AFTER swiping gesture, to update the view correctly.
   *
   * @param soughtPeople
   * @returns {boolean}
   */
  checkEmpty = (soughtPeople = this.props.soughtPeople) => {
    let swiperDisabled = () => this._deckSwiper && this._deckSwiper._root.state.disabled;
    let swiperLastCard = () => this._deckSwiper && this._deckSwiper._root.state.lastCard;
    let peopleData = () => soughtPeople && soughtPeople.length > 0;
    let isEmptyValue;

    isEmptyValue = swiperDisabled() || (!swiperDisabled() && !peopleData()); //|| swiperLastCard();

    let debugCheckObj = {
      swiperEmpty: swiperDisabled(),
      peopleData: peopleData(),
      swiperLastCard: swiperLastCard(),
      isEmptyValue
    };
    console.debug(
      `[SoughtPeopleDeckSwiper] _isEmpty check: ${JSON.stringify(debugCheckObj)}`
    );

    this.setState({empty: isEmptyValue});

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
    <View key={this.state.swiperRandomKey}>
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
                <Text>{`${item.User.Firstname || "[Firstname]"} ${item.User.Lastname || "[Lastname]"}`}</Text>
                <Text note>{`@${item.User.Username}`}</Text>
                </Body>
              </Left>
            </CardItem>
            <CardItem cardBody>
              <Image style={{height: 250, flex: 1}} source={{uri: item.User.imageUri}}/>
            </CardItem>
            <CardItem>
              <Icon name="heart" style={{color: '#ED4A6A'}}/>
              <Text>{`Se lo vio ${item.Distance > 0 ? `a ${item.Distance} mts tuyos` : `cerca tuyo`}.`}</Text>
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

  handleSwipeRightButtonPress = async () => {
    this._deckSwiper._root.swipeRight();
    let personCard = this._deckSwiper._root.state.selectedItem;
    await this.handleIveSeenHimSwipe(personCard);
  };

  _renderSwipeButtons = () => {
    return !this.state.empty && (
        <View style={{
          flexDirection: "row",
          flex: 1,
          position: "absolute",
          bottom: 10,
          left: 0,
          right: 0,
          justifyContent: 'space-between',
          padding: 10
        }}>
          <Button iconLeft style={{backgroundColor: '#063450'}} onPress={this.handleSwipeLeftButtonPress}>
            <Icon name="arrow-back"/>
            <Text>No lo he visto :(</Text>
          </Button>
          <Button iconRight style={{backgroundColor: '#063450'}} onPress={this.handleSwipeRightButtonPress}>
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
