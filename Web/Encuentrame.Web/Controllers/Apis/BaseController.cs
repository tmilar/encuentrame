using Encuentrame.Web.Filters;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.Session;
using NailsFramework.UserInterface;

namespace Encuentrame.Web.Controllers.Apis
{
    
    [AuthorizationApiFilter]
    public abstract class BaseApiController : NailsApiController
    {
        [Inject]
        public ISessionContext SessionContext { get; set; }

        [Inject]
        public ICurrentUnitOfWork CurrentUnitOfWork { get; set; }

        [Inject]
        public ILog Log { get; set; }



     
    }
}