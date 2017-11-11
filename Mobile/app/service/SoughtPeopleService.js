import Service from './Service';
import sleep from "../util/sleep";
import AccountsService from './AccountsService';

class SoughtPeopleService {

  /**
   * Get sought people list relevant for me.
   * //TODO returning RANDOM user accounts, replace with actual soughtPeople endpoint when working.
   * @returns {Promise.<*>}
   */
  getSoughtPeople = async () => {
    let url = "/soughtPeople";
    // let soughtPeople = await Service.sendRequest(url);
    let maxCount = 7;
    let soughtPeople = await this._getSomeUsersRandomized(maxCount);
    console.debug(`[SoughtPeopleService] Received ${soughtPeople.length} RANDOMIZED sought people.`);
    return soughtPeople;
  };


  _getSomeUsersRandomized = async (maxCount) => {
    let allUsers = await AccountsService.getAllUserAccounts();
    let allUsersRandomized = this._randomizeArray(allUsers);
    let usersCount = this._getRandomInt(0, Math.min(allUsersRandomized.length, maxCount));
    let someRandomUsers = allUsers.slice(0, usersCount);

    return someRandomUsers.map(u => ({
      User: {...u},
      Distance: this._getRandomInt(0, 10000) / 100
    }))
  };

  /**
   * Naive array randomization.
   * @param arr
   * @returns {Array.<T>}
   * @private
   */
  _randomizeArray = (arr) => {
    return arr.sort(() => (0.5 - Math.random()));
  };
  /**
   *
   * Returns a random integer between min (inclusive) and max (inclusive)
   * Using Math.round() will give you a non-uniform distribution!
   *
   * @param min
   * @param max
   * @returns {*}
   * @private
   */
  _getRandomInt = (min, max) => {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  };

  /**
   * Send sought person info.
   *
   * @param soughtPersonId
   * @param suppliedInfo (when, isOk)
   * @returns {Promise.<void>}
   */
  soughtPersonSupplyInfo = async (soughtPersonId, suppliedInfo) => {
    let url = `/soughtPerson/seen/${soughtPersonId}`;

    return await Service.sendRequest(url, {
      method: 'POST',
      body: JSON.stringify({
        "When": suppliedInfo.when,
        "IsOk": suppliedInfo.isOk
      })
    })
  };

  /**
   * Dismiss a sought person request.
   *
   * @param soughtPersonId
   * @returns {Promise.<void>}
   */
  soughtPersonDismiss = async (soughtPersonId) => {
    let url = `/soughtPerson/dismiss/${soughtPersonId}`;

    return await Service.sendRequest(url, {
      method: 'POST'
    })
  }
}

let soughtPeopleService = new SoughtPeopleService();
export default soughtPeopleService;
