import React from 'react';
import {Modal, StyleSheet, Alert, Text, View, Picker, TextInput, Button} from 'react-native';
import {text} from '../style';
import {MapView} from 'expo';
import GeolocationService from '../service/GeolocationService';
import {hideLoading, showLoading} from "react-native-notifyer";
import mapStyles from '../style/map';
import LoadingIndicator from "../component/LoadingIndicator";

const SupplyInfo = React.createClass({

  getInitialState() {
    const bsasCoordinates = GeolocationService.getBsAsRegion();

    return {
      textInfo: "",
      initialMapRegionCoordinates: bsasCoordinates
    };
  },

  setModalVisible(visible) {
    this.setState({modalVisible: visible});
  },

  _handleTextInfoChange(inputValue) {
    this.setState({textInfo: inputValue})
  },

  async _handleSupplyInfoButtonPress() {
    showLoading("Aportando datos!...");

    let info = {
      text: this.state.textInfo,
      latitude: this.state.lastSeenLocation.latitude,
      longitude: this.state.lastSeenLocation.longitude
    };

    try {
      //TODO create SupplyInfoService as soon as there is an API definition
      console.log("Aportando datos..");
    } catch (e) {
      console.log("Error..");
      return;
    }
    hideLoading();
    Alert.alert(
      'Gracias por tu ayuda!'
    );
    this._goToHome();
  },

  _handleCancelSupplyInfo() {
    this._goToHome();
  },

  _goToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  },

  async componentWillMount() {
    this.setState({loading: true});
    try {
      let deviceLocation = await GeolocationService.getDeviceLocation({enableHighAccuracy: true});
      let lastSeenLocation = {
        "latitude": deviceLocation.latitude,
        "longitude": deviceLocation.longitude
      };
      this.setState({"lastSeenLocation": lastSeenLocation  });
    } catch (e) {
      console.log("Error retrieving location from device: ", e);
      Alert.alert(
        'Error retrieving location from device.',
        e.message || e
      );
    } finally {
      this.setState({loading: false});
    }
  },

  componentDidMount() {
    this.setModalVisible(!this.state.modalVisible)
  },

  render() {
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }

    return (
      <View style={{marginTop: 22}}>
        <Modal
          animationType={"fade"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={() => {/* handle modal 'back' close? */
          }}
        >
          <View style={{flex: 1}}>
            <Text style={[text.p, styles.supplyInfoTitle]}>
              Ayuda a encontrar a esta persona!
            </Text>
            <View style={{flex: 2, flexDirection: 'column', justifyContent: 'flex-start', alignItems: "center"}}>
              <TextInput
                value={this.state.textInfo}
                placeholder="Como estaba?"
                ref="textInfo"
                style={styles.textInfo}
                selectTextOnFocus
                onChangeText={this._handleTextInfoChange}
              />

            </View>
            <Text style={text.title}>
              Donde lo/la viste?
            </Text>
            <MapView style={styles.map}
                     customMapStyle={mapStyles}
                     initialRegion={this.state.initialMapRegionCoordinates}
            >
              <MapView.Marker draggable
                              coordinate={this.state.lastSeenLocation}
                              title={"Donde fue?"}
                              onDragEnd={(e) => this.setState({ lastSeenLocation: e.nativeEvent.coordinate })}
              />
            </MapView>
            <View style={[styles.footer, {flexDirection: "row", justifyContent: "center", flexWrap: "wrap"}]}>
              <View style={{flex: 0.5}}>
                <Button
                  title="Aportar Datos"
                  onPress={this._handleSupplyInfoButtonPress}
                />
              </View>

              <View style={{flex: 0.5}}>
                <Button
                  color="grey"
                  title="Cancelar"
                  onPress={this._handleCancelSupplyInfo}
                />
              </View>

            </View>
          </View>

        </Modal>

      </View>
    )
  }
});

const styles = StyleSheet.create({
  message: {
    flex: 5,
    height: 3,
    alignItems: 'center',
    justifyContent: 'center',
  },
  picker: {
    width: 200,
    paddingBottom: 10
  },
  textInfo: {
    width: 200,
    height: 90,
    textAlign: 'center',
    paddingBottom: 10
  },
  supplyInfoTitle: {
    backgroundColor: "#2962FF",
    color: "white",
    flex: 1,
    width: 400,
    alignSelf: "center",
    textAlignVertical: "center"
  },
  map: {
    flex: 3,
    margin: 50
  }
});

export default SupplyInfo;
