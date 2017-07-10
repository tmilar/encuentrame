using System.Data.Linq.Mapping;
using System.Reflection;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.Tests.Persistence.LinqToSql
{
    public class TestHelper : PersistenceTestHelper
    {
        private static MappingSource mappingSource;

        public override DataMapper CreateDataMapper()
        {
            if (mappingSource == null)
            {
                var mapping = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "NailsFramework.Tests.Persistence.LinqToSql.TestModel.Mappings.xml");
                mappingSource = XmlMappingSource.FromStream(mapping);
                mapping.Dispose();
            }

            var linq2Sql = new NailsFramework.Persistence.LinqToSql
                               {
                                   MappingSource = mappingSource
                               };
            return linq2Sql;
        }

        protected override void DoCreateDatabase()
        {
            var dataContext = Nails.ObjectFactory.GetObject<ILinqToSqlContext>().CurrentDataContext;

            if (dataContext.DatabaseExists())
                dataContext.DeleteDatabase();
            dataContext.CreateDatabase();
        }
    }
}