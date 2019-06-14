using System;

namespace EazySDK.Exceptions
{
    public class RecordAlreadyExistsException : Exception
    {
        public RecordAlreadyExistsException() : base()
        {

        }

        public RecordAlreadyExistsException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
