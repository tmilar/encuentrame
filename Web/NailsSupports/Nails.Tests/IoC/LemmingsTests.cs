using System;
using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC
{
    [TestFixture]
    public class LemmingsTests
    {
        [Test]
        public void ConcreteType()
        {
            var lemming = Lemming.From(typeof (Service));
            Assert.AreEqual(lemming.ConcreteType, typeof (Service));
        }

        [Test]
        public void GenericLemmingDefinitionDefaultNameShouldNotContainTypeParameters()
        {
            var lemming = Lemming.From(typeof (GenericService<>));
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.GenericService", lemming.Name);
        }

        [Test]
        public void GenericLemmingDefinitionUniqueNameShouldThrowInvalid()
        {
            var lemming = Lemming.From(typeof (GenericService<>));
            Assert.Throws<InvalidOperationException>(() => { var name = lemming.UniqueName; });
        }

        [Test]
        public void GenericLemmingNameShouldNotContainTypeParameters()
        {
            var lemming = Lemming.From(typeof (GenericService<>)).MakeGenericLemming(new[] {typeof (string)});
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.GenericService", lemming.Name);
        }

        [Test]
        public void GenericLemmingUniqueNameShouldContainTypeParameters()
        {
            var lemming = Lemming.From(typeof (GenericService<>)).MakeGenericLemming(new[] {typeof (string)});
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.GenericService<System.String>", lemming.UniqueName);
        }

        [Test]
        public void NonGenericLemmingUniqueNameShouldBeEqualToName()
        {
            var lemming = Lemming.From(typeof (Service));
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.Service", lemming.Name);
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.Service", lemming.UniqueName);
        }

        [Test]
        public void ShouldConfigureNoSingletonLemmingsFromAttribute()
        {
            var lemming = Lemming.From(typeof (NoSingletonLemming));
            Assert.IsFalse(lemming.Singleton);
        }

        [Test]
        public void ShouldSetLemmingName()
        {
            var lemming = Lemming.From(typeof (NamedLemming));
            Assert.AreEqual(lemming.Name, "sarasa");
        }

        [Test]
        public void SingletonByDefault()
        {
            var lemming = Lemming.From(typeof (Service));
            Assert.IsTrue(lemming.Singleton);
        }

        [Test]
        public void TypeNameAsLemmingNameByDefault()
        {
            var lemming = Lemming.From(typeof (Service));
            Assert.AreEqual(lemming.Name, "NailsFramework.Tests.IoC.Lemmings.Service");
        }
    }
}