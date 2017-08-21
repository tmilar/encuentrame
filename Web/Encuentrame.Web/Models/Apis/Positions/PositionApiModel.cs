using System;
using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Apis.Positions
{
    public class PositionApiModel
    {
        [Required]
        [Range(1,Int32.MaxValue)]
        public int UserId { get; set; }
        [Required]
        [Range(-90,90)]
        public decimal Latitude { get; set; }
        [Required]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }
    }
}