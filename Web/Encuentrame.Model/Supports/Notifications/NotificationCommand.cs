using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Model.Supports.Notifications
{
    [Lemming]
    public class NotificationCommand : BaseCommand, INotificationCommand
    {
        [Inject]
        public IBag<Notification> Notifications { get; set; }

        [Inject]
        public IBag<NotificationAccess> NotificationAccesses { get; set; }

        [Inject]
        public IBag<NotificationAccessException> NotificationAccessExceptions { get; set; }

        [Inject]
        public IBag<Role> Roles { get; set; }

        [Inject]
        public IBag<BaseUser> Users { get; set; }

        [Inject]
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        [Inject]
        public INotificationService NotificationService { get; set; }

        public IList<Notification> GetList()
        {
            return Notifications.ToList();
        }

        public IList<NotificationAccess> GetNotificationAccesses()
        {
            return NotificationAccesses.ToList();
        }

        public void UpdateNotificationAccesses(CreateOrEditParameters notificationAccessParameters)
        {
            foreach (var notificationAccessInfo in notificationAccessParameters.NotificationAccessParametersList)
            {
                var notificationAccess = NotificationAccesses[notificationAccessInfo.NotificationAccessId];
                notificationAccess.AllowEveryone = notificationAccessInfo.AllowEveryone;
                if (notificationAccess.AllowEveryone)
                {
                    notificationAccess.Roles.Clear();
                    notificationAccess.Users.Clear();
                }
                else
                {
                    EditListFromIds(notificationAccessInfo.Roles, notificationAccess.Roles, i => Roles[i]);
                    EditListFromIds(notificationAccessInfo.Users, notificationAccess.Users, i => Users[i]);
                }
            }            
        }

        public IList<NotificationAccess> GetNotificationAccessesForCurrentUser()
        {
            var loggedUser = AuthenticationProvider.GetLoggedUser();
            var loggedUserRole = loggedUser.Role;
            var userNotificationAccesses = NotificationAccesses.Where(x => x.Roles.Any(y => y == loggedUserRole)
                                                                        || x.Users.Any(y => y == loggedUser)
                                                                        || x.AllowEveryone).ToList();
            return userNotificationAccesses;
        }

        public IList<NotificationAccessException> GetNotificationAccessExceptionsForCurrentUser()
        {
            var loggedUser = AuthenticationProvider.GetLoggedUser();
            var userNotificationExceptions = NotificationAccessExceptions.Where(x => x.Users.Any(y => y == loggedUser)).ToList();
            return userNotificationExceptions;
        }

        public void UpdateExceptionsForUser(IEnumerable<int> exceptions)
        {            
            var loggedUser = AuthenticationProvider.GetLoggedUser();
            var oldExceptions = NotificationAccessExceptions.Where(x => x.Users.Any(y => y == loggedUser));
            var oldExceptionsIds = oldExceptions.Select(x => x.Id);
            var newExceptionsIds =NotificationAccessExceptions.Where(x => exceptions.Contains(x.AssociatedNotification.Id)).Select(x=>x.Id);

            var exceptionsToRemove = oldExceptions.Where(x => !newExceptionsIds.Contains(x.Id));
            foreach (var notificationAccessException in exceptionsToRemove)
            {                
                notificationAccessException.Users.Remove(loggedUser);
            }

            var exceptionsToAdd = newExceptionsIds.Where(x => !oldExceptionsIds.Contains(x));
            foreach (var newException in exceptionsToAdd)
            {
                var notificationException = NotificationAccessExceptions[newException];
                notificationException.Users.Add(loggedUser);
            }
        }

        public class CreateOrEditParameters
        {
            protected CreateOrEditParameters()
            {
            }

            public static CreateOrEditParameters Instance()
            {
                var parameters = new CreateOrEditParameters();
                parameters.NotificationAccessParametersList = new List<NotificationAccessParameters>();
                return parameters;
            }

            public IList<NotificationAccessParameters> NotificationAccessParametersList { get; set; }
            public IList<int> Exceptions { get; set; }
        }

        public class NotificationAccessParameters
        {
            public static NotificationAccessParameters Instance()
            {
                var parameters = new NotificationAccessParameters();
                parameters.Roles = new List<int>();
                parameters.Users = new List<int>();
                return parameters;
            }
            public virtual int NotificationAccessId { get; set; }
            public virtual List<int> Roles { get; set; }
            public virtual List<int> Users { get; set; }
            public virtual bool AllowEveryone { get; set; }
        }
    }
}
