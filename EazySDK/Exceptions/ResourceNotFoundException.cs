using System;

namespace EazySDK.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base()
        {

        }

        public ResourceNotFoundException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
