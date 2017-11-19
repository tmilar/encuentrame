import React, {Component} from 'react';
import {Alert, Button, Image, Modal, StyleSheet, Text, View} from 'react-native';
import {text} from '../style';
import ContactsService from '../service/ContactsService';
import {showToast} from "react-native-notifyer";
import AccountsService from '../service/AccountsService';
import NewsDispatcher from '../model/NewsDispatcher';

export default class ContactRequest extends Component {
  state = {
    modalVisible: false
  };

  contactRequestUserId = this.props.navigation.state.params.contactRequestUserId;
  contactRequestUsername = this.props.navigation.state.params.contactRequestUsername;

  componentDidMount = () => {
    let accountImgUri = AccountsService.getAccountImageUriById(this.contactRequestUserId);
    this.setState({accountImgUri: accountImgUri});
    this.setModalVisible(!this.state.modalVisible);
  };

  setModalVisible = (visible) => {
    this.setState({modalVisible: visible});
  };

  _replyRequest = async (response) => {
    this._goBack();
    try {
      await ContactsService.reply(this.contactRequestUserId, response);
    } catch (e) {
      showToast("¡Ups! Hubo un problema al responder la solicitud de contacto. \n" + (e.message || e), {duration: 1500});
      return;
    }
    showToast(`Has ${response ? "aceptado" : "rechazado"} la solicitud de ${this.contactRequestUsername}.`, {duration: 1500});
  };

  _handleAcceptContactRequest = async () => {
    await this._resolveNews(true);
    await this._replyRequest(true);
  };

  _resolveNews = async (replied) => {
    await NewsDispatcher.resolveNews(this.props.navigation.state.params.newsId, {replied: replied});
  };

  _handleDenyContactRequest = async () => {
    await this._resolveNews(false);
    await this._replyRequest(false);
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
          onRequestClose={() => {
            showToast("Podrás responder esta solicitud más tarde, desde tu pestaña Home.", {duration: 2000});
            this.props.navigation.goBack(null);
          }}
        >
          <View style={styles.message}>
            <View style={{flex: 1}}>
              <Text style={text.title}>{this.contactRequestUsername} quiere agregarte.</Text>
            </View>
            <View style={{flex: 5}}>
              <Image source={{uri: this.state.accountImgUri}} style={{width: 200, height: 200}}/>
            </View>
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
