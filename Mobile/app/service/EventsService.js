import Service from './Service';

class EventsService {

  async getEvents() {
    let eventsUrl = 'Event/getactives';

    let eventsResponse =  await Service.sendRequest(eventsUrl, {
      method: 'GET'
    });

    return eventsResponse;
  }
}

let eventsService = new EventsService();
export default eventsService;
