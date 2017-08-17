using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support;
using Encuentrame.Support.Mappings;


namespace Encuentrame.Web.Controllers
{
    [AllowAnonymous]
    public class DatabaseController : BaseController
    {        
       

        [Inject]
        public IBag<User> Users { get; set; }

        [Inject]
        public IBag<SystemUser> SystemUsers { get; set; }

      
        [Inject]
        public IBag<Configuration> Configurations { get; set; }

      

        [Inject]
        public IBag<Notification> Notifications { get; set; }

        [Inject]
        public IBag<NotificationAccessException> NotificationAccessExceptions { get; set; }

        [Inject]
        public IBag<NotificationAccess> NotificationAccesses { get; set; }

        public ActionResult Index()
        {
            ViewBag.CS = ConfigurationManager.ConnectionStrings["Encuentrame"].ConnectionString;
            return View();
        }

        [HttpPost]
        // GET: Database
        public ActionResult Update(string password)
        {
            if (password != "destruir")
            {
                AddModelError("Contraseña incorrecta");
                return View("Index");
            }

            DatabaseCreator.Update();
          
          

            if (SystemUsers.Count() == 0)
            {
                
                var systemUser = new SystemUser()
                {
                    Username = "System",
                    Password = "123456",
                    FirstName = "System",
                    LastName = "System",
                    Email = "system@Encuentrame.com",
                    Role = RoleEnum.Administrator,
                };

                SystemUsers.Put(systemUser);
            }
        
            UpdateNotifications();

            return View("IndexMessage", null, "La base de datos se modificó con exito");
        }

        [HttpPost]
        // GET: Database
        public ActionResult Create(string password)
        {
            if (password != "destruir")
            {
                AddModelError("Contraseña incorrecta");
                return View("Index");
            }

            DatabaseCreator.Create();

           //Users
            var user1 = new User()
            {
                Username = "javier.wamba",
                Password = "123",
                FirstName = "Javier",
                LastName = "Wamba",
                Email = "javier.wamba@Encuentrame.com",
                Role = RoleEnum.Administrator,
            };

            Users.Put(user1);

            var user2 = new User()
            {
                Username = "emiliano.soto",
                Password = "123",
                FirstName = "Emiliano",
                LastName = "Soto",
                Email = "emiliano.soto@Encuentrame.com",
                Role = RoleEnum.User,
            };

            Users.Put(user2);

            var systemUser = new SystemUser()
            {
                Username = "System",
                Password = "123456",
                FirstName = "System",
                LastName = "System",
                Email = "system@Encuentrame.com",
                Role = RoleEnum.User,
            };

            SystemUsers.Put(systemUser);

          

            CreateNotifications();

            return View("IndexMessage", null, "La base de datos se creo con exito");
        }
        
        private void CreateNotifications()
        {
            var notificationTypes = NotificationTypes();
            foreach (var notificationType in notificationTypes)
            {
                var notification = (Notification)Activator.CreateInstance(notificationType);
                var notificationAccess = new NotificationAccess(notification);                
                Notifications.Put(notification);
                NotificationAccesses.Put(notificationAccess);
                var notificationAccessException = new NotificationAccessException(notification);
                NotificationAccessExceptions.Put(notificationAccessException);
                notification.NotificationAccessException = notificationAccessException;
                notification.NotificationAccess = notificationAccess;
            }            
        }

        private void UpdateNotifications()
        {
            var notificationTypes = NotificationTypes();
            //Todo: In case of deleting a Type create a Script to delete it.
           
            foreach (var notificationType in notificationTypes)
            {
                var notificationExists = Notifications.ToList().SingleOrDefault(x => x.GetType().IsAssignableFrom(notificationType));
                if (notificationExists == null)
                {
                    var notification = (Notification)Activator.CreateInstance(notificationType);
                    var notificationAccess = new NotificationAccess(notification);
                    Notifications.Put(notification);
                    NotificationAccesses.Put(notificationAccess);
                }                
            }
        }

        private static IEnumerable<Type> NotificationTypes()
        {
            var notificationTypes =
                typeof(Notification).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Notification)) && !x.IsAbstract);
            return notificationTypes;
        }

       
    }
}