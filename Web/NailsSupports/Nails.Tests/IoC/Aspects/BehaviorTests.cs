using System.Collections.Generic;
using System.Linq;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture]
    public class BehaviorTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        [Test]
        public void ShouldSetBehaviorToAspect()
        {
            Nails.Configure()
                .Aspects.ApplyBehavior<NullBehavior>().ToInheritorsOf<IList<string>>();

            var aspect = Nails.Configuration.Aspects.Single();

            Assert.IsInstanceOf<NullBehavior>(aspect.Behavior);
        }
    }
}