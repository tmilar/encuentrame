import React, {Component} from 'react';
import {Alert, Button, Modal, StyleSheet, Text, View} from 'react-native';
import {text} from '../style';

export default class AreYouOk extends Component {
  state = {
    modalVisible: false
  };

  componentDidMount = () => {
    this.setModalVisible(!this.state.modalVisible)
  };

  setModalVisible = (visible) => {
    this.setState({modalVisible: visible});
  };

  _handleImOk = () => {
    Alert.alert(
      'Ok!',
      `Avisando a tus amigos`
    );
    this._goBack();
  };

  _handleINeedHelp = () => {
    Alert.alert(
      'No te muevas!',
      `Vamos a buscar ayuda`
    );
    this._goBack();
  };

  _goBack = () => {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  };

  render() {

    return (
      <View>
        <Modal
          animationType={"slide"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={() => {/* handle modal 'back' close? */
          }}
        >
          <View style={styles.message}>
            <Text style={text.title}>Estas bien?</Text>
          </View>
          <View style={{flex: 1}}>
            <View style={{flexDirection: 'row', justifyContent: 'space-around'}}>
              <Button
                title="Estoy bien!"
                color="#64DD17"
                onPress={this._handleImOk}
              />
              <Button
                title="Necesito ayuda"
                color='#ff5c5c'
                onPress={this._handleINeedHelp}
              />

            </View>
          </View>
        </Modal>

      </View>
    )
  }
}

const styles = StyleSheet.create({
  message: {
    flex: 5,
    height: 3,
    alignItems: 'center',
    justifyContent: 'center',
  }
});
