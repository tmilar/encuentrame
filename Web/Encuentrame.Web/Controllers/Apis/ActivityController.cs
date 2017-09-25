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
            });

            return Ok(new ActivityApiResultModel()
            {

            });

        }
    }
}