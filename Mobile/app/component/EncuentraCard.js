import React, {Component} from 'react';
import {Text, Button, View, Image} from 'react-native'
import {StyleSheet} from 'react-native';
import {text} from '../style';

import {
  Card,
  CardImage,
  CardTitle,
  CardContent,
  CardAction
} from 'react-native-card-view';

export default class EncuentraCard extends Component {

  constructor(props) {
    super(props);
    this._handleIveSeenHim = this._handleIveSeenHim.bind(this);
  }

  _handleIveSeenHim() {
    const {navigate} = this.props.navigation;
    navigate('SupplyInfo');
  }

  render () {
    return (
      <Card style={styles.card}>
        <CardTitle>
          <Text style={text.p}>Has visto a Pepe?</Text>
        </CardTitle>
        <CardContent style={{flex:0.4}}>
          <Image source={require('../img/personImgExample.jpg')} />
        </CardContent>
        <CardAction >
          <Button color="green"
            title="¡Lo he visto!"
            style={styles.confirmButton}
            onPress={this._handleIveSeenHim}>
            Button 1
          </Button>
          <Button
            color="grey"
            title="No lo he visto"
            style={styles.dismissButton}
            onPress={() => {}}>
            Button 2
          </Button>
        </CardAction>
        <View style={{flex: 0.2}}/>
      </Card>
    );
  }
}

const styles = StyleSheet.create({
  card: {
    flex: 1
  },
  button: {
    marginRight: 10,
    flex: 0.2
  },
  confirmButton: {
    color: 'green',
    marginRight: 10,
    flex: 0.2
  },
  dismissButton: {
    backgroundColor: 'grey',
    marginRight: 10,
    flex: 0.2
  },
});
