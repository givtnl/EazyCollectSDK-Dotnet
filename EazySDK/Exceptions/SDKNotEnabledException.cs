using System;

namespace EazySDK.Exceptions
{
    public class SDKNotEnabledException : Exception
    {
        public SDKNotEnabledException() : base()
        {

        }

        public SDKNotEnabledException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
