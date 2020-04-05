using System;
using System.Runtime.Serialization;

namespace DistSysACWClient
{
    [Serializable]
    internal class UnsuportedOperationException : Exception
    {
        public UnsuportedOperationException()
        {
        }

        public UnsuportedOperationException(string message) : base(message)
        {
        }

        public UnsuportedOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsuportedOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}