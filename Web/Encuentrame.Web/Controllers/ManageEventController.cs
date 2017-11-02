﻿using System.Web.Mvc;
using Encuentrame.Model;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Events;
using Encuentrame.Model.Events.Seekers;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models;
using Encuentrame.Web.Models.Events;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator, RoleEnum.EventAdministrator, })]
    public class ManageEventController : ListBaseController<Event, EventListModel>
    {
        [Inject]
        public IEventCommand EventCommand { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var eventt = EventCommand.Get(id);

            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Index");
            }

            var eventtModel = new EventModel()
            {
                Id = eventt.Id,
                Name = eventt.Name,
                Latitude = eventt.Latitude,
                Longitude = eventt.Longitude,
                BeginDateTime = eventt.BeginDateTime,
                EndDateTime = eventt.EndDateTime,
                Number = eventt.Address.Number,
                City = eventt.Address.City,
                Province = eventt.Address.Province,
                Street = eventt.Address.Street,
                Zip = eventt.Address.Zip,
                FloorAndDepartament = eventt.Address.FloorAndDepartament,
                OrganizerDisplay = eventt.Organizer.ToDisplay(),
                Status = eventt.Status
            };

            return View(eventtModel);
        }

        public ActionResult Monitor(int id)
        {
            var eventt = EventCommand.Get(id);
            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Index");
            }
            var eventtModel = new EventMonitorModel()
            {
                Id = eventt.Id,
                Name = eventt.Name,
                Latitude = eventt.Latitude,
                Longitude = eventt.Longitude,
                BeginDateTime = eventt.BeginDateTime,
                EndDateTime = eventt.EndDateTime,
                Address = eventt.Address.ToDisplay(),
                OrganizerDisplay = eventt.Organizer.ToDisplay(),
                Status = eventt.Status
            };

            if (eventt.EmergencyDateTime.HasValue)
            {
                eventtModel.EmergencyDateTime = eventt.EmergencyDateTime.Value;
            }

            return View(eventtModel);
        }

        public ActionResult Create()
        {
            var eventtModel = new EventModel();

            return View(eventtModel);
        }

        [HttpPost]
        public ActionResult Create(EventModel eventtModel)
        {
            if (ModelState.IsValid)
            {
                var eventtParameters = GetCreateOrEditParameters(eventtModel);
                if (LoggedUserIs(RoleEnum.EventAdministrator))
                {
                    eventtParameters.OrganizerId = GetLoggedUser().Id;
                }

                EventCommand.Create(eventtParameters);

                AddModelSuccess(Translations.CreateSuccess.FormatWith(TranslationsHelper.Get<Event>()));

                return RedirectToAction("Index");
            }
            else
            {
                return View(eventtModel);
            }
        }

        private EventCommand.CreateOrEditParameters GetCreateOrEditParameters(EventModel eventtModel)
        {
            var eventtParameters = new EventCommand.CreateOrEditParameters
            {
                Name = eventtModel.Name,
                Latitude = eventtModel.Latitude,
                Longitude = eventtModel.Longitude,
                BeginDateTime = eventtModel.BeginDateTime,
                EndDateTime = eventtModel.EndDateTime,
                Number = eventtModel.Number,
                City = eventtModel.City,
                Province = eventtModel.Province,
                Street = eventtModel.Street,
                Zip = eventtModel.Zip,
                FloorAndDepartament = eventtModel.FloorAndDepartament,
                OrganizerId = eventtModel.Organizer,
            };

            return eventtParameters;
        }

        public ActionResult Edit(int id)
        {
            var eventt = EventCommand.Get(id);
            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Index");
            }
            var eventtModel = new EventModel()
            {
                Id = eventt.Id,
                Name = eventt.Name,
                Latitude = eventt.Latitude,
                Longitude = eventt.Longitude,
                BeginDateTime = eventt.BeginDateTime,
                EndDateTime = eventt.EndDateTime,
                Number = eventt.Address.Number,
                City = eventt.Address.City,
                Province = eventt.Address.Province,
                Street = eventt.Address.Street,
                Zip = eventt.Address.Zip,
                FloorAndDepartament = eventt.Address.FloorAndDepartament,
                Organizer = eventt.Organizer.Id,
                Status = eventt.Status
            };

            return View(eventtModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, EventModel eventtModel)
        {
            if (ModelState.IsValid)
            {
                var eventtParameters = GetCreateOrEditParameters(eventtModel);
                if (LoggedUserIs(RoleEnum.EventAdministrator))
                {
                    eventtParameters.OrganizerId = GetLoggedUser().Id;
                }
                EventCommand.Edit(id, eventtParameters);

                AddModelSuccess(Translations.EditSuccess.FormatWith(TranslationsHelper.Get<Event>()));
                return RedirectToAction("Index");
            }
            else
            {
                return View(eventtModel);
            }
        }

        [HttpPost]
        public ActionResult ButtonAction(int id, string buttonAction)
        {
            var eventt = EventCommand.Get(id);
            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Index");
            }

            switch (buttonAction)
            {
                case "delete":
                    return Delete(id);
                case "begin":
                    return Begin(id);
                case "finalize":
                    return Finalize(id);
                case "emergency":
                    return Emergency(id);
                case "cancelEmergency":
                    return CancelEmergency(id);
                case "startCollaborativeSearch":
                    return StartCollaborativeSearch(id);

            }

            return RedirectToAction("Index");

        }

        protected ActionResult StartCollaborativeSearch(int id)
        {
            EventCommand.StartCollaborativeSearch(id);

            return RedirectToAction("Monitor", new { id });
        }

        protected ActionResult Delete(int id)
        {
            EventCommand.Delete(id);

            return RedirectToAction("Index");
        }

        protected ActionResult Begin(int id)
        {
            EventCommand.BeginEvent(id);

            return RedirectToAction("Monitor", new { id });
        }
        protected ActionResult Finalize(int id)
        {
            EventCommand.FinalizeEvent(id);

            return RedirectToAction("Monitor", new { id });
        }
        protected ActionResult Emergency(int id)
        {
            EventCommand.DeclareEmergency(id);

            return RedirectToAction("Monitor", new { id });
        }

        protected ActionResult CancelEmergency(int id)
        {
            EventCommand.CancelEmergency(id);

            return RedirectToAction("Monitor", new { id });
        }

        protected override EventListModel GetViewModelFrom(Event eventt)
        {
            var eventtListModel = new EventListModel
            {
                Id = eventt.Id,
                Name = eventt.Name,
                Latitude = eventt.Latitude,
                Longitude = eventt.Longitude,
                BeginDateTime = eventt.BeginDateTime,
                EndDateTime = eventt.EndDateTime,
                City = eventt.Address.City,
                Status = eventt.Status,
                Organizer = new ItemModel()
                {
                    Id = eventt.Organizer.Id,
                    Name = eventt.Organizer.FullName
                }

            };
            return eventtListModel;
        }

        protected override void ApplyDefaultFilters(IGenericSeeker<Event> seeker)
        {
            if (LoggedUserIs(RoleEnum.EventAdministrator))
            {
                ((IEventSeeker)seeker).ByOrganizerUsername(this.User.Identity.Name);
            }
            base.ApplyDefaultFilters(seeker);
        }

    }
}