using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;

namespace NailsFramework.Tests.Persistence.LinqToSql.TestModel
{
    public class City : Model<City>, ICity
    {
        public int? CountryId { get; set; }
        public int? Id { get; set; }
        object ICity.Id
        {
            get { return Id; }
        }
        #region ICity Members

        public string Name { get; set; }

        #endregion
    }
}