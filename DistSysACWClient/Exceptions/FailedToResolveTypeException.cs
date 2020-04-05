using System;
using System.Runtime.Serialization;

namespace DistSysACWClient.Exceptions
{
    [Serializable]
    internal class FailedToResolveTypeException : Exception
    {
        public FailedToResolveTypeException()
        {
        }

        public FailedToResolveTypeException(string message) : base(message)
        {
        }

        public FailedToResolveTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToResolveTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}