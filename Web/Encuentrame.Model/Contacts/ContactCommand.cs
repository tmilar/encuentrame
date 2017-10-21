
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using Encuentrame.Support.ExpoNotification;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork;

namespace Encuentrame.Model.Contacts
{
    [Lemming]
    public class ContactCommand : BaseCommand, IContactCommand
    {
        [Inject]
        public IBag<User> Users { get; set; }

        public IList<Contact> List(int userId)
        {
            var user = Users[userId];
            return user.Contacts;
        }



        public void RequestContact(RequestParameters parameters)
        {
            var user = Users[parameters.UserId];
            var contactUser = Users[parameters.RequestUserId];

            if (user.Contacts.Where(x => x.User.Id == contactUser.Id).Any())
            {
                return;
            }

            var contact = new Contact()
            {
                User = contactUser,
                Status = ContactRequestStatus.Pending
            };

            user.Contacts.Add(contact);

            this.CurrentUnitOfWork.Checkpoint();

            var list = contactUser.Devices.Select(x => new BodySend()
            {
                Token = x.Token,
                Body = $"{user.FullName} quiere agregarte como contacto",
                Title = "Encuentrame",
                Data = new
                {
                    userId = user.Id,
                    Type = "Contact.Request",
                }
            }).ToList();

            ExpoPushHelper.SendPushNotification(list);

        }

        public void RejectContact(RejectParameters parameters)
        {
            var rejectUser = Users[parameters.RejectUserId];

            var contacts = rejectUser.Contacts.Where(x => x.User.Id == parameters.UserId && x.Status == ContactRequestStatus.Pending).ToList();

            foreach (var contact in contacts)
            {
                rejectUser.Contacts.Remove(contact);
            }
        }

        public void DeleteContact(DeleteParameters parameters)
        {
            var user = Users[parameters.UserId];

            var contacts = user.Contacts.Where(x => x.User.Id == parameters.ContactUserId).ToList();

            foreach (var contact in contacts)
            {
                user.Contacts.Remove(contact);
            }
        }

        public void AcceptContact(AcceptParameters parameters)
        {
            var user = Users[parameters.AcceptUserId];

            var accepterUser = Users[parameters.UserId];


            var contacts = user.Contacts.Where(x => x.User.Id == parameters.UserId && x.Status==ContactRequestStatus.Pending);

            foreach (var contact in contacts)
            {
                contact.Status = ContactRequestStatus.Accepted;
                contact.AcceptedDatetime = SystemDateTime.Now;

            }
            this.CurrentUnitOfWork.Checkpoint();

            var list = user.Devices.Select(x => new BodySend()
            {
                Token = x.Token,
                Body = $"{accepterUser.FullName} ha aceptado tu solicitud de contacto!",
                Title = "Encuentrame",
                Data = new
                {
                    userId = user.Id,
                    Type = "Contact.Confirm",
                }
            }).ToList();

            ExpoPushHelper.SendPushNotification(list);
        }


        public class DeleteParameters
        {
            public int UserId { get; set; }
            public int ContactUserId { get; set; }

        }
        public class RequestParameters
        {
            public int UserId { get; set; }
            public int RequestUserId { get; set; }

        }

        public class RejectParameters
        {
            public int UserId { get; set; }
            public int RejectUserId { get; set; }
        }

        public class AcceptParameters
        {
            public int UserId { get; set; }
            public int AcceptUserId { get; set; }
        }
    }
}
