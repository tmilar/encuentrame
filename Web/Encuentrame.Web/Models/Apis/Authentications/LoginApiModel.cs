using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Apis.Authentications
{
    public class LoginApiModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Password { get; set; }

    }
}