using System;
using NailsFramework.Config;
using NailsFramework.TestSupport;
using NUnit.Framework;

namespace NailsFramework.Tests.TestSupport
{
    [TestFixture]
    public class NailsTestsConfigurableTests
    {
        class UnconfiguredConfigurableTests : NailsTestsConfigurable
        {
            public override string ConfigurationName { get { return "UnconfiguredConfigurableTests"; } }
        }

        class ConfiguredConfigurableTests : NailsTestsConfigurable
        {
            public override string ConfigurationName { get { return "ConfiguredConfigurableTests"; } }

            static ConfiguredConfigurableTests()
            {
                NailsTestsConfigurationRepository.Instance.Set("ConfiguredConfigurableTests", ConfigureTestDelegate);
            }
        }

        private static bool configureTestDelegateCalled;
        static void ConfigureTestDelegate(INailsConfigurator nails)
        {
            configureTestDelegateCalled = true;
            nails.IoC.Container<NailsFramework.IoC.Spring>()
               .Initialize();
        }

        [SetUp]
        public void SetUp()
        {
            configureTestDelegateCalled = false;
        }

        [Test, ExpectedException(typeof(MissingNailsTestsConfigurationException))]
        public void SetUpWithoutTheConfiguration_ShouldThrowException()
        {
            new UnconfiguredConfigurableTests().SetUp();
        }

        [Test]
        public void SetUpWithTheConfiguration_ShouldCallConfigurationDelegate()
        {
            new ConfiguredConfigurableTests().SetUp();
            Assert.IsTrue(configureTestDelegateCalled);
        }
    }
}
