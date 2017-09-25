using System.Linq;
using System.Web.Http;
using Encuentrame.Model.Events;
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

            var list=EventCommand.List().Select(x=> new
            {
                Id=x.Id,
                Name=x.Name,
            }).ToList();

            return Ok(new EventApiResultModel()
            {
                Events = list
            });

        }
    }
}