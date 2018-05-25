using System;
using Encuentrame.Support;

namespace Encuentrame.Web.Models.Positions
{
    public class PositionListModel
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int UserId { get; set; }
        public DateTime Creation { get; set; }
    }
}