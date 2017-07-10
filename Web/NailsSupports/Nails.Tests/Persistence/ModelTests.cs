using System.Collections.Generic;
using System.Linq;
using MemoDb;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    [TestFixture]
    public class ModelTests : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            var memo = new Memo(deleteOrphansByCascadeByDefault: true,
                                insertByCascadeByDefault: true)
                           .Map<TestModel>(c => c.SetId("Id", autoAssign: true));

            Nails.Configure()
                    .IoC.Container<Unity>()
                    .Persistence.DataMapper( new NailsFramework.Persistence.Memory(memo))
                    .Initialize();
        }

        private class TestModel : Model<TestModel>
        {
            public int Id { get; set; }

            public IList<string> Items { get; set; }

            public IEnumerable<string> QueryItems()
            {
                return QueryCollection(x => x.Items);
            }
        }

        [Test]
        public void ShouldQueryCollections()
        {
            RunInUnitOfWork(() =>
                                {
                                    var bag = Nails.ObjectFactory.GetObject<IBag<TestModel>>();

                                    bag.Put(new TestModel
                                                {
                                                    Items = new List<string>
                                                                {
                                                                    "test1",
                                                                    "test2"
                                                                }
                                                });
                                    bag.Put(new TestModel
                                                {
                                                    Items = new List<string>
                                                                {
                                                                    "test3",
                                                                    "test4"
                                                                }

                                                });

                                    var items = new TestModel
                                                    {
                                                        Id = 1
                                                    }.QueryItems().ToList();

                                    Assert.AreEqual(2, items.Count);
                                    Assert.IsTrue(items.Contains("test1"));
                                    Assert.IsTrue(items.Contains("test2"));
                                });
        }
    }
}