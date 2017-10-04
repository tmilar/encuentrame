import React from 'react';
import {Modal, StyleSheet, Alert, Text, View, Picker, TextInput, Button} from 'react-native';
import {text} from '../style';
import { MapView } from 'expo';
import EventsService from '../service/EventsService';
import GeolocationService from '../service/GeolocationService';
import ActivityService from '../service/ActivityService';
import {hideLoading, showLoading} from "react-native-notifyer";

const NewActivity = React.createClass({

  getInitialState () {
    return {
      activityName: "",
      selectedEventId: 0,
      selectedEventName: "Selecciona un evento",
    };
  },

  setModalVisible(visible) {
    this.setState({modalVisible: visible});
  },

  _handleActivitynameTextChange(inputValue) {
    this.setState({activityName: inputValue})
  },
  async _handleCreateActivityButtonPress() {
    showLoading("Espera...");
    //TODO: add real inputs for activity start and end times. or now its mocked
    let beginDateTime = new Date();
    let endDateTime = new Date();
    //dura 5 horas
    let durationInHours = 5;
    endDateTime.setTime(endDateTime.getTime() + (durationInHours*60*60*1000));
    try {
      let activity = {
        "Name": this.state.activityName,
        "Latitude": this.state.bsasCoordinates.latitude,
        "Longitude": this.state.bsasCoordinates.longitude,
        "BeginDateTime": beginDateTime,
        "EndDateTime": endDateTime,
        "EventId": this.state.selectedEventId
      };
      await ActivityService.createActivity(activity);
    } catch (e) {
      hideLoading();
      console.log("Error creating activity in server: ", e);
      Alert.alert(
        'Ocurrió un problema al crear la actividad. ',
        e.message || e
      );
      return;
    }
    hideLoading();
    Alert.alert(
      'Actividad creada con exito!'
    );
    this._goToHome();
  },
  _handleCancelActivityCreation() {
    this._goToHome();
  },
  _goToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  },

  async componentWillMount() {
    this.setState({loading: true});
    try {
      let events = await EventsService.getEvents();
      this.setState({events});
      let bsasCoordinates = GeolocationService.getBsAsRegion();
      this.setState({bsasCoordinates});
      this.setState({loading: false});
    } catch (e) {
      this.setState({loading: false});
      console.log("Error retrieving events from server: ", e);
      Alert.alert(
        'Error retrieving events from server',
        e.message || e
      );
      return;
    }
  },

  componentDidMount() {
    this.setModalVisible(!this.state.modalVisible)
  },

  render() {
    if (this.state.loading){
      return null;
    }
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
                {this.state.events.map((event, i) => {
                  return (
                    <Picker.Item label={event.Name} value={event.Id}/>
                  )
                })}
              </Picker>

            </View>
            <Text style={text.p}>
              Ubicacion de la actividad?
            </Text>
            <MapView style={styles.map}
                     customMapStyle={mapStyles}
                     initialRegion={{
                       latitude: this.state.bsasCoordinates.latitude,
                       longitude: this.state.bsasCoordinates.longitude,
                       latitudeDelta: this.state.bsasCoordinates.latitudeDelta,
                       longitudeDelta: this.state.bsasCoordinates.longitudeDelta
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
