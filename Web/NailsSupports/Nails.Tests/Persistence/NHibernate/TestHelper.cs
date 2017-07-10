using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;
using NHibernate.Tool.hbm2ddl;

namespace NailsFramework.Tests.Persistence.NHibernate
{
    public class TestHelper : PersistenceTestHelper
    {
        public override DataMapper CreateDataMapper()
        {
            return new NailsFramework.Persistence.NHibernate();
        }

        protected override void DoCreateDatabase()
        {
            var context = Nails.ObjectFactory.GetObject<NHibernateContext>();
            var se = new SchemaExport(context.Configuration);
            se.Create(false, true);
        }
    }
}