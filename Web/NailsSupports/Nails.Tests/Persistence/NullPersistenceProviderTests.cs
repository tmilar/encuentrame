using NailsFramework.Persistence;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    [TestFixture]
    public class NullPersistenceProviderTests
    {
        [Test]
        public void ShouldDoNothing()
        {
            var provider = new NullPersistenceContext();

            new NullObjectTester().Test<ITransactionalContext>(new NullTransactionalContext())
                .Test<DataMapper>(new NullDataMapper())
                .Test<IPersistenceContext>(provider);

            Assert.IsInstanceOf<NullTransactionalContext>(provider.CreateTransactionalContext());
        }
    }
}