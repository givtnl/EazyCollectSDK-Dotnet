using System;

namespace EazySDK.Exceptions
{
    public class InvalidEnvironmentException : Exception
    {
        public InvalidEnvironmentException() : base()
        {

        }

        public InvalidEnvironmentException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
