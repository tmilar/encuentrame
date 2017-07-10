using System.Collections.Generic;
using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class ClassWithStaticCollectionReference
    {
        [Inject]
        public static IEnumerable<IServiceDependency> Dependency { get; set; }
    }
}