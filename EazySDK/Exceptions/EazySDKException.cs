using System;

namespace EazySDK.Exceptions
{
    public class EazySDKException : Exception
    {
        public EazySDKException() : base()
        {

        }

        public EazySDKException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
