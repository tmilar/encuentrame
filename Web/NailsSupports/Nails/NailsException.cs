using System;
using System.Runtime.Serialization;

namespace NailsFramework
{
    [Serializable]
    public class NailsException : Exception
    {
        public NailsException()
        {
        }

        public NailsException(string message) : base(message)
        {
        }

        public NailsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NailsException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}