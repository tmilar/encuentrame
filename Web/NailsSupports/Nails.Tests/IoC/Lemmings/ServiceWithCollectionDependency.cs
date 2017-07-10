using System.Collections.Generic;
using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class ServiceWithCollectionDependency
    {
        [Inject]
        public IEnumerable<IServiceDependency> Dependency { get; set; }
    }
}