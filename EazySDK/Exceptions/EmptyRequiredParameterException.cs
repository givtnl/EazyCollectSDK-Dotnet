using System;

namespace EazySDK.Exceptions
{
    public class EmptyRequiredParameterException : Exception
    {
        public EmptyRequiredParameterException() : base()
        {

        }

        public EmptyRequiredParameterException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
