using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpApp.Core.Exceptions
{
    public class ExternalApiException : Exception
    {
        public int StatusCode { get; }

        public ExternalApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
