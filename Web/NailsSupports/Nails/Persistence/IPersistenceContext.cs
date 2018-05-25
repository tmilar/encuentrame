using System.Reflection;

namespace NailsFramework.Persistence
{
    /// <summary>
    ///   Persistence context interface.
    ///   Implementations will handle opening and closing connections, and creating 
    ///   a transactional context.
    /// </summary>
    public interface IPersistenceContext
    {
        /// <summary>
        ///   Opens a session.
        /// </summary>
        void OpenSession();

        /// <summary>
        ///   Closes a session.
        /// </summary>
        void CloseSession();

        /// <summary>
        ///   Creates and returns a transactional context.
        /// </summary>
        /// <returns></returns>
        ITransactionalContext CreateTransactionalContext();

        PropertyInfo GetIdPropertyOf<T>() where T : class;
        bool IsSessionOpened { get; }
    }
}