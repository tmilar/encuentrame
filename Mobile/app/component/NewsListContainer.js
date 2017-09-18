import React, {Component} from 'react';
import NewsList from "./NewsList";
import {news} from "../config/newsFixture";

export default class NewsListContainer extends Component {
  state = {
    news: []
  };

  async componentDidMount() {
    let news = await this.fetchNews();
    this.setState({news});
  }

  fetchNews() {
    // TODO fetch news from actual remote API
    return news;
  }

  render() {
    return <NewsList news={this.state.news}/>;
  }
}
