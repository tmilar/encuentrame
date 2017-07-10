using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.Async;
using NUnit.Framework;

namespace NailsFramework.Tests.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkInfoTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        [AllowAsync]
        public void AsyncMethod()
        {
        }

        [NotAsync]
        public void NotAsyncMethod()
        {
        }

        public void MethodWithoutAttributes()
        {
        }

        [UnitOfWork]
        public void MethodWithDefaultUnitOfWorkAttribute()
        {
        }

        [UnitOfWork(Name = "Test", TransactionMode = TransactionMode.NoTransaction)]
        public void MethodWithUnitOfWorkAttribute()
        {
        }

        [Test]
        public void AttributeShouldUseConfigurationTransactionMode()
        {
            Nails.Configure().UnitOfWork.DefaultTransactionMode(TransactionMode.NoTransaction);
            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithDefaultUnitOfWorkAttribute"));
            Assert.IsFalse(info.IsTransactional);
        }

        [Test]
        public void FromMethodShouldBeAsyncByDefaultIfDefaultIsAsync()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true)
                 .AllowAsyncExecution(true);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithoutAttributes"));
            Assert.IsTrue(info.IsAsync);
        }

        [Test]
        public void FromMethodShouldBeTransactionalByDefault()
        {
            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithDefaultUnitOfWorkAttribute"));
            Assert.IsTrue(info.IsTransactional);
        }

        [Test]
        public void FromMethodShouldIgnoreDefaultIfAsyncExecutionIsNotAllowed()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true)
                .AllowAsyncExecution(false);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithoutAttributes"));
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void FromMethodShouldNotBeAsyncByDefaultIfDefaultIsNotAsync()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(false)
                .AllowAsyncExecution(true);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithoutAttributes"));
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void FromMethodWithAllowAsyncAttribute()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(false)
                .AllowAsyncExecution(true);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("AsyncMethod"));
            Assert.IsTrue(info.IsAsync);
        }

        [Test]
        public void FromMethodWithNotAsyncAttribute()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true)
                .AllowAsyncExecution(true);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("NotAsyncMethod"));
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void MethodWithOverridenUnitOfWorkAttributeShouldHaveDeafaultValues()
        {
            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithUnitOfWorkAttribute"));
            Assert.IsFalse(info.IsTransactional);
        }

        [Test]
        public void MethodWithoutUnitOfWorkAttributeShouldHaveDeafaultValues()
        {
            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithoutAttributes"));
            Assert.IsTrue(info.IsTransactional);
        }

        [Test]
        public void ShouldBeTransactionalByDefault()
        {
            var info = new UnitOfWorkInfo();
            Assert.IsTrue(info.IsTransactional);
        }

        [Test]
        public void ShouldIgnoreDefaultIfNotFromMethod()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true)
                .AllowAsyncExecution(true);

            var info = new UnitOfWorkInfo();
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void ShouldIgnoreNotAsyncAttributeIfAsyncExecutionIsNotAllowed()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true)
                .AllowAsyncExecution(false);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("NotAsyncMethod"));
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void ShouldIgnoreWithAllowAsyncAttributeIfAsyncExecutionIsNotAllowed()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(false)
                .AllowAsyncExecution(false);

            var info = UnitOfWorkInfo.From(GetType().GetMethod("AsyncMethod"));
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void ShouldNotBeAsyncByDefaultIfDefaultIsNotAsync()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true)
                .AllowAsyncExecution(true);

            var info = new UnitOfWorkInfo();
            Assert.IsFalse(info.IsAsync);
        }

        [Test]
        public void ShouldUseConfigurationTransactionMode()
        {
            Nails.Configure().UnitOfWork.DefaultTransactionMode(TransactionMode.NoTransaction);
            var info = UnitOfWorkInfo.From(GetType().GetMethod("MethodWithoutAttributes"));
            Assert.IsFalse(info.IsTransactional);
        }
    }
}