import {AsyncStorage} from 'react-native';

class NewsService {
  STORAGE_NEWS_KEY = 'STORAGE_NEWS_KEY';
  NEWS_TTL = 2 * 60 * 60 * 1000; //half hour
  async initializeNews(callbackNewsComponentUpdate) {
    this.callbackNewsComponentUpdate = callbackNewsComponentUpdate;
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (!currentNewsJson) {
      let currentNews = [];
      await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));
    }
  }

  async _cleanOldNews() {
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (currentNewsJson) {
      let currentNews = JSON.parse(currentNewsJson);
      let finalNews = currentNews.filter((news) => new Date() < new Date(news.expires));
      await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(finalNews));
    }
  }

  async saveNews({type, message}) {
    let date = new Date();
    let timestamp = date.getTime();
    let newsItem = {
      type,
      message,
      time: date,
      expires: new Date(timestamp + this.NEWS_TTL).getTime()
    };
    let currentNews = await this.getCurrentNews();
    currentNews.push(newsItem);
    await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));
    await this.callbackNewsComponentUpdate();
  }

  async getCurrentNews() {
    await this._cleanOldNews();
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    return JSON.parse(currentNewsJson);
  }
}

let newsService = new NewsService();
export default newsService;
