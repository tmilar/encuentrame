import React, {Component} from 'react';
import {Alert, Button, Modal, StyleSheet, Text, View} from 'react-native';
import {text} from '../style';
import ContactsService from '../service/ContactsService';

export default class ContactRequest extends Component {
  state = {
    modalVisible: false
  };

  contactRequestUserId = this.props.navigation.state.params.contactRequestUserId;
  contactRequestUsername = this.props.navigation.state.params.contactRequestUsername;

  componentDidMount = () => {
    this.setModalVisible(!this.state.modalVisible)
  };

  setModalVisible = (visible) => {
    this.setState({modalVisible: visible});
  };

  _handleAcceptContactRequest = async () => {
    await ContactsService.reply(this.contactRequestUserId, true);
    this._goBack();
  };

  _handleDenyContactRequest = async () => {
    await ContactsService.reply(this.contactRequestUserId, false);
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
            <Text style={text.title}>{this.contactRequestUsername} quiere agregarte</Text>
          </View>
          <View style={{flex: 1}}>
            <View style={{flexDirection: 'row', justifyContent: 'space-around'}}>
              <Button
                title="Aceptar solicitud"
                color="#64DD17"
                onPress={this._handleAcceptContactRequest}
              />
              <Button
                title="Rechazar"
                color='#ff5c5c'
                onPress={this._handleDenyContactRequest}
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
