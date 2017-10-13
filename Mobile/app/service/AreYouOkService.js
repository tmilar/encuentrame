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
      body: JSON.stringify({
        IAmOk: iAmOkReply
      })
    });
  };

  /**
   * Ask a target user if is OK.
   * Server will notify target user prompting for an answer.
   *
   * @param targetUser <id:<number>>
   * @returns {Promise.<*>}
   */
  ask = async (targetUser) => {
    const url = "Areyouok/ask";
    return await Service.sendRequest(url, {
      method: "POST",
      body: JSON.stringify({
        TargetUserId: targetUser.id
      })
    });
  }
}

const instance = new AreYouOkService();
export default instance;
