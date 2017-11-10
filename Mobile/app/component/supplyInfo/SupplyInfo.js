import React, {Component} from 'react'
import {View, StyleSheet, Alert, Text, TouchableHighlight, TouchableOpacity} from "react-native";
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
    return (
      <TouchableHighlight style={[styles.answerButton, {flex: 1 / answersCount}, {backgroundColor}]} key={index}
                          underlayColor="white"
                          activeOpacity={0.5}
      >
        <Text style={styles.answerText} adjustsFontSizeToFit={true}>
          {answerText}
        </Text>
      </TouchableHighlight>
    );
  };

  renderDontKnowAnswer = () => {
    return this.renderAnswerItem({answerText: "No lo se", backgroundColor: "gray"});
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
      <View style={[{flex: 1}, styles.questionAnswersContainer]}>
        {this.renderCurrentQuestionAnswers()}
      </View>
      <View style={[{height: 100}, styles.questionAnswersContainer]}>
        {this.renderDontKnowAnswer()}
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
  questionAnswersContainer: {
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
