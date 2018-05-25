using System;
using System.Collections;
using System.Collections.Generic;
using NailsFramework.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Support
{
    [TestFixture]
    public class GenericDefinitionIsAssignablerFromTests
    {
        private class TestClass<T>
        {
        }

        private class TestClass<T1, T2> : TestClass<T1>, ITestInterface<T1>
        {
        }

        private class TestClass<T1, T2, T3> : TestClass<T1, T2>, ITestInterface<T1, T2>
        {
        }

        private class TestClassWithLess<T1> : TestClass<T1, int>, ITestInterface<T1, int>
        {
        }

        private class TestClass : TestClass<int>, ITestInterface<int>
        {
        }

        private interface ITestInterface<T>
        {
        }

        private interface ITestInterface<T1, T2> : ITestInterface<T1>
        {
        }

        private interface ITestInterface<T1, T2, T3> : ITestInterface<T1, T2>
        {
        }

        private interface ITestInterfaceWithLess<T1> : ITestInterface<T1, int>
        {
        }

        private interface ITestInterface : ITestInterface<int>
        {
        }


        [Test]
        public void ClassAssignableFromAGenericClassWithLessArguments()
        {
            Assert.That(typeof (TestClass<,>).GenericDefinitionIsAssignableFrom(typeof (TestClassWithLess<string>)));
        }

        [Test]
        public void ClassAssignableFromAGenericClassWithMoreArguments()
        {
            Assert.That(
                typeof (TestClass<,>).GenericDefinitionIsAssignableFrom(typeof (TestClass<int, string, DateTime>)));
        }

        [Test]
        public void ClassAssignableFromAGenericInterfaceWithLessArguments()
        {
            Assert.That(typeof (ITestInterface<,>).GenericDefinitionIsAssignableFrom(typeof (TestClassWithLess<string>)));
        }

        [Test]
        public void ClassAssignableFromAGenericInterfaceWithMoreArguments()
        {
            Assert.That(
                typeof (ITestInterface<,>).GenericDefinitionIsAssignableFrom(typeof (TestClass<int, string, DateTime>)));
        }

        [Test]
        public void ClassAssignableFromANonGenericClass()
        {
            Assert.That(typeof (TestClass<>).GenericDefinitionIsAssignableFrom(typeof (TestClass)));
        }

        [Test]
        public void ClassAssignableFromANonGenericInterface()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (TestClass)));
        }

        [Test]
        public void ClassAssignableFromSameType()
        {
            Assert.That(typeof (TestClass<>).GenericDefinitionIsAssignableFrom(typeof (TestClass<int>)));
        }

        [Test]
        public void ClassNotAssignableFromGenericInterfaceWithSameParameters()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (IList<int>)), Is.False);
        }

        [Test]
        public void ClassNotAssignableFromGenericWithSameParameters()
        {
            Assert.That(typeof (TestClass<>).GenericDefinitionIsAssignableFrom(typeof (List<int>)), Is.False);
        }

        [Test]
        public void ClassNotAssignableFromNonGeneric()
        {
            Assert.That(typeof (TestClass<>).GenericDefinitionIsAssignableFrom(typeof (ArrayList)), Is.False);
        }

        [Test]
        public void ClassNotAssignableFromNonGenericInterface()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (IList)), Is.False);
        }

        [Test]
        public void InterfaceAssignableFromAGenericInterfaceWithLessArguments()
        {
            Assert.That(
                typeof (ITestInterface<,>).GenericDefinitionIsAssignableFrom(typeof (ITestInterfaceWithLess<string>)));
        }

        [Test]
        public void InterfaceAssignableFromAGenericInterfaceWithMoreArguments()
        {
            Assert.That(
                typeof (ITestInterface<,>).GenericDefinitionIsAssignableFrom(
                    typeof (ITestInterface<int, string, DateTime>)));
        }

        [Test]
        public void InterfaceAssignableFromANonGenericInterface()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (ITestInterface)));
        }

        [Test]
        public void InterfaceAssignableFromSameType()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (ITestInterface<int>)));
        }

        [Test]
        public void InterfaceNotAssignableFromGenericWithSameParameters()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (IList<int>)), Is.False);
        }

        [Test]
        public void InterfaceNotAssignableFromNonGeneric()
        {
            Assert.That(typeof (ITestInterface<>).GenericDefinitionIsAssignableFrom(typeof (IList)), Is.False);
        }
    }
}