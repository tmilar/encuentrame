using System.Collections.Generic;
using System.Linq;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;

namespace NailsFramework.Tests.Persistence.NHibernate.TestModel
{
    public class Country : Model<Country>, ICountry<City>
    {
        public Country()
        {
            Cities = new List<City>();
        }

        public virtual IList<City> Cities { get; protected set; }

        #region ICountry<City> Members

        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }

        public virtual IEnumerable<City> CitiesStartingWith(string start)
        {
            return QueryCollection(x => x.Cities).Where(x => x.Name.StartsWith(start));
        }

        public virtual void AddCity(string name)
        {
            Cities.Add(new City {Name = name});
        }

        #endregion

        public virtual IQueryable<City> QueryCities()
        {
            return QueryCollection(x => x.Cities);
        }

        public virtual void RemoveCity(City city)
        {
            Cities.Remove(city);
        }

    }
}