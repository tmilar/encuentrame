import React, {Component} from 'react';
import {Alert, Modal, StyleSheet, Text, View, Picker, TextInput} from 'react-native';
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

  /*_backToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  }*/

  componentDidMount() {
    this.setModalVisible(!this.state.modalVisible)
  },

  render() {
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
            <View style={{flex: 3, flexDirection:'column', justifyContent:'flex-start', alignItems: "center"}}>
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
              <MapView style={styles.map}
                initialRegion={{
                  latitude: 37.78825,
                  longitude: -122.4324,
                  latitudeDelta: 0.0922,
                  longitudeDelta: 0.0421,
                }}
              />
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
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
  }
});

export default NewActivity;
