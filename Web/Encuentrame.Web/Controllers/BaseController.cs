using System.IO;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.Session;
using NailsFramework.UserInterface;
using Encuentrame.Web.Filters;


namespace Encuentrame.Web.Controllers
{
    [ExceptionFilter(View = "Error")]
    [AuthorizationFilter]
    public abstract class BaseController : NailsController
    {
        [Inject]
        public ISessionContext SessionContext { get; set; }

        [Inject]
        public ICurrentUnitOfWork CurrentUnitOfWork { get; set; }

        [Inject]
        public ILog Log { get; set; }

        [Inject]
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        protected User GetLoggedUser()
        {
            var user = AuthenticationProvider.GetLoggedUser() as User;
            return user;
        }

        protected bool LoggedUserIs(RoleEnum role)
        {
            var user = AuthenticationProvider.GetLoggedUser() as User;
            return user.Role==role;
        }

        protected void ValidateReference()
        {
            
        }

        protected void AddModelPersistentError(string message)
        {
            TempData.Add("ErrorMessage", message);
        }

        protected void AddModelError(string message)
        {
            ModelState.AddModelError("controller" , message);
        }
        protected void AddModelSuccess(string message)
        {
             TempData.Add("SuccessMessage",message);
        }
    }
}