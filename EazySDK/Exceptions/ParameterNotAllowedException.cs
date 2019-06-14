using System;

namespace EazySDK.Exceptions
{
    public class ParameterNotAllowedException : Exception
    {
        public ParameterNotAllowedException() : base()
        {

        }

        public ParameterNotAllowedException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
