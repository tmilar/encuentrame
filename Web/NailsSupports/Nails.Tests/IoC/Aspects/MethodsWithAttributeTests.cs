using System.Linq;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class MethodsWithAttributeTests : AspectConditionsTests
    {
        [Test]
        public void ShouldApplyToMethodWithAttribute()
        {
            ApplyBehavior().ToMethodsWithAttribute<MethodLevelAttribute>();
            var aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestSubClass).GetMethods().First()));
        }

        [Test]
        public void ShouldNotApplyToMethodFromClassWithAttribute()
        {
            ApplyBehavior().ToMethodsWithAttribute<ClassLevelAttribute>();
            var aspect = Nails.Configuration.Aspects.Single();
            Assert.IsFalse(aspect.AppliesTo(typeof (TestClass).GetMethods().First()));
        }

        [Test]
        public void ShouldNotApplyToMethodWithoutAttribute()
        {
            ApplyBehavior().ToMethodsWithAttribute<MethodLevelAttribute>();
            var aspect = Nails.Configuration.Aspects.Single();
            Assert.IsFalse(aspect.AppliesTo(typeof (TestClass).GetMethods().First()));
        }
    }
}