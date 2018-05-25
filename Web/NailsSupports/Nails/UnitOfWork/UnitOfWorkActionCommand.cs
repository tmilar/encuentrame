using System;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkActionCommand : IUnitOfWorkCommand
    {
        private readonly Action command;

        public UnitOfWorkActionCommand(Action command)
        {
            this.command = command;
        }

        #region IUnitOfWorkCommand Members

        public object Execute()
        {
            command();
            return null;
        }

        #endregion
    }
}