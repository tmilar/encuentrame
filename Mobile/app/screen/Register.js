import React, {Component} from 'react';
import ReactNative, {Text, View, StyleSheet, Button, Alert, TextInput, ScrollView, Image} from 'react-native';
import UserService from '../service/UserService';
import {showLoading, hideLoading} from 'react-native-notifyer';
import {containers, text} from '../style';

export default class Register extends Component {

  static navigationOptions = {
    title: 'Registro'
  };


  constructor(props) {
    super(props);
    this.state = {
      username: '',
      email: (props.navigation.state.params &&
      props.navigation.state.params.form &&
      props.navigation.state.params.form.email) ?
        props.navigation.state.params.form.email : '',
      password: props.navigation.state.params.form ?
        props.navigation.state.params.form.password : ''
    };

    this.onDone = props.navigation.state.params.onDone;

    this._clearForm = this._clearForm.bind(this);

    this._handleUsernameTextChange = this._handleUsernameTextChange.bind(this);
    this._handleEmailTextChange = this._handleEmailTextChange.bind(this);
    this._handlePasswordTextChange = this._handlePasswordTextChange.bind(this);

    this._handleRegisterButtonPress = this._handleRegisterButtonPress.bind(this);
  }

  _clearForm() {
    this.setState({
      username: '',
      email: '',
      password: ''
    });
  }

  _handleUsernameTextChange(inputValue) {
    this.setState({username: inputValue})
  }

  _handleEmailTextChange(inputValue) {
    this.setState({email: inputValue})
  }

  _handlePasswordTextChange(inputValue) {
    this.setState({password: inputValue})
  }

  async _handleRegisterButtonPress() {
    const registerData = {
      username: this.state.username.trim(),
      email: this.state.email.trim(),
      password: this.state.password
    };

    showLoading("Registro en progreso...");

    try {
      await UserService.registerUser(registerData);
    } catch (e) {
      hideLoading();
      console.log("Register error:", e);
      Alert.alert(
        "Error de registro",
        `Hubo un problema: ${e.message || e}`
      );
      return;
    }

    hideLoading();

    Alert.alert(
      "Registro exitoso",
      `¡Bienvenido, ${registerData.username}!`
    );
    this.onDone && this.onDone(this.state.username);
    this.props.navigation.goBack();
  }

  render() {
    return (
      <View style={[containers.container, {flex: 1, backgroundColor: '#3CB393'}]}>
          <View style={[{flex: 1}]} >

            <View style={[styles.header, {flex: 3}]} >
              <Image
                style={{width: 400, height: 200}}
                source={require('../img/eme_final.png')} />
            </View>

            <View style={[styles.content, {flex: 4, justifyContent: 'center', alignItems: 'center'}]} >
              <TextInput
                value={this.state.username}
                placeholder="Usuario"
                ref="usuario"
                style={[styles.textInput, styles.registerTextInput, {width: 250, color: 'black' }]}
                selectTextOnFocus
                placeholderTextColor="black"
                autoCapitalize='none'
                returnKeyType='next'
                onChangeText={this._handleUsernameTextChange}
                onSubmitEditing={() => this.refs.email.focus()}
              />

              <TextInput
                value={this.state.email}
                placeholder="E-mail"
                ref="email"
                style={[styles.textInput, styles.registerTextInput, {width: 250,color: 'black' }]}
                keyboardType="email-address"
                selectTextOnFocus
                placeholderTextColor="black"
                autoCapitalize='none'
                returnKeyType='next'
                onChangeText={this._handleEmailTextChange}
                onSubmitEditing={() => this.refs.password.focus()}
              />

              <TextInput
                value={this.state.password}
                placeholder="Contraseña"
                ref="password"
                style={[styles.textInput, styles.registerTextInput, {width: 250,color: 'black' }]}
                autoCapitalize='none'
                placeholderTextColor="black"
                autoCorrect={false}
                secureTextEntry
                returnKeyType="go"
                onChangeText={this._handlePasswordTextChange}
                onSubmitEditing={this._handleLoginButtonPress}
              />
            </View>

            <View style={{flex: 2, justifyContent: 'space-around', alignItems: 'center'}}>
              <Button
                title="Registro"
                color="#063450"
                style={{backgroundColor: '#063450',width: 250}}
                onPress={this._handleRegisterButtonPress}
              />
            </View>

          </View>
      </View>
    )

  }
}

const styles = StyleSheet.create({
  input: {
    width: 200,
    height: 44,
    padding: 8
  },
  registerTextInput: {
    padding: 8,
    backgroundColor: 'white',
    borderRadius: 10,
    margin: 5
  }
});
