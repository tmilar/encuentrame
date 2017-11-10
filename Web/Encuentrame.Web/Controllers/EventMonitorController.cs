using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Events;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Models.EventMonitors;
using Encuentrame.Web.Models.Events;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator, RoleEnum.EventAdministrator, })]
    public class EventMonitorController : ListBaseController<EventMonitorUserInfo, EventMonitorUserListModel>
    {

        [Inject]
        public IEventCommand EventCommand { get; set; }


        private int EventId => Convert.ToInt32(this.Url.RequestContext.RouteData.Values["eventId"]);

        public ActionResult Monitor(int id)
        {
            var eventt = EventCommand.Get(id);
            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Index", "ManageEvent");
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

        [HttpPost]
        public ActionResult ButtonAction(int id, string buttonAction)
        {
            var eventt = EventCommand.Get(id);
            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Index", "ManageEvent");
            }

            switch (buttonAction)
            {
              
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

            return RedirectToAction("Index", "ManageEvent");

        }

        protected ActionResult StartCollaborativeSearch(int id)
        {
            EventCommand.StartCollaborativeSearch(id);

            return RedirectToAction("Monitor", new { id });
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

        protected override EventMonitorUserListModel GetViewModelFrom(EventMonitorUserInfo item)
        {
            return new EventMonitorUserListModel()
            {
                Id = item.Id,
                Firstname = item.Firstname,
                Lastname = item.Lastname,
                LastPositionUpdate = item.LastPositionUpdate,
                Seen = item.Seen,
                NotSeen = item.NotSeen,
                IAmOk =(IAmOkEnum) item.IAmOk,
                WasSeen = item.WasSeen
            };
        }

        protected override IList<EventMonitorUserInfo> GetItemList()
        {

            return EventCommand.EventMonitorUsers(EventId);
        }
    }
}