using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Models.Apis.Accounts
{
    public class UserApiModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [EmailAddress]
        public  string EmailAlternative { get; set; }

        public  string InternalNumber { get; set; }
        public  string PhoneNumber { get; set; }
        public  string MobileNumber { get; set; }
        
        public  string Image { get; set; }


    }
}