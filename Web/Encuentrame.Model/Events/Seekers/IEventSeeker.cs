using System;
using System.Collections.Generic;

namespace Encuentrame.Model.Events.Seekers
{
    public interface IEventSeeker : ISeeker<Event>
    {
        IEventSeeker ByName(string name);
        IEventSeeker ByCity(string name);
        IEventSeeker ByLatitude(decimal? minValue, decimal? maxValue);
        IEventSeeker ByLongitude(decimal? minValue, decimal? maxValue);
        IEventSeeker ByBeginDateTime(DateTime? from, DateTime? to);
        IEventSeeker ByEndDateTime(DateTime? from, DateTime? to);
        IEventSeeker ByOrganizer(int id);
        IEventSeeker ByOrganizer(IList<int> ids);
       
        IEventSeeker ByOrganizerUsername(string username);
        IEventSeeker OrderByOrganizer(SortOrder sortOrder);
        IEventSeeker OrderByName(SortOrder sortOrder);
        IEventSeeker OrderByLatitude(SortOrder sortOrder);
        IEventSeeker OrderByLongitude(SortOrder sortOrder);
        IEventSeeker OrderByBeginDateTime(SortOrder sortOrder);
        IEventSeeker OrderByEndDateTime(SortOrder sortOrder);
        IEventSeeker OrderByCity(SortOrder sortOrder);

    }
}