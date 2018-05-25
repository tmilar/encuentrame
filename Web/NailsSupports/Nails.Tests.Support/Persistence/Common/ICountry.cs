using System.Collections.Generic;
using System.Linq;

namespace NailsFramework.Tests.Persistence.Common
{
    public interface ICountry
    {
        int Id { get; }
        string Name { get; set; }
    }

    public interface ICountry<TCity> : ICountry where TCity : ICity
    {
        IEnumerable<TCity> CitiesStartingWith(string start);
        void AddCity(string name);
        IQueryable<TCity> QueryCities();
        void RemoveCity(TCity city);
    }
}