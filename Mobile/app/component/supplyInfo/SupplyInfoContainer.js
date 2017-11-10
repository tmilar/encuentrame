import React, {Component} from 'react'
import {View, StyleSheet, Alert} from "react-native";
import {hideLoading, showLoading} from "react-native-notifyer";
import SoughtPeopleService from '../../service/SoughtPeopleService';
import SupplyInfo from "./SupplyInfo";
import formatDateForBackend from "../../util/formatDateForBackend";

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
        getValue: () => {
          let now = new Date();
          return formatDateForBackend(now);
        }
      }, {
        text: "Hace un rato\n(10 a 30 minutos)",
        getValue: () => {
          let now = new Date();
          let minutes20 = 20 * 60 * 1000;
          let minutes20ago = new Date(now.getTime() - minutes20);
          return formatDateForBackend(minutes20ago);
        }
      }, {
        text: "Hace bastante\n(más de media hora)",
        getValue: () => {
          let now = new Date();
          let minutes60 = 60 * 60 * 1000;
          let minutes60ago = new Date(now.getTime() - minutes60);
          return formatDateForBackend(minutes60ago);
        }
      }],
      onAnswer: (option) => {
        this.setState({whenAnswer: option});
      }
    }, {
      text: "¿Bien o Mal?",
      answers: [{
        text: "Bien \ud83d\udc4d",
        getValue: () => true
      }, {
        text: "Necesitaba Ayuda \u{26A0} \u{1F198}",
        getValue: () => false
      }],
      onAnswer: (option) => {
        this.setState({personStateAnswer: option});
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
    return <SupplyInfo
      questions={this.questions}
      onSubmit={this.handleSubmitAnswers}
      onClose={this.handleClose}
    />
  }
}
