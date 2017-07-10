using System;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Persistence;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   The unit in which transactional and non transactional application operations are
    ///   contained.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private readonly UnitOfWorkCache cache = new UnitOfWorkCache();
        private readonly bool connectionBoundUnitOfWork;
        private readonly UnitOfWorkInfo info;
        private readonly IPersistenceContext persistenceContext;
        private UnitOfWorkStatus status = UnitOfWorkStatus.NotStarted;
        private ITransactionalContext transactionContext;

        /// <summary>
        ///   Constructor.
        /// </summary>
        public UnitOfWork(UnitOfWorkInfo info, UnitOfWorkEventSubscriptions subscriptions,
                          IPersistenceContext persistenceContext, bool connectionBoundUnitOfWork)
        {
            this.connectionBoundUnitOfWork = connectionBoundUnitOfWork;

            Subscriptions = subscriptions;
            this.persistenceContext = persistenceContext;
            transactionContext = new NullTransactionalContext();
            this.info = info;
            try
            {
                LoadTransactionContext();

                if (connectionBoundUnitOfWork)
                    persistenceContext.OpenSession();
            }
            catch (Exception ex)
            {
                Log.Error("Error creating a new Unit of Work", ex);
                throw;
            }
        }

        [Inject]
        public static ILog Log { private get; set; }

        /// <summary>
        ///   Gets or sets the subscriptions.
        /// </summary>
        /// <value>The subscriptions.</value>
        public virtual UnitOfWorkEventSubscriptions Subscriptions { get; private set; }

        /// <summary>
        ///   Current status.
        /// </summary>
        public UnitOfWorkStatus Status
        {
            get { return status; }
        }

        /// <summary>
        ///   The related information object.
        /// </summary>
        public UnitOfWorkInfo Info
        {
            get { return info; }
        }

        /// <summary>
        ///   Returns a UnitOfWork lifecycle cache
        /// </summary>
        public virtual UnitOfWorkCache Cache
        {
            get { return cache; }
        }

        #region IDisposable Members

        /// <summary>
        ///   Disposes this Unit of work, and frees the Transactional Context by closing the connection.
        /// </summary>
        public void Dispose()
        {
            if (connectionBoundUnitOfWork)
                persistenceContext.CloseSession();
        }

        #endregion

        /// <summary>
        ///   Configures this Unit of Work as a transactional unit.
        /// </summary>
        protected virtual void LoadTransactionContext()
        {
            if (info.IsTransactional)
            {
                transactionContext = persistenceContext.CreateTransactionalContext();
            }
        }

        /// <summary>
        ///   Begin the Unit of work.
        /// </summary>
        public virtual void Begin()
        {
            transactionContext.Begin();
            status = UnitOfWorkStatus.Running;
        }

        /// <summary>
        ///   Complete the Unit of Work successfully.
        /// </summary>
        public virtual void Success()
        {
            if (canceled)
            {
                transactionContext.Rollback();
                status = UnitOfWorkStatus.Canceled;
            }
            else
            {
                transactionContext.Commit();
                status = UnitOfWorkStatus.Successfull;
            }
        }

        /// <summary>
        ///   Commits the current transactional context and begins a new one.
        /// </summary>
        public void Checkpoint()
        {
            transactionContext.Checkpoint();
        }
        /// <summary>
        ///   Make the Unit of Work fail.
        /// </summary>
        /// <param name = "exception">The Exception that was thrown.</param>
        public virtual void Fail(Exception exception)
        {
            transactionContext.Rollback();
            status = UnitOfWorkStatus.Failed;
        }

        private bool canceled;

        public void Cancel()
        {
            canceled = true;
        }
    }
}