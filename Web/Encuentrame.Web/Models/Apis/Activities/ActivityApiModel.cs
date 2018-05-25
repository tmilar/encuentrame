using System;
using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Apis.Activities
{
    public class ActivityApiModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        public decimal Latitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        public decimal Longitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public DateTime BeginDateTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public DateTime EndDateTime { get; set; }

        public int? EventId { get; set; }

    }
}