using System;

namespace EazySDK.Exceptions
{
    public class UnsupportedHTTPMethodException : Exception
    {
        public UnsupportedHTTPMethodException() : base()
        {

        }

        public UnsupportedHTTPMethodException(string formattedString, params object[] args) : base(string.Format(formattedString, args))
        {
        }
    }
}
