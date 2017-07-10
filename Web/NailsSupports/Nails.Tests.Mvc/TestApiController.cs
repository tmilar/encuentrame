using NailsFramework.IoC;
using NailsFramework.UnitOfWork.ContextProviders;
using NailsFramework.UserInterface;

namespace NailsFramework.Mvc.Tests
{
    public class TestApiController : NailsApiController
    {
        [Inject]
        public virtual IWorkContextProvider WorkContextProvider { get; set; }

        public virtual bool Index()
        {
            return WorkContextProvider.CurrentContext.IsUnitOfWorkRunning;
         }
    }
}