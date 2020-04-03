using System;
using System.Net;
using System.Runtime.Serialization;

namespace DistSysACW.Exceptions
{
    [Serializable]
    internal class UserRoleDoesNotExistException : StatusException
    {
        public UserRoleDoesNotExistException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        public UserRoleDoesNotExistException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}