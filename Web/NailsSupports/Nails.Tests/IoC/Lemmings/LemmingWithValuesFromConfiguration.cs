using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming]
    public class LemmingWithValuesFromConfiguration
    {
        [Inject]
        public string ConfigurationValue { get; set; }
    }
}