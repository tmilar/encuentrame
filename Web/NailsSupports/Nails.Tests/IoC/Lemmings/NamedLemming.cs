using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming("sarasa")]
    public class NamedLemming
    {
        [Inject("lalala")]
        public IServiceDependency Dependency { get; set; }
    }
}