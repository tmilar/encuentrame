using Encuentrame.Model.Accounts;

namespace Encuentrame.Web.Models.Accounts
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string InternalNumber { get; set; }
        public string PhoneNumber { get; set; }
        public RoleEnum Role { get; set; }
    }
}