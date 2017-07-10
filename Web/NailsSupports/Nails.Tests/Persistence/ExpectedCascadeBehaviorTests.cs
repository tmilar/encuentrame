using NailsFramework.Tests.Persistence.Common;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    [TestFixture(typeof(Memory.TestModel.Country), typeof(Memory.TestModel.City), typeof(Memory.TestHelper))]
    [TestFixture(typeof(NHibernate.TestModel.Country), typeof(NHibernate.TestModel.City), typeof(NHibernate.TestHelper))]
    [TestFixture(typeof(LinqToSql.TestModel.Country), typeof(LinqToSql.TestModel.City), typeof(LinqToSql.TestHelper))]
    public class ExpectedCascadeBehaviorTests<TCountry, TCity, TPersistenceTestHelper> : ExpectedCascadeBehaviorTestsBase<TCountry, TCity, TPersistenceTestHelper>
        where TCountry : class, ICountry<TCity>, new()
        where TCity : class, ICity
        where TPersistenceTestHelper : PersistenceTestHelper, new()
    {
    }
}
