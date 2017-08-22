import React, {Component} from 'react';
import NewsList from "./NewsList";
import {news} from "../config/newsFixture";

export default class NewsListContainer extends Component {


  constructor(props) {
    super(props);
    this.state = {
      news: news
    }
  }

  render() {
    return <NewsList news={this.state.news}/>;
  }
}
