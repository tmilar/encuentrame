using System;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkFunctionCommand<T> : IUnitOfWorkCommand
    {
        private readonly Func<T> command;

        public UnitOfWorkFunctionCommand(Func<T> command)
        {
            this.command = command;
        }

        #region IUnitOfWorkCommand Members

        public object Execute()
        {
            return command();
        }

        #endregion
    }
}