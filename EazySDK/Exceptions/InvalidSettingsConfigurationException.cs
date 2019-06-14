using System;

namespace EazySDK.Exceptions
{
    public class InvalidSettingsConfigurationException : Exception
    {
        public InvalidSettingsConfigurationException() : base()
        {

        }

        public InvalidSettingsConfigurationException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
