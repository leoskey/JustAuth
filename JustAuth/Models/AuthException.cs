using System;
using System.Collections.Generic;
using System.Text;

namespace JustAuth.Models
{
    public class AuthException : Exception
    {
        private AuthException()
        {
        }

        public AuthException(string message) : base(message)
        {
        }

        public AuthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthException(string error, string errorDescription) : base($"{error}:{errorDescription}")
        {
        }
    }
}
