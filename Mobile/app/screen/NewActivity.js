import React, {Component} from 'react';
import {Alert, Modal, StyleSheet, Text, View, Picker, TextInput} from 'react-native';

export default class NewActivity extends Component {
  constructor(props) {
    super(props);
    this.state = {
      activityName: "",
      selectedEventId: -1
    };

    this._handleActivitynameTextChange = this._handleActivitynameTextChange.bind(this);
  }

  setModalVisible(visible) {
    this.setState({modalVisible: visible});
  }

  _handleActivitynameTextChange(inputValue) {
    this.setState({activityName: inputValue})
  }

  /*_backToHome() {
    this.setModalVisible(false);
    this.props.navigation.goBack(null);
  }*/

  componentDidMount() {
    this.setModalVisible(!this.state.modalVisible)
  }

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
          <View style={styles.message}>
            <View>

              <Text>Nueva Actividad</Text>

            </View>
          </View>
          <View style={{flex: 1}}>
            <TextInput
              value={this.state.activityName}
              placeholder="Nombre de la actividad"
              ref="activityName"
              style={styles.activityName}
              selectTextOnFocus
              onChangeText={this._handleActivitynameTextChange}
            />
            <Picker
              selectedValue={this.state.selectedEventId}
              onValueChange={(itemValue, itemIndex) => this.setState({selectedEventId: itemValue})}>
              <Picker.Item label="Recital del Indio" value="1" />
              <Picker.Item label="Boca-RiBer" value="2" />
            </Picker>
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
  }
});
