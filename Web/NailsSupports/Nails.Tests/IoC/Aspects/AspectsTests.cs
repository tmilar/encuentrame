using NailsFramework.Aspects;
using NailsFramework.Tests.IoC.Support;
using NailsFramework.Tests.Support;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture("Spring")]
    [TestFixture("Unity")]
    public class AspectsTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Nails.Configure()
                .IoC.Container(IoCContainers.GetContainer(ioc));
        }

        #endregion

        private readonly string ioc;

        public AspectsTests(string ioc)
        {
            this.ioc = ioc;
        }

        public interface ITest
        {
            void Test();
        }

        public class TestClass : ITest
        {
            #region ITest Members

            public void Test()
            {
            }

            #endregion
        }

        [Test]
        public void ShouldCallBehaviorWhenConditionMatches()
        {
            var mocks = new MockRepository();

            var behavior = mocks.DynamicMock<ILemmingBehavior>();
            behavior.Expect(x => x.ApplyTo(null)).IgnoreArguments();

            mocks.ReplayAll();

            Nails.Configure()
                .IoC.Lemming<TestClass>()
                .Aspects.ApplyBehavior(behavior).ToMethodsSatisfying(m => true)
                .Initialize(configureDefaults: false);

            var obj = Nails.ObjectFactory.GetObject<ITest>();
            obj.Test();

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldCallNotBehaviorWhenConditionDoesntMatch()
        {
            var mocks = new MockRepository();

            var behavior = mocks.DynamicMock<ILemmingBehavior>();
            behavior.Expect(x => x.ApplyTo(null)).IgnoreArguments().Repeat.Never();

            mocks.ReplayAll();

            Nails.Configure()
                .IoC.Lemming<TestClass>()
                .Aspects.ApplyBehavior(behavior).ToMethodsSatisfying(m => false)
                .Initialize(configureDefaults: false);

            var obj = Nails.ObjectFactory.GetObject<ITest>();
            obj.Test();

            mocks.VerifyAll();
        }
    }
}