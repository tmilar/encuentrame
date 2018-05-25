using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class GenericServiceWithGenericDependencyOfSameType<T> : IGenericService<T>
    {
        [Inject]
        public GenericService<T> Dependency { get; set; }
    }
}