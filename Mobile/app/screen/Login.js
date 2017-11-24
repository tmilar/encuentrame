import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput, Keyboard, Image} from 'react-native'
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
    password: '',
    showFooter: true
  };

  constructor(props) {
    super(props);

    this._clearForm = this._clearForm.bind(this);
    this._doLogin = this._doLogin.bind(this);
    this._handleUsernameTextChange = this._handleUsernameTextChange.bind(this);
    this._handlePasswordTextChange = this._handlePasswordTextChange.bind(this);
    this._handleLoginButtonPress = this._handleLoginButtonPress.bind(this);
    this._handleRegisterTextPress = this._handleRegisterTextPress.bind(this);
    this._handleKeyBoardToggle = this._handleKeyBoardToggle.bind(this);
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

  _handleKeyBoardToggle(visible) {
    this.setState({showFooter: !visible});
  }

  render() {
    return (
      <View style={[containers.container, styles.scroll, {backgroundColor: '#3CB393'}]}>
        <View style={[{flex: 1,justifyContent: 'space-around'}]}>

          <View style={styles.header}>
            <Image
              resizeMode="contain"
              style={{height: "100%"}}
              source={require('../img/eme_final2.png')} />
          </View>

          <View style={styles.loginForm}>
            <TextInput
              value={this.state.username}
              placeholder="Usuario"
              ref="usuario"
              style={[styles.textInput, {color: 'black' }]}
              selectTextOnFocus
              placeholderTextColor="black"
              underlineColorAndroid="transparent"
              autoCapitalize='none'
              returnKeyType='next'
              onSubmitEditing={() => this.refs.password.focus()}
              onChangeText={this._handleUsernameTextChange}
            />
            <TextInput
              value={this.state.password}
              placeholder="Contraseña"
              ref="password"
              style={[styles.textInput, {color: 'black' }]}
              autoCapitalize='none'
              autoCorrect={false}
              placeholderTextColor="black"
              underlineColorAndroid="transparent"
              secureTextEntry
              returnKeyType="go"
              onChangeText={this._handlePasswordTextChange}
              onSubmitEditing={this._handleLoginButtonPress}
            />
          </View>

          {this.state.showFooter && <View style={{flex: 1, justifyContent: 'space-around', alignItems: 'center'}}>
            <View style={styles.actionButtons}>
              <Button
                title="Login"
                color="#063450"
                style={styles.Login}
                onPress={this._handleLoginButtonPress}
              />

              <Text
                onPress={this._handleRegisterTextPress}
                style={styles.notRegistered}>
                No estoy registrado
              </Text>
            </View>
          </View>}

          {this.state.showFooter && <View style={{height: 0, alignItems: "center"}}>
            <Text style={{fontSize: 14, color: "gray"}}>
              {`${name} v${version}`}
            </Text>
          </View>
          }
          {/* The next view will animate to match the actual keyboards height */}
          <KeyboardSpacer
            onToggle={this._handleKeyBoardToggle}
          />
        </View>
      </View>
    )
  }
}

const styles = StyleSheet.create({
  header: {
    flex: 1.5
  },
  loginForm: {
    flex: 2,
    justifyContent: 'center',
    alignItems: 'center'
  },
  actionButtons: {
    bottom: 0,
    width: 250
  },
  textInput: {
    width: 200,
    height: 44,
    padding: 8,
    backgroundColor: 'white',
    borderRadius: 10,
    margin: 5
  },
  Login: {
    backgroundColor: '#063450',
    width: 250
  },
  scroll: {
    padding: 30,
  },
  notRegistered: {
    textAlign: 'center',
    marginTop: 15,
    fontSize: 16,
    color: '#063450',
    textDecorationLine: 'underline'
  }
});
