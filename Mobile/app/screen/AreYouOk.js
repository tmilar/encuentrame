import React, {Component} from 'react';
import {Alert, Button, Modal, StyleSheet, Text, View} from 'react-native';

export default class AreYouOk extends Component {
  constructor(props) {
    super(props);
    this.state = {
      estoyBien: true,
      modalVisible: false
    };

    this._handleImOk = this._handleImOk.bind(this);
    this._handleINeedHelp = this._handleINeedHelp.bind(this);
  }

  setModalVisible(visible) {
    this.setState({modalVisible: visible});
  }

  _handleImOk() {
    Alert.alert(
      'Ok!',
      `Avisando a tus amigos`
    );
    this._backToHome();
  }

  _backToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  }

  _handleINeedHelp() {
    Alert.alert(
      'No te muevas!',
      `Vamos a buscar ayuda`
    );
    this._backToHome();
  }

  componentDidMount() {
    this.setModalVisible(!this.state.modalVisible)
  }

  render() {

    return (
      <View style={{marginTop: 22}}>
        <Modal
          animationType={"slide"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={() => {/* handle modal 'back' close? */
          }}
        >
          <View style={styles.message}>
            <View>

              <Text>Estas bien?</Text>

            </View>
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