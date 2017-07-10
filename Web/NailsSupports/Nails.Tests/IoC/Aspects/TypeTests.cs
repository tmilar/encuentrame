using System.Collections.Generic;
using System.Linq;
using NailsFramework.Aspects;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class TypeTests : AspectConditionsTests
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();
            ApplyBehavior().ToType<TestClass>();
            aspect = Nails.Configuration.Aspects.Single();
        }

        #endregion

        private Aspect aspect;

        [Test]
        public void Inheritor()
        {
            Assert.IsFalse(aspect.AppliesTo(typeof (TestSubClass).GetMethods().First()));
        }


        [Test]
        public void Other()
        {
            Assert.IsFalse(aspect.AppliesTo(typeof (List<int>).GetMethods().First()));
        }

        [Test]
        public void Parent()
        {
            Assert.IsFalse(aspect.AppliesTo(typeof (ITestInterface).GetMethods().First()));
        }

        [Test]
        public void SameType()
        {
            Assert.IsTrue(aspect.AppliesTo(typeof (TestClass).GetMethods().First()));
        }
    }
}