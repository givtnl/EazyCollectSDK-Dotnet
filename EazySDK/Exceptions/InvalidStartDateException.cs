using System;

namespace EazySDK.Exceptions
{
    public class InvalidStartDateException : Exception
    {
        public InvalidStartDateException() : base()
        {

        }

        public InvalidStartDateException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
