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

  renderAnswerItem = (answerText, index, answersCount) => {
    // TODO convert to actual touchable button.
    return <View style={{flex: 1 / answersCount, alignItems: 'center'}} key={index}>
      <Text style={{textAlign: 'center'}}>{answerText}</Text>
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

    return <View style={{flex: 1}}>
      {answers.map((a, i) => this.renderAnswerItem(a.text, i, questionsCount))}
    </View>;
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
    alignItems: "center"
  }
});
