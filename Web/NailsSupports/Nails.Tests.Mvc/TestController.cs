using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork.ContextProviders;
using NailsFramework.UserInterface;

namespace NailsFramework.Mvc.Tests
{
    public class TestController : NailsController
    {
        [Inject]
        public virtual IWorkContextProvider WorkContextProvider { get; set; }

        public virtual ActionResult Index()
        {
            ViewData["UnitOfWork"] = WorkContextProvider.CurrentContext.IsUnitOfWorkRunning;
            return View();
        }
    }
}