using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Model.Contacts;
using Encuentrame.Support;
using Encuentrame.Web.Models.Apis.Accounts;
using Encuentrame.Web.Models.SoughtPersons;
using NailsFramework.IoC;
using NHibernate.Util;

namespace Encuentrame.Web.Controllers.Apis
{
    public class SoughtPersonController : BaseApiController
    {
        [Inject]
        public AreYouOkCommand AreYouOkCommand { get; set; }

        [Inject]
        public IUserCommand UserCommand { get; set; }

        [HttpGet]
        public IHttpActionResult SoughtPeople()
        {
            var userId = this.GetIdUserLogged();
            var loggedUser = UserCommand.Get(userId);
            var soughtPeople = AreYouOkCommand.SoughtPeople(loggedUser);

            var users = UserCommand.GetUsersByIds(soughtPeople.Select(x => x.UserId));

            var list = new List<SoughtPersonApiResultModel>();
            foreach (var soughtPerson in soughtPeople)
            {
                var user = users.First(x => x.Id == soughtPerson.UserId);
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
                    Distance=soughtPerson.Distance.AsInt(),
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