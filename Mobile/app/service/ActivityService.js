import Service from './Service';

class ActivityService {

  async createActivity(activity) {
    let createActivityUrl = 'Activity/create';
    let activityCreationResponse =  await Service.sendRequest(createActivityUrl, {
      method: 'POST',
      body: JSON.stringify({
        "Name": activity.Name,
        "Latitude": activity.Latitude,
        "Longitude": activity.Longitude,
        "BeginDateTime": activity.BeginDateTime,
        "EndDateTime": activity.EndDateTime,
        "EventId": activity.EventId
      })
    });
    return activityCreationResponse;
  }
}

let activityService = new ActivityService();
export default activityService;
