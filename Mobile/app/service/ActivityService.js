import Service from './Service';

class ActivityService {

  async createActivity(activity) {
    let createActivityUrl = 'Activity/create';
    let activityCreationResponse =  await Service.sendRequest(createActivityUrl, {
      method: 'POST',
      body: JSON.stringify({
        "Name": activity.name,
        "Latitude": activity.latitude,
        "Longitude": activity.longitude,
        "BeginDateTime": activity.beginDateTime,
        "EndDateTime": activity.endDateTime,
        "EventId": activity.eventId
      })
    });
    return activityCreationResponse;
  }
}

let activityService = new ActivityService();
export default activityService;
