using System;

namespace Encuentrame.Web.Models.Apis.SoughtPersons
{
    public class SoughtPersonSeenApiModel
    {
        public string Info { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsOk { get; set; }
        public DateTime When { get; set; }
    }
}