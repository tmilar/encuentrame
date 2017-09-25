using System;
using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Events
{
    public class EventModel
    {

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        [Display(ResourceType = typeof(Translations), Name = "Latitude")]
        public decimal Latitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        [Display(ResourceType = typeof(Translations), Name = "Longitude")]
        public decimal Longitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "BeginDateTime")]
        public DateTime BeginDateTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "EndDateTime")]
        public DateTime EndDateTime { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "City")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string City { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Province")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Province { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Zip")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Zip { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Street")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Street { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Number")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Number { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "FloorAndDepartament")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string FloorAndDepartament { get; set; }

    }
}