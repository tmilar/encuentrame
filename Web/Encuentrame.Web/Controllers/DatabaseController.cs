using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Activities;
using Encuentrame.Model.Addresses;
using Encuentrame.Model.Businesses;
using Encuentrame.Model.Contacts;
using Encuentrame.Model.Events;
using Encuentrame.Model.Positions;
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
        public IBag<Activity> Activities { get; set; }

        [Inject]
        public IBag<Position> Positions { get; set; }

        [Inject]
        public IBag<Business> Businesses { get; set; }


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
                    Firstname = "System",
                    Lastname = "System",
                    Email = "system@Encuentrame.com",
                    Role = RoleEnum.Administrator,
                };

                SystemUsers.Put(systemUser);
            }

            UpdateNotifications();

            CreateStoredProcedures();


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
                Firstname = "Admin",
                Lastname = "Admin",
                Email = "Admin@Encuentrame.com",
                Role = RoleEnum.Administrator,
            };

            Users.Put(userAdmin);
            var user1 = new User()
            {
                Username = "javier.wamba",
                Password = "123",
                Firstname = "Javier",
                Lastname = "Wamba",
                Email = "javier.wamba@Encuentrame.com",
                Role = RoleEnum.Administrator,
            };

            Users.Put(user1);



            var business1 = new Business()
            {
                Name = "Coca-cola",
                Cuit = "20287495782",
                Created = SystemDateTime.Now,
                Address = new Address()
                {
                    City = "CABA",
                    FloorAndDepartament = "1 'C'",
                    Number = "243",
                    Province = "CABA",
                    Street = "Aguirre",
                    Zip = "1414",
                }
            };

            Businesses.Put(business1);

            var user3 = new User()
            {
                Username = "juan.organizador",
                Password = "123",
                Firstname = "juan",
                Lastname = "organizador",
                Email = "juan.organizador@Encuentrame.com",
                Role = RoleEnum.EventAdministrator,
                Business = business1,
            };

            Users.Put(user3);

            var user4 = new User()
            {
                Username = "fernando.organizador",
                Password = "123",
                Firstname = "fernando",
                Lastname = "organizador",
                Email = "fernando.organizador@Encuentrame.com",
                Role = RoleEnum.EventAdministrator,
                Business = business1,
            };

            Users.Put(user4);


            var eventt1 = new Event()
            {
                Name = "Obelisco",
                BeginDateTime = SystemDateTime.Now.AddHours(-3),
                EndDateTime = SystemDateTime.Now.AddHours(3),
                Organizer = user3,
                Status = EventStatusEnum.InProgress,
                Address = new Address()
                {
                    City = "CABA",
                    FloorAndDepartament = "",
                    Number = "0",
                    Province = "CABA",
                    Street = "Obelisco",
                    Zip = "1414",
                },
                Latitude = (decimal)-34.59974,
                Longitude = (decimal)-58.4336,
            };

            Events.Put(eventt1);

            var eventt2 = new Event()
            {
                Name = "UTN",
                BeginDateTime = SystemDateTime.Now.AddHours(1),
                EndDateTime = SystemDateTime.Now.AddHours(3),
                Organizer = user4,
                Status = EventStatusEnum.Pending,
                Address = new Address()
                {
                    City = "CABA",
                    FloorAndDepartament = "",
                    Number = "953",
                    Province = "CABA",
                    Street = "Medrano",
                    Zip = "1414",
                },
                Latitude = (decimal)-34.59858,
                Longitude = (decimal)-58.41989,
            };

            Events.Put(eventt2);


            var user2 = new User()
            {
                Username = "emiliano.soto",
                Password = "123",
                Firstname = "Emiliano",
                Lastname = "Soto",
                Email = "emiliano.soto@Encuentrame.com",
                Role = RoleEnum.User,
            };
            Users.Put(user2);

            var user21 = new User()
            {
                Username = "lionel.messi",
                Password = "123",
                Firstname = "lionel",
                Lastname = "messi",
                Email = "messi@Encuentrame.com",
                Role = RoleEnum.User,
            };

            Users.Put(user21);

            var user22 = new User()
            {
                Username = "gonzalo.bonadeo",
                Password = "123",
                Firstname = "gonzalo",
                Lastname = "bonadeo",
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

           
            CreateStoredProcedures();

            CreateUsers();


            CurrentUnitOfWork.Checkpoint();

            foreach (var user in Users.Where(x=>x.Role==RoleEnum.User))
            {
                var activityIn= new Activity()
                {
                    BeginDateTime = SystemDateTime.Now.AddDays(-1),
                    EndDateTime = SystemDateTime.Now,
                    Event = eventt1,
                    Latitude = eventt1.Latitude,
                    Longitude = eventt1.Longitude,
                    Name = "actividad 1",
                    User = user
                };
                Activities.Put(activityIn);
            }

            foreach (var user in Users.Where(x => x.Role == RoleEnum.User))
            {
                var activityIn = new Activity()
                {
                    BeginDateTime = SystemDateTime.Now.AddDays(3),
                    EndDateTime = SystemDateTime.Now.AddDays(4),
                    Event = eventt2,
                    Latitude = eventt2.Latitude,
                    Longitude = eventt2.Longitude,
                    Name = "actividad 2",
                    User = user
                };
                Activities.Put(activityIn);
            }

            var dictionary = new Dictionary<int, dynamic>();
            foreach (var user in Users.Where(x => x.Role == RoleEnum.User))
            {
                var position=new Position()
                {
                    Latitude = eventt1.Latitude,
                    Longitude = eventt1.Longitude,
                    Creation = eventt1.BeginDateTime,
                    UserId = user.Id,

                };

                Positions.Put(position);
                dictionary[user.Id] = new { Latitude = eventt1.Latitude, Longitude = eventt1.Longitude };
            }

            var rnd=new Random(DateTime.Now.Millisecond);
            for (int ii = 0; ii < 180; ii++)
            {
                foreach (var user in Users.Where(x => x.Role == RoleEnum.User))
                {
                   
                    var position = new Position()
                    {
                        Latitude = dictionary[user.Id].Latitude + (decimal)rnd.Next(-3, 3) / 10000,
                        Longitude = dictionary[user.Id].Longitude + (decimal)rnd.Next(-3, 3) / 10000,
                        Creation = eventt1.BeginDateTime.AddMinutes(ii),
                        UserId = user.Id,

                    };

                    Positions.Put(position);
                    
                        dictionary[user.Id] = new { Latitude = position.Latitude, Longitude = position.Longitude };
                    
                }
            }

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


        private void CreateStoredProcedures()
        {
            CreateStoredEventMonitorUsers();
            CreateStoredSoughtPeople();
            CreateStoredEventMonitorPositions();
        }



        private void CreateStoredEventMonitorPositions()
        {
            var drop = @"
IF OBJECT_ID('EventMonitorPositions', 'P') IS NOT NULL
	DROP PROC EventMonitorPositions
";

            var add = @"
CREATE PROCEDURE EventMonitorPositions @eventId int, @datetimeTo datetime AS BEGIN
	SELECT aa.user_id AS Id,
		   uu.username As Username,
		   iif(bayo.iamok is null,0,  iif(bayo.iamok=0 , 10 , 20)  ) as IAmOk, 
		   bayo.replydatetime AS ReplyDatetime,
		   pp.latitude AS Latitude,
		   pp.longitude AS Longitude,
		   pp.creation AS LastPositionUpdate
	FROM activities aa
	INNER JOIN users uu ON uu.id=aa.user_id
	LEFT JOIN
	  (SELECT iamok,
			  created,
			  target_id,
			  replydatetime
	   FROM
		 (SELECT iamok,
				 created,
				 target_id,
				 replydatetime,
				 Row_number() OVER (PARTITION BY target_id
									ORDER BY created DESC) rank
		  FROM baseareyouoks
		  WHERE created < @datetimeTo) ba
	   WHERE ba.rank = 1) bayo ON bayo.target_id = aa.user_id
	AND NOT bayo.replydatetime IS NULL
	LEFT JOIN
	  (SELECT id,
			  userid,
			  latitude,
			  longitude,
			  creation
	   FROM
		 (SELECT id,
				 userid,
				 latitude,
				 longitude,
				 creation,
				 Row_number() OVER (PARTITION BY userid
									ORDER BY creation DESC) rank
		  FROM positions
		  WHERE creation < @datetimeTo) po
	   WHERE po.rank = 1) pp ON aa.user_id = pp.userid
	WHERE aa.event_id = @eventId 
END 
                                                                        ";

            var dropCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            dropCommand.CommandText = drop;

            NHibernateContext.CurrentSession.Transaction.Enlist(dropCommand);

            dropCommand.ExecuteNonQuery();



            var addCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            addCommand.CommandText = add;

            NHibernateContext.CurrentSession.Transaction.Enlist(addCommand);

            addCommand.ExecuteNonQuery();
        }

        private void CreateStoredEventMonitorUsers()
        {
            var drop = @"
                                    IF OBJECT_ID('EventMonitorUsers', 'P') IS NOT NULL
                                    DROP PROC EventMonitorUsers
                                    ";

            var add = @"
                                    CREATE PROCEDURE EventMonitorUsers 
	                                    @eventId int
	                                    AS
	                                    BEGIN
		                                    SELECT	aa.User_id as Id, 
                                                    uu.Username as Username , 
				                                    uu.Lastname as Lastname , 
				                                    uu.Firstname as Firstname, 
				                                    iif(bayo.IAmOk is null,0,  iif(bayo.IAmOk=0 , 10 , 20)  ) as IAmOk, 
				                                    cast(iif(count(sp.seen)>0,1,0) as bit) as WasSeen, 
				                                    count(sp.seen) as Seen, 
				                                    count(spn.seen) as NotSeen,
                                                    MAX(po.Creation) as LastPositionUpdate
		                                    FROM Activities aa 
			                                    inner join Users uu on uu.Id=aa.User_id  
			                                    left join BaseAreYouOks bayo on bayo.Target_id=aa.User_id
			                                    left join SoughtPersonAnswers sp on sp.TargetUser_id=aa.User_id and sp.Seen=1
			                                    left join SoughtPersonAnswers spn on spn.TargetUser_id=aa.User_id and spn.Seen=0
                                                left join Positions po on po.UserId=aa.User_id
		                                    WHERE aa.Event_id=@eventId
		                                    GROUP BY aa.User_id,uu.Username,uu.Lastname, uu.Firstname, bayo.IAmOk;
                                    END
                                    
                                                                        ";

            var dropCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            dropCommand.CommandText = drop;

            NHibernateContext.CurrentSession.Transaction.Enlist(dropCommand);

            dropCommand.ExecuteNonQuery();



            var addCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            addCommand.CommandText = add;

            NHibernateContext.CurrentSession.Transaction.Enlist(addCommand);

            addCommand.ExecuteNonQuery();
        }


        private void CreateStoredSoughtPeople()
        {
            var drop = @"
                                    IF OBJECT_ID('SoughtPeople', 'P') IS NOT NULL
                                    DROP PROC SoughtPeople
                                    ";

            var add = @"
                                    CREATE PROCEDURE SoughtPeople 
	                                    @userId int , @eventId int
                                    AS
                                    BEGIN
	                                    DECLARE @source geography 
                                    set @source = (select top 1 geography::Point(Latitude, Longitude , 4326) from Positions where UserId=@userId order by Creation desc);

                                    SELECT top 20 aa.Target_id AS [UserId], min( @source.STDistance(geography::Point(Latitude, Longitude , 4326)) ) as Distance
                                        FROM BaseAreYouOks aa 
                                        inner join positions pp 
                                        on aa.Target_id=pp.UserId  
                                        where aa.Event_id=@eventId and aa.ReplyDatetime is null and aa.Target_id<>@userId
                                        group by Target_id
                                        order by min( @source.STDistance(geography::Point(Latitude, Longitude , 4326)) ); 
                                    END
                                    
                                                                        ";

            var dropCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            dropCommand.CommandText = drop;

            NHibernateContext.CurrentSession.Transaction.Enlist(dropCommand);

            dropCommand.ExecuteNonQuery();



            var addCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            addCommand.CommandText = add;

            NHibernateContext.CurrentSession.Transaction.Enlist(addCommand);

            addCommand.ExecuteNonQuery();





        }

        protected void CreateUsers()
        {
            var sqls = @"
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('agraddon0', 'Ami', 'Graddon', 'agraddon0@patch.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('rhasser1', 'Rex', 'Hasser', 'rhasser1@tiny.cc', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ccasajuana2', 'Chrisy', 'Casajuana', 'ccasajuana2@nbcnews.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dmontford3', 'Damien', 'Montford', 'dmontford3@examiner.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('zcrump4', 'Zuzana', 'Crump', 'zcrump4@freewebs.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('oosgar5', 'Onida', 'Osgar', 'oosgar5@indiegogo.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('sferia6', 'Silvain', 'Feria', 'sferia6@theatlantic.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('vmotherwell7', 'Vernen', 'Motherwell', 'vmotherwell7@wired.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('vmalshinger8', 'Vonny', 'Malshinger', 'vmalshinger8@utexas.edu', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('fanelay9', 'Fidelio', 'Anelay', 'fanelay9@exblog.jp', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('vthoumasa', 'Veriee', 'Thoumas', 'vthoumasa@flickr.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('lrudgleyb', 'Laurena', 'Rudgley', 'lrudgleyb@aboutads.info', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('isallec', 'Itch', 'Salle', 'isallec@unc.edu', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('wwalklingd', 'Waiter', 'Walkling', 'wwalklingd@fema.gov', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('cbroomhalle', 'Cobby', 'Broomhall', 'cbroomhalle@cbc.ca', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('hplottf', 'Hephzibah', 'Plott', 'hplottf@technorati.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('tantoniewiczg', 'Tiffi', 'Antoniewicz', 'tantoniewiczg@google.nl', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('nbrealeyh', 'Noemi', 'Brealey', 'nbrealeyh@hubpages.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('adepietrii', 'Ashia', 'De Pietri', 'adepietrii@360.cn', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dblannj', 'Deidre', 'Blann', 'dblannj@ycombinator.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('jkumark', 'Jodie', 'Kumar', 'jkumark@cafepress.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('hupcraftl', 'Hyacinthia', 'Upcraft', 'hupcraftl@google.es', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('nmcauslandm', 'Normy', 'McAusland', 'nmcauslandm@vistaprint.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('gemminesn', 'Gabriello', 'Emmines', 'gemminesn@nymag.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('kmariao', 'Karon', 'Maria', 'kmariao@sitemeter.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('rbauchopp', 'Ruthie', 'Bauchop', 'rbauchopp@com.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('bwainq', 'Brooke', 'Wain', 'bwainq@irs.gov', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('mwoolamr', 'Marlon', 'Woolam', 'mwoolamr@cbsnews.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('amuzzinis', 'Ardra', 'Muzzini', 'amuzzinis@mozilla.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('opristnort', 'Olympe', 'Pristnor', 'opristnort@naver.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('mswantonu', 'Marcellina', 'Swanton', 'mswantonu@tripadvisor.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dmapplebeckv', 'Dominga', 'Mapplebeck', 'dmapplebeckv@mapy.cz', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('mworboyw', 'Maribeth', 'Worboy', 'mworboyw@t.co', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dmuddimanx', 'Darsey', 'Muddiman', 'dmuddimanx@simplemachines.org', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dphytheany', 'Dede', 'Phythean', 'dphytheany@wikipedia.org', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('patlingz', 'Pooh', 'Atling', 'patlingz@disqus.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('jglinde10', 'Jane', 'Glinde', 'jglinde10@dropbox.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('zchace11', 'Zarla', 'Chace', 'zchace11@usa.gov', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('lfesby12', 'Liliane', 'Fesby', 'lfesby12@1und1.de', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ereely13', 'Elga', 'Reely', 'ereely13@unicef.org', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ncorroyer14', 'Noam', 'Corroyer', 'ncorroyer14@wix.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('pshoppee15', 'Phaidra', 'Shoppee', 'pshoppee15@washington.edu', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('kletham16', 'Keslie', 'Letham', 'kletham16@mac.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('sshemming17', 'Stefa', 'Shemming', 'sshemming17@umn.edu', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('hhows18', 'Hester', 'Hows', 'hhows18@wp.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('jelies19', 'Jed', 'Elies', 'jelies19@zdnet.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('aaldgate1a', 'Andromache', 'Aldgate', 'aaldgate1a@furl.net', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('hfennick1b', 'Hyatt', 'Fennick', 'hfennick1b@ezinearticles.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('hteasey1c', 'Ham', 'Teasey', 'hteasey1c@quantcast.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('tbalk1d', 'Tom', 'Balk', 'tbalk1d@ibm.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('tflory1e', 'Trula', 'Flory', 'tflory1e@godaddy.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('wbrownlie1f', 'Willie', 'Brownlie', 'wbrownlie1f@tripadvisor.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('djeandillou1g', 'Donna', 'Jeandillou', 'djeandillou1g@marriott.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dbullen1h', 'Dorisa', 'Bullen', 'dbullen1h@purevolume.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('nlongthorne1i', 'Nicko', 'Longthorne', 'nlongthorne1i@cyberchimps.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('lbergin1j', 'Lisetta', 'Bergin', 'lbergin1j@huffingtonpost.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('cbrandone1k', 'Cort', 'Brandone', 'cbrandone1k@cargocollective.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ssuff1l', 'Sophia', 'Suff', 'ssuff1l@whitehouse.gov', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('bstempe1m', 'Burnaby', 'Stempe', 'bstempe1m@photobucket.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('owrighton1n', 'Oliviero', 'Wrighton', 'owrighton1n@vkontakte.ru', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('cmiddup1o', 'Cale', 'Middup', 'cmiddup1o@jugem.jp', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('nbenjamin1p', 'Nariko', 'Benjamin', 'nbenjamin1p@fema.gov', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('tubsdell1q', 'Tobias', 'Ubsdell', 'tubsdell1q@mashable.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('htavner1r', 'Harwell', 'Tavner', 'htavner1r@opera.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('rstate1s', 'Rosita', 'State', 'rstate1s@vinaora.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('acheesman1t', 'Adler', 'Cheesman', 'acheesman1t@mail.ru', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dlebel1u', 'Deerdre', 'Lebel', 'dlebel1u@symantec.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('abairstow1v', 'Astrid', 'Bairstow', 'abairstow1v@netvibes.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ebraidford1w', 'Elysha', 'Braidford', 'ebraidford1w@nyu.edu', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('amoutrayread1x', 'Anabella', 'Moutray Read', 'amoutrayread1x@independent.co.uk', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('nhardypiggin1y', 'Niki', 'Hardy-Piggin', 'nhardypiggin1y@barnesandnoble.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('skyndred1z', 'Sauveur', 'Kyndred', 'skyndred1z@webeden.co.uk', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('tmedmore20', 'Tanhya', 'Medmore', 'tmedmore20@unicef.org', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('bcouvert21', 'Brigit', 'Couvert', 'bcouvert21@google.ru', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('cborgesio22', 'Catrina', 'Borgesio', 'cborgesio22@xrea.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('smassinger23', 'Stafford', 'Massinger', 'smassinger23@about.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('jkeiley24', 'Jeni', 'Keiley', 'jkeiley24@is.gd', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ldoumerc25', 'Lorens', 'Doumerc', 'ldoumerc25@reddit.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ggitthouse26', 'Garrett', 'Gitthouse', 'ggitthouse26@sogou.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('rwinchester27', 'Rollo', 'Winchester', 'rwinchester27@yellowpages.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('spolino28', 'Shena', 'Polino', 'spolino28@redcross.org', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('wcallis29', 'Wadsworth', 'Callis', 'wcallis29@google.com.hk', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('ebremner2a', 'Estevan', 'Bremner', 'ebremner2a@dailymail.co.uk', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('lwitherington2b', 'Lavina', 'Witherington', 'lwitherington2b@4shared.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dharpin2c', 'Delmer', 'Harpin', 'dharpin2c@phoca.cz', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('crosenthaler2d', 'Corey', 'Rosenthaler', 'crosenthaler2d@blogs.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('cmctague2e', 'Cherie', 'McTague', 'cmctague2e@de.vu', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('gspurgin2f', 'Gannie', 'Spurgin', 'gspurgin2f@howstuffworks.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('llaw2g', 'Locke', 'Law', 'llaw2g@hud.gov', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('sdacombe2h', 'Sandro', 'Dacombe', 'sdacombe2h@google.co.jp', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('sgilburt2i', 'Sinclair', 'Gilburt', 'sgilburt2i@mlb.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('mtrathen2j', 'Morse', 'Trathen', 'mtrathen2j@wunderground.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('asimoncello2k', 'Aloysia', 'Simoncello', 'asimoncello2k@miibeian.gov.cn', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('wmurkin2l', 'Whitney', 'Murkin', 'wmurkin2l@webmd.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('calenikov2m', 'Cos', 'Alenikov', 'calenikov2m@msn.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('lainslee2n', 'Liane', 'Ainslee', 'lainslee2n@chron.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('nbaselli2o', 'Norris', 'Baselli', 'nbaselli2o@booking.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('tbeech2p', 'Tani', 'Beech', 'tbeech2p@nydailynews.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('dgavey2q', 'Dacy', 'Gavey', 'dgavey2q@nbcnews.com', 'User', 'User');
insert into Users (Username, firstname, lastname, email, Role, Usertype) values ('pwarrier2r', 'Phillipe', 'Warrier', 'pwarrier2r@alibaba.com', 'User', 'User');
";

            var command = NHibernateContext.CurrentSession.Connection.CreateCommand();

            command.CommandText = sqls;

            NHibernateContext.CurrentSession.Transaction.Enlist(command);

            command.ExecuteNonQuery();
        }

    }
}