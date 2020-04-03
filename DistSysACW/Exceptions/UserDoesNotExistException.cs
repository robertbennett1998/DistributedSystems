using System;
using System.Net;
using System.Runtime.Serialization;

namespace DistSysACW.Exceptions
{
    [Serializable]
    internal class UserDoesNotExistException : StatusException
    {
        public UserDoesNotExistException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        public UserDoesNotExistException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}