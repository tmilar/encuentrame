using System.IO;
using System.Web.Mvc;
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