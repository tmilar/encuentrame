using System;
using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Apis.Positions
{
    public class PositionApiModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-90,90, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        public decimal Latitude { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        public decimal Longitude { get; set; }
        public decimal Accuracy { get; set; }
        /// <summary>
        /// rotacion respecto al norte
        /// </summary>
        public decimal Heading { get; set; }
        public decimal Speed { get; set; }
        
    }
}