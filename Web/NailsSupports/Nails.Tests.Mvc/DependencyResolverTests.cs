using System;
using NailsFramework.IoC;
using NailsFramework.UserInterface;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Mvc.Tests
{
    [TestFixture]
    public class DependencyResolverTests
    {
        private IObjectFactory objectFactory;
        private Type serviceType;
        private DependencyResolver dependencyResolver;

        [SetUp]
        public void Setup()
        {
            objectFactory = MockRepository.GenerateMock<IObjectFactory>();
            serviceType = typeof(string);

            dependencyResolver = new DependencyResolver(objectFactory);
        }

        [Test]
        public void TestGetService_ShouldDelegateToObjectFactoryGetObject()
        {
            var service = "Fake Service";
            objectFactory.Expect(x => x.GetObject(serviceType)).Return(service);

            var returnedService = dependencyResolver.GetService(serviceType);

            objectFactory.VerifyAllExpectations();
            Assert.AreEqual(service, returnedService);
        }

        [Test]
        public void TestGetServices_ShouldDelegateToObjectFactoryGetObjects()
        {
            var services = new[] { "Fake Service", "Another Fake Service", "And yet another one" };
            objectFactory.Expect(x => x.GetObjects(serviceType)).Return(services);

            var returnedServices = dependencyResolver.GetServices(serviceType);

            objectFactory.VerifyAllExpectations();
            CollectionAssert.AreEqual(services, returnedServices);
        }
    }
}
