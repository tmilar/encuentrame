using System.Web.Http;
using Encuentrame.Model.Positions;
using Encuentrame.Web.Models.Apis.Positions;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class PositionController : BaseApiController
    {
        [Inject]
        public IPositionCommand PositionCommand { get; set; }

        [HttpPost]
        public IHttpActionResult Set(PositionApiModel positionApiModel)
        {
            if (positionApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PositionCommand.Create(new PositionCommand.CreateOrEditParameters()
            {
                Latitude = positionApiModel.Latitude,
                Longitude = positionApiModel.Longitude,
                Accuracy = positionApiModel.Accuracy,
                Heading = positionApiModel.Heading,
                Speed = positionApiModel.Speed,
                UserId = this.GetIdUserLogged(),
            });

            return Ok(new PositionApiResultModel()
            {
               
            });

        }
    }
}