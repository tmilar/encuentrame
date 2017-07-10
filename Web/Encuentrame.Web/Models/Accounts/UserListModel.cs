using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Models.Accounts
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string InternalNumber { get; set; }
        public string PhoneNumber { get; set; }
        public ItemModel Role { get; set; }
    }
}