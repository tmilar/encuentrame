import React, {Component} from 'react';
import ReactNative, {Text, View, StyleSheet, Button, Alert, TextInput, ScrollView} from 'react-native';
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
      username: this.state.username,
      email: this.state.email,
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
        `Hubo un problema: ${e}.`
      );
      return;
    }

    hideLoading();

    Alert.alert(
      "Registro exitoso",
      `Bienvenido, ${registerData.username}!`
    );
    this.onDone && this.onDone(this.state.username);
    this.props.navigation.goBack();
  }

  render() {
    return (
      <View style={containers.container}>
        <ScrollView keyboardShouldPersistTaps="always"
                    scrollEnabled={false}
                    showsVerticalScrollIndicator={false}
        >
          <View style={styles.header}>
            <Text style={text.title}>
              Encuentrame
            </Text>
          </View>

          <View style={styles.content}>
            <TextInput
              value={this.state.username}
              placeholder="Usuario"
              ref="usuario"
              style={styles.input}
              selectTextOnFocus
              autoCapitalize='none'
              returnKeyType='next'
              onChangeText={this._handleUsernameTextChange}
              onSubmitEditing={() => this.refs.email.focus()}
            />

            <TextInput
              value={this.state.email}
              placeholder="E-mail"
              ref="email"
              style={styles.input}
              keyboardType="email-address"
              selectTextOnFocus
              autoCapitalize='none'
              returnKeyType='next'
              onChangeText={this._handleEmailTextChange}
              onSubmitEditing={() => this.refs.password.focus()}
            />

            <TextInput
              value={this.state.password}
              placeholder="Contraseña"
              ref="password"
              style={styles.input}
              autoCapitalize='none'
              autoCorrect={false}
              secureTextEntry
              returnKeyType="go"
              onChangeText={this._handlePasswordTextChange}
              onSubmitEditing={this._handleLoginButtonPress}
            />
          </View>

          <View style={styles.footer}>
            <Button
              title="Registro"
              onPress={this._handleRegisterButtonPress}
            />
          </View>
        </ScrollView>
      </View>
    )

  }
}

const styles = StyleSheet.create({
  input: {
    width: 200,
    height: 44,
    padding: 8
  }
});
