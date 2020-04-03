using System;
using System.Net;
using System.Runtime.Serialization;

namespace DistSysACW.Exceptions
{
    [Serializable]
    internal class UserRoleException : StatusException
    {
        public UserRoleException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        public UserRoleException(string message) : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}