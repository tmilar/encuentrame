using System;

namespace Encuentrame.Model.AreYouOks
{
    public class PositionWhereWasSeenInfo
    {
        public int Id { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public DateTime? When { get; set; }
        public int Status { get; set; }
    }
}
