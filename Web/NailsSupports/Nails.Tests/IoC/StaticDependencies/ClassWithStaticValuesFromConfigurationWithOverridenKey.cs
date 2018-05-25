using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.StaticDependencies
{
    public class ClassWithStaticValuesFromConfigurationWithOverridenKey
    {
        [Inject("AppSettingsValue")]
        public static string ConfigurationValue { get; set; }
    }
}