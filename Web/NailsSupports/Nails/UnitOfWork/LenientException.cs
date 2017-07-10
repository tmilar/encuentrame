using System;
using System.Runtime.Serialization;
using NailsFramework.Persistence;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   This exception type prevents the current <see cref = "ITransactionalContext" /> from 
    ///   rolling back. If any other Exception type is thrown then the Transaction will be 
    ///   rolled back automatically.
    /// </summary>
    [Serializable]
    public class LenientException : Exception
    {
        public LenientException()
        {
        }

        public LenientException(string message) : base(message)
        {
        }

        public LenientException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LenientException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}