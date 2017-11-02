import React, {Component} from 'react';
import { Button, Image, View, Alert} from 'react-native';
import { ImagePicker } from 'expo';
import UserService from '../service/UserService';

export default class UserProfile extends Component {
  state = {
    image: null,
  };

  render() {
    let { image } = this.state;
    return (
      <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
        <View style={{ flex: 6, alignItems: 'center', justifyContent: 'center' }}>
          <Button
            title="¡Elige tu foto de usuario!"
            onPress={this._pickImage}
          />
          <Image source={{ uri: image }} style={{ width: 200, height: 200 }} />
        </View>

        <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
          <Button
            title="Guardar cambios"
            onPress={this._saveImage}
          />
        </View>
      </View>
    );
  }

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
      this.setState({ image: result.uri });
    }
  };

  _saveImage = async() => {
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

    this._goBack();
  };

  componentWillMount = async () => {
    let userImg = await UserService.getLoggedUserImgUrl();
    //NECESARIO para evitar que react native cachee eternamente la imagen. No hay fix a esto por ahora para react, solo este workaround de cambiar la URI.....
    this.setState({ image: userImg + "?rand=" + Math.random().toString() });
  };
  _goBack = () => {
    this.props.navigation.goBack(null);
  };
}
