using NHibernate;

namespace NailsFramework.Persistence
{
    /// <summary>
    ///   <see cref = "ITransactionalContext" /> implementation for NHibernate.
    /// </summary>
    public class NHibernateTransactionalContext : ITransactionalContext
    {
        private readonly INHibernateContext context;
        private ITransaction transaction;

        public NHibernateTransactionalContext(INHibernateContext context)
        {
            this.context = context;
        }

        #region ITransactionalContext Members

        /// <summary>
        ///   Begin transaction.
        /// </summary>
        public void Begin()
        {
            var session = context.CurrentSession;
            session.FlushMode = FlushMode.Auto;
            transaction = session.BeginTransaction();
        }

        /// <summary>
        ///   Commit transaction.
        /// </summary>
        public void Commit()
        {
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }

        /// <summary>
        ///   Persists changes without committing the transaction.
        /// </summary>
        public void Checkpoint()
        {
            context.CurrentSession.Flush();
        }

        /// <summary>
        ///   Rollback transaction.
        /// </summary>
        public void Rollback()
        {
            transaction.Rollback();
            transaction.Dispose();
            transaction = null;
        }

        #endregion
    }
}