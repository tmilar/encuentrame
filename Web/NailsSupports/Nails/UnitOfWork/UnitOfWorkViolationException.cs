namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkViolationException : NailsException
    {
        public UnitOfWorkViolationException(string message) : base(message)
        {
        }
    }
}