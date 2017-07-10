using System;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.Support;

namespace NailsFramework.UnitOfWork
{
    public class RunningPersistenceContextValidationBehavior : ILemmingBehavior
    {
        [Inject]
        public static IPersistenceContext PersistenceContext { private get; set; }

        public object ApplyTo(MethodInvocationInfo invocation)
        {
            if (!PersistenceContext.IsSessionOpened)
                throw new InvalidOperationException(
                    string.Format("{0} should be called within an open persistence session.",
                                  invocation.Method.FullFriendlyName()));
            return invocation.Proceed();
        }
    }
}