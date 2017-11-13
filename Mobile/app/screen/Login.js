import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput, Keyboard} from 'react-native'
import UserService from '../service/UserService';
import SessionService from '../service/SessionService';
import {showLoading, hideLoading} from 'react-native-notifyer';
import {containers, text} from '../style';
import KeyboardSpacer from 'react-native-keyboard-spacer';
import {expo} from '../../app.json';
const {version, name} = expo;


export default class Login extends Component {
  static navigationOptions = {
    title: 'Login',
    // header: null
  };

  state = {
    username: '',
    password: ''
  };

  constructor(props) {
    super(props);

    this._clearForm = this._clearForm.bind(this);
    this._doLogin = this._doLogin.bind(this);
    this._handleUsernameTextChange = this._handleUsernameTextChange.bind(this);
    this._handlePasswordTextChange = this._handlePasswordTextChange.bind(this);
    this._handleLoginButtonPress = this._handleLoginButtonPress.bind(this);
    this._handleRegisterTextPress = this._handleRegisterTextPress.bind(this);
  }

  _clearForm() {
    this.setState({
      username: '',
      password: ''
    });
  }

  async _doLogin() {

    if (this.state.username === '' || this.state.password === '') {
      throw "¡El usuario o la contraseña no pueden estar vacíos!";
    }

    const credentials = {
      username: this.state.username,
      password: this.state.password
    };

    await UserService.doLogin(credentials);
  }

  async _handleLoginButtonPress() {
    Keyboard.dismiss();
    showLoading("Cargando...");
    try {
      await this._doLogin();
    } catch (e) {
      hideLoading();
      console.log("Login error: ", e);
      Alert.alert(
        'Error de Login',
        e.message || e
      );
      return;
    }
    hideLoading();
    Alert.alert(
      '¡Login!',
      `¡Bienvenido, ${this.state.username}!`
    );

    this._clearForm();
    this._goToHome();
  }

  _handleUsernameTextChange(inputValue) {
    this.setState({username: inputValue})
  }

  _handlePasswordTextChange(inputValue) {
    this.setState({password: inputValue})
  }

  async componentWillMount() {
    await this.checkLogout();
    await this.checkSessionAlive();
  }

  checkLogout = async () => {
    let isLogout =
      (
        this.props.screenProps &&
        this.props.screenProps.logout
      ) || (
        this.props.navigation.state &&
        this.props.navigation.state.params &&
        this.props.navigation.state.params.logout
      );

    if (isLogout) {
      console.log("Logout: destroying session.");
      await SessionService.clearSession();
    }
  };

  checkSessionAlive = async () => {
    let sessionAlive = await SessionService.isSessionAlive();
    if (sessionAlive) {
      this._goToHome();
    }
  };

  _handleRegisterTextPress() {
    const {navigate} = this.props.navigation;
    const self = this;

    navigate('Register', {
      form: this.state,
      onDone: (username) => {
        self.setState({username});
      }
    })
  }

  _goToHome() {
    const {navigate} = this.props.navigation;
    navigate('PostLogin');
  }

  render() {
    return (
      <View style={[containers.container, styles.scroll]}>
        <View style={[{flex: 1}]}>

          <View style={styles.header}>
            <Text style={text.title}>
              Encuentrame
            </Text>
          </View>

          <View style={styles.loginForm}>
            <TextInput
              value={this.state.username}
              placeholder="Usuario"
              ref="usuario"
              style={styles.textInput}
              selectTextOnFocus
              autoCapitalize='none'
              returnKeyType='next'
              onSubmitEditing={() => this.refs.password.focus()}
              onChangeText={this._handleUsernameTextChange}
            />
            <TextInput
              value={this.state.password}
              placeholder="Contraseña"
              ref="password"
              style={styles.textInput}
              autoCapitalize='none'
              autoCorrect={false}
              secureTextEntry
              returnKeyType="go"
              onChangeText={this._handlePasswordTextChange}
              onSubmitEditing={this._handleLoginButtonPress}
            />
          </View>

          <View style={{flex: 1}}>
            <View style={styles.actionButtons}>
              <Button
                title="Login"
                style={styles.Login}
                onPress={this._handleLoginButtonPress}
              />

              <Text
                onPress={this._handleRegisterTextPress}
                style={styles.notRegistered}>
                No estoy registrado
              </Text>
            </View>
          </View>

          <View style={{height: 0, alignItems: "center"}}>
            <Text style={{fontSize: 14, color: "gray"}}>
              {`${name} v${version}`}
            </Text>
          </View>
          {/* The next view will animate to match the actual keyboards height */}
          <KeyboardSpacer/>
        </View>
      </View>
    )
  }
}

const styles = StyleSheet.create({
  header: {
    flex: 1,
  },
  loginForm: {
    flex: 2,
  },
  actionButtons: {
    bottom: 0
  },
  textInput: {
    width: 200,
    height: 44,
    padding: 8
  },
  Login: {
    // marginTop: 5, //200
    // marginBottom: 10
  },
  scroll: {
    padding: 30,
  },
  notRegistered: {
    textAlign: 'center',
    marginTop: 15,
    fontSize: 16
  }
});
