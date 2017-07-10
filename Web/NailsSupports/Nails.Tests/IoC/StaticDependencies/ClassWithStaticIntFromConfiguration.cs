
using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.StaticDependencies
{
    public class ClassWithStaticIntFromConfiguration
    {
        [Inject]
        public static int ConfigurationValue { get; set; }
    }
}