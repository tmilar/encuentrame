import React, {Component} from 'react'
import {View, StyleSheet, Alert} from "react-native";
import {hideLoading, showLoading, showToast} from "react-native-notifyer";
import SoughtPeopleService from '../../service/SoughtPeopleService';
import SupplyInfo from "./SupplyInfo";
import formatDateForBackend from "../../util/formatDateForBackend";

export default class SupplyInfoContainer extends Component {

  state = {
    whenAnswer: null,
    personStateAnswer: null
  };

  static navigationOptions = {
    title: "Por favor, ayuda aportando datos"
  };

  constructor(props) {
    super(props);
    let {soughtPersonId, onClose, onSubmit} = props.navigation.state.params;
    this.soughtPersonId = soughtPersonId;
    if (!this.soughtPersonId) {
      throw 'Debe indicarse el id de la persona buscada!';
    }

    this.onClose = onClose || (() => {
      });
    this.onSubmit = onSubmit || (() => {
      });
  }

  /**
   * Questions for supply info view.
   */
  questions = [
    {
      text: "¿Cuándo?",
      answers: [{
        text: "Recién",
        getValue: () => {
          let now = new Date();
          return formatDateForBackend(now);
        },
        color: "#64dd17"
      }, {
        text: "Hace un rato\n(10 a 30 minutos)",
        getValue: () => {
          let now = new Date();
          let minutes20 = 20 * 60 * 1000;
          let minutes20ago = new Date(now.getTime() - minutes20);
          return formatDateForBackend(minutes20ago);
        },
        color: "#ffea00"
      }, {
        text: "Hace bastante\n(más de media hora)",
        getValue: () => {
          let now = new Date();
          let minutes60 = 60 * 60 * 1000;
          let minutes60ago = new Date(now.getTime() - minutes60);
          return formatDateForBackend(minutes60ago);
        },
        color: "#ff3d00"
      }],
      onAnswer: (option) => {
        this.setState({whenAnswer: option});
      }
    }, {
      text: "¿Bien o Mal?",
      answers: [{
        text: "Bien \ud83d\udc4d",
        getValue: () => true,
        color: "#64DD17"
      }, {
        text: "Necesitaba Ayuda \u{26A0} \u{1F198}",
        getValue: () => false,
        color: '#ff1919'
      }],
      onAnswer: (option) => {
        this.setState({personStateAnswer: option});
      }
    }
  ];

  /**
   * Get submitted answers and submit them.
   */
  handleSubmitAnswers = async () => {
    this.props.navigation.goBack(null);
    showToast("Aportando datos...", {duration: 2000});

    let suppliedInfo = {
      when: this.state.whenAnswer,
      isOk: this.state.personStateAnswer,
    };

    try {
      console.log("Aportando datos...", suppliedInfo);
      await SoughtPeopleService.soughtPersonSupplyInfo(this.soughtPersonId, suppliedInfo);
    } catch (e) {
      console.error("Error al aportar datos: ", e);
      Alert.alert("Error", "Ups, ocurrio un error al aportar los datos! " + (e.message || e));
      // TODO callback this.onError
      return;
    }

    this.onSubmit(suppliedInfo);
  };

  /**
   * User closed form or navigated back.
   */
  componentWillUnmount() {
    console.log("[SupplyInfoContainer] Canceling (closing or going back). ");
    this.onClose();
  }

  render() {
    return <SupplyInfo
      questions={this.questions}
      onSubmit={this.handleSubmitAnswers}
    />
  }
}
