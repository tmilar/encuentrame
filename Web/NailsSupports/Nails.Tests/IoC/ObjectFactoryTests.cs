using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;
using NailsFramework.Tests.IoC.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC
{
    [TestFixture("Spring")]
    [TestFixture("Unity")]
    public class ObjectFactoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            objectFactory = IoCContainers.GetObjectFactory(ioc);
        }

        #endregion

        private IConfigurableObjectFactory objectFactory;

        private readonly string ioc;

        public ObjectFactoryTests(string ioc)
        {
            this.ioc = ioc;
        }

        [Test]
        public void GenericLemmings()
        {
            objectFactory.Configure(new[] {Lemming.From(typeof (GenericService<>))}, new Injection[0]);
            var service = objectFactory.GetObject<GenericService<string>>();
            Assert.IsNotNull(service);
        }

        [Test]
        public void GenericReferenceInjections()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (GenericService<>)),
                                            Lemming.From(typeof (ServiceWithGenericDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<ServiceWithGenericDependency>();
            Assert.IsInstanceOf<GenericService<string>>(service.GenericDependency);
        }

        [Test]
        public void GetFromInterface()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (Service)),
                                            Lemming.From(typeof (ServiceDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<IService>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetFromName()
        {
            var lemming = Lemming.From(typeof (Service));
            lemming.Name = "test";
            objectFactory.Configure(new[]
                                        {
                                            lemming,
                                            Lemming.From(typeof (ServiceDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<IService>("test");

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetFromNameNonGeneric()
        {
            var lemming = Lemming.From(typeof (Service));
            lemming.Name = "test";
            objectFactory.Configure(new[]
                                        {
                                            lemming,
                                            Lemming.From(typeof (ServiceDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject("test");

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetFromType()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (Service)),
                                            Lemming.From(typeof (ServiceDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<Service>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetFromTypeGeneric()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (ServiceWithGenericDependency)),
                                            Lemming.From(typeof (GenericService<>))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<GenericService<string>>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetFromTypeGenericNotInjected()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (GenericService<>))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<GenericService<string>>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetFromTypeGenericNotInjectedWithGenericDependencies()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (GenericServiceWithGenericDependency<>)),
                                            Lemming.From(typeof (GenericService<>))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<GenericServiceWithGenericDependency<string>>();

            Assert.IsNotNull(service);
            Assert.IsNotNull(service.Dependency);
        }

        [Test]
        public void GetFromTypeGenericNotInjectedWithGenericDependenciesOfSameType()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (GenericServiceWithGenericDependencyOfSameType<>)),
                                            Lemming.From(typeof (GenericService<>))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<GenericServiceWithGenericDependencyOfSameType<string>>();

            Assert.IsNotNull(service);
            Assert.IsNotNull(service.Dependency);
        }

        [Test]
        public void GetFromTypeNonGeneric()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (Service)),
                                            Lemming.From(typeof (ServiceDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject(typeof (Service));

            Assert.IsNotNull(service);
        }

        [Test]
        public void GetGenericByNameAndGenericType()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (GenericService<>))
                                        }, new Injection[0]);

            var service =
                objectFactory.GetObject<IGenericService<string>>("NailsFramework.Tests.IoC.Lemmings.GenericService");
            Assert.IsNotNull(service);
        }

        [Test]
        public void GetGenericByNameAndTypeParameters()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (GenericService<>))
                                        }, new Injection[0]);

            var service = objectFactory.GetGenericObject<string>("NailsFramework.Tests.IoC.Lemmings.GenericService");
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<IGenericService<string>>(service);
        }

        [Test]
        public void GetObjects()
        {
            var lemming = Lemming.From(typeof (ServiceDependency));
            lemming.Name = "test";
            objectFactory.Configure(new[]
                                        {
                                            lemming,
                                            Lemming.From(typeof (ServiceDependency)),
                                        }, new Injection[0]);

            var services = objectFactory.GetObjects<IServiceDependency>();

            Assert.AreEqual(2, services.Count());
        }

        [Test]
        public void InstanceInjection()
        {
            objectFactory.Configure(new[] {Lemming.From<ServiceDependency>()}, new Injection[0]);

            var instance = new NonLemmingWithInjections();

            objectFactory.Inject(instance);
            Assert.IsNotNull(instance.Dependency);
            Assert.AreEqual("test123", instance.Value);
        }

        [Test]
        public void NonSingletonsShouldNotBeSingletons()
        {
            var lemming = Lemming.From<ServiceDependency>();
            lemming.Singleton = false;
            objectFactory.Configure(new[] {lemming}, new Injection[0]);
            var o1 = objectFactory.GetObject<ServiceDependency>();
            var o2 = objectFactory.GetObject<ServiceDependency>();

            Assert.AreNotEqual(o1, o2);
        }

        [Test]
        public void NullValueInjections()
        {
            var property = typeof (LemmingWithValuesFromConfiguration).GetProperty("ConfigurationValue");
            var lemming = Lemming.From(typeof (LemmingWithValuesFromConfiguration));
            lemming.RemoveInjectionOf(property);
            lemming.Inject(new ValueInjection(property, null));

            objectFactory.Configure(new[] {lemming}, new Injection[0]);

            var service = objectFactory.GetObject<LemmingWithValuesFromConfiguration>();
            Assert.AreEqual(null, service.ConfigurationValue);
        }

        [Test]
        public void ReferenceInjections()
        {
            objectFactory.Configure(new[]
                                        {
                                            Lemming.From(typeof (Service)),
                                            Lemming.From(typeof (ServiceDependency))
                                        }, new Injection[0]);

            var service = objectFactory.GetObject<Service>();
            Assert.IsInstanceOf<ServiceDependency>(service.Dependency);
        }

        [Test]
        public void ShouldBeAbleToAddGenericLemmingsOfDifferentTypes()
        {
            var serviceLemming = Lemming.From<ServiceWithGenericDependency>();
            serviceLemming.Injections.OfType<ReferenceInjection>().Single().ReferencedLemming = "Test1";

            var anotherServiceLemming = Lemming.From<AnotherServiceWithGenericDependency>();
            anotherServiceLemming.Injections.OfType<ReferenceInjection>().Single().ReferencedLemming = "Test2";

            var test1 = Lemming.From(typeof (GenericService<>));
            test1.Name = "Test1";

            var test2 = Lemming.From(typeof (GenericService<>));
            test2.Name = "Test2";

            var lemmings = new[] {serviceLemming, anotherServiceLemming, test1, test2};

            objectFactory.Configure(lemmings, new Injection[0]);

            var service = objectFactory.GetObject<ServiceWithGenericDependency>();
            Assert.IsNotNull(service);
            Assert.IsNotNull(service.GenericDependency);

            var service2 = objectFactory.GetObject<ServiceWithGenericDependency>();
            Assert.IsNotNull(service2);
            Assert.IsNotNull(service2.GenericDependency);
        }

        [Test]
        public void SingletonsShouldBeSingletons()
        {
            objectFactory.Configure(new[] {Lemming.From<ServiceDependency>()}, new Injection[0]);
            var o1 = objectFactory.GetObject<ServiceDependency>();
            var o2 = objectFactory.GetObject<ServiceDependency>();

            Assert.AreEqual(o1, o2);
        }


        [Test]
        public void SingletonsShouldBeSingletonsWhenGettedFromDifferentTypes()
        {
            objectFactory.Configure(new[] {Lemming.From<ServiceDependency>()}, new Injection[0]);
            var o1 = objectFactory.GetObject<ServiceDependency>();
            var o2 = objectFactory.GetObject<IServiceDependency>();

            Assert.AreEqual(o1, o2);
        }

        [Test]
        public void SingletonsShouldBeSingletonsWhenGettedFromTypeAndName()
        {
            var lemming = Lemming.From<ServiceDependency>();
            lemming.Name = "Test";
            objectFactory.Configure(new[] {lemming}, new Injection[0]);

            var o1 = objectFactory.GetObject<ServiceDependency>();
            var o2 = objectFactory.GetObject("Test");

            Assert.AreEqual(o1, o2);
        }

        [Test]
        public void ValueInjections()
        {
            var property = typeof (LemmingWithValuesFromConfiguration).GetProperty("ConfigurationValue");
            var lemming = Lemming.From(typeof (LemmingWithValuesFromConfiguration));
            lemming.RemoveInjectionOf(property);
            lemming.Inject(new ValueInjection(property, "test"));

            objectFactory.Configure(new[] {lemming}, new Injection[0]);

            var service = objectFactory.GetObject<LemmingWithValuesFromConfiguration>();
            Assert.AreEqual("test", service.ConfigurationValue);
        }
    }
}