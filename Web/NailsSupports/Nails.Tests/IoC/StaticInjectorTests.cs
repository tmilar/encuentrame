using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;
using NailsFramework.Tests.Support;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.IoC
{
    [TestFixture]
    public class StaticInjectorTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            objectFactory = MockRepository.GenerateMock<IConfigurableObjectFactory>();
            var iocProvider = MockRepository.GenerateMock<IoCContainer>();
            iocProvider.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure().IoC.Container(iocProvider).Initialize();
            injector = new StaticInjector(objectFactory);
        }

        #endregion

        private StaticInjector injector;
        private IConfigurableObjectFactory objectFactory;

        private class TestClass
        {
            public static IServiceDependency StaticDependency { get; set; }
            public static string StaticValue { get; set; }
        }

        [Test]
        public void ShouldSetReference()
        {
            var dependency = new ServiceDependency();

            objectFactory.Expect(x => x.GetObject("test")).Return(dependency);

            var property = typeof (TestClass).GetProperty("StaticDependency");
            var reference = new ReferenceInjection(property, "test");

            injector.Inject(new[] {reference});

            Assert.AreEqual(dependency, TestClass.StaticDependency);
        }

        [Test]
        public void ShouldSetValues()
        {
            var property = typeof (TestClass).GetProperty("StaticValue");
            var reference = new ValueInjection(property, "test");

            injector.Inject(new[] {reference});

            Assert.AreEqual("test", TestClass.StaticValue);
        }
    }
}