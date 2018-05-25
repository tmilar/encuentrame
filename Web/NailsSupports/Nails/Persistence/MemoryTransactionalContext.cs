using MemoDb;

namespace NailsFramework.Persistence
{
    public class MemoryTransactionalContext : ITransactionalContext
    {
        private readonly MemoryContext context;
        private MemoSession session;
        public MemoryTransactionalContext(MemoryContext context)
        {
            this.context = context;
        }

        public void Begin()
        {
            session = context.CurrentSession;            
        }

        public void Commit()
        {
            session.Flush();
        }

        public void Checkpoint()
        {
            this.Commit();
        }

        public void Rollback()
        {
            session.Clear();
        }
    }
}