using System;

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
    }
}
