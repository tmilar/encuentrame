using System;
using System.Linq;
using System.Security.Principal;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Security.Authentications
{
    public class DomainAuthenticationProvider : AuthenticationProvider
    {
        [Inject("TokenExpiredMinutes")]
        public int TokenExpiredMinutes { get; set; }
        [Inject]
        public IBag<User> Users { get; set; }
        [Inject]
        public IBag<TokenApiSession> TokenApiSessions { get; set; }

        [Inject]
        public IAuthenticationDataProvider AuthenticationDataProvider { get; set; }

        public override bool ValidateUser(string username, string password)
        {
            return Users.FirstOrDefault(x => x.Username == username && x.Password == password) != null;
        }

        public override TokenApiSession GenerateApiTokenUser(string username)
        {
            var user=Users.FirstOrDefault(x => x.Username == username);
            var token = Guid.NewGuid().ToString();
            var tokenApiSession=new TokenApiSession()
            {
                UserId = user.Id,
                ExpiredDateTime = SystemDateTime.Now.AddMinutes(TokenExpiredMinutes),
                Token = token,
            };
            TokenApiSessions.Put(tokenApiSession);
            return tokenApiSession;

        }
        public override void RegenerateApiTokenUser(TokenApiSession tokenApiSession)
        {

            tokenApiSession.ExpiredDateTime = SystemDateTime.Now.AddMinutes(TokenExpiredMinutes);


        }
        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            var username = CurrentUser.Identity.Name;
            var user=Users.FirstOrDefault(x => x.Username == username && x.Password == oldPassword);
            if (user != null)
            {
                user.Password = newPassword;
            }
            return user != null;
        }

        public override bool ResetPassword(string username)
        {
            var user = Users.FirstOrDefault(x => x.Username == username);
            if (user != null)
            {
                user.Password = "";
            }
            return user != null;
        }

        public override BaseUser GetLoggedUser()
        {
            var username = CurrentUser.Identity.Name;
            var user = Users.FirstOrDefault(x => x.Username == username);
            return user;
        }

        private IPrincipal CurrentUser
        {
            get { return AuthenticationDataProvider.GetCurrentUser; }
        }
    }
}
