using System;
using System.Runtime.Serialization;

namespace DistSysACWClient.Exceptions
{
    [Serializable]
    internal class AlreadyRegisteredException : Exception
    {
        public AlreadyRegisteredException()
        {
        }

        public AlreadyRegisteredException(string message) : base(message)
        {
        }

        public AlreadyRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}