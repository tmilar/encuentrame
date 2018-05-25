using System;
using NailsFramework.Aspects;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkInterceptorCommand : IUnitOfWorkCommand
    {
        private readonly MethodInvocationInfo invocationInfo;

        public UnitOfWorkInterceptorCommand(MethodInvocationInfo invocationInfo)
        {
            this.invocationInfo = invocationInfo;
        }

        #region IUnitOfWorkCommand Members

        /// <summary>
        ///   Execute the Command.
        /// </summary>
        /// <returns></returns>
        public Object Execute()
        {
            return invocationInfo.Proceed();
        }

        #endregion
    }
}