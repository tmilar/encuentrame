using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming]
    public class ServiceWithGenericDependency
    {
        [Inject]
        public IGenericService<string> GenericDependency { get; set; }
    }
}