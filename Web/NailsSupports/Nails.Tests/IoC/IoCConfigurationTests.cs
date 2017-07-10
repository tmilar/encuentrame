using System;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Persistence;
using NailsFramework.Tests.IoC.Lemmings;
using NailsFramework.Tests.IoC.StaticDependencies;
using NailsFramework.Tests.Support;
using NailsFramework.Tests.UserInterface.MVP;
using NailsFramework.UnitOfWork;
using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace NailsFramework.Tests.IoC
{
    [TestFixture]
    public class IoCConfigurationTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            iocContainer = MockRepository.GenerateMock<IoCContainer>();
        }

        #endregion

        private static MockRepository mocks;
        private IoCContainer iocContainer;
        private BaseObjectFactory objectFactory;

        private class TestService
        {
            public IService Dependency { get; set; }
            public string Value { get; set; }
        }

        private static IEnumerable<Lemming> AddedLemmings
        {
            get { return Nails.Configuration.LemmingsSchema.Where(x => x.ConcreteType != typeof (NullLog)); }
        }

        private void ConfigureIoC(params Type[] expectedTypes)
        {
            objectFactory = mocks.PartialMock<BaseObjectFactory>();

            var typesList = new List<Type>(expectedTypes)
                                {
                                    typeof (NullPersistenceContext),
                                    //by default
                                    typeof (TestExecutionContext)
                                };

            objectFactory.Expect("AddToContext").Repeat.Times(typesList.Count)
                .Callback((Lemming l) => typesList.Remove(l.ConcreteType));

            objectFactory.Expect("FindObjectByName").IgnoreArguments().Repeat.Any().Return(null);
            objectFactory.Expect("FindObjectByType").IgnoreArguments().Repeat.Any().Return(null);
            objectFactory.Expect(x => x.GetObject(default(Type))).IgnoreArguments().Repeat.Any().Return(null);
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            mocks.ReplayAll();
        }

        private class AnotherGenericService<T> : IGenericService<T>
        {
        }

        private class NullUnitOfWork : ICurrentUnitOfWork
        {
            #region ICurrentUnitOfWork Members

            public UnitOfWorkCache Cache
            {
                get { return null; }
            }

            public void OnSuccessCall(Action handler)
            {
            }

            public void OnFailureCall(Action<Exception> handler)
            {
            }

            public void Checkpoint()
            {
            }

            public void Cancel()
            {
            }

            #endregion
        }

        [Test]
        public void ObjectFactoryConfiguration()
        {
            var objectFactory = MockRepository.GenerateMock<IConfigurableObjectFactory>();

            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Initialize(configureDefaults: false);

            Assert.AreEqual(objectFactory, Nails.ObjectFactory);
        }

        [Test]
        public void ShouldAcceptTwoOfTheSameType()
        {
            ConfigureIoC(typeof (ServiceDependency), typeof (ServiceDependency));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<ServiceDependency>()
                .Lemming<ServiceDependency>("test")
                .Initialize(configureDefaults: false);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldAddGenericDefinitionLemmingsExplictly()
        {
            ConfigureIoC(typeof (GenericService<string>), typeof (ServiceWithGenericDependency));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<>))
                .Lemming(typeof (ServiceWithGenericDependency))
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldAddGenericLemmingsExplicitly()
        {
            ConfigureIoC(typeof (GenericService<string>));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<string>))
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldAddInjectionsByAttributesWhenAddingLemmingExplicitly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<Service>()
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (Service));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual("Dependency", injection.Property.Name);
        }

        [Test]
        public void ShouldAddLemmingsExplictly()
        {
            ConfigureIoC(typeof (ServiceDependency));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<ServiceDependency>()
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldAddNotDependedGenericLemmings()
        {
            ConfigureIoC(typeof (GenericService<>));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<>))
                .Initialize(configureDefaults: false);

            iocContainer.VerifyAllExpectations();
        }

        [Test]
        public void ShouldAddToContextGenericTypeNotPreviouslyAdded()
        {
            var objectFactory = mocks.PartialMock<BaseObjectFactory>();

            objectFactory.Expect("AddToContext")
                .Callback((Lemming l) => l.ConcreteType == typeof (GenericService<int>))
                .Repeat.Once();

            objectFactory.Expect("AddToContext")
                .Callback((Lemming l) => l.ConcreteType != typeof (GenericService<int>))
                .Repeat.Any();

            objectFactory.Expect("FindObjectByType")
                .Constraints(Is.Same(typeof (GenericService<int>)))
                .Return(null)
                .Repeat.Once();

            objectFactory.Expect("FindObjectByType")
                .Constraints(Is.Same(typeof (GenericService<int>)))
                .Return(new GenericService<int>())
                .Repeat.Once();

            mocks.DynamicMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            mocks.ReplayAll();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<>))
                .Initialize(configureDefaults: false);

            objectFactory.GetObject<GenericService<int>>();

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldBeAbleToAddGenericLemmingsOfDifferentTypes()
        {
            ConfigureIoC(typeof (AnotherServiceWithGenericDependency),
                         typeof (ServiceWithGenericDependency),
                         typeof (GenericService<string>),
                         typeof (GenericService<int>));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<ServiceWithGenericDependency>()
                .Lemming<AnotherServiceWithGenericDependency>()
                .Lemming(typeof (GenericService<>))
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldConfigureNameExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<Service>(x => x.Name("test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (Service));

            Assert.AreEqual(lemming.Name, "test");
        }

        [Test]
        public void ShouldConfigureNameExplictlyNonGeneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (Service), x => x.Name("test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (Service));

            Assert.AreEqual(lemming.Name, "test");
        }

        [Test]
        public void ShouldConfigureRefereceExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<TestService>(x => x.Reference(l => l.Dependency))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual("Dependency", injection.Property.Name);
        }

        [Test]
        public void ShouldConfigureRefereceExplictlyNonGeneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (TestService), x => x.Reference("Dependency"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual("Dependency", injection.Property.Name);
        }

        [Test]
        public void ShouldConfigureRefereceWithImplementationExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<TestService>(x => x.Reference<Service>(l => l.Dependency))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.SingleOrDefault(x => x.ConcreteType == typeof (TestService));
            var reference = AddedLemmings.SingleOrDefault(x => x.ConcreteType == typeof (Service));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual(reference.Name, injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureRefereceWithImplementationExplictlyNonGeneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (TestService), x => x.Reference<Service>(property: "Dependency"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.SingleOrDefault(x => x.ConcreteType == typeof (TestService));
            var reference = AddedLemmings.SingleOrDefault(x => x.ConcreteType == typeof (Service));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual(reference.Name, injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureRefereceWithOverridenNameExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<TestService>(x => x.Reference(l => l.Dependency, referencedLemming: "test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual("test", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureRefereceWithOverridenNameExplictlyNonGeneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (TestService), x => x.Reference("Dependency", referencedLemming: "test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual("test", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureSingletonExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<Service>(x => x.Singleton(false))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (Service));

            Assert.IsFalse(lemming.Singleton);
        }

        [Test]
        public void ShouldConfigureSingletonExplictlyNonGeneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (Service), x => x.Singleton(false))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (Service));

            Assert.IsFalse(lemming.Singleton);
        }

        [Test]
        public void ShouldConfigureStaticGenericDependency()
        {
            ConfigureIoC(typeof (GenericService<string>));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<>))
                .InjectStaticPropertiesOf<ClassWithAStaticGenericDependency>()
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldConfigureValueExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<TestService>(x => x.Value(l => l.Value, "test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ValueInjection) lemming.Injections.Single();

            Assert.AreEqual("test", injection.Value);
        }

        [Test]
        public void ShouldConfigureValueExplictlyNonGneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (TestService), x => x.Value("Value", "test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ValueInjection) lemming.Injections.Single();

            Assert.AreEqual("test", injection.Value);
        }

        [Test]
        public void ShouldConfigureValueFromAppSettingsExplictly()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<TestService>(x => x.ValueFromConfiguration(l => l.Value, appSetting: "AppSettingsValue"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ConfigurationInjection) lemming.Injections.Single();

            Assert.AreEqual("test123", injection.Value);
        }

        [Test]
        public void ShouldConfigureValueFromAppSettingsExplictlyNonGeneric()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (TestService), x => x.ValueFromConfiguration("Value", appSetting: "AppSettingsValue"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (TestService));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ConfigurationInjection) lemming.Injections.Single();

            Assert.AreEqual("test123", injection.Value);
        }

        [Test]
        public void ShouldFailIfConfiguringAReferenceToANonExistingLemming()
        {
            ConfigureIoC(typeof (Service));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<Service>();

            Assert.Throws<InvalidOperationException>(() => Nails.Configure().Initialize(configureDefaults: false));
        }

        [Test]
        public void ShouldHandleRefereceCollections()
        {
            ConfigureIoC(typeof (ServiceWithCollectionDependency),
                         typeof (LemmingsCollection<IServiceDependency>),
                         typeof (ServiceDependency),
                         typeof (NullObjectFactory));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<NullObjectFactory>() //Needed by the LemmingsCollection
                .Lemming<ServiceWithCollectionDependency>()
                .Lemming<ServiceDependency>()
                .Initialize(configureDefaults: false);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldHandleRefereceCollectionsExplicitly()
        {
            ConfigureIoC(typeof (ServiceWithCollectionDependencyWithoutInjectAttribute),
                         typeof (LemmingsCollection<IServiceDependency>),
                         typeof (ServiceDependency),
                         typeof (ObjectFactoryProxy));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<ObjectFactoryProxy>() //Needed by the LemmingsCollection
                .Lemming<ServiceWithCollectionDependencyWithoutInjectAttribute>(l => l.Reference(x => x.Dependency))
                .Lemming<ServiceDependency>()
                .Initialize(configureDefaults: false);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotAddNotDependedGenericLemmings()
        {
            ConfigureIoC();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<>))
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldNotHandleRefereceCollectionsIfReferencedLemmingIsSetted()
        {
            ConfigureIoC(typeof (ServiceWithManagedCollectionDependency),
                         typeof (DependencyCollection));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<ServiceWithManagedCollectionDependency>()
                .Lemming<DependencyCollection>("ServiceDependencyCollection")
                .Initialize(configureDefaults: false);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldOverrideGenericDefinitionsWithSpecificImplementations()
        {
            ConfigureIoC(typeof (GenericService<int>),
                         typeof (ServiceWithGenericDependency),
                         typeof (AnotherServiceWithGenericDependency),
                         typeof (AnotherGenericService<string>));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming(typeof (GenericService<>))
                .Lemming(typeof (ServiceWithGenericDependency))
                .Lemming(typeof (AnotherServiceWithGenericDependency))
                .Lemming<AnotherGenericService<string>>()
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldOverrideInjectAttributeValues()
        {
            var objectFactory = MockRepository.GenerateStub<IConfigurableObjectFactory>();
            MockRepository.GenerateMock<IoCContainer>();
            iocContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<NamedLemming>(x => x.Reference(l => l.Dependency, referencedLemming: "test"))
                .Initialize(configureDefaults: false);

            var lemming = AddedLemmings.Single(x => x.ConcreteType == typeof (NamedLemming));

            Assert.AreEqual(lemming.Injections.Count(), 1);
            var injection = (ReferenceInjection) lemming.Injections.Single();

            Assert.AreEqual("test", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldReadLemmingsFromAssembly()
        {
            ConfigureIoC(typeof (Service),
                         typeof (ServiceDependency),
                         typeof (ServiceWithGenericDependency),
                         typeof (GenericService<string>),
                         typeof (NamedLemming),
                         typeof (LemmingWithValuesFromConfiguration),
                         typeof (LemmingWithValuesFromConfigurationWithOverridenKey),
                         typeof (NoSingletonLemming),
                         typeof (LemmingWithStaticInjection),
                         typeof (LemmingsCollection<IServiceDependency>),
                         typeof (NullObjectFactory),
                         typeof (ConcreteLemming),
                         typeof (NullUnitOfWork),
                         typeof (TestPresenter),
                         typeof (TestSingletonPresenter));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<NullObjectFactory>()
                .Lemming<NullUnitOfWork>()
                .InspectAssembly("Nails.Tests.dll")
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldBeAbleToIgnoreLemmingsFromAssembly()
        {
            ConfigureIoC(typeof(Service),
                         typeof(ServiceDependency),
                         typeof(ServiceWithGenericDependency),
                         typeof(GenericService<string>),
                         typeof(NamedLemming),
                         typeof(LemmingWithValuesFromConfiguration),
                         typeof(LemmingWithValuesFromConfigurationWithOverridenKey),
                         typeof(NoSingletonLemming),
                         typeof(LemmingWithStaticInjection),
                         typeof(LemmingsCollection<IServiceDependency>),
                         typeof(NullObjectFactory),
                         typeof(ConcreteLemming),
                         typeof(NullUnitOfWork),
                         typeof(TestSingletonPresenter));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<NullObjectFactory>()
                .Lemming<NullUnitOfWork>()
                .Lemming<TestPresenter>(x => x.Ignore())
                .InspectAssembly("Nails.Tests.dll")
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }

        [Test]
        public void ShouldBeAbleToIgnoreLemmingsFromAssemblyUsingNonGenericIgnore()
        {
            ConfigureIoC(typeof(Service),
                         typeof(ServiceDependency),
                         typeof(ServiceWithGenericDependency),
                         typeof(GenericService<string>),
                         typeof(NamedLemming),
                         typeof(LemmingWithValuesFromConfiguration),
                         typeof(LemmingWithValuesFromConfigurationWithOverridenKey),
                         typeof(NoSingletonLemming),
                         typeof(LemmingWithStaticInjection),
                         typeof(LemmingsCollection<IServiceDependency>),
                         typeof(NullObjectFactory),
                         typeof(ConcreteLemming),
                         typeof(NullUnitOfWork),
                         typeof(TestSingletonPresenter));

            Nails.Configure()
                .IoC.Container(iocContainer)
                .Lemming<NullObjectFactory>()
                .Lemming<NullUnitOfWork>()
                .Lemming(typeof (TestPresenter), x => x.Ignore())
                .InspectAssembly("Nails.Tests.dll")
                .Initialize(configureDefaults: false);

            objectFactory.VerifyAllExpectations();
        }
    }
}