using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Model.Events;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models.EventMonitors;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator, RoleEnum.EventAdministrator, })]
    public class EventMonitorController : ListBaseController<EventMonitorUserInfo, EventMonitorUserListModel>
    {

        [Inject]
        public IEventCommand EventCommand { get; set; }

        [Inject]
        public IUserCommand UserCommand { get; set; }


        [Inject]
        public IAreYouOkCommand AreYouOkCommand { get; set; }


        [Inject]
        public IBag<AreYouOkEvent> AreYouOkEvents { get; set; }

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
                Status = eventt.Status,

                IAmOk = true,
                LastUpdate = eventt.EndDateTime,
                WithoutAnswer = true,
                IAmNotOk = true,
                Clustered = true,
                


            };

            if (eventt.EmergencyDateTime.HasValue)
            {
                eventtModel.EmergencyDateTime = eventt.EmergencyDateTime.Value;
            }

            return View(eventtModel);
        }


        public ActionResult PersonMonitor(int eventId, int userId)
        {
            var eventt = EventCommand.Get(eventId);
            var user = UserCommand.Get(userId);

            var seenInfo = AreYouOkCommand.GetSeenInfo(eventId, userId);

            if (seenInfo == null)
            {
                seenInfo=new SeenInfo();
            }

            if (LoggedUserIs(RoleEnum.EventAdministrator) && eventt.Organizer.Id != GetLoggedUser().Id)
            {
                return RedirectToAction("Monitor", "EventMonitor", new { id = eventId });
            }

            var status = IAmOkEnum.WithoutAnswer;

            var statusRow = AreYouOkEvents.Where(x => x.Target == user && x.Event == eventt).FirstOrDefault();

            if (statusRow?.ReplyDatetime != null)
            {
                status = statusRow.IAmOk ? IAmOkEnum.Ok : IAmOkEnum.NotOk;
            }

            var eventPersonMonitorModel = new EventPersonMonitorModel()
            {
                Username = user.Username,
                Image = user.Image,
                EventId = eventId,
                UserId = userId,
                EventLatitude = eventt.Latitude,
                EventLongitude = eventt.Longitude,
                EventName = eventt.Name,
                Status = status,
                Seen = seenInfo.Seen,
                SeenOk = seenInfo.SeenOk,
                SeenNotOk = seenInfo.SeenNotOk,
                SeenWithoutAnswer = seenInfo.SeenWithoutAnswer
            };

            return View(eventPersonMonitorModel);


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
                Username = item.Username,
                Firstname = item.Firstname,
                Lastname = item.Lastname,
                LastPositionUpdate = item.LastPositionUpdate,
                Seen = item.Seen,
                NotSeen = item.NotSeen,
                IAmOk = (IAmOkEnum)item.IAmOk,
                WasSeen = item.WasSeen
            };
        }

        protected override IList<EventMonitorUserInfo> GetItemList()
        {

            return EventCommand.EventMonitorUsers(EventId);
        }


        [HttpPost]
        public JsonResult Positions(int eventId, DateTime? datetimeTo)
        {
            var positions = EventCommand.PositionsFromEvent(eventId, datetimeTo);

            return Json(JsReturnHelper.Return(positions));
        }


        [HttpPost]
        public JsonResult UserPositions(int eventId, int userId)
        {
            var positions = EventCommand.PositionsUserFromEvent(eventId, userId);
            var seenPositions = AreYouOkCommand.PositionsWhereWasSeen(eventId, userId);

            return Json(JsReturnHelper.Return(new { Positions = positions, SeenPositions = seenPositions }));
        }
    }
}