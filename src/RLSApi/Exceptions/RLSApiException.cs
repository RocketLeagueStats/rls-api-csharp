using System;
using RLSApi.Net.Models;

namespace RLSApi.Exceptions
{
    public class RLSApiException : Exception
    {
        public RLSApiException()
        {
        }

        public RLSApiException(string message) : base(message)
        {
        }

        public RLSApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public int HttpStatusCode { get; set; }

        public Error RlsError { get; set; }
    }
}
