using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class NonLemmingWithInjections
    {
        [Inject("test")]
        public IServiceDependency Dependency { get; set; }

        [Inject("AppSettingsValue")]
        public string Value { get; set; }
    }
}