using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;

namespace NailsFramework.Tests.Persistence.LinqToSql.TestModel
{
    public class Country : Model<Country>, ICountry<City>
    {
        public IQueryable<City> QueryCities()
        {
            return QueryCollection(x => x.Cities);
        }

        public void RemoveCity(City city)
        {
            Cities.Remove(city);
            city.CountryId = null;
        }

        private EntitySet<City> cities;

        public Country()
        {
            Cities = new EntitySet<City>();
        }

        public IList<City> Cities
        {
            get { return cities; }
            set
            {
                if (value is EntitySet<City>)
                {
                    cities = (EntitySet<City>) value;
                }
                else
                {
                    var e = new EntitySet<City>();
                    e.AddRange(value);
                    cities = e;
                }
            }
        }

        #region ICountry<City> Members

        public int Id { get; protected set; }
        public string Name { get; set; }

        public IEnumerable<City> CitiesStartingWith(string start)
        {
            return QueryCollection(x => x.Cities).Where(x => x.Name.StartsWith(start));
        }

        public void AddCity(string name)
        {
            Cities.Add(new City {Name = name, CountryId = Id});
        }

        #endregion
    }
}