import React, {Component} from 'react';
import {Text, View, StyleSheet, Button, Alert, TextInput} from 'react-native';
import UserService from '../service/UserService';

export default class Register extends Component {
  constructor(props) {
    super(props);
    this.state = {
      userName: '',
      userEmail: (props.navigation.state.params &&
        props.navigation.state.params.form &&
        props.navigation.state.params.form.userEmail) ?
        props.navigation.state.params.form.userEmail : '',
      password: props.navigation.state.params.form ?
        props.navigation.state.params.form.password : ''
    };

    this.onDone = props.navigation.state.params.onDone;

    this._clearForm = this._clearForm.bind(this);

    this._handleUserNameTextChange = this._handleUserNameTextChange.bind(this);
    this._handleEmailTextChange = this._handleEmailTextChange.bind(this);
    this._handlePasswordTextChange = this._handlePasswordTextChange.bind(this);

    this._handleRegisterButtonPress = this._handleRegisterButtonPress.bind(this);
  }

  _clearForm() {
    this.setState({
      userName: '',
      userEmail: '',
      password: ''
    });
  }

  _handleUserNameTextChange(inputValue) {
    this.setState({userName: inputValue})
  }

  _handleEmailTextChange(inputValue) {
    this.setState({userEmail: inputValue})
  }

  _handlePasswordTextChange(inputValue) {
    this.setState({password: inputValue})
  }

  async _handleRegisterButtonPress() {
    const registerData = {
      email: this.state.userEmail,
      password: this.state.password
    };

    try {
      await UserService.registerUser(registerData);
    } catch (e) {
      console.log("Register error:", e);
      Alert.alert(
        "Error de registro",
        `Hubo un problema: ${e}`
      );
      return;
    }

    Alert.alert(
      "Registro OK!",
      `Bienvenido, ${registerData.email}`
    );

    this.onDone && this.onDone(this.state.userEmail);
    this.props.navigation.goBack();
  }

  static navigationOptions = {
    title: 'Registro'
  };

  render() {

    return (
      <View style={styles.container}>
        <Text style={styles.paragraph}>
          Encuentrame
        </Text>

        <TextInput
          value={this.state.userName}
          placeholder="Nombre"
          style={styles.input}
          selectTextOnFocus
          onChangeText={this._handleUserNameTextChange}
        />

        <TextInput
          value={this.state.userEmail}
          placeholder="E-mail"
          style={styles.input}
          keyboardType="email-address"
          selectTextOnFocus
          onChangeText={this._handleEmailTextChange}
        />

        <TextInput
          value={this.state.password}
          placeholder="Contraseña"
          style={styles.input}
          secureTextEntry
          returnKeyType="done"
          onChangeText={this._handlePasswordTextChange}
          onSubmitEditing={this._handleLoginButtonPress}
        />

        <Button
          title="Registro"
          onPress={this._handleRegisterButtonPress}
        />

      </View>
    )
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  paragraph: {
    margin: 24,
    fontSize: 18,
    fontWeight: 'bold',
    textAlign: 'center',
    color: '#34495e'
  },
  input: {
    width: 200,
    height: 44,
    padding: 8
  }
});
