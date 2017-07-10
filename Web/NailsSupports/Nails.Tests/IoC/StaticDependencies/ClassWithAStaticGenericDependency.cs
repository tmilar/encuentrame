using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;

namespace NailsFramework.Tests.IoC.StaticDependencies
{
    public class ClassWithAStaticGenericDependency
    {
        [Inject]
        public static IGenericService<string> StaticDependency { get; set; }
    }
}