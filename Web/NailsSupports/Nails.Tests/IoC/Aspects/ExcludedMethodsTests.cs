using System.Linq;
using NailsFramework.Aspects;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class ExcludedMethodsTests : AspectConditionsTests
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();
            ApplyBehavior().ExcludingMethods("AnotherTestMethod").ToMethodsSatisfying(x => true);
            aspect = Nails.Configuration.Aspects.Single();
        }

        #endregion

        private Aspect aspect;

        [Test]
        public void ShouldIgnoreExcludedMethods()
        {
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethod("TestMethod")));
        }

        [Test]
        public void ShouldNotIgnoreNotExcludedMethods()
        {
            Assert.IsFalse(aspect.AppliesTo(typeof (TestClass).GetMethod("AnotherTestMethod")));
        }
    }
}