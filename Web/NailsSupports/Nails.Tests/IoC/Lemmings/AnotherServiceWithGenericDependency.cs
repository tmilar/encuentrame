using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class AnotherServiceWithGenericDependency
    {
        [Inject]
        public IGenericService<int> GenericDependency { get; set; }
    }
}