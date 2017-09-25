using System;

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

        IEventSeeker OrderByName(SortOrder sortOrder);
        IEventSeeker OrderByLatitude(SortOrder sortOrder);
        IEventSeeker OrderByLongitude(SortOrder sortOrder);
        IEventSeeker OrderByBeginDateTime(SortOrder sortOrder);
        IEventSeeker OrderByEndDateTime(SortOrder sortOrder);
        IEventSeeker OrderByCity(SortOrder sortOrder);

    }
}