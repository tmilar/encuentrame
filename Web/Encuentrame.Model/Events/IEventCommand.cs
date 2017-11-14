using System;
using System.Collections.Generic;

namespace Encuentrame.Model.Events
{
    public interface IEventCommand
    {
        Event Get(int id);
        void Create(EventCommand.CreateOrEditParameters eventParameters);

        IList<Event> List();
        void Edit(int id, EventCommand.CreateOrEditParameters eventParameters);
        void Delete(int id);
        void DeclareEmergency(int id);
        void CancelEmergency(int id);

        void BeginEvent(int id);
        void FinalizeEvent(int id);
        void StartCollaborativeSearch(int id);
        IList<EventMonitorUserInfo> EventMonitorUsers(int eventId);
        IList<EventMonitorPositionInfo> PositionsFromEvent(int eventId, DateTime? datetimeTo);

        IList<EventPersonMonitorPositionInfo> PositionsUserFromEvent(int eventId, int userId);
    }
}