using System;

namespace Encuentrame.Web.Models.Apis.Accounts
{
    public class UserApiResultModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
      
        public string Username { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string EmailAlternative { get; set; }
        public string InternalNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
       
    }
}