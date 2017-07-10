using NailsFramework.Support;
using NailsFramework.Tests.IoC.Lemmings;
using NUnit.Framework;

namespace NailsFramework.Tests.Support
{
    [TestFixture]
    public class TypeFullNameTests
    {
        public class Nested
        {
        }

        public class GenericNested<T1, T2>
        {
        }

        [Test]
        public void GenericClass()
        {
            Assert.AreEqual("NailsFramework.Tests.IoC.Lemmings.GenericService<System.String>",
                            typeof (GenericService<string>).FullFriendlyName());
        }

        [Test]
        public void GenericClassWithNestedTypeAsTypeParameter()
        {
            Assert.AreEqual(
                "NailsFramework.Tests.IoC.Lemmings.GenericService<NailsFramework.Tests.Support.TypeFullNameTests.Nested>",
                typeof (GenericService<Nested>).FullFriendlyName());
        }

        [Test]
        public void GenericNestedClass()
        {
            Assert.AreEqual("NailsFramework.Tests.Support.TypeFullNameTests.GenericNested<System.String, System.Int32>",
                            typeof (GenericNested<string, int>).FullFriendlyName());
        }

        [Test]
        public void NonGenericClass()
        {
            Assert.AreEqual("NailsFramework.Tests.Support.TypeFullNameTests",
                            typeof (TypeFullNameTests).FullFriendlyName());
        }

        [Test]
        public void NonGenericNestedClass()
        {
            Assert.AreEqual("NailsFramework.Tests.Support.TypeFullNameTests.Nested", typeof (Nested).FullFriendlyName());
        }

        [Test]
        public void TheWholeTest()
        {
            Assert.AreEqual(
                "NailsFramework.Tests.Support.TypeFullNameTests.GenericNested<NailsFramework.Tests.Support.TypeFullNameTests.GenericNested<System.String, System.Int32>, System.Int32>",
                typeof (GenericNested<GenericNested<string, int>, int>).FullFriendlyName());
        }
    }
}