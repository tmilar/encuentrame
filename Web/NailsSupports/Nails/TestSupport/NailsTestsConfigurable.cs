using System;
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
    /// This base class is recommended for both NUnit and MSTest test lifecycle over the legacy <see cref="NailsTests" /> as it reduces the 
    /// test configuration overhead and also supports concurrent testing.
    /// Note: if perfoming changes in this class implementation please apply thsoe changes also to <see cref="NailsTests" />
    /// </remarks>
    /// <example>
    /// Inherit and implement the abstract property <see cref="ConfigurationName"/>. Also make sure that in some bootstrap of your testing suite
    /// you add that configuration to the <see cref="NailsTestsConfigurationRepository"/> so that's available when running the test.
    /// 
    /// <code>
    ///     public class MyTest : NailsTestsConfigurable
    ///     {
    ///         static MyTest()
    ///         {
    ///             NailsTestsConfigurationRepository.Instance.Get("MyTestConfiguration", (nails) => { /* the configuration delegate */ });
    ///         }
    ///     
    ///         public virtual string ConfigurationName { get { return "MyTestConfiguration"; } }
    ///
    ///         //Rest of the test class!
    ///     }
    /// </code>
    /// </example>
    public abstract class NailsTestsConfigurable : IInjector
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

        public abstract string ConfigurationName { get; }

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
            try
            {
                referenceResolver = NailsTestsConfigurator.Instance.AcquireConfiguration(this);
                InjectTestProperties();
                configured = true;

                PersistenceContext.OpenSession();
                if (TestsInUnitOfWork)
                    uow = WorkContextProvider.CurrentContext.BeginUnitOfWork(new UnitOfWorkInfo(true));
            }
            catch (Exception) 
            {
                //if configuration throws an exception, need to release the lock. This is needed because in MSTest lifecycle if Initialize method fails, the Cleanup is not called
                NailsTestsConfigurator.Instance.ReleaseConfiguration();
                throw;
            }
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
            NailsTestsConfigurator.Instance.ReleaseConfiguration();
        }

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
