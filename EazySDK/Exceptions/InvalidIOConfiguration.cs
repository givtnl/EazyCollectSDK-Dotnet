using System;

namespace EazySDK.Exceptions
{
    public class InvalidIOConfiguration : Exception
    {
        public InvalidIOConfiguration() : base()
        {

        }

        public InvalidIOConfiguration(string formattedString, params object[] args) : base(string.Format(formattedString, args))
        {

        }
    }
}
