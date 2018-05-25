using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming]
    public class LemmingWithStaticInjection
    {
        [Inject]
        public static string ConfigurationValue { get; set; }
    }
}