using System;
using System.Reflection;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Persistence;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkExecution
    {
        private bool success = true;

        [Inject]
        public static ILog Log { protected get; set; }

        public Exception Exception { get; private set; }

        private UnitOfWork unitOfWork;
        private readonly IPersistenceContext persistenceContext;

        private readonly UnitOfWorkInfo info;

        private readonly UnitOfWorkEventSubscriptions subscriptions = new UnitOfWorkEventSubscriptions();

        public UnitOfWorkExecution(IPersistenceContext persistenceContext, UnitOfWorkInfo info)
        {
            this.persistenceContext = persistenceContext;
            this.info = info;
        }

        public UnitOfWork Begin()
        {
            unitOfWork = new UnitOfWork(info, subscriptions, persistenceContext, Nails.Configuration.ConnectionBoundUnitOfWork);
            unitOfWork.Begin();
            return unitOfWork;
        }

        public void End()
        {
            if (success)
                unitOfWork.Success();

            subscriptions.ExecuteCalls(Exception);
            
            unitOfWork.Dispose();
            onEnd();
        }

        public void HandleException(Exception exception)
        {
            success = false;

            Exception = exception is TargetInvocationException
                            ? exception.GetBaseException()
                            : exception;

            if (Exception is LenientException)
            {
                Log.Info("Lenient Exception running UOW", Exception);
                unitOfWork.Success();
            }
            else
            {
                Log.Warn("Exception running UOW", Exception);
                unitOfWork.Fail(Exception);
            }
        }

        private Action onEnd = () => { };

        public void OnEnd(Action onEnd)
        {
            this.onEnd += onEnd;
        }
    }
}