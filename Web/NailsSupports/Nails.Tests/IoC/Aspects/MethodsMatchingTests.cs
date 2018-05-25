using System.Linq;
using NailsFramework.Aspects;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class MethodsMatchingTests : AspectConditionsTests
    {
        private Aspect aspect;

        [Test]
        public void ClassName()
        {
            ApplyBehavior().ToMethodsMatching(".*TestClass*");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethod("TestMethod")));
        }

        [Test]
        public void FullClassName()
        {
            ApplyBehavior().ToMethodsMatching(@"NailsFramework\.Tests\.IoC\.Aspects\.AspectConditionsTests\.TestClass*");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethod("TestMethod")));
        }

        [Test]
        public void FullMethodName()
        {
            ApplyBehavior().ToMethodsMatching(
                @"NailsFramework\.Tests\.IoC\.Aspects\.AspectConditionsTests\.TestClass\.TestMethod*");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethod("TestMethod")));
        }

        [Test]
        public void FullNamespace()
        {
            ApplyBehavior().ToMethodsMatching(@"NailsFramework\.Tests\.IoC\.Aspects\.*");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestSubClass).GetMethod("TestMethod")));
        }

        [Test]
        public void MethodName()
        {
            ApplyBehavior().ToMethodsMatching(@".*TestClass\.TestMethod");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethod("TestMethod")));
        }

        [Test]
        public void Namespace()
        {
            ApplyBehavior().ToMethodsMatching(@".*Tests\.IoC\.Aspects*");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestSubClass).GetMethod("TestMethod")));
        }

        [Test]
        public void NotMatching()
        {
            ApplyBehavior().ToMethodsMatching("lalala");
            aspect = Nails.Configuration.Aspects.Single();
            Assert.IsFalse(aspect.AppliesTo(typeof (ITestInterface).GetMethod("TestMethod")));
        }
    }
}