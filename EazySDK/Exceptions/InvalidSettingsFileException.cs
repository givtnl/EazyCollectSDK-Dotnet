using System;

namespace EazySDK.Exceptions
{
    public class InvalidSettingsFileException : Exception
    {
        public InvalidSettingsFileException() : base()
        {

        }

        public InvalidSettingsFileException(string formattedString, params object[] args) : base(string.Format(formattedString, args))
        {

        }
    }
}
