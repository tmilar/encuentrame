using System.Collections.Generic;
using System.Linq;
using NailsFramework.Aspects;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class GenericInheritorsTests : AspectConditionsTests
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();
            ApplyBehavior().ToInheritorsOf(typeof (GenericTestClass<>));
            aspect = Nails.Configuration.Aspects.Single();
        }

        #endregion

        private Aspect aspect;

        [Test]
        public void Inheritor()
        {
            Assert.IsTrue(aspect.AppliesTo(typeof (GenericTestSubClass<int>).GetMethods().First()));
        }

        [Test]
        public void NotInheritor()
        {
            Assert.IsFalse(aspect.AppliesTo(typeof (List<int>).GetMethods().First()));
        }

        [Test]
        public void ParentType()
        {
            Assert.IsFalse(aspect.AppliesTo(typeof (IGenericTestInterface<int>).GetMethods().First()));
        }

        [Test]
        public void SameType()
        {
            Assert.IsTrue(aspect.AppliesTo(typeof (GenericTestClass<int>).GetMethods().First()));
        }
    }
}