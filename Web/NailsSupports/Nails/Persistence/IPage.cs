using System.Collections.Generic;

namespace NailsFramework.Persistence
{
    public interface IPage<out T> : IEnumerable<T>
    {
        int PageNumber { get; }
        int PageSize { get; }
        int TotalPages { get; }
    }
}