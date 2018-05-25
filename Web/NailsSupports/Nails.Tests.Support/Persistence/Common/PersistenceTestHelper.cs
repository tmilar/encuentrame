using System;
using System.Reflection;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.Tests.Persistence.Common
{
    public abstract class PersistenceTestHelper
    {
        [Inject]
        public IWorkContextProvider WorkContextProvider { private get; set; }
        [Inject]
        public IPersistenceContext PersistenceContext  { private get; set; }

        public void CreateDatabase()
        {
            if (CreateDatabaseInUnitOfWork)
            {
                if(!Nails.Configuration.ConnectionBoundUnitOfWork)
                    PersistenceContext.OpenSession();
                try
                {
                    WorkContextProvider.CurrentContext.RunUnitOfWork(DoCreateDatabase, new UnitOfWorkInfo(false));
                }
                finally
                {
                    if (!Nails.Configuration.ConnectionBoundUnitOfWork)
                        PersistenceContext.CloseSession();
                }
            }
            else
            {
                DoCreateDatabase();
            }
        }

        protected abstract void DoCreateDatabase();
        public abstract DataMapper CreateDataMapper();
        protected virtual bool CreateDatabaseInUnitOfWork { get { return true; } }

        public virtual bool AddsIdsToCollectionItems { get { return true; } }
    }
}