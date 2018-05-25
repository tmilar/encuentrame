using System;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.Support;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.UnitOfWork
{
    public class RunningUnitOfWorkValidationBehavior : ILemmingBehavior
    {
        private readonly IDictionary<Type, List<string>> transactionalMethodsByType;

        public RunningUnitOfWorkValidationBehavior()
        {
            transactionalMethodsByType = new Dictionary<Type, List<string>>();
        }

        [Inject]
        public static IWorkContextProvider WorkContextProvider { private get; set; }

        #region ILemmingBehavior Members

        public object ApplyTo(MethodInvocationInfo invocation)
        {
            var workContext = WorkContextProvider.CurrentContext;

            if (!workContext.IsUnitOfWorkRunning)
                throw new UnitOfWorkViolationException(
                    string.Format("{0} should be called within an active Unit Of Work.",
                                  invocation.Method.FullFriendlyName()));

            var unitOfWorkInfo = workContext.CurrentUnitOfWork.Info;

            var type = invocation.Method.ReflectedType;

            if (!unitOfWorkInfo.IsTransactional)
            {
                var baseType = transactionalMethodsByType.Keys
                    .SingleOrDefault(x => x.IsGenericTypeDefinition
                                              ? x.GenericDefinitionIsAssignableFrom(type)
                                              : x.IsAssignableFrom(type));

                if (baseType != null && transactionalMethodsByType[baseType].Contains(invocation.Method.Name))
                {
                    throw new UnitOfWorkViolationException(
                        string.Format("{0} should be called within an active transactional Unit Of Work.",
                                      invocation.Method.FullFriendlyName()));
                }
            }

            return invocation.Proceed();
        }

        #endregion

        public void SetTransactionalMethods<T>(params string[] methods)
        {
            SetTransactionalMethods(typeof (T), methods);
        }

        public void SetTransactionalMethods(Type type, params string[] methods)
        {
            List<string> transactionalMethods;

            if (!transactionalMethodsByType.TryGetValue(type, out transactionalMethods))
            {
                transactionalMethods = new List<string>();
                transactionalMethodsByType.Add(type, transactionalMethods);
            }

            transactionalMethods.AddRange(methods);
        }
    }
}