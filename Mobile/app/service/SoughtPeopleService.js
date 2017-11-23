import Service from './Service';
import AccountsService from './AccountsService';

class SoughtPeopleService {

  /**
   * Get sought people list relevant for me.
   *
   * @returns {Promise.<*>}
   */
  getSoughtPeople = async () => {
    let url = "soughtPeople";
    let soughtPeople = await Service.sendRequest(url, {
      method: 'GET'
    });

    soughtPeople.forEach(({User}) => {
      Object.assign(User, {imageUri: AccountsService.getAccountImageUriById(User.Id)})
    });

    // let maxCount = 7;
    // let soughtPeople = await this._getSomeUsersAsSoughtPeople(maxCount);

    // let debuggingPeople = soughtPeople.map(p => ({
    //   ...(p.User), Distance: p.Distance
    // }));
    console.debug(`[SoughtPeopleService] Received ${soughtPeople.length} sought people.`);
    console.table(soughtPeople);

    return soughtPeople;
  };


  _getSomeUsersAsSoughtPeople = async (maxCount) => {
    let allUsers = await AccountsService.getAllUserAccounts();
    let _mapUsersToSoughtPeople = (users) => users.map(u => ({
      User: {...u},
      Distance: this._getRandomInt(0, 10000) / 100
    }));

    let soughtPeople = _mapUsersToSoughtPeople(allUsers); // soughtPeopleFixture

    return this._randomizeLimitPeopleArray(soughtPeople, maxCount);
  };

  _randomizeLimitPeopleArray = (people, maxCount) => {
    let allUsersRandomized = this._randomizeArray(people);
    let usersCount = this._getRandomInt(0, Math.min(allUsersRandomized.length, maxCount));
    return people.slice(0, usersCount);
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
    let url = `soughtPerson/seen/${soughtPersonId}`;

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
    let url = `soughtPerson/dismiss/${soughtPersonId}`;

    return await Service.sendRequest(url, {
      method: 'POST'
    })
  }
}

let soughtPeopleService = new SoughtPeopleService();
export default soughtPeopleService;
