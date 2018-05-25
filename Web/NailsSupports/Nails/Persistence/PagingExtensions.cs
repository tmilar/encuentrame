using System.Linq;

namespace NailsFramework.Persistence
{
    public static class PagingExtensions
    {
        public static IPage<T> Page<T>(this IQueryable<T> self, int pageNumber)
        {
            return new Page<T>(self, pageNumber);
        }

        public static IPage<T> Page<T>(this IQueryable<T> self, int pageNumber, int pageSize)
        {
            return new Page<T>(self, pageNumber, pageSize);
        }
    }
}