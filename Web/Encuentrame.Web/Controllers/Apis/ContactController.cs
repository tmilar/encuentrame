using System.Collections.Generic;
using System.Web.Http;
using Encuentrame.Model.Contacts;
using Encuentrame.Web.Models.Apis.Accounts;
using Encuentrame.Web.Models.Apis.Contacts;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class ContactController : BaseApiController
    {
        [Inject]
        public IContactCommand ContactCommand { get; set; }


        [HttpPost]
        public IHttpActionResult Request(int id)
        {
            ContactCommand.RequestContact(new ContactCommand.RequestParameters()
            {
                UserId = this.GetIdUserLogged(),
                RequestUserId = id
            });

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Reject(int id)
        {
            ContactCommand.RejectContact(new ContactCommand.RejectParameters()
            {
                UserId = this.GetIdUserLogged(),
                RejectUserId = id
            });

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            ContactCommand.DeleteContact(new ContactCommand.DeleteParameters()
            {
                UserId = this.GetIdUserLogged(),
                ContactUserId = id
            });

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Confirm(int id)
        {
            ContactCommand.AcceptContact(new ContactCommand.AcceptParameters()
            {
                UserId = this.GetIdUserLogged(),
                AcceptUserId = id
            });

            return Ok();
        }


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var userId = this.GetIdUserLogged();
            var contacts = ContactCommand.List(userId);

            var list = new List<ContactApiResultModel>();
            foreach (var contact in contacts)
            {
                var userApiModel = new UserApiResultModel()
                {
                    Id = contact.User.Id,
                    Username = contact.User.Username,
                    FirstName = contact.User.FirstName,
                    LastName = contact.User.LastName,
                    Email = contact.User.Email,
                    EmailAlternative = contact.User.EmailAlternative,
                    InternalNumber = contact.User.InternalNumber,
                    PhoneNumber = contact.User.PhoneNumber,
                    MobileNumber = contact.User.MobileNumber,
                };

                var contactModel=new ContactApiResultModel()
                {
                    User = userApiModel,
                    Pending = contact.Status==ContactRequestStatus.Pending
                };

                list.Add(contactModel);
            }
            
            return Ok(list);
        }


    }
}