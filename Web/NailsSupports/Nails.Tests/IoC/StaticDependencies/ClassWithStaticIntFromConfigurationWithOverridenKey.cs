
using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.StaticDependencies
{
    public class ClassWithStaticIntFromConfigurationWithOverridenKey
    {
        [Inject("AppSettingsValueInt")]
        public static int ConfigurationValue { get; set; }
    }
}