using System;
using System.ComponentModel.DataAnnotations;
using Encuentrame.Web.MetadataProviders.ConditionalValidations;
using Encuentrame.Web.MetadataProviders.CustomValidations;

namespace Encuentrame.Web.Models.Businesses
{
    public class BusinessModel
    {
      
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "Name")]
        public string Name { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "Cuit")]
        [Cuit]
        public string Cuit { get; set; }


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

        [Display(ResourceType = typeof(Translations), Name = "Username")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [RequiredIf("Id",0, Operation = OperationsEnum.Equals , ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Username { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Lastname")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public string Lastname { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Firstname")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public string Firstname { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Email")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailError")]
        [UIHint("string")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Firstname")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [RequiredIf("Id", 0, Operation = OperationsEnum.Equals, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [UIHint("password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "RePassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RePassword")]
        [Compare("Password")]
        [UIHint("password")]
        public string RePassword { get; set; }
    }
}