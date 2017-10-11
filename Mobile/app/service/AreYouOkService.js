import Service from './Service';

class AreYouOkService {

  /**
   * Send iAmOk reply.
   * Server will notify asking user with replied answer.
   *
   * @param iAmOkReply {boolean}
   * @returns {Promise.<*>}
   */
  reply = async (iAmOkReply) => {
    const url = "Areyouok/reply";
    return await Service.sendRequest(url, {
      method: "POST",
      data: JSON.stringify({
        IAmOk: iAmOkReply
      })
    });
  };

}
