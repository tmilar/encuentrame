using System;

namespace Encuentrame.Model.Positions.Seekers
{
    public interface IPositionSeeker : ISeeker<Position>
    {
        IPositionSeeker ByUserId(int? minValue, int? maxValue);
        IPositionSeeker ByLatitude(decimal? minValue, decimal? maxValue);
        IPositionSeeker ByLongitude(decimal? minValue, decimal? maxValue);
        IPositionSeeker ByCreation(DateTime? from, DateTime? to);
     
     
       
      
        IPositionSeeker OrderByUserId(SortOrder sortOrder);
      
        IPositionSeeker OrderByLatitude(SortOrder sortOrder);
        IPositionSeeker OrderByLongitude(SortOrder sortOrder);
        IPositionSeeker OrderByCreation(SortOrder sortOrder);
     

    }
}