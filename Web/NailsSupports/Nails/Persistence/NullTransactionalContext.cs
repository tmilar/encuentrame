namespace NailsFramework.Persistence
{
    /// <summary>
    ///   Null implementation of <see cref = "ITransactionalContext" />.
    /// </summary>
    public class NullTransactionalContext : ITransactionalContext
    {
        /// <summary>
        ///   Begin transaction.
        /// </summary>
        public void Begin()
        {
        }

        /// <summary>
        ///   Commit transaction.
        /// </summary>
        public void Commit()
        {
        }

        /// <summary>
        ///   Persists changes without committing the transaction.
        /// </summary>
        public void Checkpoint()
        {
        }

        /// <summary>
        ///   Rollback transaction.
        /// </summary>
        public void Rollback()
        {
        }
    }
}