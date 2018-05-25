using System.Reflection;

namespace NailsFramework.IoC
{
    /// <summary>
    ///   Handles the dependency of properties
    /// </summary>
    public interface IDependencyHandler
    {
        /// <summary>
        ///   Returns if this instances handles the specified property.
        /// </summary>
        /// <param name = "property">The property.</param>
        /// <returns></returns>
        bool Handles(PropertyInfo property);

        /// <summary>
        ///   Returns the Lemming the name for the specified property
        /// </summary>
        /// <param name = "property">The property.</param>
        /// <returns></returns>
        string LemmingNameFor(PropertyInfo property);
    }
}