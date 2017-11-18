import React, {Component} from 'react';
import {Alert, Modal, StyleSheet, Text, View} from 'react-native';
import {Button} from 'react-native-elements';
import {text} from '../style';
import AreYouOkService from '../service/AreYouOkService';
import {showToast} from "react-native-notifyer";
import NewsDispatcher from '../model/NewsDispatcher';

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

  _handleImOk = async () => {
    Alert.alert(
      '¡Ok!',
      `Avisando a tus amigos`
    );
    await NewsDispatcher.resolveNews(this.props.navigation.state.params.newsId, {replied: true});
    this._goBack();
    try {
      await AreYouOkService.reply(true);
    } catch (e) {
      showToast("¡Ups! No se pudo enviar que estás bien... \n" + (e || e.message), {duration: 2000});
    }
  };

  _handleINeedHelp = async () => {
    Alert.alert(
      '¡No te muevas!',
      `Vamos a buscar ayuda.`
    );
    await NewsDispatcher.resolveNews(this.props.navigation.state.params.newsId, {replied: false});
    this._goBack();
    try {
      await AreYouOkService.reply(false);
    } catch (e) {
      showToast("¡Ups! No se pudo enviar que necesitas ayuda... \n" + (e || e.message), {duration: 2000});
    }
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
            Alert.alert("¿Estás Bien?", "¡Por favor, responde! Alguien está preocupado por vos.");
          }}
        >
          <View style={styles.message}>
            <Text style={[text.title, {fontSize: 28}]}>¿Estás bien?</Text>
          </View>
          <View style={{flex: 1, justifyContent: 'center'}}>
            <View style={{
              flexDirection: 'row',
              justifyContent: 'space-around',
              alignItems: 'center',
            }}>
              <View style={{flex: 1, margin: 3}}>
                <Button
                  title="¡ESTOY BIEN!"
                  backgroundColor='#64DD17'
                  onPress={this._handleImOk}
                  iconRight={{name: 'thumbs-up', type: 'font-awesome'}}
                  raised={true}
                  textStyle={{fontWeight: 'bold'}}
                  borderRadius={5}
                  containerViewStyle={{borderRadius: 5}}
                  large={true}
                />
              </View>
              <View style={{flex: 1, margin: 3}}>
                <Button
                  title="NECESITO AYUDA"
                  backgroundColor='#ff5c5c'
                  onPress={this._handleINeedHelp}
                  icon={{name: 'warning'}}
                  raised={true}
                  textStyle={{fontWeight: 'bold', fontSize: 12}}
                  borderRadius={5}
                  containerViewStyle={{borderRadius: 5}}
                  large={true}
                />
              </View>
            </View>
          </View>
        </Modal>
      </View>
    )
  }
}

const styles = StyleSheet.create({
  message: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'flex-end',
  }
});
