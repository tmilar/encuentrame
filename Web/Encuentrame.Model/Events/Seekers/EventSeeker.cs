using System;
using System.Collections.Generic;
using NHibernate.Linq;

namespace Encuentrame.Model.Events.Seekers
{
    public class EventSeeker : BaseSeeker<Event>, IEventSeeker
    {
        protected override void ApplyDefaultFilters()
        {
            base.ApplyDefaultFilters();
            Where(x => x.DeletedKey == null);
        }

        public IEventSeeker ByName(string name)
        {
            Where(x => x.Name.Like($"%{name}%"));
            return this;
        }
        public IEventSeeker ByCity(string city)
        {
            Where(x => x.Address.City.Like($"%{city}%"));
            return this;
        }
        public IEventSeeker ByLatitude(decimal? minValue, decimal? maxValue)
        {
            if (minValue.HasValue || maxValue.HasValue)
                if (minValue.HasValue && maxValue.HasValue)
                    Where(x => x.Latitude >= minValue.Value && x.Latitude <= maxValue.Value);
                else if (maxValue.HasValue)
                    Where(x => x.Latitude <= maxValue.Value);
                else
                    Where(x => x.Latitude >= minValue.Value);
            return this;
        }
        public IEventSeeker ByLongitude(decimal? minValue, decimal? maxValue)
        {
            if (minValue.HasValue || maxValue.HasValue)
                if (minValue.HasValue && maxValue.HasValue)
                    Where(x => x.Longitude >= minValue.Value && x.Longitude <= maxValue.Value);
                else if (maxValue.HasValue)
                    Where(x => x.Longitude <= maxValue.Value);
                else
                    Where(x => x.Longitude >= minValue.Value);
            return this;
        }

        public IEventSeeker ByBeginDateTime(DateTime? from, DateTime? to)
        {
            if (from.HasValue || to.HasValue)
                if (from.HasValue && to.HasValue)
                    Where(x => x.BeginDateTime >= from.Value && x.BeginDateTime <= to.Value);
                else if (to.HasValue)
                    Where(x => x.BeginDateTime <= to.Value);
                else
                    Where(x => x.BeginDateTime >= from.Value);
            return this;
        }

        public IEventSeeker ByEndDateTime(DateTime? from, DateTime? to)
        {
            if (from.HasValue || to.HasValue)
                if (from.HasValue && to.HasValue)
                    Where(x => x.EndDateTime >= from.Value && x.EndDateTime <= to.Value);
                else if (to.HasValue)
                    Where(x => x.EndDateTime <= to.Value);
                else
                    Where(x => x.EndDateTime >= from.Value);
            return this;
        }
        public IEventSeeker ByOrganizer(int id)
        {
            Where(x => x.Organizer.Id==id);
            return this;
        }
        public IEventSeeker ByOrganizer(IList<int> ids)
        {
            Where(x => ids.Contains(x.Organizer.Id));
            return this;
        }
       
        public IEventSeeker ByOrganizerUsername(string username)
        {
            Where(x => x.Organizer.Username.Like($"{username}"));
            return this;
        }

        public IEventSeeker ByStatus(EventStatusEnum status)
        {
            Where(x => x.Status == status);
            return this;
        }

        public IEventSeeker ByStatus(IList<EventStatusEnum> values)
        {
            Where(x => values.Contains(x.Status));
            return this;
        }
        public IEventSeeker OrderByOrganizer(SortOrder sortOrder)
        {
            OrderBy(x => x.Organizer.FullName, sortOrder);
            return this;
        }

        public IEventSeeker OrderByName(SortOrder sortOrder)
        {
            OrderBy(x => x.Name, sortOrder);
            return this;
        }
        public IEventSeeker OrderByLongitude(SortOrder sortOrder)
        {
            OrderBy(x => x.Longitude, sortOrder);
            return this;
        }
        public IEventSeeker OrderByLatitude(SortOrder sortOrder)
        {
            OrderBy(x => x.Latitude, sortOrder);
            return this;
        }

        public IEventSeeker OrderByBeginDateTime(SortOrder sortOrder)
        {
            OrderBy(x => x.BeginDateTime, sortOrder);
            return this;
        }
        public IEventSeeker OrderByEndDateTime(SortOrder sortOrder)
        {
            OrderBy(x => x.EndDateTime, sortOrder);
            return this;
        }

        public IEventSeeker OrderByCity(SortOrder sortOrder)
        {
            OrderBy(x => x.Address.City, sortOrder);
            return this;
        }
        public IEventSeeker OrderByStatus(SortOrder sortOrder)
        {
            OrderBy(x => x.Status, sortOrder);
            return this;
        }
    }
}
