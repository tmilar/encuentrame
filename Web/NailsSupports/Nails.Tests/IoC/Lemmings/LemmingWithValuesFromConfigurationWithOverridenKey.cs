using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming]
    public class LemmingWithValuesFromConfigurationWithOverridenKey
    {
        [Inject("AppSettingsValue")]
        public string ConfigurationValue { get; set; }
    }
}