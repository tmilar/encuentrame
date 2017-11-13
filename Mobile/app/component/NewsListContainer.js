import React, {Component} from 'react';
import NewsList from "./NewsList";
import {Text, View} from "react-native";
import NewsService from "../service/NewsService";

export default class NewsListContainer extends Component {
  state = {
    news: []
  };

  fetchNews = async ()  => {
    let news = await NewsService.getCurrentNews();
    this.setState({news});
  };

  callbackNewsComponentUpdate = async () => {
    await this.fetchNews();
  };

  componentWillMount = async () => {
    await NewsService.initializeNews(this.callbackNewsComponentUpdate);
    await this.fetchNews();
  };

  render() {
      return <NewsList news={this.state.news}/>;
  }
}
