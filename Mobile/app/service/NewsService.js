import {AsyncStorage} from 'react-native';

class NewsService {
  STORAGE_NEWS_KEY = 'STORAGE_NEWS_KEY';
  NEWS_TTL = 2 * 60 * 60 * 1000; //half hour

  /**
   * Setup method.
   *  - Register onUpdate callback
   *  - initialize AsyncStorage empty 'currentNews' for the first time (TODO check if needed?)
   *
   * @param onUpdate
   * @returns {Promise.<void>}
   */
  initializeNews = async (onUpdate) => {
    this.onUpdate = onUpdate;

    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (!currentNewsJson) {
      let currentNews = [];
      await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));
    }
  };

  /**
   * Delete news that are older than NEWS_TTL time.
   *
   * @returns {Promise.<void>}
   * @private
   */
  _cleanOldNews = async () => {
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (currentNewsJson) {
      let currentNews = JSON.parse(currentNewsJson);
      let finalNews = currentNews.filter((news) => new Date() < new Date(news.expires));
      await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(finalNews));
    }
  };

  /**
   * Store new newsItem .
   *
   * @param type
   * @param message
   * @returns {Promise.<void>}
   */
  saveNews = async ({type, message, data}) => {
    let date = new Date();
    let timestamp = date.getTime();
    let newsItem = {
      type,
      message,
      time: date,
      data: data,
      expires: new Date(timestamp + this.NEWS_TTL).getTime()
    };

    let currentNews = await this.getCurrentNews();
    currentNews.push(newsItem);
    await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));

    await this.onUpdate(currentNews);
  };

  /**
   * Clean old news + read and return stored newsItems list.
   *
   * // TODO don't clean the news here... this method should only parse and return the news.
   * @returns {Promise.<void>}
   */
  getCurrentNews = async () => {
    await this._cleanOldNews();
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    return JSON.parse(currentNewsJson);
  }
}

let newsService = new NewsService();
export default newsService;
