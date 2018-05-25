using System.Collections.Generic;
using System.Linq;

namespace NailsFramework.Tests.Persistence.Common
{
    public static class CityQueries
    {
        public static TCity GetCityNamed<TCity>(this IEnumerable<TCity> self, string name) where TCity : ICity
        {
            return self.SingleOrDefault(x => x.Name == name);
        }
    }
}