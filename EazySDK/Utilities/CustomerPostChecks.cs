using System.Text.RegularExpressions;

namespace EazySDK.Utilities
{
    public class CustomerPostChecks
    {
        /// <summary>
        /// Check that a post code is formatted correctly. This does not verify the integrity of a post code, just that it could possibly exist. The responsibility of verifying a post code lies with the client.
        /// </summary>
        /// 
        /// 
        /// <param name="PostCode">A post code provided by an external function</param>
        /// 
        /// <example>
        /// CheckPostCodeIsCorectlyFormatted("GL52 2NF")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckPostCodeIsCorectlyFormatted(string PostCode)
        {
            Regex search = new Regex("^(([A-Z][0-9]{1,2})|(([A-Z][A-HJ-Y][0-9]{1,2})|(([A-Z][0-9][A-Z])|([A-Z][A-HJ-Y][0-9]?[A-Z])))) [0-9][A-Z]{2}$");
            var result = search.IsMatch(PostCode);

            if (!result)
            {
                throw new Exceptions.InvalidParameterException(
                    string.Format("{0} is not a valid UK post code. Please double check the post code and re-submit.", PostCode)
                    );
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Check an email address is formatted correctly. This will not verify the integrity of an email address, instead it verifies that an email address could exist. 
        /// The responsibility of verifying an email address lies with the client.
        /// </summary>
        /// 
        /// <param name="EmailAddress">An email address provided by an external function</param>
        ///
        /// <example>
        /// CheckEmailAddressIsCorrectlyFormatted("test@email.com")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckEmailAddressIsCorrectlyFormatted(string EmailAddress)
        {
            Regex search = new Regex("(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$)");
            var result = search.IsMatch(EmailAddress);

            if (!result)
            {
                throw new Exceptions.InvalidParameterException(
                    string.Format("{0} is not a valid email address. Please double check the email address and re-submit.", EmailAddress)
                    );
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Check a bank account number is formatted correctly. This will not verify the integrity of a bank account number, and does not perform any mathematical checks on the account number. 
        /// The responsibility of verifying a bank account number lies with the client. We can offer a bank checking API to check the validity of bank details. For more information, contact our sales team on 01242 650052.
        /// </summary>
        /// 
        /// <param name="AccountNumber">A bank account number provided by an external function</param>
        ///
        /// <example>
        /// CheckAccountNumberIsFormattedCorrectly("12345678")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckAccountNumberIsFormattedCorrectly(string AccountNumber)
        {
            Regex search = new Regex("^[0-9]{8}$");
            var result = search.IsMatch(AccountNumber);

            if (!result)
            {
                throw new Exceptions.InvalidParameterException(
                    string.Format("{0} is not a valid UK bank account number. Please double check that the account number is 8 characters long and only contains digits.", AccountNumber)
                    );
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Check a bank sort code is formatted correctly. This will not verify the integrity of a bank sort code, and does not perform any mathematical checks on the sort code. 
        /// The responsibility of verifying a bank sort code lies with the client. We can offer a bank checking API to check the validity of bank details. For more information, contact our sales team on 01242 650052.
        /// </summary>
        /// 
        /// <param name="SortCode">A bank account number provided by an external function</param>
        ///
        /// <example>
        /// CheckAccountNumberIsFormattedCorrectly("123456")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckSortCodeIsFormattedCorrectly(string SortCode)
        {
            Regex search = new Regex("^[0-9]{6}$");
            var result = search.IsMatch(SortCode);

            if (!result)
            {
                throw new Exceptions.InvalidParameterException(
                    string.Format("{0} is not a valid UK bank sort code. Please double check that the sort code is 6 characters long and only contains digits.", SortCode)
                    );
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Check an account holder name is formatted correctly. This will not verify the account holder name of a UK bank account, instead it performs several checks to ensure the account holder name could be correct. 
        /// The responsibility of verifying a bank account holder name lies with the client. 
        /// </summary>
        /// 
        /// <param name="AccountHolderName">A bank account number provided by an external function</param>
        ///
        /// <example>
        /// AccountHolderName("Mr John Doe")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckAccountHolderNameIsFormattedCorrectly(string AccountHolderName)
        {
            Regex search = new Regex("^[A-Z0-9\\-\\/& ]{3,18}$");
            var result = search.IsMatch(AccountHolderName.ToUpper());

            if (!result)
            {
                throw new Exceptions.InvalidParameterException(
                    string.Format("{0} is not formatted as a UK bank account holder name. A UK bank account holder name must be between 3 and 18 characters, contain only alphabetic characters(a-z), ampersands (&)," +
                    "hyphens (-), forward slashes (/) and spaces ( ). Please double check that the bank account holder name meets these criteria and re-submit.", AccountHolderName)
                    );
            }
            else
            {
                return result;
            }
        }
    } 
}
