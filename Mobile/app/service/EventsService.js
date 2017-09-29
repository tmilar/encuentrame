import Service from './Service';

class EventsService {

  async getEvents() {
    let eventsUrl = 'Event/getactives';

    let rawEventsResponse =  await Service.sendRequest(eventsUrl, {
      method: 'GET'
    });
    let eventsResponse = await this.parseEventsResponse(rawEventsResponse);
    return eventsResponse;
  }

  async parseEventsResponse(rawResponse) {
    try {
      return await rawResponse.json();
    } catch (e) {
      console.error("Invalid events raw response", e);
      throw 'Ocurrió un problema en la comunicación con el servidor.'
    }
  }
}

let eventsService = new EventsService();
export default eventsService;
