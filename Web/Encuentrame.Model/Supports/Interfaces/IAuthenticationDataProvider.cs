using System.Security.Principal;

namespace Encuentrame.Model.Supports.Interfaces
{
    public interface IAuthenticationDataProvider
    {
        IPrincipal GetCurrentUser { get; }
    }
}
