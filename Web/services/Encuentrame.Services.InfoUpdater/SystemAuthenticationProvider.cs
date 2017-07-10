﻿using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Security.Authentications;

namespace Encuentrame.Services.InfoUpdater
{
    [Lemming]
    public class SystemAuthenticationProvider : AuthenticationProvider
    {
        public IBag<SystemUser> SystemUsers { get; set; }

        public override bool ValidateUser(string username, string password)
        {
            return true;
        }

        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            return true;
        }

        public override bool ResetPassword(string username)
        {
            return true;
        }

        public override BaseUser GetLoggedUser()
        {
            return SystemUsers.ToList().FirstOrDefault();
        }
    }
}