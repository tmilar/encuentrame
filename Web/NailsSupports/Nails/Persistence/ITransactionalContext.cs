namespace NailsFramework.Persistence
{
    /// <summary>
    ///   Transactional content interface.
    /// </summary>
    public interface ITransactionalContext
    {
        /// <summary>
        ///   Begin transaction.
        /// </summary>
        void Begin();

        /// <summary>
        ///   Commit transaction.
        /// </summary>
        void Commit();

        /// <summary>
        ///   Persists changes without committing the transaction.
        /// </summary>
        void Checkpoint();

        /// <summary>
        ///   Rollback transaction.
        /// </summary>
        void Rollback();
    }
}