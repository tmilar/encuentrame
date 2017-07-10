using System;
using NailsFramework.Logging;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NUnit.Framework;

namespace NailsFramework.Tests.Support
{
    public class BaseTest
    {
        public BaseTest()
        {
            WorkContext.Log = new NullLog();
            UnitOfWorkExecution.Log = new NullLog();
            NailsFramework.UnitOfWork.UnitOfWork.Log = new NullLog();
        }

        [SetUp]
        public void Reset()
        {
            Nails.Reset(false);
            if (UseTestContext)
                Nails.Configure().IoC.Lemming<TestExecutionContext>();
        }
        
        public virtual bool UseTestContext { get { return true; } }

        protected void RunInUnitOfWork(Action command, bool transactional = true)
        {
            var workContextProvider = Nails.ObjectFactory.GetObject<IWorkContextProvider>();
            workContextProvider.CurrentContext.RunUnitOfWork(command, new UnitOfWorkInfo(transactional));
        }

        protected TResult RunInUnitOfWork<TResult>(Func<TResult> command, bool transactional = true)
        {
            var workContextProvider = Nails.ObjectFactory.GetObject<IWorkContextProvider>();
            return workContextProvider.CurrentContext.RunUnitOfWork(command, new UnitOfWorkInfo(transactional));
        }

    }
}