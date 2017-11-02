import React from 'react';
import {
  Modal, StyleSheet, Alert, Text, View, Picker, TextInput, Button, TouchableOpacity,
  ScrollView
} from 'react-native';
import {text} from '../style';
import {MapView} from 'expo';
import EventsService from '../service/EventsService';
import GeolocationService from '../service/GeolocationService';
import ActivityService from '../service/ActivityService';
import {hideLoading, showLoading} from "react-native-notifyer";
import DateTimePicker from 'react-native-modal-datetime-picker';
import mapStyles from '../config/map';
import LoadingIndicator from "../component/LoadingIndicator";


const ModalMap = React.createClass({

  getInitialState() {
    const bsasCoordinates = GeolocationService.getBsAsRegion();

    return {
      initialMapRegionCoordinates: bsasCoordinates,
      activityLocation: {
        latitude: 0,
        longitude: 0
      },
      loading: true,
      modalVisible: false
    };
  },

  _saveLocation() {
    this.props.saveActivityLocation({
      latitude: this.state.activityLocation.latitude,
      longitude: this.state.activityLocation.longitude
    });
    this._goBack();
  },

  _goBack() {
    this.setState({modalVisible: false});
    this.props.onClose && this.props.onClose();
  },

  async componentWillMount() {
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
      this.setState({modalVisible: true});
      this.setState({loading: false});
    }
  },

  componentDidMount() {
    this.setState({modalVisible: true})
  },

  render() {
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }
    return (
      <View style={{marginTop: 22}}>
        <Modal
          animationType={"slide"}
          transparent={false}
          visible={this.state.modalVisible}
          onRequestClose={this._goBack}
        >
          <View style={{flex: 1}}>
            <Text style={[text.p, styles.title]}>
              Escoge la ubicacion
            </Text>
            {/* TODO: add some instructions text explaining how this MapView & MapMarker works... */}
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
          </View>
          <View style={[styles.footer, {flex: 0.2, flexDirection: "row", justifyContent: "center", flexWrap: "wrap"}]}>
            <View style={{flex: 0.5}}>
              <Button
                title="Aceptar"
                onPress={this._saveLocation}
              />
            </View>

            <View style={{flex: 0.5}}>
              <Button
                color="grey"
                title="Cancelar"
                onPress={this._goBack}
              />
            </View>

          </View>

        </Modal>

      </View>
    )
  }
});

const styles = StyleSheet.create({
  map: {
    flex: 3,
    margin: 50
  },
  title: {
    backgroundColor: "#2962FF",
    color: "white",
    flex: 1,
    width: 400,
    alignSelf: "center",
    textAlignVertical: "center"
  }
});

export default ModalMap;
