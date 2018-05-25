using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;

namespace NailsFramework.Tests.IoC.StaticDependencies
{
    public class ClassWithStaticDependencies
    {
        [Inject]
        public static IServiceDependency StaticDependency { get; set; }

        public static IServiceDependency StaticDependencyWithoutAttribute { get; set; }
    }
}