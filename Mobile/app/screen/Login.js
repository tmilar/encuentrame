import React, {Component} from 'react'
import {Text, View, StyleSheet, Button, Alert, TextInput} from 'react-native'
import UserService from '../service/UserService';
import ReactNative, {ScrollView} from 'react-native';
import SessionService from '../service/SessionService';
export default class Login extends Component {

  constructor(props) {
    super(props);

    this.state = {
      username: '',
      password: ''
    };

    this._clearForm = this._clearForm.bind(this);
    this._validateLogin = this._validateLogin.bind(this);
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

  async _validateLogin() {

    if (this.state.username === '' || this.state.password === '') {
      throw "Username or password can't empty!";
    }

    const credentials = {
      username: this.state.username,
      password: this.state.password
    };

    let result = await UserService.checkCredentials(credentials);

    console.log(`Logged! ->`, result.ok);

    return result;
  }

  async _handleLoginButtonPress() {
    let loginResult;
    try {
      loginResult = await this._validateLogin();
      if (loginResult.ok){
        await SessionService.setSessionToken(this.state.username);
      }
    } catch (e) {
      console.log("Login error: ", e);
      Alert.alert(
        'Error',
        `Nombre de usuario o password incorrecto?`
      );
      return;
    }
    if (loginResult.ok){
      Alert.alert(
        'Login!',
        `Bienvenido, ${this.state.username}!`
      );
      this._clearForm();
      this._goToHome();
    } else {
      Alert.alert(
        'Error',
        `Error en las credenciales!`
      );
    }



  }

  _handleUsernameTextChange(inputValue) {
    this.setState({username: inputValue})
  }

  _handlePasswordTextChange(inputValue) {
    this.setState({password: inputValue})
  }

  async componentWillMount() {
    let sessionAlive = await SessionService.isSessionAlive();
    if (sessionAlive) {
      this._goToHome();
    }
  }

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

  static navigationOptions = {
    title: 'Login',
    // header: null
  };

  _goToHome() {
    const {navigate} = this.props.navigation;
    navigate('PostLogin');
  }

  inputFocused(refName) {
    setTimeout(() => {
      let scrollResponder = this.refs.scrollView.getScrollResponder();
      scrollResponder.scrollResponderScrollNativeHandleToKeyboard(
        ReactNative.findNodeHandle(this.refs[refName]),
        110, //additionalOffset
        true
      );
    }, 50);
  }


  render() {
    return (
      <View style={styles.container}>

        <ScrollView ref='scrollView'
                    style={styles.scroll}>
          <View style={styles.header}>
            <Text style={styles.paragraph}>
              Encuentrame
            </Text>
          </View>

          <View style={styles.content}>
            <TextInput
              value={this.state.username}
              placeholder="Usuario"
              ref="usuario"
              style={styles.textInput}
              onFocus={this.inputFocused.bind(this, 'usuario')}
              keyboardType="text"
              selectTextOnFocus
              onChangeText={this._handleUsernameTextChange}
            />

            <TextInput
              value={this.state.password}
              placeholder="Contraseña"
              ref="password"
              style={styles.textInput}
              secureTextEntry
              returnKeyType="done"
              onFocus={this.inputFocused.bind(this, 'password')}
              onChangeText={this._handlePasswordTextChange}
              onSubmitEditing={this._handleLoginButtonPress}
            />
          </View>

          <View style={styles.footer}>
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

        </ScrollView>


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
  header: {
    flex: 1,
    height: 100,
  },
  content: {
    flex: 4,
    height: 400,
  },
  footer: {
    flex: 1,
    height: 100,
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
  },
  Login: {
    marginTop: 200
  },
  scroll: {
    padding: 30,
    flexDirection: 'column'
  },
  notRegistered: {
    textAlign: 'center'
  }
});
