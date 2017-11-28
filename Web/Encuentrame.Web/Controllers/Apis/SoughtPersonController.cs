using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Support;
using Encuentrame.Web.Models.Apis.Accounts;
using Encuentrame.Web.Models.Apis.SoughtPersons;
using NailsFramework.IoC;

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
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
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
            if (model == null || id<=0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parameter=new AreYouOkCommand.SoughtPersonSeenParameters()
            {
                IsOk = model.IsOk,
                SourceUserId = this.GetIdUserLogged(),
                TargetUserId = id,
                When = model.When,
            };

            AreYouOkCommand.SoughtPersonSeen(parameter);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Dismiss(int id)
        {
            if ( id <= 0)
            {
                return BadRequest();
            }

            var parameter = new AreYouOkCommand.SoughtPersonDismissParameters()
            {
                SourceUserId = this.GetIdUserLogged(),
                TargetUserId = id,
                When = SystemDateTime.Now
            };

            AreYouOkCommand.SoughtPersonDismiss(parameter);

            return Ok();
        }
    }
}