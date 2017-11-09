using System.Linq;
using System.Web.Http;
using Encuentrame.Model.Events;
using Encuentrame.Web.Models.Apis.Commons;
using Encuentrame.Web.Models.Apis.Events;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class EventController : BaseApiController
    {
        [Inject]
        public IEventCommand EventCommand { get; set; }

        [HttpGet]
        public IHttpActionResult GetActives()
        {

            var list = EventCommand.List().Select(x => new EventsApiResultModel
            {
                Id = x.Id,
                Name = x.Name,
                BeginDateTime = x.BeginDateTime,
                EndDateTime = x.EndDateTime,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Address = new AddressModel()
                {
                    City = x.Address.City,
                    FloorAndDepartament = x.Address.FloorAndDepartament,
                    Number = x.Address.Number,
                    Province = x.Address.Province,
                    Street = x.Address.Street,
                    Zip = x.Address.Zip,
                },
            }).ToList();

            return Ok(list);

        }
    }
}