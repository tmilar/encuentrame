using MemoDb;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;
using NailsFramework.Tests.Persistence.Memory.TestModel;

namespace NailsFramework.Tests.Persistence.Memory
{
    public class TestHelper : PersistenceTestHelper
    {
        public override DataMapper CreateDataMapper()
        {
            var memo = new Memo(deleteOrphansByCascadeByDefault:true, 
                                insertByCascadeByDefault:true)
                          .Map<Country>(c=>c.SetId("Id", autoAssign: true))
                          .Map<City>(c => c.SetId("Id", autoAssign: true));

            return new NailsFramework.Persistence.Memory(memo);
        }

        protected override void DoCreateDatabase()
        {
        }

        public override bool AddsIdsToCollectionItems
        {
            get { return false; }
        }
    }
}