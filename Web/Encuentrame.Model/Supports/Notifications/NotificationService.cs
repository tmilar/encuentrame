using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Support.Email;
using Encuentrame.Support.Email.Templates.EmailModels;

namespace Encuentrame.Model.Supports.Notifications
{
    public interface INotificationService
    {
        void SendNotification<TNotification>(string subject, dynamic model) where TNotification : Notification;

        void SendCreatedNotificationFor<T>(string subject, string description);
        void SendCreatedNotificationFor<T>(string description);
        void SendCreatedNotificationFor(Type type, string description);
        void SendUpdatedNotificationFor(Type type, string description, int id);
        void SendDeletedNotificationFor(Type type, string description, int id);        
    }

    [Lemming]
    public class NotificationService : INotificationService
    {
        [Inject]
        public IBag<Notification> Notifications { get; set; }

        [Inject]
        public IBag<NotificationAccess> NotificationAccesses { get; set; }

        [Inject]
        public IBag<NotificationAccessException> NotificationAccessExceptions { get; set; }

        [Inject]
        public IEmailService EmailService { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }

        [Inject]
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        [Inject]
        public ILog Log { get; set; }

        [Inject]
        public ITranslationService TranslationService { get; set; }

        public void SendNotification<TNotification>(string subject, dynamic model) where TNotification : Notification
        {
            var notification = Notifications.ToList().Single(x => x.GetType().IsAssignableFrom(typeof(TNotification)));

            SendNotification(notification, subject, model);
        }

        private void SendNotification(Notification notification, string subject, dynamic model)
        {
            if (notification == null)
                return;

            var notificationAccess = notification.NotificationAccess;
            var notificationException = notification.NotificationAccessException;
            var finalList = new List<BaseUser>();

            if (notificationAccess.AllowEveryone)
            {
                finalList.AddRange(Users.ToList());
            }
            else if ( notificationAccess.Users.Count > 0)
            {
                var usersToSend = notificationAccess.Users ?? new List<BaseUser>();

                

                finalList = usersToSend.ToList();
                if (notificationException.Users.Count > 0)
                {
                    finalList = usersToSend.Where(x => !notificationException.Users.Contains(x)).ToList();
                }
            }

            if (finalList.Count > 0)
            {
                foreach (var user in finalList)
                {
                    var emailHeader = new MailHeader();
                    emailHeader.To = user.Email;
                    emailHeader.Subject = subject;
                    try
                    {
                        EmailService.Send(emailHeader, notification.TemplateName, model);
                    }
                    catch (Exception e)
                    {
                        Log.Error(string.Format("Error sending email to: {0}", user.Email), e);                        
                    }
                }
            }
        }

        public void SendCreatedNotificationFor<T>(string subject, string description)
        {
            this.SendCreatedNotificationFor(typeof(T), subject, description);
        }

        public void SendCreatedNotificationFor(Type type, string subject, string description)
        {
            var loggedUser = AuthenticationProvider.GetLoggedUser();

            var email = new ObjectCreatedEmailModel();
            email.UserName = loggedUser.ToDisplay();
            email.ObjectTypeCreated = TranslationService.Translate(type.Name);
            email.Description = description;
            var notification = GetCreatedNotification(type);
            this.SendNotification(notification, subject, email);
        }

        public void SendUpdatedNotificationFor(Type type, string subject, string description, int id)
        {
            var loggedUser = AuthenticationProvider.GetLoggedUser();

            var email = new ObjectUpdatedEmailModel();
            email.UserName = loggedUser.ToDisplay();
            email.ObjectTypeCreated = TranslationService.Translate(type.Name);
            email.Description = description;
            email.Id = id;
            var notification = GetUpdatedNotification(type);
            this.SendNotification(notification, subject, email);
        }

        public void SendDeletedNotificationFor(Type type, string subject, string description, int id)
        {
            var loggedUser = AuthenticationProvider.GetLoggedUser();

            var email = new ObjectDeletedEmailModel();
            email.UserName = loggedUser.ToDisplay();
            email.ObjectTypeCreated = TranslationService.Translate(type.Name);
            email.Description = description;
            email.Id = id;
            var notification = GetDeletedNotification(type);
            this.SendNotification(notification, subject, email);
        }

        public void SendCreatedNotificationFor<T>(string description)
        {           
            this.SendCreatedNotificationFor(typeof(T), description);
        }

        public void SendCreatedNotificationFor(Type type, string description)
        {
            var subject = TranslationService.Translate(string.Format("{0}:Subject:Create", type.Name));
            this.SendCreatedNotificationFor(type, subject, description);
        }

        public void SendUpdatedNotificationFor(Type type, string description, int id)
        {
            var subject = TranslationService.Translate(string.Format("{0}:Subject:Update", type.Name));
            this.SendUpdatedNotificationFor(type, subject, description, id);
        }

        public void SendDeletedNotificationFor(Type type, string description, int id)
        {
            var subject = TranslationService.Translate(string.Format("{0}:Subject:Delete", type.Name));
            this.SendDeletedNotificationFor(type, subject, description, id);
        }

        private readonly ConcurrentDictionary<Type, int> _typeToCreatedNotification = new ConcurrentDictionary<Type, int>();
        private readonly ConcurrentDictionary<Type, int> _typeToUpdatedNotification = new ConcurrentDictionary<Type, int>();
        private readonly ConcurrentDictionary<Type, int> _typeToDeletedNotification = new ConcurrentDictionary<Type, int>();

        private Notification GetCreatedNotification<T>()
        {
            return GetCreatedNotification(typeof(T));
        }

        private Notification GetCreatedNotification(Type type)
        {
            if (_typeToCreatedNotification == null || _typeToCreatedNotification.Count == 0)
            {
                UpdateCreatedNotifications();
            }

            if (_typeToCreatedNotification.ContainsKey(type))
            {
                return Notifications[_typeToCreatedNotification[type]];
            }
            return null;
        }

        private Notification GetUpdatedNotification(Type type)
        {
            if (_typeToUpdatedNotification == null || _typeToUpdatedNotification.Count == 0)
            {
                UpdateUpdatedNotifications();
            }

            if (_typeToUpdatedNotification.ContainsKey(type))
            {
                return Notifications[_typeToUpdatedNotification[type]];
            }
            return null;
        }

        private Notification GetDeletedNotification(Type type)
        {
            if (_typeToDeletedNotification == null || _typeToDeletedNotification.Count == 0)
            {
                UpdateDeletedNotifications();
            }

            if (_typeToDeletedNotification.ContainsKey(type))
            {
                return Notifications[_typeToDeletedNotification[type]];
            }
            return null;
        }

        public void UpdateCreatedNotifications()
        {
           
            var userCreatedNotification = Notifications.Single(x => x is UserCreatedNotification);
            _typeToCreatedNotification.AddOrUpdate(typeof(User), (x) => userCreatedNotification.Id, (x, y) => userCreatedNotification.Id);

           
           
           }

        public void UpdateUpdatedNotifications()
        {
           
            var userUpdatedNotification = Notifications.Single(x => x is UserUpdatedNotification);
            _typeToUpdatedNotification.AddOrUpdate(typeof(User), (x) => userUpdatedNotification.Id, (x, y) => userUpdatedNotification.Id);

           
           
           
        }

        public void UpdateDeletedNotifications()
        {
           
            var userDeletedNotification = Notifications.Single(x => x is UserDeletedNotification);
            _typeToDeletedNotification.AddOrUpdate(typeof(User), (x) => userDeletedNotification.Id, (x, y) => userDeletedNotification.Id);

           
           
          
        }                
    }
}
