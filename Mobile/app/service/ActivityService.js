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
  async getPersonActivities() {
    let activitiesUrl = 'Activities';
    let activities = await Service.sendRequest(activitiesUrl, {
      method: 'GET'
    });
    return activities;
  }

  async activeEvent() {
    let activities = await this.getPersonActivities();
    activities.sort((act1, act2 ) => {return act1.BeginDateTime < act2.BeginDateTime;}).filter((act)=>{return act.EventId || false});
    if (activities.length > 0)
      return activities[0];
    return false;
  }

  async deleteActivity(activityId) {
    let deleteUrl = 'Activity/delete/' + activityId;
    let activityDeletionResponse =  await Service.sendRequest(deleteUrl, {
      method: 'DELETE'
    });
    return activityDeletionResponse;
  }
}

let activityService = new ActivityService();
export default activityService;
