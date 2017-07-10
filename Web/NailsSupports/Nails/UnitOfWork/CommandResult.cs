using System;

namespace NailsFramework.UnitOfWork
{
    public class CommandResult
    {
        public CommandResult(object result, Exception error)
        {
            ReturnValue = result;
            Error = error;
        }

        public object ReturnValue { get; private set; }

        public Exception Error { get; private set; }

        public bool HasFailed
        {
            get { return Error != null; }
        }
    }
}