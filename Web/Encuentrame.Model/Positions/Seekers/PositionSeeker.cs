using System;

namespace Encuentrame.Model.Positions.Seekers
{
    public class PositionSeeker : BaseSeeker<Position>, IPositionSeeker
    {



        public IPositionSeeker ByLatitude(decimal? minValue, decimal? maxValue)
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
        public IPositionSeeker ByLongitude(decimal? minValue, decimal? maxValue)
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

        public IPositionSeeker ByUserId(int? minValue, int? maxValue)
        {
            if (minValue.HasValue || maxValue.HasValue)
                if (minValue.HasValue && maxValue.HasValue)
                    Where(x => x.UserId >= minValue.Value && x.UserId <= maxValue.Value);
                else if (maxValue.HasValue)
                    Where(x => x.UserId <= maxValue.Value);
                else
                    Where(x => x.UserId >= minValue.Value);
            return this;
        }
        public IPositionSeeker ByCreation(DateTime? from, DateTime? to)
        {
            if (from.HasValue || to.HasValue)
                if (from.HasValue && to.HasValue)
                    Where(x => x.Creation >= from.Value && x.Creation <= to.Value);
                else if (to.HasValue)
                    Where(x => x.Creation <= to.Value);
                else
                    Where(x => x.Creation >= from.Value);
            return this;
        }







        public IPositionSeeker OrderByLongitude(SortOrder sortOrder)
        {
            OrderBy(x => x.Longitude, sortOrder);
            return this;
        }
        public IPositionSeeker OrderByLatitude(SortOrder sortOrder)
        {
            OrderBy(x => x.Latitude, sortOrder);
            return this;
        }

        public IPositionSeeker OrderByCreation(SortOrder sortOrder)
        {
            OrderBy(x => x.Creation, sortOrder);
            return this;
        }


        public IPositionSeeker OrderByUserId(SortOrder sortOrder)
        {
            OrderBy(x => x.UserId, sortOrder);
            return this;
        }

    }
}
