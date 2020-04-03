using System;
using System.Net;
using System.Runtime.Serialization;

namespace DistSysACW.Exceptions
{
    [Serializable]
    internal class StatusException : Exception
    {
        public StatusException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; internal set; }
    }
}