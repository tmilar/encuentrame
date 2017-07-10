using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.StaticDependencies
{
    public class ClassWithStaticValuesFromConfiguration
    {
        [Inject]
        public static string ConfigurationValue { get; set; }
    }
}