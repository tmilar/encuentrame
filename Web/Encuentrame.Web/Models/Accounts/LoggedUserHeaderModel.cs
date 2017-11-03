using System.ComponentModel.DataAnnotations;
using Encuentrame.Model.Accounts;

namespace Encuentrame.Web.Models.Accounts
{
    public class LoggedUserHeaderModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public RoleEnum Role { get; set; }
        public int Id { get; set; }

        public bool IsLogged => Id > 0;

        [UIHint("image")]
        public string Image { get; set; }
    }
}