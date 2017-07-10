using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class GenericLemmingWithValuesFromConfiguration<T>
    {
        [Inject]
        public string ConfigurationValue { get; set; }
    }
}