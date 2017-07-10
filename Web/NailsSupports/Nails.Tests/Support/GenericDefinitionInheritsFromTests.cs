using System;
using System.Collections.Generic;
using NailsFramework.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Support
{
    [TestFixture]
    public class GenericDefinitionInheritsFromTests
    {
        private interface Interface<T, T2> : SuperInterface<T, T2>
        {
        }

        private interface Interface<T> : SuperInterface<T, int>
        {
        }

        private interface Interface<T, T2, T3> : SuperInterface<T, T2>
        {
        }

        public interface SuperInterface<T, T2>
        {
        }

        private class SuperClass<T, T2> : Interface<T, T2>
        {
        }

        private class Class<T, T2> : SuperClass<T, T2>
        {
        }

        private class Class<T> : SuperClass<T, int>
        {
        }

        private class Class<T, T2, T3> : SuperClass<T, T2>
        {
        }

        private interface IConstraint
        {
        }

        private class ClassWithConstraint<T, T1> : InterfaceWithConstraint<T, T1> where T : IConstraint
        {
        }

        private interface InterfaceWithConstraint<T, T1> : Interface<T, T1> where T : IConstraint
        {
        }

        public class Constraint : IConstraint
        {
        }

        [Test]
        public void ClassThatDoesntInheritsAGenericClass()
        {
            Assert.That(typeof (Class<,>).GenericDefinitionInheritsFrom(typeof (List<int>)), Is.False);
        }

        [Test]
        public void ClassThatDoesntInheritsAGenericInterface()
        {
            Assert.That(typeof (Class<,>).GenericDefinitionInheritsFrom(typeof (IList<int>)), Is.False);
        }

        [Test]
        public void ClassThatInheritsAGenericClassWithLessParameters()
        {
            Assert.That(typeof (Class<,,>).GenericDefinitionInheritsFrom(typeof (SuperClass<int, string>)), Is.False);
        }

        [Test]
        public void ClassThatInheritsAGenericClassWithSameParameters()
        {
            Assert.That(typeof (Class<,>).GenericDefinitionInheritsFrom(typeof (SuperClass<int, string>)));
        }

        [Test]
        public void ClassThatInheritsAGenericInterfaceMatchingConstraints()
        {
            Assert.That(
                typeof (ClassWithConstraint<,>).GenericDefinitionInheritsFrom(typeof (Interface<Constraint, string>)));
        }

        [Test]
        public void ClassThatInheritsAGenericInterfaceNotMatchingConstraints()
        {
            Assert.That(typeof (ClassWithConstraint<,>).GenericDefinitionInheritsFrom(typeof (Interface<int, string>)),
                        Is.False);
        }

        [Test]
        public void ClassThatInheritsAGenericInterfaceWithMoreParametersWithRightType()
        {
            Assert.That(typeof (Class<>).GenericDefinitionInheritsFrom(typeof (Interface<DateTime, int>)));
        }

        [Test]
        public void ClassThatInheritsAGenericInterfaceWithMoreParametersWithWrongType()
        {
            Assert.That(typeof (Class<>).GenericDefinitionInheritsFrom(typeof (Interface<int, string>)), Is.False);
        }

        [Test]
        public void ClassThatInheritsAGenericInterfaceWithSameParameters()
        {
            Assert.That(typeof (Class<,>).GenericDefinitionInheritsFrom(typeof (Interface<int, string>)));
        }

        [Test]
        public void IntefaceThatDoesntInheritFromAGenericInterface()
        {
            Assert.That(typeof (Interface<,,>).GenericDefinitionInheritsFrom(typeof (SuperInterface<int, string>)),
                        Is.False);
        }

        [Test]
        public void InterfaceThatInheritsAGenericInterfaceMatchingConstraints()
        {
            Assert.That(
                typeof (InterfaceWithConstraint<,>).GenericDefinitionInheritsFrom(typeof (Interface<Constraint, string>)));
        }

        [Test]
        public void InterfaceThatInheritsAGenericInterfaceNotMatchingConstraints()
        {
            Assert.That(
                typeof (InterfaceWithConstraint<,>).GenericDefinitionInheritsFrom(typeof (Interface<int, string>)),
                Is.False);
        }

        [Test]
        public void InterfaceThatInheritsAGenericInterfaceWithMoreParametersWithRightType()
        {
            Assert.That(typeof (Interface<>).GenericDefinitionInheritsFrom(typeof (SuperInterface<DateTime, int>)));
        }

        [Test]
        public void InterfaceThatInheritsAGenericInterfaceWithMoreParametersWithWrongType()
        {
            Assert.That(typeof (Interface<>).GenericDefinitionInheritsFrom(typeof (SuperInterface<int, string>)),
                        Is.False);
        }

        [Test]
        public void InterfaceThatInheritsAGenericInterfaceWithSameParameters()
        {
            Assert.That(typeof (Interface<,>).GenericDefinitionInheritsFrom(typeof (SuperInterface<int, string>)));
        }

        [Test]
        public void ParentTypeShouldNotBeBeAGenericTypeDefinitions()
        {
            Assert.Throws<InvalidOperationException>(
                () => typeof (Interface<>).GenericDefinitionInheritsFrom(typeof (Interface<>)));
        }

        [Test]
        public void ShouldBeAppliedToAGenericTypeDefinitions()
        {
            Assert.Throws<InvalidOperationException>(
                () => typeof (Interface<int>).GenericDefinitionInheritsFrom(typeof (Interface<int>)));
        }
    }
}