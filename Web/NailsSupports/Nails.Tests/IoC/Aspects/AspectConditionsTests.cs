using System;
using NailsFramework.Config;
using NailsFramework.IoC;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC.Aspects
{
    public abstract class AspectConditionsTests : BaseTest
    {
        [SetUp]
        public virtual void SetUp()
        {
            Nails.Configure()
                .IoC.Container<NullIoCContainer>();
        }

        protected static BehaviorConfigurator ApplyBehavior()
        {
            return Nails.Configure()
                .Aspects.ApplyBehavior<NullBehavior>();
        }

        #region Nested type: ClassLevelAttribute

        [AttributeUsage(AttributeTargets.Class)]
        protected class ClassLevelAttribute : Attribute
        {
        }

        #endregion

        #region Nested type: GenericTestClass

        protected class GenericTestClass<T> : IGenericTestInterface<T>
        {
            #region IGenericTestInterface<T> Members

            public void TestMethod()
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: GenericTestSubClass

        protected class GenericTestSubClass<T> : GenericTestClass<T>
        {
            public void TestMethodWithAttribute()
            {
            }
        }

        #endregion

        #region Nested type: IGenericTestInterface

        protected interface IGenericTestInterface<T>
        {
            void TestMethod();
        }

        #endregion

        #region Nested type: ITestInterface

        protected interface ITestInterface
        {
            void TestMethod();
        }

        #endregion

        #region Nested type: MethodLevelAttribute

        [AttributeUsage(AttributeTargets.Method)]
        protected class MethodLevelAttribute : Attribute
        {
        }

        #endregion

        #region Nested type: TestClass

        [ClassLevelAttribute]
        protected class TestClass : ITestInterface
        {
            #region ITestInterface Members

            public void TestMethod()
            {
            }

            #endregion

            public void AnotherTestMethod()
            {
            }
        }

        #endregion

        #region Nested type: TestSubClass

        protected class TestSubClass : TestClass
        {
            [MethodLevelAttribute]
            public void TestMethodWithAttribute()
            {
            }
        }

        #endregion
    }
}