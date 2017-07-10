using System.Collections.Generic;
using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class ServiceWithManagedCollectionDependency
    {
        [Inject("ServiceDependencyCollection")]
        public IEnumerable<IServiceDependency> Dependency { get; set; }
    }
}