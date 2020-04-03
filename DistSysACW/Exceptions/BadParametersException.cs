using System;
using System.Net;
using System.Runtime.Serialization;

namespace DistSysACW.Exceptions
{
    [Serializable]
    internal class BadParametersException : StatusException
    {
        public BadParametersException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        public BadParametersException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}