using System.Collections.Generic;
using NailsFramework.Aspects;
using NailsFramework.Tests.IoC.Support;
using NailsFramework.Tests.Support;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.IoC.Aspects
{
    [TestFixture("Spring")]
    [TestFixture("Unity")]
    public class VirtualMethodsAspectsTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();

            var behavior = mocks.DynamicMock<ILemmingBehavior>();
            
           Nails.Configure()
                .IoC.Container(IoCContainers.GetContainer(ioc))
               .IoC.Lemming<VirtualTest>()
               .Initialize(configureDefaults: false);

            proxyFactory = Nails.ObjectFactory.GetObject<IVirtualMethodsProxyFactory>();
            aspect = new Aspect(behavior, new BehaviorCondition(m => true, new List<string>()));

        }

        #endregion

        private readonly string ioc;
        private IVirtualMethodsProxyFactory proxyFactory;
        private Aspect aspect;
        private MockRepository mocks;

        public VirtualMethodsAspectsTests(string ioc)
        {
            this.ioc = ioc;
        }
       

        [Test]
        public void ShouldApplyAspectToVirtualMethodsIgnoringInterfaces()
        {
            aspect.Behavior.Expect(x => x.ApplyTo(null)).IgnoreArguments().Repeat.Once();
            mocks.ReplayAll();

            var aopTest = proxyFactory.Create<VirtualTest>(with: aspect);

            Assert.IsNotNull(aopTest);

            aopTest.Virtual();
            mocks.VerifyAll();
        }

        [Test]
        public void ShouldExposeNonVirtualMethodsFromOriginalClass()
        {
            aspect.Behavior.Expect(x => x.ApplyTo(null)).IgnoreArguments().Repeat.Never();
            mocks.ReplayAll();

            var aopTest = proxyFactory.Create<VirtualTest>(with: aspect);

            Assert.IsNotNull(aopTest);

            Assert.AreEqual("test", aopTest.NonVirtual());
            mocks.VerifyAll();
        }

        public class VirtualTest : IIgnore
        {
            public virtual void Virtual()
            {

            }

            public string NonVirtual()
            {
                return "test";
            }
        }
        public interface IIgnore
        {
            string NonVirtual();
        }
    }
}