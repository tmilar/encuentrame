using System;
using System.Reflection;
using MemoDb;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.UnitOfWork.Session;

namespace NailsFramework.Persistence
{
    public class MemoryContext : IPersistenceContext
    {
        private const string MemoSessionKey = "NailsFramework.Persistence.MemoDb.CurrentSession";

        [Inject]
        public ILog Log { private get; set; }

        [Inject]
        public IExecutionContext ExecutionContext { private get; set; }

        /// <summary>
        ///   Open a persistence session.
        /// </summary>
        public virtual void OpenSession()
        {
            if (IsSessionOpened)
                throw new InvalidOperationException("MemoDb session already open");

            var session = Memo.CreateSession();
            CurrentSession = session;
        }

        /// <summary>
        ///   Close the persistence session.
        /// </summary>
        public virtual void CloseSession()
        {
            if (!IsSessionOpened)
            {
                Log.Info("Closing MemoDb Session - Session is not open");
                return;
            }

            Log.Info("Closing MemoDb Session");
            CurrentSession.Dispose();
            CurrentSession = null;
        }

        public ITransactionalContext CreateTransactionalContext()
        {
            return new MemoryTransactionalContext(this);
        }

        public PropertyInfo GetIdPropertyOf<T>() where T : class
        {
            return Memo.Mappings[typeof(T)].Id;
        }

        public bool IsSessionOpened { get { return CurrentSession != null; } }

        public Memo Memo { get; set; }

        public MemoSession CurrentSession
        {
            get { return ExecutionContext.GetObject<MemoSession>(MemoSessionKey); }
            private set { ExecutionContext.SetObject(MemoSessionKey, value); }
        }
    }
}