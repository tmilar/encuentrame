using System;
using MemoDb;

namespace NailsFramework.Persistence
{
    public class Memory : DataMapper
    {
        private readonly Memo memo;

        public Memory Configure( Action<Memo> configure )
        {
            configure(memo);
            return this;
        }

        public Memory( Memo memo)
        {
            this.memo = memo;
        }

        public Memory() : this(new Memo())
        {
        }
        public override Type BagType
        {
            get { return typeof (MemoryBag<>); }
        }

        public override Type PersistenceContextType
        {
            get { return typeof (MemoryContext); }
        }
        
        public override void Initialize()
        {
            base.Initialize();
            var context = Nails.ObjectFactory.GetObject<MemoryContext>();
            context.Memo = memo;
        }
    }
}