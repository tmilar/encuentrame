using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC
{
    [TestFixture]
    public class InjectionsTests
    {
        [Test]
        public void ConfigurationInjectionForGenericNamedLemmingDefaultKey()
        {
            var lemming = Lemming.From(typeof (GenericLemmingWithValuesFromConfiguration<string>));
            lemming.Name = "GenericLemming";
            Assert.AreEqual(lemming.Injections.Count(), 1);

            var reference = (ConfigurationInjection) lemming.Injections.Single();
            Assert.AreEqual("ConfigurationValue", reference.Property.Name);
            Assert.AreEqual("generic-value", reference.Value);
        }

        [Test]
        public void ConfigurationInjectionForGenericNamedLemmingDefaultNonGenericKey()
        {
            var lemming = Lemming.From(typeof (GenericLemmingWithValuesFromConfiguration<string>));
            lemming.Name = "GenericLemming2";
            Assert.AreEqual(lemming.Injections.Count(), 1);

            var reference = (ConfigurationInjection) lemming.Injections.Single();
            Assert.AreEqual("ConfigurationValue", reference.Property.Name);
            Assert.AreEqual("generic2-value", reference.Value);
        }

        [Test]
        public void ConfigurationInjectionForNamedLemmingDefaultKey()
        {
            var lemming = Lemming.From(typeof (LemmingWithValuesFromConfiguration));
            lemming.Name = "Bar";
            Assert.AreEqual(lemming.Injections.Count(), 1);

            var reference = (ConfigurationInjection) lemming.Injections.Single();
            Assert.AreEqual("ConfigurationValue", reference.Property.Name);
            Assert.AreEqual("bar-value", reference.Value);
        }

        [Test]
        public void ConfigurationInjectionsOverridenKey()
        {
            var lemming = Lemming.From(typeof (LemmingWithValuesFromConfigurationWithOverridenKey));
            Assert.AreEqual(lemming.Injections.Count(), 1);

            var value = (ConfigurationInjection) lemming.Injections.Single();
            Assert.AreEqual("ConfigurationValue", value.Property.Name);
            Assert.AreEqual("test123", value.Value);
        }

        [Test]
        public void ConfigurationInjectionsShouldHaveValueAsAppSetingsWithLemmingPropertyAsKeyByDefault()
        {
            var lemming = Lemming.From(typeof (LemmingWithValuesFromConfiguration));
            Assert.AreEqual(lemming.Injections.Count(), 1);

            var reference = (ConfigurationInjection) lemming.Injections.Single();
            Assert.AreEqual("ConfigurationValue", reference.Property.Name);
            Assert.AreEqual("testConfigurationValueForDefaultKey", reference.Value);
        }

        [Test]
        public void ShouldAddInjectionsDueAttributes()
        {
            var lemming = Lemming.From(typeof (Service));
            Assert.AreEqual(1, lemming.Injections.Count());
            Assert.AreEqual("Dependency", lemming.Injections.Single().Property.Name);
        }

        [Test]
        public void ShouldSetInjectionNamesWithAttributes()
        {
            var lemming = Lemming.From(typeof (NamedLemming));
            Assert.AreEqual(lemming.Injections.Count(), 1);

            var reference = (ReferenceInjection) lemming.Injections.Single();
            Assert.AreEqual("Dependency", reference.Property.Name);
            Assert.AreEqual("lalala", reference.ReferencedLemming);
        }
    }
}