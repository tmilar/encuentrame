using System;
using NailsFramework.Config;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.TestSupport
{
    /// <summary>
    /// Base class for unit testing that will check the behavior of an application relying on Nails. Provides support for injecting properties 
    /// in the test as well as accessing the unit of work or the persistence context.
    /// </summary>
    /// <remarks>
    /// This base class is suitable for NUnit test lifecycle but not for MSTest. In any case is reccomended to use the <see cref="NailsTestsConfigurable" />
    /// which reduces the test configuration overhead and also supports concurrent testing.
    /// Note: if perfoming changes in this class implementation please apply thsoe changes also to <see cref="NailsTestsConfigurable" />
    /// </remarks>
    public abstract class NailsTests : IInjector
    {
        private UnitOfWorkExecution uow;
        protected bool TestsInUnitOfWork { get; set; }

        private bool configured;
        private ReferenceResolver referenceResolver;

        [Inject]
        public IWorkContextProvider WorkContextProvider { protected get; set; }

        [Inject]
        public ICurrentUnitOfWork CurrentUnitOfWork { protected get; set; }

        [Inject]
        public IPersistenceContext PersistenceContext { private get; set; }

        #region IInjector Members

        void IInjector.Inject(ValueInjection injection)
        {
            injection.Property.SetValue(this, injection.Value, null);
        }

        void IInjector.Inject(ReferenceInjection injection)
        {
            var value = referenceResolver.GetReferenceFor(injection);
            injection.Property.SetValue(this, value, null);
        }

        #endregion
        
        public virtual void SetUp()
        {
            if (!configured)
            {
                Nails.Reset();
                ConfigureNails(Nails.Configure().UnitOfWork.ConnectionBoundUnitOfWork(false));

                referenceResolver = new ReferenceResolver(Nails.ObjectFactory);
            }
            
            InjectTestProperties();
            configured = true;

            PersistenceContext.OpenSession();
            if (TestsInUnitOfWork)
                uow = WorkContextProvider.CurrentContext.BeginUnitOfWork(new UnitOfWorkInfo(true));
        }


        private void InjectTestProperties()
        {
            var lemming = Lemming.From(GetType());
            foreach (var injection in lemming.Injections)
            {
                if (!configured || !lemming.Singleton)
                    injection.Accept(this);
            }
        }

        public virtual void TearDown()
        {
            if (uow != null)
            {
                uow.End();
                uow = null;
            }
            PersistenceContext.CloseSession();
        }

        protected abstract void ConfigureNails(INailsConfigurator nails);

        protected void RunInUnitOfWork(Action command, bool transactional = true)
        {
            WorkContextProvider.CurrentContext.RunUnitOfWork(command, new UnitOfWorkInfo(transactional));
        }

        protected TResult RunInUnitOfWork<TResult>(Func<TResult> command, bool transactional = true)
        {
            return WorkContextProvider.CurrentContext.RunUnitOfWork(command, new UnitOfWorkInfo(transactional));
        }
    }
}