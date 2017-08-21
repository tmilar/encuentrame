using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Apis.Authentications
{
    public class LoginApiModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}