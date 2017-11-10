import Service from './Service';

class SoughtPeopleService {

  /**
   * Get sought people list relevant for me.
   *
   * @returns {Promise.<*>}
   */
  getSoughtPeople = async () => {
    let url = "/soughtPeople";
    let soughtPeople = await Service.sendRequest(url);
    //TODO request all people images here?
    console.debug(`[SoughtPeopleService] Received ${soughtPeople.length} sought people.`);
    return soughtPeople;
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
      }
    )
  };

  /**
   * Dismiss a sought person request.
   *
   * @param soughtPersonId
   * @returns {Promise.<void>}
   */
  soughtPersonDismiss = async (soughtPersonId) => {
    console.log(`[SoughtPeopleService] Dismissing sought person ${soughtPersonId}`);
    // TODO send request to server
  }
}

let soughtPeopleService = new SoughtPeopleService();
export default soughtPeopleService;
