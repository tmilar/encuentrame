import React, {Component} from 'react'
import TabProgressTracker from "./TabProgressTracker";
import {View, StyleSheet, Alert} from "react-native";
import {hideLoading, showLoading} from "react-native-notifyer";
import SoughtPeopleService from '../../service/SoughtPeopleService';

export default class SupplyInfoContainer extends Component {

  state = {
    whenAnswer: null,
    personStateAnswer: null,
    whereAnswer: null
  };

  static navigationOptions = {
    title: "Por favor, ayuda aportando datos"
  };

  constructor(props) {
    super(props);
    let {soughtPersonId, onClose, onSuccess} = props.navigation.state.params;
    this.soughtPersonId = soughtPersonId;
    if (!this.soughtPersonId) {
      throw 'Debe indicarse el id de la persona buscada!';
    }

    this.onClose = onClose || (() => {
      });
    this.onSuccess = onSuccess || (() => {
      });
  }

  /**
   * Questions to show for supply info.
   * //TODO : bind answer values/structure to latest server API (after v0.92).
   *
   * @type {[*]}
   */
  questions = [
    {
      text: "¿Cuándo?",
      answers: [{
        text: "Recién",
      }, {
        text: "Hace un rato\n(10 a 30 minutos)"
      }, {
        text: "Hace bastante\n(más de media hora)"
      }],
      onAnswer: (option) => {
        this.setState({whenAnswer: option});
      }
    }, {
      text: "¿Bien o Mal?",
      answers: [{
        text: "Bien \ud83d\udc4d"
      }, {
        text: "Necesitaba Ayuda \u{26A0} \u{1F198}"
      }],
      onAnswer: (option) => {
        this.setState({personStateAnswer: option});
      }
    }, {
      text: "¿Dónde?",
      answers: [{
        text: "Cerca mio (indicar en el mapa)"
      }, {
        text: "No me acuerdo"
      }],
      onAnswer: (option) => {
        this.setState({whereAnswer: option});
      }
    }
  ];

  /**
   * Get submitted answers and send them to server.
   *
   * // TODO
   * 1. read state answers data
   * 2. send to SoughtPeopleService.soughtPersonSupplyInfo()
   */
  handleSubmitAnswers = async () => {
    showLoading("Aportando datos!...");

    // TODO get correct data structure to send to SoughtPeopleService.soughtPersonSupplyInfo()
    let info = {
      when: this.state.whenAnswer,
      personState: this.state.personStateAnswer,
      where: this.state.whereAnswer
    };

    try {
      console.log("Aportando datos...", info);
      // await SoughtPeopleService.soughtPersonSupplyInfo(this.soughtPersonId, info);
    } catch (e) {
      console.error("Error al aportar datos: ", e);
      Alert.alert("Error", "Ups, ocurrio un error! " + (e.message || e));
      return;
    } finally {
      hideLoading();
    }

    this.onSuccess();
    this.props.navigation.goBack(null);
  };

  /**
   * User closed form or navigated back: no sending info to server.
   */
  handleClose = () => {
    this.onClose();
    this.props.navigation.goBack(null)
  };

  render() {
    return <View style={styles.container}>
      <TabProgressTracker
        questions={this.questions}
        onSubmit={this.handleSubmitAnswers}
        onClose={this.handleClose}
      />
    </View>;
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#D2D100', // '#D2D100', //#ecf0f1',
  }
});
