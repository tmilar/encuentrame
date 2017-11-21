import React, {Component} from 'react';
import {Modal, StyleSheet, Alert, Text, View, Button} from 'react-native';
import {text} from '../style';
import {MapView} from 'expo';
import GeolocationService from '../service/GeolocationService';
import mapStyles from '../config/map';
import LoadingIndicator from "../component/LoadingIndicator";

export default class ModalMap extends Component {

  bsasCoordinates = GeolocationService.getBsAsRegion();
  state = {
    initialMapRegionCoordinates: this.bsasCoordinates,
    activityLocation: {
      latitude: this.props.currentLocation.latitude,
      longitude: this.props.currentLocation.longitude
    },
    loading: true,
    modalVisible: false
  };

  _saveLocation = () => {
    this.props.saveActivityLocation({
      latitude: this.state.activityLocation.latitude,
      longitude: this.state.activityLocation.longitude
    });
    this._goBack();
  };

  _goBack = () => {
    this.setState({modalVisible: false});
    this.props.onClose && this.props.onClose();
  };

  _checkValidLocation = () => {
    return this.state.activityLocation.latitude !== null && this.state.activityLocation.longitude !== null;
  };

  componentWillMount = async () => {
    if (!this._checkValidLocation()) {
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
      }
    }
    this.setState({modalVisible: true});
    this.setState({loading: false});
  };

  zoomToMarker = (location) => {
    let zoom = GeolocationService.getLocationSurroundings(location);
    let context = this;
    setTimeout(function(){
      context.refs.map.fitToCoordinates(zoom.surroundings, zoom.zoomProps.edgePadding, zoom.zoomProps.animated);
    }, 2000);
  };


  componentDidMount = () => {
    this.setState({modalVisible: true});
    this.zoomToMarker(this.state.activityLocation);
  };

  _renderActionButtons = () => {
    let buttons = this.props.enableEditing ?
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
      :
      <View style={[styles.footer, {flex: 0.2, flexDirection: "row", justifyContent: "center", flexWrap: "wrap"}]}>
        <View style={{flex: 0.5}}>
          <Button
            color="grey"
            title="Volver"
            onPress={this._goBack}
          />
        </View>
      </View>;
    return buttons;
  };

  render() {
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }
    let draggable = this.props.enableEditing;
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
              Escoge la ubicación
            </Text>
            {/* TODO: add some instructions text explaining how this MapView & MapMarker works... */}
            <MapView ref="map"
                     style={styles.map}
                     customMapStyle={mapStyles}
                     initialRegion={this.state.initialMapRegionCoordinates}>
                             <MapView.Marker draggable={draggable}
                                             identifier="Ubicacion"
                              coordinate={this.state.activityLocation}
                              title={"Ubicacion"}
                              onValueChange={this._handleEventSelected}
                              onDragEnd={(e) => this.setState({activityLocation: e.nativeEvent.coordinate})}/>
            </MapView>
          </View>
          {this._renderActionButtons()}

        </Modal>

      </View>
    )
  }
};

const styles = StyleSheet.create({
  map: {
    flex: 3,
    margin: 5
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
