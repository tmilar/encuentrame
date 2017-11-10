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

  renderAnswerItem = (answerText, index = 0, answersCount = 1) => {
    // TODO convert to actual touchable button.
    return <View style={[styles.answerButton, {flex: 1 / answersCount}]} key={index}>
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

    return answers.map((a, i) => this.renderAnswerItem(a.text, i, questionsCount));
  };

  render() {
    return <View style={styles.container}>
      <TabProgressTracker
        items={this.props.questions.map(q => q.text)}
        selectedIndex={this.state.currentQuestionIndex}
      />
      <View style={styles.answersContainer}>
        {this.renderCurrentQuestionAnswers()}
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
    flex: 1,
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
  },
  answerText: {
    textAlign: 'center',
    // textAlignVertical: "center"
  }
});
