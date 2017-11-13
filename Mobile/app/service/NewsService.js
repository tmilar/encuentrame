import {AsyncStorage} from 'react-native';

class NewsService {
  STORAGE_NEWS_KEY = 'STORAGE_NEWS_KEY';
  NEWS_TTL = 2 * 60 * 60 * 1000; //half hour
  async initializeNews(callbackNewsComponentUpdate) {
    this.callbackNewsComponentUpdate = callbackNewsComponentUpdate;
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (!currentNewsJson){
      let currentNews = [];
      await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(currentNews));
    }
  }

  async _cleanOldNews() {
    let currentNewsJson = await AsyncStorage.getItem(this.STORAGE_NEWS_KEY);
    if (currentNewsJson){
      let currentNews = JSON.parse(currentNewsJson);
      let finalNews = currentNews.filter((news) => new Date() < new Date(news.expires));
      await AsyncStorage.setItem(this.STORAGE_NEWS_KEY, JSON.stringify(finalNews));
    }
   }

  getIcon(type) {
    switch(type) {
      case 'Areyouok.Ask':
        return {
          tagName: 'EvilIcons',
          iconName: 'question',
          color: 'orange'
        };
        break;
      case 'Areyouok.Reply':
        return {
          tagName: 'Ionicons',
          iconName: 'md-happy',
          color: 'green'
        };
        break;
      case 'Contact.Request':
        return {
          tagName: 'Ionicons',
          iconName: 'md-contacts'
        };
        break;
      case 'Contact.Confirm':
        return {
          tagName: 'MaterialCommunityIcons',
          iconName: 'account-check'
        };
        break;
      case 'Event/StartCollaborativeSearch':
        return {
          tagName: 'FontAwesome',
          iconName: 'warning',
          color: 'red'
        };
        break;
      default:
        return {
          tagName: 'Ionicons',
          iconName: 'md-notifications'
        };
    }
  }

  async saveNews(news) {
    let timestamp = new Date().getTime();
    news = {
      ...(news),
      icon: this.getIcon(news.type),
      time: timestamp,
      expires: new Date(timestamp + this.NEWS_TTL).getTime()
    };
    let currentNews = await this.getCurrentNews();
    currentNews.push(news);
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
