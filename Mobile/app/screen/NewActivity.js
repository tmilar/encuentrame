import React, {Component} from 'react';
import {Alert, Modal, StyleSheet, Text, View, Picker, TextInput, Button} from 'react-native';
import {text} from '../style';
import { MapView } from 'expo';

const NewActivity = React.createClass({

  getInitialState () {
    return {
      activityName: "",
      selectedEventId: 0,
      selectedEventName: "Selecciona un evento"
    };
  },

  setModalVisible(visible) {
    this.setState({modalVisible: visible});
  },

  _handleActivitynameTextChange(inputValue) {
    this.setState({activityName: inputValue})
  },
  _handleCreateActivityButtonPress() {
    //TODO retrieve and save activity data
    this._goToHome();
  },
  _handleCancelActivityCreation() {
    this._goToHome();
  },
  _goToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  },

  /*_backToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  }*/

  componentDidMount() {
    this.setModalVisible(!this.state.modalVisible)
  },

  render() {
    var mapStyles = [
      {
        "featureType": "administrative",
        "elementType": "geometry",
        "stylers": [
          {
            "visibility": "off"
          }
        ]
      },
      {
        "featureType": "poi",
        "stylers": [
          {
            "visibility": "off"
          }
        ]
      },
      {
        "featureType": "road",
        "elementType": "labels.icon",
        "stylers": [
          {
            "visibility": "off"
          }
        ]
      },
      {
        "featureType": "transit",
        "stylers": [
          {
            "visibility": "off"
          }
        ]
      }
    ];
    return (
      <View style={{marginTop: 22}}>
        <Modal
          animationType={"slide"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={() => {/* handle modal 'back' close? */
          }}
        >
          <View style={{flex: 1}}>
            <Text style={[text.p, styles.activityTitle]}>
              Nueva Actividad
            </Text>
            <View style={{flex: 2, flexDirection:'column', justifyContent:'flex-start', alignItems: "center"}}>
              <TextInput
                value={this.state.activityName}
                placeholder="Nombre de la actividad"
                ref="activityName"
                style={styles.activityName}
                selectTextOnFocus
                onChangeText={this._handleActivitynameTextChange}
              />
              <Text style={text.p}>
                {this.state.selectedEventName || "Si vas a un evento eligelo"}
              </Text>
              <Picker
                selectedValue={this.state.selectedEventId}
                style={styles.picker}
                onValueChange={(itemValue, itemIndex) => this.setState({selectedEventId: itemValue})}
                color= "red"
              >
                <Picker.Item label="Recital del Indio" value="1"/>
                <Picker.Item label="Boca-RiBer" value="2"/>
              </Picker>

            </View>
            <Text style={text.p}>
              Ubicacion de la actividad?
            </Text>
            <MapView style={styles.map}
                     customMapStyle={mapStyles}
                     initialRegion={{
                       latitude: 37.78825,
                       longitude: -122.4324,
                       latitudeDelta: 0.0922,
                       longitudeDelta: 0.0421,
                     }}
            />
            <View style={[styles.footer, {flexDirection: "row", justifyContent: "center" , flexWrap: "wrap"}]}>
              <View style={{width: 300 , flex: 1}}>
                <Button
                  title="Crear Actividad"
                  onPress={this._handleCreateActivityButtonPress}
                />
              </View>

              <View style={{width: 300 , flex: 1}}>
                <Button
                  color="grey"
                  title="Cancelar"
                  onPress={this._handleCancelActivityCreation}
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
  activityName: {
    width: 200,
    textAlign: 'center',
    paddingBottom: 10
  },
  activityTitle: {
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

export default NewActivity;
