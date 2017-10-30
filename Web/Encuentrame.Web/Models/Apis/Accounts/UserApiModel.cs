using System;
using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Apis.Accounts
{
    public class UserApiModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailError")]
        public string Email { get; set; }
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailError")]
        public  string EmailAlternative { get; set; }
        public  string InternalNumber { get; set; }
        public  string PhoneNumber { get; set; }
        public  string MobileNumber { get; set; }
        


    }
}