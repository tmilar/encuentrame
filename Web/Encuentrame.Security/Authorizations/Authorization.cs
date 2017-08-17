using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Security.Authorizations
{
    [Lemming]
    public class Authorization : IAuthorization
    {
        [Inject]
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }

        [Inject]
        public IBag<TokenApiSession> TokenApiSessions { get; set; }


        public bool Validate(string username, RoleEnum[] roles)
        {
          
            var user = Users.FirstOrDefault(x => x.Username == username && x.DeletedKey == null );
            if(user==null)
            {
                return false;
            }
            return roles.Any(x =>x== user.Role);
        }

        public bool ValidateToken(int userId, string token)
        {
            var user = Users[userId];
            if (user == null)
            {
                return false;
            }
            var now = SystemDateTime.Now;
             var tokenApiSession= TokenApiSessions.Where(x => x.ExpiredDateTime <= now && x.Token == token && x.UserId == userId)
                .FirstOrDefault();
            if (tokenApiSession != null)
            {
                AuthenticationProvider.RegenerateApiTokenUser(tokenApiSession);
                return true;
            }
            return false;
        }
    }
}
