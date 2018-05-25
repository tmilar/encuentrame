using System.Web.Http;
using NailsFramework.IoC;

namespace NailsFramework.UserInterface
{
    [ApiUnitOfWorkFilter, Lemming(Singleton = false)]
    public abstract class NailsApiController : ApiController
    {
    }
}
