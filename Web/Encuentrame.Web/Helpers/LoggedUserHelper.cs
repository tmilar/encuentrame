using System;
using System.Web.Security;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Web.Models.Accounts;
using NailsFramework.IoC;

namespace Encuentrame.Web.Helpers
{
    public static class LoggedUserHelper
    {
        [Inject]
        public static IAuthenticationProvider AuthenticationProvider { get; set; }

        public static LoggedUserHeaderModel Get()
        {
            try
            {
                var loggedUser = AuthenticationProvider.GetLoggedUser() as User;
               
                if (loggedUser == null)
                {
                    FormsAuthentication.SignOut();
                    return new LoggedUserHeaderModel();
                }

                var userModel = new LoggedUserHeaderModel
                {
                    Id = loggedUser.Id,
                    Email = loggedUser.Email,
                    FullName = loggedUser.FullName,
                    Image= loggedUser.Image,
                    Role = loggedUser.Role,
                    Business= loggedUser.Role==RoleEnum.EventAdministrator? loggedUser.Business.ToDisplay():"",
                };
                return userModel;
            }
            catch (Exception ex)
            {
                return new LoggedUserHeaderModel();
            }
        }


    }
}