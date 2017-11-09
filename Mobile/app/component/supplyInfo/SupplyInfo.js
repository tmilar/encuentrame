import React, {Component} from 'react'
import {View, StyleSheet, Alert} from "react-native";
import TabProgressTracker from "./TabProgressTracker";

/**
 * - get the questions
 * - build the views
 * - pass them to the progress tracker
 */
export default class SupplyInfo extends Component {

  state = {
    currentQuestionIndex: 0
  };

  static defaultProps = {
    questions: []
  };

  renderCurrentQuestionView() {
    let currentQuestion = this.props.questions[this.state.currentQuestionIndex];

    // TODO render the current question: answers, text.
  }

  render() {
    return <View>
      <TabProgressTracker
        items={this.props.questions.map(q => q.text)}
        selectedIndex={this.state.currentQuestionIndex}
      />
      {this.renderCurrentQuestionView()}
    </View>
  }
}
