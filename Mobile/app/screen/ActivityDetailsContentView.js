import React, {Component} from 'react';
import { StyleSheet, Text, View } from 'react-native';
import GeolocationService from '../service/GeolocationService';
import {MapView} from "expo";
import mapStyles from '../config/map';
import {prettyDate} from "../util/prettyDate";
import LoadingIndicator from "../component/LoadingIndicator";

export default class ActivityDetailsContentView extends Component {

  state = {
    loading: true
  };
  componentWillMount = async () => {
    this.bsasCoordinates = GeolocationService.getBsAsRegion();
    this.activeActivity = this.props.activeActivity;
    this.location = {latitude: this.activeActivity.Latitude,longitude: this.activeActivity.Longitude};
    this.activeEvent = this.props.activeEvent;
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
    this.zoomToMarker(this.location);
  };

  render() {
    if (this.state.loading) {
      return <LoadingIndicator/>;
    }
    return <View style={{flex: 1}}>
      <View style={{
        flex: 0.15,
        flexDirection: 'column',
        justifyContent: 'space-around',
        alignItems: "center"
      }}>
        <Text>
          Evento: {this.activeEvent.Name}
        </Text>

      </View>
      <View style={{flex: 0.15, justifyContent: "space-around", borderBottomColor: '#47315a', borderBottomWidth: 1}}>
        <Text style={{flex: 1, justifyContent: 'space-around',textAlign: 'center'}}>
          {prettyDate(new Date(this.activeActivity.BeginDateTime)) } - {prettyDate(new Date(this.activeActivity.EndDateTime)) }
        </Text>
      </View>
      <View style={{
        flex: 3,
        justifyContent: "space-around",
        borderBottomColor: '#47315a',
        borderBottomWidth: 1
      }}>
        <View style={{flex: 1, flexDirection: "row", justifyContent: "space-around", flexWrap: "wrap"}}>
          <MapView ref="map" style={styles.map}
                   customMapStyle={mapStyles}
                   initialRegion={this.bsasCoordinates}>
            <MapView.Marker draggable={false}
                            coordinate={this.location}
                            title={"Ubicacion"}
                            onValueChange={()=>{}}
                            onDragEnd={(e) => {}}/>
          </MapView>

        </View>
      </View>


    </View>;
  }
};

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
