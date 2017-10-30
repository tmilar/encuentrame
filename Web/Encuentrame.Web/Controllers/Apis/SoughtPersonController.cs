using System.Collections.Generic;
using System.Web.Http;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Contacts;
using Encuentrame.Web.Models.Apis.Accounts;
using Encuentrame.Web.Models.SoughtPersons;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class SoughtPersonController : BaseApiController
    {
        [Inject]
        public IUserCommand UserCommand { get; set; }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var userId = this.GetIdUserLogged();
            var users = UserCommand.List();

            var list = new List<SoughtPersonApiResultModel>();
            foreach (var user in users)
            {
                var userApiModel = new UserApiResultModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailAlternative = user.EmailAlternative,
                    InternalNumber = user.InternalNumber,
                    PhoneNumber = user.PhoneNumber,
                    MobileNumber = user.MobileNumber,
                };

                var soughtPeopleModel = new SoughtPersonApiResultModel()
                {
                    User = userApiModel,
                   
                };

                list.Add(soughtPeopleModel);
            }

            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult Seen(int id, SoughtPersonSeenApiModel model )
        {
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Dismiss(int id)
        {
            return Ok();
        }
    }
}