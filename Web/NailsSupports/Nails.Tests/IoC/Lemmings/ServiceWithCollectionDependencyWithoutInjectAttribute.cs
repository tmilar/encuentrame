using System.Collections.Generic;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class ServiceWithCollectionDependencyWithoutInjectAttribute
    {
        public IEnumerable<IServiceDependency> Dependency { get; set; }
    }
}