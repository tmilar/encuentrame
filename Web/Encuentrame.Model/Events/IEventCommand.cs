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
    }
}