using System.Linq;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class TypeWithAttributeTests : AspectConditionsTests
    {
        [Test]
        public void ShouldApplyToClassWithAttribute()
        {
            ApplyBehavior().ToTypesWithAttribute<ClassLevelAttribute>();
            var aspect = Nails.Configuration.Aspects.Single();
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethods().First()));
        }

        [Test]
        public void ShouldNotApplyToMethodWithAttributeOfATypeWithoutAttribute()
        {
            ApplyBehavior().ToTypesWithAttribute<MethodLevelAttribute>();
            var aspect = Nails.Configuration.Aspects.Single();
            Assert.IsFalse(aspect.AppliesTo(typeof (TestSubClass).GetMethods().First()));
        }

        [Test]
        public void ShouldNotApplyToTypeWithoutAttribute()
        {
            ApplyBehavior().ToMethodsWithAttribute<ClassLevelAttribute>();
            var aspect = Nails.Configuration.Aspects.Single();
            Assert.IsFalse(aspect.AppliesTo(typeof (TestSubClass).GetMethods().First()));
        }
    }
}