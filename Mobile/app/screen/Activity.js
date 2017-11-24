import React, {Component} from 'react';
import {
  Modal, StyleSheet, Alert, Text, View, Picker, TextInput, Button, TouchableOpacity, TouchableHighlight
} from 'react-native';
import {text} from '../style';
import EventsService from '../service/EventsService';
import GeolocationService from '../service/GeolocationService';
import ActivityService from '../service/ActivityService';
import {hideLoading, showLoading, showToast} from "react-native-notifyer";
import DateTimePicker from 'react-native-modal-datetime-picker';
import ModalMap from './ModalMap';
import LoadingIndicator from "../component/LoadingIndicator";
import {Icon} from 'react-native-elements';
import ActivityDetailsContentView from './ActivityDetailsContentView';
import formatDateForBackend from "../util/formatDateForBackend";
import {prettyDate} from "../util/prettyDate";

export default class Activity extends Component {

  state = {
    activityName: "",
    activeActivity: false,
    showActivityDetails: true,
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
      latitude: null,
      longitude: null
    },
    locationOk: false
  };

  formFields = [
    {name: "activityName", errorMsg: "¡Nombre incompleto!"},
    {name: "selectedEventId", errorMsg: "¡Selecciona un evento!"},
    {name: "startDate", errorMsg: "¡Elija fecha de inicio!"},
    {name: "endDate", errorMsg: "¡Elija fecha de fin!"},
    {name: "locationOk", errorMsg: "¡Elija ubicación!"}
  ];

  _showStartDateTimePicker = () => {
    if (this._eventsAvailable())
      return;
    this.setState({isStartDateTimePickerVisible: true});
  };

  _hideStartDateTimePicker = () => {
    this.setState({isStartDateTimePickerVisible: false});
  };

  _showEndDateTimePicker = () => {
    if (this._eventsAvailable())
      return;
    this.setState({isEndDateTimePickerVisible: true});
  };

  _hideEndDateTimePicker = () => {
    this.setState({isEndDateTimePickerVisible: false});
  };

  _handleActivityLocationButtonpress = () => {
    this.setState({showMapLocation: true});
  };

  _handleStartDatePicked = (startDate) => {
    let formattedDate = formatDateForBackend(startDate);
    this.setState({startDate: formattedDate});
    console.log('A date has been picked: ', startDate);
    this._hideStartDateTimePicker();
  };

  _handleEndDatePicked = (endDate) => {
    let formattedDate = formatDateForBackend(endDate);
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
      this.props.activityCreated();
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

  _goBack = () => {
    this.props.goBack(null);
  };
  _loadActiveActivity = (activeActivity) => {
    this.setState({activeActivity: true});
    this.setState({showActivityDetails: false});
    this.setState({selectedEventId: activeActivity.EventId});
    this.setState({activityName: activeActivity.Name});
    this.saveActivityLocation({latitude: activeActivity.Latitude, longitude: activeActivity.Longitude});
    this.setState({startDate: activeActivity.BeginDateTime});
    this.setState({endDate: activeActivity.EndDateTime});
  };

  _getSelectedEvent = () => {
    return this.state.events.find((evt) => {return evt.Id === this.state.selectedEventId;});
  };

  _eventsAvailable = () => {
    return this.state.events.length > 0
  };

  componentWillMount = async () => {
    this.state.loading = true;
    try {
      let events = await EventsService.getEvents();
      this.setState({events});
      let activeActivity = await ActivityService.activeEvent();
      if (activeActivity){
        this.activeActivity = activeActivity;
        this._loadActiveActivity(activeActivity);
      } else if (this._eventsAvailable()){
        this.setEvent(events[0].Id);
      }
    } catch (e) {
      console.log("Error retrieving events from server: ", e);
      Alert.alert(
        "Error",
        'Error al cargar información de Eventos existentes. \n' +
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
      },
      locationOk: true
    })
  };

  setEvent= (eventId) => {
    this.setState({selectedEventId: eventId});
    let selectedEvent = this.state.events.find((evt) => {return evt.Id == eventId;});
    this.setState({activityName: selectedEvent.Name});
    this.saveActivityLocation({latitude: selectedEvent.Latitude, longitude: selectedEvent.Longitude});
    this.setState({startDate: selectedEvent.BeginDateTime});
    this.setState({endDate: selectedEvent.EndDateTime});
  };

  _handleEventSelected = (itemValue) => {
    this.setEvent(itemValue);
  };

  _getTitle = () => {
    let title = "Nueva Actividad";
    if (this.state.activeActivity){
      title = "Actividad actual: " +this.activeActivity.Name;
    }
    return title;
  };

  _deleteActivity = async() => {
    try {
      this.setState({loading: true});
      await ActivityService.deleteActivity(this.activeActivity.Id);
      showToast("Actividad eliminada.", {duration: 5000});
      this.props.activityDeleted();
      this._goBack();
    } catch (e) {
      console.log("Error deleting activity in server: ", e);
      Alert.alert(
        'Ocurrió un problema al eliminar la actividad. ',
        e.message || e
      );
    }
  };

  _cancelActivityButtonpress = async() => {
    Alert.alert(
      'Confirma',
      '¿Eliminar la Actividad?',
      [
        {text: 'Cancelar', onPress: () => {}, style: 'cancel'},
        {text: 'Confirmar', onPress: () => this._deleteActivity()},
      ],
      { cancelable: false }
    )
  };

  _getNewActivityHeader = () => {
    return <View style={{justifyContent: "space-around", backgroundColor: "#3db395ff", height: 70}}>
        <Text style={styles.activityTitle}>
          {this._getTitle()}
        </Text>
      </View>;
  };

  _getActiveActivityHeader = () => {
    return <View style={{justifyContent: "space-around", backgroundColor: "#3db395ff", height: 70}}>
        <View style={{justifyContent: "space-around", flexDirection: "row"}}>
          <View style={{justifyContent: "space-around"}}>
            <Text style={styles.activityTitle}>
              {this._getTitle()}
            </Text>
          </View>
          <View style={{justifyContent: "space-around"}}>
            <TouchableHighlight style={{flex: 1, height: 50 }} onPress={this._cancelActivityButtonpress}>
              <View>
                <Icon name="delete" size={35} color='white'/>
              </View>
            </TouchableHighlight>
          </View>
        </View>
      </View>;
  };

  _getNewActivityFooter = () => {
    return <View style={[styles.footer, {flex: 1, flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap", borderWidth: 1, borderColor: 'white'}]}>
      <View style={{justifyContent: "space-around", flex: 1, marginLeft: 5, marginRight: 5}}>
        <Button
          title="Crear Actividad"
          onPress={this._handleCreateActivityButtonPress}
          color="#063450"
        />
      </View>
    </View>;
  };

  _renderActivityContent = () => {
    return this.state.activeActivity ?
      <ActivityDetailsContentView activeActivity={this.activeActivity} activeEvent={this._getSelectedEvent()}/>
      :
      this._renderNewActivityContent();
  };

  _renderNewActivityContent = () => {
    return <View style={{flex: 1}}>
          <View style={{
            flex: 0.3,
            flexDirection: 'column',
            justifyContent: 'space-around',
            alignItems: "center",
            borderBottomColor: '#47315a',
            borderBottomWidth: 1
          }}>
            <Text>
              Evento:
            </Text>
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
            flexDirection: 'column',
            justifyContent: 'flex-start',
            alignItems: "center",
            borderBottomColor: '#47315a',
            borderBottomWidth: 1,
            paddingTop: 10
          }}>
            <Text>
              Nombre de la actividad:
            </Text>
            <TextInput
              value={this.state.activityName}
              placeholder="Nombre de la actividad"
              ref="activityName"
              style={styles.activityName}
              editable={!this._eventsAvailable()}
              selectTextOnFocus
              onChangeText={this._handleActivitynameTextChange}
              underlineColorAndroid='transparent'
            />
          </View>
          <View style={{
            flex: 0.25,
            justifyContent: "space-around",
            borderBottomColor: '#47315a',
            borderBottomWidth: 1
          }}>
            <View style={{flex: 1, flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap"}}>
              <View style={{justifyContent: "space-around", width: 150}}>
                <Button
                  style={{width: 100, height: 50}}
                  color="#063450"
                  title="Ubicacion:"
                  onPress={this._handleActivityLocationButtonpress}
                />
              </View>
              {this.state.locationOk > 0 && <View style={{justifyContent: "space-around", width: 40}}>
                <Icon name="done" size={25} color='green'/>
              </View>}
            </View>
            {
              this.state.showMapLocation &&
              <ModalMap saveActivityLocation={this.saveActivityLocation} currentLocation={this.state.activityLocation} enableEditing={!this._eventsAvailable()}
                        onClose={() => this.setState({showMapLocation: false})}/>
            }
          </View>
          <View
            style={{flex: 0.4, justifyContent: "space-around", borderBottomColor: '#47315a', borderBottomWidth: 1}}>
            <Text style={[text.p, styles.activityDatesLabel]}>
              Fecha - Horario:
            </Text>
            <View style={{flex: 1, flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap"}}>
              <View style={{flex: 1}}>
                <TouchableOpacity style={{flex: 1, alignSelf: "center"}} onPress={this._showStartDateTimePicker}>
                  <Text>{this.state.startDate ? prettyDate(new Date(this.state.startDate)) : "Fecha inicio"}</Text>
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
                  <Text>{this.state.endDate ? prettyDate(new Date(this.state.endDate)) : "Fecha fin"}</Text>
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
        </View>;
  };

  render() {
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }
    return (
      <View style={{flex: 1}}>
          <View style={{flex: 1}}>
            {this.state.activeActivity ? this._getActiveActivityHeader() : this._getNewActivityHeader()}
            <View style={{flex: 6}}>
              {this._renderActivityContent()}
            </View>
            {!this.state.activeActivity && this._getNewActivityFooter()}
          </View>
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
    backgroundColor: "#3db395ff",
    color: "white",
    textAlign: 'center',
    alignSelf: "center",
    textAlignVertical: "center",
    fontSize: 18
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
    margin: 5
  }
});
