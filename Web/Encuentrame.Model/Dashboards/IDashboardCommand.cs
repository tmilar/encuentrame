using System.Collections.Generic;

namespace Encuentrame.Model.Dashboards
{
    public interface IDashboardCommand
    {
        int PeopleThatIAmTracking(int? businessId);
        int PeopleInEvents(int? businessId);

        int EventsInEmergency(int? businessId);
        int PeopleWithoutAnswer(int? businessId);

        EventStatusQuantityInfo EventsByStatus(int? businessId);
        List<EventsAlongTheTimeQuantityInfo> EventsAlongTheTime(int? businessId);
    }
}