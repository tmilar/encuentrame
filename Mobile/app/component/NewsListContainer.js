import React, {Component} from 'react';
import NewsList from "./NewsList";
import NewsService from "../service/NewsService";

export default class NewsListContainer extends Component {
  state = {
    news: []
  };

  componentDidMount = async () => {
    await NewsService.initializeNews(this._handleNewsUpdate);
  };

  _handleNewsUpdate = (news) => {
    this.setState({news})
  };

  render() {
    return <NewsList news={this.state.news}/>;
  }
}
