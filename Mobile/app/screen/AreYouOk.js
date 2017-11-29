import React, {Component} from 'react';
import {Alert, Image, Modal, StyleSheet, Text, View} from 'react-native';
import {Button} from 'react-native-elements';
import {text} from '../style';
import AreYouOkService from '../service/AreYouOkService';
import {showToast} from "react-native-notifyer";
import NewsDispatcher from '../model/NewsDispatcher';

export default class AreYouOk extends Component {
  state = {
    modalVisible: false,
    submitted: false
  };

  componentDidMount = () => {
    this.setModalVisible(!this.state.modalVisible)
  };

  setModalVisible = (visible) => {
    this.setState({modalVisible: visible});
  };

  _resolveNews = async (replied) => {
    let originNewsId = this.props.navigation.state.params.newsId;
    await NewsDispatcher.resolveNews(originNewsId, {replied});
  };

  _handleImOk = async () => {
    this.setState({submitted: true});
    Alert.alert(
      '¡Ok!',
      `Avisando a tus amigos`
    );
    await this._resolveNews(true);
    this._goBack();
    try {
      await AreYouOkService.reply(true);
    } catch (e) {
      showToast("¡Ups! No se pudo enviar que estás bien... \n" + (e.message || e), {duration: 2000});
    }
  };

  _handleINeedHelp = async () => {
    this.setState({submitted: true});
    Alert.alert(
      '¡No te muevas!',
      `Vamos a buscar ayuda.`
    );
    await this._resolveNews(false);
    this._goBack();
    try {
      await AreYouOkService.reply(false);
    } catch (e) {
      showToast("¡Ups! No se pudo enviar que necesitas ayuda... \n" + (e.message || e), {duration: 2000});
    }
  };

  _goBack = () => {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  };

  render() {
    return (
      <View style={{backgroundColor: '#3CB393'}}>
        <Modal
          animationType={"slide"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={() => {
            Alert.alert("¿Estás Bien?", "¡Por favor, responde! Alguien está preocupado por vos.");
          }}
        >
          <View style={{flex: 1, backgroundColor: '#3CB393'}}>
            <View style={[styles.message, {flex: 1.4, justifyContent: 'space-around', paddingTop: 30}]}>
              <Text style={[text.title, {fontSize: 60, color: 'white'}]}>¿Estás bien?</Text>
            </View>
            <View style={{flex: 1, justifyContent: 'center'}}>
              <View style={{
                flexDirection: 'row',
                justifyContent: 'space-around',
                alignItems: 'center',
                flex: 1,
                marginBottom: 15
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
                    disabled={this.state.submitted}
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
                    disabled={this.state.submitted}
                  />
                </View>
              </View>
              <View style={{flex: 1, justifyContent: 'space-around', alignItems: 'center'}}>
                <Image
                  resizeMode="contain"
                  style={{height: "100%"}}
                  source={require('../img/eme_final2.png')}/>
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
