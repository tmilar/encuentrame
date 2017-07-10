using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class GenericServiceWithGenericDependency<T> : IGenericService<T>
    {
        [Inject]
        public GenericService<string> Dependency { get; set; }
    }
}