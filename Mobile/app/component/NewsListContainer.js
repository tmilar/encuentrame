import React, {Component} from 'react';
import NewsList from "./NewsList";

class NewsListContainer extends Component {
  state = {
    news: []
  };

  constructor(props) {
    super(props);
    this.state = {
      news: this.props.news
    }
  }

  render() {
    return <NewsList news={this.state.news}/>;
  }
}


export default NewsListContainer;
