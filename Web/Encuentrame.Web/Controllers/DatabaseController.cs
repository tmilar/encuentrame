using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Contacts;
using Encuentrame.Model.Events;
using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support;
using Encuentrame.Support.Mappings;


namespace Encuentrame.Web.Controllers
{
    [AllowAnonymous]
    public class DatabaseController : BaseController
    {

        [Inject]
        public INHibernateContext NHibernateContext { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }

        [Inject]
        public IBag<Event> Events { get; set; }

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
            var userAdmin = new User()
            {
                Username = "user.admin",
                Password = "123",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "Admin@Encuentrame.com",
                Role = RoleEnum.Administrator,
            };

            Users.Put(userAdmin);
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

            var user21 = new User()
            {
                Username = "lionel.messi",
                Password = "123",
                FirstName = "lionel",
                LastName = "messi",
                Email = "messi@Encuentrame.com",
                Role = RoleEnum.User,
            };

            Users.Put(user21);

            var user22 = new User()
            {
                Username = "gonzalo.bonadeo",
                Password = "123",
                FirstName = "gonzalo",
                LastName = "bonadeo",
                Email = "bonadeo@Encuentrame.com",
                Role = RoleEnum.User,
            };

            Users.Put(user22);

            user2.Contacts.Add(new Contact()
            {
                Created = SystemDateTime.Now,
                AcceptedDatetime = SystemDateTime.Now.AddHours(5),
                Status = ContactRequestStatus.Accepted,
                User = user22
            });
            user2.Contacts.Add(new Contact()
            {
                Created = SystemDateTime.Now,
                Status = ContactRequestStatus.Pending,
                User = user21
            });


            var user3 = new User()
            {
                Username = "juan.organizador",
                Password = "123",
                FirstName = "juan",
                LastName = "organizador",
                Email = "juan.organizador@Encuentrame.com",
                Role = RoleEnum.EventAdministrator,
            };

            Users.Put(user3);

            var user4 = new User()
            {
                Username = "fernando.organizador",
                Password = "123",
                FirstName = "fernando",
                LastName = "organizador",
                Email = "fernando.organizador@Encuentrame.com",
                Role = RoleEnum.EventAdministrator,
            };

            Users.Put(user4);

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

          
            var eventt1=new Event()
            {
                Name="Evento 1",
                BeginDateTime = SystemDateTime.Now.AddHours(-3),
                EndDateTime = SystemDateTime.Now.AddHours(3),
                Organizer = user3,
                Status = EventStatusEnum.InProgress
            };

            Events.Put(eventt1);

            var eventt2 = new Event()
            {
                Name = "Evento 2",
                BeginDateTime = SystemDateTime.Now.AddHours(1),
                EndDateTime = SystemDateTime.Now.AddHours(3),
                Organizer = user4,
                Status = EventStatusEnum.Pending
            };

            Events.Put(eventt2);

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