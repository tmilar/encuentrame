using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming]
    public class Service : IService
    {
        [Inject]
        public IServiceDependency Dependency { get; set; }
    }
}