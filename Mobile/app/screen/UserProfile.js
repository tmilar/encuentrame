import React, {Component} from 'react';
import {Button, StyleSheet, Image, View, Alert, TextInput} from 'react-native';
import { ImagePicker } from 'expo';
import UserService from '../service/UserService';
import AccountsService from '../service/AccountsService';
import LoadingIndicator from "../component/LoadingIndicator";
import {showToast} from "react-native-notifyer";

export default class UserProfile extends Component {
  state = {
    name: null,
    lastName: null,
    image: null,
    imageChanged: false,
    saveChanges: false,
    loading: false
  };

  _pickImage = async() => {
    // Display the camera to the user and wait for them to take a photo or to cancel
    // the action
    let result = await ImagePicker.launchImageLibraryAsync({
      allowsEditing: true,
      aspect: [3, 3],
      quality: 0.3
    });

    if (result.cancelled) {
      return;
    } else {
      this.setState({ imageChanged: true });
      this.setState({ image: result.uri });
    }
  };

  _saveChanges = async () => {
    this.setState({loading: true});
    let modifiedFields = {
      Firstname: this.state.name || this.userData.Firstname,
      Lastname: this.state.lastName || this.userData.Lastname
    };
    let modifiedProfile = Object.assign(this.userData, modifiedFields);
    try {
      console.log("Guardando cambios en user profile...");
      if (this.state.imageChanged){
        await this._saveImage();
      }
      await AccountsService.updateAccount(modifiedProfile);
    } catch (e) {
      console.error("Error al actualizar el perfil de usuario: ", e);
      Alert.alert("Error", "¡Ups! Ocurrió un error al actualizar el perfil de usuario. \n" + (e.message || e));
      return;
    } finally {
      this.setState({loading: false});
    }
    showToast("¡Datos actualizados exitosamente!", {duration: 2000});
    this._goBack();
  };

  _saveImage = async() => {
    if (!this.state.imageChanged)
      return;
    // ImagePicker saves the taken photo to disk and returns a local URI to it
    let localUri = this.state.image;
    let filename = localUri.split('/').pop();

    // Infer the type of the image
    let match = /\.(\w+)$/.exec(filename);
    let type = match ? `image/${match[1]}` : `image`;

    // Upload the image using the fetch and FormData APIs
    let formData = new FormData();
    // Assume "photo" is the name of the form field the server expects
    formData.append('image', { uri: localUri, name: filename, type });
    try {
      await UserService.uploadUserProfileImage(formData);
    } catch (e) {
      let errMessage = e.message || e;
      Alert.alert("Error al subir la foto", errMessage);
    }
  };

  componentWillMount = async () => {
    let userImg = await UserService.getLoggedUserImgUrl();
    //NECESARIO para evitar que react native cachee eternamente la imagen. No hay fix a esto por ahora para react, solo este workaround de cambiar la URI.....
    this.setState({ image: userImg + "?rand=" + Math.random().toString() });
    this.userData = await AccountsService.getLoggedUserAccount();
    this.setState({
      name: this.userData.Firstname,
      lastName: this.userData.Lastname
    });
  };
  _goBack = () => {
    this.setState({saveChanges: false});
    this.setState({imageChanged: false});
    this.props.navigation.goBack(null);
  };

  _handleNameChange = (name) => {
    this.setState({name});
    this._setDataChanged();
  };

  _handleLastNameChange = (lastName) => {
    this.setState({lastName});
    this._setDataChanged();
  };

  _setDataChanged = () => {
    this.setState({saveChanges: true})
  };

  _renderActionButtons = () => {
    let showSaveButton = this.state.saveChanges || this.state.imageChanged;
    let buttons = <View style={{flex: 1, flexDirection: 'row', justifyContent: 'space-around'}}>
      {showSaveButton &&
      <View style={{width: 100, margin: 3}}>
        <Button
          title="Aceptar"
          onPress={this._saveChanges}
        />
      </View>}
      <View style={{width: 100, margin: 3}}>
        <Button
          title="Volver"
          color='#ff5c5c'
          onPress={this._goBack}
        />
      </View>

    </View>;
    return buttons;
  };

  render(){
    let { image } = this.state;
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }
    return (
      <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
        <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center', paddingTop: 60 }}>
          <TextInput
            value={this.state.name}
            placeholder="Nombre"
            style={styles.textInput}
            selectTextOnFocus
            autoCapitalize='none'
            returnKeyType='next'
            onChangeText={this._handleNameChange}
          />
          <TextInput
            value={this.state.lastName}
            placeholder="Apellido"
            style={styles.textInput}
            autoCapitalize='none'
            autoCorrect={false}
            returnKeyType="go"
            onChangeText={this._handleLastNameChange}
          />
        </View>
        <View style={{ flex: 6, alignItems: 'center', justifyContent: 'center' }}>
          <Button
            title="¡Elige tu foto de usuario!"
            onPress={this._pickImage}
          />
          <Image source={{ uri: image }} style={{ width: 200, height: 200 }} />
        </View>

        {this._renderActionButtons()}
      </View>
    );
  }

}

const styles = StyleSheet.create({
  textInput: {
    width: 200,
    height: 44,
    padding: 8
  }
});
