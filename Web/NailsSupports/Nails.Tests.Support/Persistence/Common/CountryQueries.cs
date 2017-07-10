using System.Collections.Generic;
using System.Linq;

namespace NailsFramework.Tests.Persistence.Common
{
    public static class CountryQueries
    {
        public static TCountry GetCountryNamed<TCountry>( this IEnumerable<TCountry> self, string name) where TCountry : ICountry
        {
            return self.Single(x => x.Name == name);
        }
    }
}