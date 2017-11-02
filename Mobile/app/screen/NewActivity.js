import React, {Component} from 'react';
import {
  Modal, StyleSheet, Alert, Text, View, Picker, TextInput, Button, TouchableOpacity
} from 'react-native';
import {text} from '../style';
import EventsService from '../service/EventsService';
import GeolocationService from '../service/GeolocationService';
import ActivityService from '../service/ActivityService';
import {hideLoading, showLoading} from "react-native-notifyer";
import DateTimePicker from 'react-native-modal-datetime-picker';
import ModalMap from './ModalMap';
import LoadingIndicator from "../component/LoadingIndicator";

export default class NewActivity extends Component {

  state = {
    activityName: "",
    selectedEventId: null,
    selectedEventName: "Selecciona un evento",
    events: [],
    startDate: null,
    endDate: null,
    isStartDateTimePickerVisible: false,
    isEndDateTimePickerVisible: false,
    showMapLocation: false,
    initialMapRegionCoordinates: GeolocationService.getBsAsRegion(),
    activityLocation: {
      latitude: 0,
      longitude: 0
    }
  };

  formFields = [
    {name: "activityName", errorMsg: "Nombre incompleto!"},
    {name: "selectedEventId", errorMsg: "Selecciona un evento!"},
    {name: "startDate", errorMsg: "Elija fecha de inicio!"},
    {name: "endDate", errorMsg: "Elija fecha de fin!"}
  ];

  _showStartDateTimePicker = () => {
    this.setState({isStartDateTimePickerVisible: true});
  };

  _hideStartDateTimePicker = () => {
    this.setState({isStartDateTimePickerVisible: false});
  };

  _showEndDateTimePicker = () => {
    this.setState({isEndDateTimePickerVisible: true});
  };

  _hideEndDateTimePicker = () => {
    this.setState({isEndDateTimePickerVisible: false});
  };

  _handleActivityLocationButtonpress = () => {
    this.setState({showMapLocation: true});
  };

  _formatDateForBackend = (date) => {
    return date.toISOString().slice(0, 19).replace(/T/g, " ");
  };

  _handleStartDatePicked = (startDate) => {
    let formattedDate = this._formatDateForBackend(startDate);
    this.setState({startDate: formattedDate});
    console.log('A date has been picked: ', startDate);
    this._hideStartDateTimePicker();
  };

  _handleEndDatePicked = (endDate) => {
    let formattedDate = this._formatDateForBackend(endDate);
    this.setState({endDate: formattedDate});
    console.log('A date has been picked: ', endDate);
    this._hideEndDateTimePicker();
  };

  _isString = (value) => {
    return typeof value === 'string' || value instanceof String;
  };

  _validateForm = () => {
    let errorMsg = "";
    this.formFields.forEach((formField) => {
      let value = this.state[formField.name];
      if ((value === null ) || ( this._isString(value) && value.length === 0)) {
        errorMsg += formField.errorMsg + "\n";
      }
    });
    return errorMsg;
  };

  setModalVisible = (visible) => {
    this.setState({modalVisible: visible});
  };

  _handleActivitynameTextChange = (inputValue) => {
    this.setState({activityName: inputValue});
  };

  _handleCreateActivityButtonPress = async () => {
    let errorMsg = this._validateForm();
    if (errorMsg.length > 0) {
      Alert.alert(
        'Formulario incorrecto ',
        errorMsg
      );
      return;
    }
    showLoading("Espera...");
    let activity = {
      name: this.state.activityName,
      latitude: this.state.activityLocation.latitude,
      longitude: this.state.activityLocation.longitude,
      beginDateTime: this.state.startDate,
      endDateTime: this.state.endDate,
      eventId: this.state.selectedEventId
    };

    try {
      await ActivityService.createActivity(activity);
    } catch (e) {
      console.log("Error creating activity in server: ", e);
      Alert.alert(
        'Ocurrió un problema al crear la actividad. ',
        e.message || e
      );
      return;
    } finally {
      hideLoading();
    }

    Alert.alert(
      "¡Éxito!",
      'Tu actividad fue creada correctamente.'
    );
    this._goBack();
  };

  _handleCancelActivityCreation = () => {
    this._goBack();
  };

  _goBack = () => {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  };

  componentWillMount = async () => {
    this.state.loading = true;
    try {
      let events = await EventsService.getEvents();
      this.setState({events});
      if (events.length > 0) {
        this.setState({selectedEventId: events[0].Id});
      }
    } catch (e) {
      console.log("Error retrieving events from server: ", e);
      Alert.alert(
        "Error",
        'Error al cargar información de Eventos existentes. \n' +
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
        "Error",
        'Error al obtener la ubicación del dispositivo.\n' +
        e.message || e
      );
    }

    this.setState({loading: false});
  };

  saveActivityLocation = (location) => {
    this.setState({
      activityLocation: {
        latitude: location.latitude,
        longitude: location.longitude
      }
    })
  };

