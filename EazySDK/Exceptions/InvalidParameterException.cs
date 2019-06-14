using System;

namespace EazySDK.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException() : base()
        {

        }

        public InvalidParameterException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
