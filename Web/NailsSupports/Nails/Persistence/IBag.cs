using System.Linq;

namespace NailsFramework.Persistence
{
    /// <summary>
    ///   Generic interface for Domain Bags.
    /// </summary>
    /// <typeparam name = "T">Object type contained by the bag</typeparam>
    public interface IBag<T> : IQueryable<T> where T : class
    {
        /// <summary>
        ///   Gets the <see cref = "T" /> with the specified id.
        /// </summary>
        /// <value></value>
        T this[object id] { get; }

        void Put(T o);
        void Remove(T o);
    }
}