  _handleEventSelected = (itemValue, itemIndex) => {
    this.setState({selectedEventId: itemValue});
  };

  componentDidMount = () => {
    this.setModalVisible(!this.state.modalVisible)
  };

  render() {
    if (this.state.loading) {
      // TODO show loading text message? maybe: waiting for GPS or something?
      return <LoadingIndicator/>;
    }

    return (
      <View style={{marginTop: 22}}>
        <Modal
          animationType={"slide"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={this._handleCancelActivityCreation}
        >
          <View style={{flex: 1}}>
            <Text style={[text.p, styles.activityTitle]}>
              Nueva Actividad
            </Text>
            <View style={{flex: 1.8}}>
              <View style={{
                flex: 0.25,
                flexDirection: 'column',
                justifyContent: 'flex-start',
                alignItems: "center",
                borderBottomColor: '#47315a',
                borderBottomWidth: 1
              }}>
                <TextInput
                  value={this.state.activityName}
                  placeholder="Nombre de la actividad"
                  ref="activityName"
                  style={styles.activityName}
                  selectTextOnFocus
                  onChangeText={this._handleActivitynameTextChange}
                  underlineColorAndroid='transparent'
                />
              </View>

              <View style={{
                flex: 0.3,
                flexDirection: 'column',
                justifyContent: 'space-around',
                alignItems: "center",
                borderBottomColor: '#47315a',
                borderBottomWidth: 1
              }}>

                <Picker
                  selectedValue={this.state.selectedEventId}
                  style={styles.picker}
                  onValueChange={this._handleEventSelected}
                  color="red"
                >
                  {this.state.events.map((event, i) => {
                    return (
                      <Picker.Item label={event.Name} key={event.Id} value={event.Id}/>
                    )
                  })}
                </Picker>

              </View>

              <View style={{
                flex: 0.25,
                justifyContent: "space-around",
                borderBottomColor: '#47315a',
                borderBottomWidth: 1
              }}>
                <View style={{flex: 1, flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap"}}>
                  <Text style={[text.p, styles.activityLocationLabel]}>
                    Donde sera?
                  </Text>
                  <View style={{flex: 1, justifyContent: "space-around"}}>
                    <Button
                      style={{width: 100, height: 50}}
                      title="Mapa"
                      onPress={this._handleActivityLocationButtonpress}
                    />
                  </View>
                </View>
                {
                  this.state.showMapLocation &&
                  <ModalMap saveActivityLocation={this.saveActivityLocation}
                            onClose={() => this.setState({showMapLocation: false})}/>
                }
              </View>

              <View
                style={{flex: 0.4, justifyContent: "space-around", borderBottomColor: '#47315a', borderBottomWidth: 1}}>
                <Text style={[text.p, styles.activityDatesLabel]}>
                  Cuando sera?
                </Text>
                <View style={{flex: 1, flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap"}}>
                  <View style={{flex: 1}}>
                    <TouchableOpacity style={{flex: 1, alignSelf: "center"}} onPress={this._showStartDateTimePicker}>
                      <Text>{this.state.startDate || "Fecha inicio"}</Text>
                    </TouchableOpacity>
                    <DateTimePicker
                      mode="datetime"
                      color="red"
                      isVisible={this.state.isStartDateTimePickerVisible}
                      onConfirm={this._handleStartDatePicked}
                      onCancel={this._hideStartDateTimePicker}
                    />
                  </View>
                  <View style={{flex: 1}}>
                    <TouchableOpacity style={{flex: 1, alignSelf: "center"}} onPress={this._showEndDateTimePicker}>
                      <Text>{this.state.endDate || "Fecha fin"}</Text>
                    </TouchableOpacity>
                    <DateTimePicker
                      mode="datetime"
                      isVisible={this.state.isEndDateTimePickerVisible}
                      onConfirm={this._handleEndDatePicked}
                      onCancel={this._hideEndDateTimePicker}
                    />
                  </View>
                </View>
              </View>


              <View style={[styles.footer, {flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap"}]}>
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

          </View>

        </Modal>

      </View>
    )
  }
}

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
    paddingBottom: 10,
    flex: 1,
    borderColor: '#47315a',
    borderWidth: 0,
    height: 70
  },
  activityTitle: {
    backgroundColor: "#2962FF",
    color: "white",
    flex: 0.15,
    width: 400,
    alignSelf: "center",
    textAlignVertical: "center"
  },
  activityLocationLabel: {
    flex: 1,
    width: 200,
    alignSelf: "center",
    textAlignVertical: "center",
    fontSize: 14
  },
  activityDatesLabel: {
    flex: 0.5,
    width: 200,
    alignSelf: "center",
    textAlignVertical: "center",
    fontSize: 14
  },
  map: {
    flex: 3,
    margin: 50
  }
});
