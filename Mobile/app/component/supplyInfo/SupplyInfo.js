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

  renderCurrentQuestionView() {
    let questionsCount = this.props.questions.length;
    let currentIndex = this.state.currentQuestionIndex;
    //TODO remove this and replace with actual answers.
    return <Text>No quedan preguntas por responder.</Text>;

    // TODO render the current question: answers, text.
  }

  render() {
    return <View style={styles.container}>
      <TabProgressTracker
        items={this.props.questions.map(q => q.text)}
        selectedIndex={this.state.currentQuestionIndex}
      />
      <View style={{flex: 1}}>
        {this.renderCurrentQuestionView()}
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
  }
});
