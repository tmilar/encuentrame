import React, {Component} from 'react'
import {View, StyleSheet, Alert, Text} from "react-native";
import TabProgressTracker from "./TabProgressTracker";

//
// - get the questions
// - build the views
// - pass them to the progress tracker
//
export default class SupplyInfo extends Component {

  state = {
    currentQuestionIndex: 0
  };

  static defaultProps = {
    questions: []
  };

  renderAnswerItem = ({answerText, backgroundColor = "transparent", index = 0, answersCount = 1}) => {
    // TODO convert to actual touchable button.
    return <View style={[styles.answerButton, {backgroundColor, flex: 1 / answersCount}]} key={index}>
      <Text style={styles.answerText} adjustsFontSizeToFit={true}>{answerText}</Text>
    </View>
  };

  renderCurrentQuestionAnswers = () => {
    let questionsCount = this.props.questions.length;
    let currentIndex = this.state.currentQuestionIndex;

    if (currentIndex > questionsCount) {
      return <Text>No quedan preguntas por responder.</Text>
    }

    let currentQuestion = this.props.questions[currentIndex];
    let answers = currentQuestion.answers;
    let answersCount = answers.length;

    return answers.map((a, index) =>
      this.renderAnswerItem({answerText: a.text, backgroundColor: a.color, index, answersCount}));
  };

  render() {
    return <View style={styles.container}>
      <TabProgressTracker
        items={this.props.questions.map(q => q.text)}
        selectedIndex={this.state.currentQuestionIndex}
      />
      <View style={[{flex: 1}, styles.answersContainer]}>
        {this.renderCurrentQuestionAnswers()}
      </View>
      <View style={[{height: 100}, styles.answersContainer]}>
        {this.renderAnswerItem({answerText: "No lo se", backgroundColor: "gray"})}
      </View>
    </View>
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#D2D100', // '#D2D100', //#ecf0f1',
  },
  answersContainer: {
    alignItems: "center",
    justifyContent: 'center',
    width: "100%",
    paddingRight: 3,
    paddingLeft: 3,
  },
  answerButton: {
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 5,
    borderColor: "lightgray",
    margin: 3,
    padding: 3,
    width: "100%",
    height: "100%",
    borderRadius: 5
  },
  answerText: {
    textAlign: 'center',
    fontSize: 16
  }
});
