import React from 'react';
import {Modal, StyleSheet, Alert, Text, View, Picker, TextInput, Button} from 'react-native';
import {text} from '../style';
import {MapView} from 'expo';
import EventsService from '../service/EventsService';
import GeolocationService from '../service/GeolocationService';
import ActivityService from '../service/ActivityService';
import {hideLoading, showLoading} from "react-native-notifyer";
import mapStyles from '../style/map';

const NewActivity = React.createClass({

  getInitialState() {
    const bsasCoordinates = GeolocationService.getBsAsRegion();

    return {
      activityName: "",
      selectedEventId: 0,
      selectedEventName: "Selecciona un evento",
      events: [],
      initialMapRegionCoordinates: bsasCoordinates,
      activityLocation: {
        latitude: 0,
        longitude: 0
      }
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
    let duration = 5 * 60 * 60 * 1000;
    endDateTime.setTime(endDateTime.getTime() + duration);

    // TODO add real input for activity latitude/longitude selection point.
    let activity = {
      name: this.state.activityName,
      latitude: this.state.activityLocation.latitude,
      longitude: this.state.activityLocation.longitude,
      beginDateTime: beginDateTime,
      endDateTime: endDateTime,
      eventId: this.state.selectedEventId
    };

    try {
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
    } catch (e) {
      console.log("Error retrieving events from server: ", e);
      Alert.alert(
        'Error al cargar información de Eventos existentes.',
        e.message || e
      );
    }

    try {
      let deviceLocation = await GeolocationService.getDeviceLocation({enableHighAccuracy: true});
      let activityLocation = {
        latitude: deviceLocation.latitude,
        longitude: deviceLocation.longitude
      };
      this.setState({activityLocation});
    } catch (e) {
      console.log("Error getting device location: ", e);
      Alert.alert(
        'Error al obtener la ubicación del dispositivo.',
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
      return null;
    }
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
            <View style={{flex: 2, flexDirection: 'column', justifyContent: 'flex-start', alignItems: "center"}}>
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
                color="red"
              >
                {this.state.events.map((event, i) => {
                  return (
                    <Picker.Item label={event.Name} key={`event_${i}`} value={event.Id}/>
                  )
                })}
              </Picker>

            </View>
            <Text style={text.title}>
              Ubicacion de la actividad?
            </Text>
            <MapView style={styles.map}
                     customMapStyle={mapStyles}
                     initialRegion={this.state.initialMapRegionCoordinates}
            >
              <MapView.Marker draggable
                              coordinate={this.state.activityLocation}
                              title={"Actividad"}
                              onDragEnd={(e) => this.setState({activityLocation: e.nativeEvent.coordinate})}
              />
            </MapView>
            <View style={[styles.footer, {flexDirection: "row", justifyContent: "center", flexWrap: "wrap"}]}>
              <View style={{flex: 0.5}}>
                <Button
                  title="Crear Actividad"
                  onPress={this._handleCreateActivityButtonPress}
                />
              </View>

              <View style={{flex: 0.5}}>
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
