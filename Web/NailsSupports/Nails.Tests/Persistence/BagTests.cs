using NailsFramework.Tests.Persistence.Common;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    [TestFixture(typeof(Memory.TestModel.Country), typeof(Memory.TestHelper))]
    [TestFixture(typeof(NHibernate.TestModel.Country), typeof(NHibernate.TestHelper))]
    [TestFixture(typeof(LinqToSql.TestModel.Country), typeof(LinqToSql.TestHelper))]
    public class BagTests<TCountry, TPersistenceTestHelper> : BagTestsBase<TCountry, TPersistenceTestHelper>
        where TCountry : class, ICountry, new()
        where TPersistenceTestHelper : PersistenceTestHelper, new()
    {
    }
}
