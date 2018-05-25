using System;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.Tests.IoC.Lemmings;
using NailsFramework.Tests.IoC.StaticDependencies;
using NailsFramework.Tests.Support;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.IoC
{
    [TestFixture]
    public class StaticInjectionTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Nails.Configure()
                .IoC.Container<NullIoCContainer>();
        }

        #endregion

        [Test]
        public void LemmingsShouldIgnoreStaticDependencies()
        {
            var lemming = Lemming.From(typeof (ClassWithStaticDependencies));
            Assert.AreEqual(lemming.Injections.Count(), 0);
        }

        [Test]
        public void ShouldAddStaticReferencesByAttributesWhenAddingAssemblies()
        {
            Nails.Configure()
                .InspectAssembly("Nails.Tests.dll")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(8, Nails.Configuration.StaticInjections.Count());
        }

        [Test]
        public void ShouldAddStaticReferencesByAttributesWhenAddingClasses()
        {
            Nails.Configure()
                .IoC.InjectStaticPropertiesOf<ClassWithStaticDependencies>()
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
        }

        [Test]
        public void ShouldAddStaticValuesFromConfigurationByAttributesWhenAddingClasses()
        {
            Nails.Configure()
                .IoC.InjectStaticPropertiesOf<ClassWithStaticValuesFromConfiguration>()
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ConfigurationInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("testConfigurationValueForDefaultKey", injection.Value);
        }

        [Test]
        public void ShouldAddStaticValuesFromConfigurationByAttributesWithOverridenKeyWhenAddingClasses()
        {
            Nails.Configure()
                .IoC.InjectStaticPropertiesOf<ClassWithStaticValuesFromConfigurationWithOverridenKey>()
                .Initialize(configureDefaults: false);

            Assert.AreEqual(1, Nails.Configuration.StaticInjections.Count());
            var injection = (ConfigurationInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("test123", injection.Value);
        }


        [Test]
        public void ShouldAddStaticValuesIntFromConfigurationByAttributesWithOverridenKeyWhenAddingClasses()
        {
            Nails.Configure()
                .IoC.InjectStaticPropertiesOf<ClassWithStaticIntFromConfigurationWithOverridenKey>()
                .Initialize(configureDefaults: false);

            Assert.AreEqual(1, Nails.Configuration.StaticInjections.Count());
            var injection = (ConfigurationInjection)Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual(123, injection.Value);
        }

        [Test]
        public void ShouldAddStaticIntFromConfigurationByAttributes()
        {
            Nails.Configure()
                .IoC.InjectStaticPropertiesOf<ClassWithStaticIntFromConfiguration>()
                .Initialize(configureDefaults: false);

            Assert.AreEqual(1, Nails.Configuration.StaticInjections.Count());
            var injection = (ConfigurationInjection)Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual(123, injection.Value);
        }

        [Test]
        public void ShouldConfigureStaticReferenceExplictly()
        {
            Nails.Configure()
                .IoC.StaticReference(() => ClassWithStaticDependencies.StaticDependency)
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
        }

        [Test]
        public void ShouldConfigureStaticReferenceExplictlyNonGeneric()
        {
            Nails.Configure()
                .IoC.StaticReference(typeof (ClassWithStaticDependencies), property: "StaticDependency")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
        }

        [Test]
        public void ShouldConfigureStaticReferenceExplitly()
        {
            Nails.Configure()
                .IoC.StaticReference(() => ClassWithStaticDependencies.StaticDependency)
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
        }

        [Test]
        public void ShouldConfigureStaticReferenceExplitlyWithOverridenName()
        {
            Nails.Configure()
                .IoC.StaticReference(() => ClassWithStaticDependencies.StaticDependency, "lala")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
            Assert.AreEqual("lala", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureStaticReferenceWithImplementationExplictly()
        {
            Nails.Configure()
                .IoC.StaticReference<ServiceDependency>(() => ClassWithStaticDependencies.StaticDependency)
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.ServiceDependency", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureStaticReferenceWithImplementationExplictlyNonGeneric()
        {
            Nails.Configure()
                .IoC.StaticReference<ServiceDependency>(typeof (ClassWithStaticDependencies),
                                                        property: "StaticDependency")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.ServiceDependency", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureStaticReferenceWithOverridenNameExplictly()
        {
            Nails.Configure()
                .IoC.StaticReference(() => ClassWithStaticDependencies.StaticDependency, "lala")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
            Assert.AreEqual("lala", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureStaticReferenceWithOverridenNameExplictlyNonGeneric()
        {
            Nails.Configure()
                .IoC.StaticReference(typeof (ClassWithStaticDependencies), property: "StaticDependency",
                                     referencedLemming: "lala")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ReferenceInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("StaticDependency", injection.Property.Name);
            Assert.AreEqual("lala", injection.ReferencedLemming);
        }

        [Test]
        public void ShouldConfigureStaticValueExplictly()
        {
            Nails.Configure()
                .IoC.StaticValue(() => ClassWithStaticValuesFromConfiguration.ConfigurationValue, "lala")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ValueInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("lala", injection.Value);
        }

        [Test]
        public void ShouldConfigureStaticValueExplictlyNonGeneric()
        {
            Nails.Configure()
                .IoC.StaticValue(typeof (ClassWithStaticValuesFromConfiguration), property: "ConfigurationValue",
                                 value: "lala")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ValueInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("lala", injection.Value);
        }

        [Test]
        public void ShouldConfigureStaticValueFromConfigurationExplictly()
        {
            Nails.Configure()
                .IoC.StaticValueFromConfiguration(() => ClassWithStaticValuesFromConfiguration.ConfigurationValue)
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ConfigurationInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("testConfigurationValueForDefaultKey", injection.Value);
        }

        [Test]
        public void ShouldConfigureStaticValueFromConfigurationExplictlyNonGeneric()
        {
            Nails.Configure()
                .IoC.StaticValueFromConfiguration(typeof (ClassWithStaticValuesFromConfiguration),
                                                  property: "ConfigurationValue")
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ConfigurationInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("testConfigurationValueForDefaultKey", injection.Value);
        }

        [Test]
        public void ShouldHandleRefereceCollectionsExplicitly()
        {
            var mocks = new MockRepository();
            var objectFactory = mocks.PartialMock<BaseObjectFactory>();

            var expectedTypes = new List<Type>
                                    {
                                        typeof (LemmingsCollection<IServiceDependency>),
                                        typeof (ServiceDependency),
                                        typeof (NullObjectFactory),
                                        //Needed by collections
                                        //by default
                                        typeof (NullPersistenceContext),
                                        typeof (TestExecutionContext)
                                    };

            objectFactory.Expect("AddToContext").Repeat.Times(expectedTypes.Count)
                .Callback((Lemming l) => expectedTypes.Remove(l.ConcreteType));

            objectFactory.Expect("FindObjectByName").IgnoreArguments().Repeat.Any().Return(null);
            objectFactory.Expect("FindObjectByType").IgnoreArguments().Repeat.Any().Return(null);

            objectFactory.Expect(x => x.GetObject(typeof (IEnumerable<IServiceDependency>))).Return(null);

            var ioCContainer = mocks.DynamicMock<IoCContainer>();
            ioCContainer.Expect(x => x.GetObjectFactory()).Return(objectFactory).Repeat.Any();

            mocks.ReplayAll();

            Nails.Configure()
                .IoC.Container(ioCContainer)
                .IoC.InjectStaticPropertiesOf<ClassWithStaticCollectionReference>()
                .Lemming<ServiceDependency>()
                .Lemming<NullObjectFactory>()
                .Initialize(configureDefaults: false);

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldInjectStaticsOfLemmings()
        {
            Nails.Configure()
                .IoC.Lemming<LemmingWithStaticInjection>()
                .Initialize(configureDefaults: false);

            Assert.AreEqual(Nails.Configuration.StaticInjections.Count(), 1);
            var injection = (ConfigurationInjection) Nails.Configuration.StaticInjections.Single();

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
        }

        [Test]
        public void ShouldOverrideInjectAttributeValues()
        {
            Nails.Configure()
                .InspectAssembly("Nails.Tests.dll")
                .IoC.StaticValue(() => ClassWithStaticValuesFromConfiguration.ConfigurationValue, "lala")
                .Initialize(configureDefaults: false);

            var injection = Nails.Configuration
                .StaticInjections
                .OfType<ValueInjection>()
                .Single(x => x.Property.DeclaringType.Name == "ClassWithStaticValuesFromConfiguration" &&
                             x.Property.Name == "ConfigurationValue");

            Assert.AreEqual("ConfigurationValue", injection.Property.Name);
            Assert.AreEqual("lala", injection.Value);
        }
    }
}