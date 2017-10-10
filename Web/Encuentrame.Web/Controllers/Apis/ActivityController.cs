using System.Collections.Generic;
using System.Web.Http;
using Encuentrame.Model.Activities;
using Encuentrame.Web.Models.Apis.Activities;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class ActivityController : BaseApiController
    {
        [Inject]
        public IActivityCommand ActivityCommand { get; set; }

        [HttpPost]
        public IHttpActionResult Create(ActivityApiModel activityApiModel)
        {
            if (activityApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ActivityCommand.Create(new ActivityCommand.CreateOrEditParameters()
            {
                UserId= GetIdUserLogged(),
                Name = activityApiModel.Name,
                Latitude = activityApiModel.Latitude,
                Longitude = activityApiModel.Longitude,
                BeginDateTime = activityApiModel.BeginDateTime,
                EndDateTime = activityApiModel.EndDateTime,
                EventId=activityApiModel.EventId,
            });

            return Ok(new ActivityApiResultModel()
            {

            });

        }
        [HttpGet]
        public IHttpActionResult GetActives()
        {
            var list=ActivityCommand.GetActivities(GetIdUserLogged());

            var listModels = new List<ActivitiesApiResultModel>();
            foreach (var activity in list)
            {
                var model = new ActivitiesApiResultModel()
                {
                    Id = activity.Id,
                    Longitude = activity.Longitude,
                    Latitude = activity.Latitude,
                    BeginDateTime = activity.BeginDateTime,
                    EndDateTime = activity.EndDateTime,
                    Name = activity.Name,
                    EventId = activity.Event?.Id
                };
                listModels.Add(model);
            }

            return Ok(listModels);

        }
    }
}