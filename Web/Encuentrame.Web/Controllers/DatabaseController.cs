﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Activities;
using Encuentrame.Model.Addresses;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Model.Businesses;
using Encuentrame.Model.Contacts;
using Encuentrame.Model.Events;
using Encuentrame.Model.Positions;
using Encuentrame.Model.SoughtPersons;
using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support;
using Encuentrame.Support.Mappings;


namespace Encuentrame.Web.Controllers
{
    [AllowAnonymous]
    public class DatabaseController : BaseController
    {
        [Inject]
        public IBag<SoughtPersonAnswer> SoughtPersonAnswers { get; set; }

        [Inject]
        public IBag<AreYouOkEvent> AreYouOkEvents { get; set; }

        [Inject]
        public IEventCommand EventCommand { get; set; }

        [Inject]
        public IAreYouOkCommand AreYouOkCommand { get; set; }


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

            CreateLogTable();

            CreateStoredProcedures();


            return View("IndexMessage", null, "La base de datos se modificó con exito");
        }



        [HttpPost]
        // GET: Database
        public ActionResult CreateDataMock(string password)
        {
            if (password != "destruir")
            {
                AddModelError("Contraseña incorrecta");
                return View("Index");
            }

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
          



            var business1 = new Business()
            {
                Name = "Pepsi Music",
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
                Latitude = (decimal)-34.60373,
                Longitude = (decimal)-58.38157,
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

            var dayRandom = new Random(SystemDateTime.Now.Millisecond);
            for (int ss = 0; ss < 50; ss++)
            {
                var bgdt = SystemDateTime.Now.AddDays(-1 * dayRandom.Next(0, 360));
                var ev = new Event()
                {
                    Name = $"Evento de prueba {ss}",
                    BeginDateTime = bgdt,
                    EndDateTime = bgdt.AddHours(3),
                    Organizer = user4,
                    Status = bgdt < SystemDateTime.Now ? EventStatusEnum.Completed : EventStatusEnum.InProgress,
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

                if (dayRandom.Next(0, 360) < 36)
                {
                    ev.EmergencyDateTime = bgdt.AddHours(1);
                    if (ev.Status == EventStatusEnum.InProgress)
                    {
                        ev.Status = EventStatusEnum.InEmergency;
                    }
                }

                Events.Put(ev);
            }
            
           
            CreateUsers();


            CurrentUnitOfWork.Checkpoint();

            var listOfUsers = Users.Where(x => x.Role == RoleEnum.User).ToList();
            foreach (var user in listOfUsers)
            {
                var activityIn = new Activity()
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

            foreach (var user in listOfUsers)
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
            foreach (var user in listOfUsers)
            {
                var position = new Position()
                {
                    Latitude = eventt1.Latitude,
                    Longitude = eventt1.Longitude,
                    Creation = eventt1.BeginDateTime,
                    UserId = user.Id,

                };

                Positions.Put(position);
                dictionary[user.Id] = new { Latitude = eventt1.Latitude, Longitude = eventt1.Longitude };
            }

            var rnd = new Random(DateTime.Now.Millisecond);
            for (int ii = 0; ii < 180; ii = ii + 3)
            {
                foreach (var user in listOfUsers)
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


            CurrentUnitOfWork.Checkpoint();

           
            EventCommand.DeclareEmergency(eventt1.Id);


            CurrentUnitOfWork.Checkpoint();

            EventCommand.StartCollaborativeSearch(eventt1.Id);

            CurrentUnitOfWork.Checkpoint();

            foreach (var user in listOfUsers)
            {
                var nmb = rnd.Next(0, 10);
                if (nmb.NotIn(new[] {0, 5, 8}))
                {
                    if (nmb % 2 == 0)
                    {
                        AreYouOkCommand.Reply(new AreYouOkCommand.ReplyParameters()
                        {
                            UserId = user.Id,
                            IAmOk = true,
                        });
                    }
                    else if (nmb % 2 != 0)
                    {
                        AreYouOkCommand.Reply(new AreYouOkCommand.ReplyParameters()
                        {
                            UserId = user.Id,
                            IAmOk = false,
                        });
                    }
                }
            }
            CurrentUnitOfWork.Checkpoint();

            var soughtPeople=AreYouOkEvents.Where(x => x.Event == eventt1 && x.ReplyDatetime == null).ToList();
            var index = soughtPeople.Count;
            foreach (var areYouOkEvent in AreYouOkEvents.Where(x=>x.Event==eventt1 && x.IAmOk && x.ReplyDatetime!=null))
            {
                var nmb = rnd.Next(0, 10);
                if (nmb.NotIn(new[] { 0, 5, 8 }))
                {
                    if (nmb % 2 == 0)
                    {
                        var soughtPersonAswer = new SoughtPersonAnswer()
                        {
                            When = SystemDateTime.Now,
                            Seen = true,
                            IsOk = nmb.NotIn(new[] { 2 }),
                            SourceUser = areYouOkEvent.Target,
                            TargetUser = soughtPeople[rnd.Next(1,index)-1].Target,
                        };

                        SoughtPersonAnswers.Put(soughtPersonAswer);
                    }
                    else if (nmb % 2 != 0)
                    {
                        var soughtPersonAswer = new SoughtPersonAnswer()
                        {
                            When = SystemDateTime.Now,
                            Seen = false,
                            SourceUser = areYouOkEvent.Target,
                            TargetUser = soughtPeople[rnd.Next(1, index) - 1].Target,
                        };

                        SoughtPersonAnswers.Put(soughtPersonAswer);
                    }
                }
            }

           



            return View("IndexMessage", null, "Los datos de prueba se creo con exito");
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

            CreateStoredProcedures();
            CreateLogTable();


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


        public void CreateLogTable()
        {
            var drop = @"IF Object_id('[Log]', 'U') IS NOT NULL
                                        DROP TABLE [Log];";

            var create = @"CREATE TABLE [Log] (
                [Id] [int] IDENTITY (1, 1) NOT NULL,
                [Date] [datetime] NOT NULL,
                [Thread] [varchar] (255) NOT NULL,
                [Level] [varchar] (50) NOT NULL,
                [Logger] [varchar] (255) NOT NULL,
                [Message] [varchar] (4000) NOT NULL,
                [Exception] [varchar] (2000) NULL
            )";

            var dropCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            dropCommand.CommandText = drop;

            NHibernateContext.CurrentSession.Transaction.Enlist(dropCommand);

            dropCommand.ExecuteNonQuery();

            var createCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

            createCommand.CommandText = create;

            NHibernateContext.CurrentSession.Transaction.Enlist(createCommand);

            createCommand.ExecuteNonQuery();

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
	                                    @eventId int, @from datetime 
	                                    AS
	                                    BEGIN
		                                    SELECT	aa.User_id as Id, 
                                                uu.Username as Username , 
		                                        uu.Lastname as Lastname , 
		                                        uu.Firstname as Firstname, 
		                                        iif(bayo.ReplyDatetime is null,0,  iif(bayo.IAmOk=0 , 10 , 20)  ) as IAmOk, 
		                                        sp.WasSeen, 
		                                        sp.Seen, 
		                                        sp.NotSeen, 
                                                po.Creation as LastPositionUpdate
                                        FROM Activities aa 
	                                        inner join Users uu on uu.Id=aa.User_id  
	                                        left join 

	                                        (SELECT iamok,			  
			                                          target_id,
			                                          replydatetime
	                                           FROM
		                                         (SELECT iamok,
				                                         target_id,
				                                         replydatetime,
				                                         Row_number() OVER (PARTITION BY target_id
									                                        ORDER BY replydatetime DESC) rank
		                                          FROM baseareyouoks
		                                          WHERE Event_id=@eventId) ba
	                                           WHERE ba.rank = 1) bayo ON bayo.target_id = aa.user_id

	                                        left join
	                                        (
	                                         select 
		                                         TargetUser_id,
		                                         cast(iif(count(seen)>0,1,0) as bit) as WasSeen, 
		                                        sum(CASE WHEN seen = 1 THEN 1 ELSE 0 END) as Seen, 
		                                        sum(CASE WHEN seen = 0 THEN 1 ELSE 0 END) as NotSeen
		                                        FROM
		                                        SoughtPersonAnswers  where seenwhen>=@from GROUP BY TargetUser_id
                                            ) sp ON sp.TargetUser_id=aa.User_id
	
	                                        left join (SELECT  Positions.UserId, MAX(Positions.Creation) AS creation FROM Positions GROUP BY Positions.UserId ) po on po.UserId=aa.User_id
                                        WHERE aa.Event_id=@eventId;
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

            //TODO: agregar fecha, tener en cuenta si alguien ya contesto
            var drop = @"
                                    IF OBJECT_ID('SoughtPeople', 'P') IS NOT NULL
                                    DROP PROC SoughtPeople
                                    ";

            var add = @"
                                    CREATE PROCEDURE SoughtPeople 
	                                    @userId int , @eventId int, @from datetime
                                    AS
                                    BEGIN
	                                DECLARE @source geography 
                                    set @source = (SELECT top 1 geography::Point(Latitude, Longitude , 4326) FROM Positions where UserId=@userId AND Creation>=@from ORDER BY Creation DESC);

                                    SELECT top 3 aa.Target_id AS [UserId], MIN( @source.STDistance(geography::Point(Latitude, Longitude , 4326)) ) AS Distance
                                        FROM BaseAreYouOks aa 
                                        INNER JOIN positions pp ON aa.Target_id=pp.UserId  	
                                        WHERE  NOT aa.Target_id in (select TargetUser_id from soughtPersonAnswers WHERE soughtPersonAnswers.SourceUser_id=@userId AND soughtPersonAnswers.seenWhen>=@from ) 
			                                    AND aa.Event_id=@eventId AND aa.ReplyDatetime IS NULL AND aa.Target_id<>@userId
                                                AND  @source.STDistance(geography::Point(Latitude, Longitude , 4326)) <=100
                                        GROUP BY Target_id
                                        ORDER BY MIN( @source.STDistance(geography::Point(Latitude, Longitude , 4326)) ); 
                                    END";

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
                        
                        ";

            var command = NHibernateContext.CurrentSession.Connection.CreateCommand();

            command.CommandText = sqls;

            NHibernateContext.CurrentSession.Transaction.Enlist(command);

            command.ExecuteNonQuery();
        }

    }
}