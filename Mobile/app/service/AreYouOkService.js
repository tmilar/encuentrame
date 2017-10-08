import Service from './Service';

class AreYouOkService {

  replyAreYouOkRequest = async (reply) => {
    const url = "Estasbien/reply";
    return await Service.sendRequest(url, {
      method: "POST",
      data: JSON.stringify({
        Estasbien: false
      })
    });
  };

}
