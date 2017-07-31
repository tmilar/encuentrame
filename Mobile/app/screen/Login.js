import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput} from 'react-native'
import UserService from '../service/UserService';

export default class Login extends Component {

  constructor(props) {
    super(props);

    this.state = {
      userEmail: '',
      password: ''
    };

    this._clearForm = this._clearForm.bind(this);
    this._validateLogin = this._validateLogin.bind(this);
    this._handleEmailTextChange = this._handleEmailTextChange.bind(this);
    this._handlePasswordTextChange = this._handlePasswordTextChange.bind(this);
    this._handleLoginButtonPress = this._handleLoginButtonPress.bind(this);
    this._handleRegisterTextPress = this._handleRegisterTextPress.bind(this);
  }

  _clearForm() {
    this.setState({
      userEmail: '',
      password: ''
    });
  }

  async _validateLogin() {

    if (this.state.userEmail === '' || this.state.password === '') {
      throw "User email or password can't empty!";
    }

    const credentials = {
      email: this.state.userEmail,
      password: this.state.password
    };

    let result = await UserService.checkCredentials(credentials);

    console.log(`Logged! ->`, result);

    return result;
  }

  async _handleLoginButtonPress() {
    try {
      await this._validateLogin();
    } catch (e) {
      console.log("Login error: ", e);
      Alert.alert(
        'Error',
        `Email o password incorrecto?`
      );
      return;
    }

    Alert.alert(
      'Login!',
      `Bienvenido, ${this.state.userEmail}!`
    );

    this._clearForm();
    this._goToHome();
  }

  _handleEmailTextChange(inputValue) {
    this.setState({userEmail: inputValue})
  }

  _handlePasswordTextChange(inputValue) {
    this.setState({password: inputValue})
  }

  _handleRegisterTextPress() {
    const {navigate} = this.props.navigation;
    const self = this;

    navigate('Register', {
      form: this.state,
      onDone: (userEmail) => {
        self.setState({userEmail});
      }
    })
  }

  static navigationOptions = {
    title: 'Login',
    // header: null
  };

  _goToHome() {
    const {navigate} = this.props.navigation;
    navigate('PostLogin');
  }

  render() {
    return (
      <View style={styles.container}>
        <Text style={styles.paragraph}>
          Encuentrame
        </Text>

        <TextInput
          value={this.state.userEmail}
          placeholder="E-mail"
          style={styles.textInput}
          keyboardType="email-address"
          selectTextOnFocus
          onChangeText={this._handleEmailTextChange}
        />

        <TextInput
          value={this.state.password}
          placeholder="Contraseña"
          style={styles.textInput}
          secureTextEntry
          returnKeyType="done"
          onChangeText={this._handlePasswordTextChange}
          onSubmitEditing={this._handleLoginButtonPress}
        />

        <Button
          title="Login"
          onPress={this._handleLoginButtonPress}
        />

        <Text
          onPress={this._handleRegisterTextPress}
          style={{textDecorationLine: "underline"}}>
          No estoy registrado
        </Text>

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
  textInput: {
    width: 200,
    height: 44,
    padding: 8
  }
});
