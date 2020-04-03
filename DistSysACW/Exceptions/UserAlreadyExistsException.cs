using System;
using System.Net;
using System.Runtime.Serialization;

namespace DistSysACW.Exceptions
{
    [Serializable]
    internal class UserAlreadyExistsException : StatusException
    {
        public UserAlreadyExistsException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        public UserAlreadyExistsException(string message) : base(HttpStatusCode.Forbidden, message)
        {
        }
    }
}