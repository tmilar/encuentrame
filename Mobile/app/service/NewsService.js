import {AsyncStorage} from 'react-native';

class NewsService {
  STORAGE_NEWS_KEY = 'STORAGE_NEWS_KEY';
  NEWS_TTL = 2 * 60 * 60 * 1000;

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
    if (!currentNewsJson) {
      throw "Error, las novedades no estan cargadas!";
    }

    let currentNews = JSON.parse(currentNewsJson);
    let finalNews = currentNews.filter((news) => new Date() < new Date(news.expires));
    await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(finalNews));
  };

  dismissNewsById = async (newsId) => {
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (!currentNewsJson) {
      throw "Error, las novedades no estan cargadas!";
    }

    let currentNews = JSON.parse(currentNewsJson);
    let finalNews = currentNews.filter((news) => news.id !== newsId);

    await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(finalNews));
    await this.onUpdate(finalNews);
  };

  /**
   * Update news to set response/resolution
   */
  updateNewsResolution = async (newsId, resolution) => {
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (!currentNewsJson) {
      throw "Error, las novedades no estan cargadas!";
    }

    let currentNews = JSON.parse(currentNewsJson);
    let newsIndex = currentNews.findIndex((news) => news.id === newsId);

    // Add resolution value to the newsItem
    currentNews[newsIndex].resolution = resolution;

    await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));
    await this.onUpdate(currentNews);
  };

  /**
   * Store new newsItem .
   *
   * @param type
   * @param data
   * @returns {Promise.<void>}
   */
  saveNews = async ({type, data}) => {
    let date = new Date();
    let expires = new Date(date.getTime() + this.NEWS_TTL).getTime();
    let id = type + date;

    let newsItem = {
      type,
      time: date,
      data,
      id,
      expires
    };

    let currentNews = await this.getCurrentNews();
    currentNews.push(newsItem);

    await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));
    await this.onUpdate(currentNews);
    return newsItem;
  };

  /**
   * Clean old news + read and return stored newsItems list.
   *
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
