using System;

namespace EazySDK.Exceptions
{
    public class InvalidPaymentDateException : Exception
    {
        public InvalidPaymentDateException() : base()
        {

        }

        public InvalidPaymentDateException(string formattedString, params object[] args) : base (string.Format(formattedString, args))
        {

        }
    }
}
