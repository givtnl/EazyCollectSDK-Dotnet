using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK
{
    /// <summary>
    /// A collection of POST requests made to the EazyCustomerManager API
    /// </summary>
    public class Post
    {
        // The client object which manages the Session and Settings
        private ClientHandler Handler { get; set; }
        // The settings object, derives from ClientHandler
        private IConfiguration Settings { get; set; }
        private Utilities.CustomerPostChecks CustomerChecks { get; set; }
        private Utilities.ContractPostChecks ContractChecks { get; set; }
        private Utilities.PaymentPostChecks PaymentChecks { get; set; }
        // The parameters passed from a function, if there are any
        private Dictionary<string, string> Parameters { get; set; }


        public Post(IConfiguration _Settings)
        {
            // Get the Settings passed from the Client Handler
            Settings = _Settings;
            // Reference the Client Handler for sending a request
            Handler = new ClientHandler();
        }

        /// <summary>
        /// Create a new callback URL for EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: We strongly recommend using a HTTPS secured URL as the return endpoint.
        /// </remarks>
        /// 
        /// <param name="entity">The entity for which to receive BACS messages. Valid choices: "contract", "customer", "payment"</param>
        /// <param name="_CallbackUrl">The new URL to set</param>
        /// 
        /// <example>
        /// CallbackUrl("contract", "https://test.com");
        /// </example>
        /// 
        /// <returns>
        /// "The new callback URL is https://test.com" 
        /// </returns>
        public string CallbackUrl(string entity, string _CallbackUrl)
        {
            string[] validEntities = { "contract", "customer", "payment" };

            if (!validEntities.Contains(entity.ToLower()))
            {
                throw new Exceptions.InvalidParameterException($"{entity} is not a valid entity; must be one of either 'contract', 'customer' or 'payment'.");
            }

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>
            {
                { "url", _CallbackUrl }
            };

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Post($"BACS/{entity}/callback", _Parameters: Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            return string.Format("The new callback URL is {0}", _CallbackUrl);
        }

        /// <summary>
        /// Create a new customer in EazyCustomerManager
        /// </summary>
        /// 
        /// <param name="Email">The email address of the new customer. This must be unique</param>
        /// <param name="Title">The title of the new customer</param>
        /// <param name="CustomerReference">The customer reference of the new customer. This must be unique.</param>
        /// <param name="FirstName">The first name of the new customer</param>
        /// <param name="Surname">The surname of the new customer</param>
        /// <param name="Line1">The first line of the new customers address</param>
        /// <param name="PostCode">The post code of the new customer</param>
        /// <param name="AccountNumber">The bank account number of the new customer</param>
        /// <param name="SortCode">The sort code of the new customer</param>
        /// <param name="AccountHolderName">The new customers full name as it appears on their bank account</param>
        /// 
        /// <optionalParams>
        /// <param name="Line2">The second line of the new customers address</param>
        /// <param name="Line3">The third line of the new customers address</param>
        /// <param name="Line4">The fourth line of the new customers address</param>
        /// <param name="CompanyName">The name of the company the new customer represents</param>
        /// <param name="DateOfBirth">The date of birth of the new customer, formatted to ISO standards(YYYY-MM-DD)</param>
        /// <param name="Initials">The initials of the new customer</param>
        /// <param name="HomePhone">The home phone number of the new customer</param>
        /// <param name="MobilePhone">The mobile phone number of the new customer</param>
        /// <param name="WorkPhone">The work phone number of the new customer</param>
        /// </optionalParams>
        /// 
        /// <example>
        /// Customer("test@email.com", "Mr", "John", "Doe", "1 Tebbit Mews", "GL52 2NF", "12345678", "123456", "Mr John Doe", WorkPhone: "12345678910")
        /// </example>
        /// 
        /// <returns>
        /// Customer JSON formatted as string
        /// </returns>
        public string Customer(string Email, string Title, string CustomerReference, string FirstName, string Surname, string Line1, string PostCode, string AccountNumber, string SortCode,
            string AccountHolderName, string Line2 = "", string Line3 = "", string Line4 = "", string CompanyName = "", string DateOfBirth = "", string Initials = "", string HomePhone = "",
            string MobilePhone = "", string WorkPhone = "")
        {
            if (Email == "" || Title == "" || CustomerReference == "" || FirstName == "" || Surname == "" || Line1 == "" || PostCode == "" || AccountNumber == "" || SortCode == "" || AccountHolderName == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                    );
            }

            CustomerChecks = new Utilities.CustomerPostChecks();
            // Perform several basic checks to ensure the information provided for the customer is fit for use
            CustomerChecks.CheckEmailAddressIsCorrectlyFormatted(Email);
            CustomerChecks.CheckPostCodeIsCorectlyFormatted(PostCode);
            CustomerChecks.CheckAccountNumberIsFormattedCorrectly(AccountNumber);
            CustomerChecks.CheckSortCodeIsFormattedCorrectly(SortCode);
            CustomerChecks.CheckAccountHolderNameIsFormattedCorrectly(AccountHolderName);

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();

            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("email", Email);
                Parameters.Add("title", Title);
                Parameters.Add("customerRef", CustomerReference);
                Parameters.Add("firstName", FirstName);
                Parameters.Add("surname", Surname);
                Parameters.Add("line1", Line1);
                Parameters.Add("postCode", PostCode);
                Parameters.Add("accountNumber", AccountNumber);
                Parameters.Add("bankSortCode", SortCode);
                Parameters.Add("accountHolderName", AccountHolderName);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@accesspaysuite.com");
            }

            if (Line2 != "") { Parameters.Add("line2", Line2); }
            if (Line3 != "") { Parameters.Add("line3", Line3); }
            if (Line4 != "") { Parameters.Add("line4", Line4); }
            if (CompanyName != "") { Parameters.Add("companyName", CompanyName); }
            if (DateOfBirth != "") { Parameters.Add("dateOfBirth", DateOfBirth); }
            if (Initials != "") { Parameters.Add("initials", Initials); }
            if (HomePhone != "") { Parameters.Add("homePhone", HomePhone); }
            if (WorkPhone != "") { Parameters.Add("workPhone", WorkPhone); }
            if (MobilePhone != "") { Parameters.Add("mobilePhone", MobilePhone); }


            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Post("customer", _Parameters: Parameters);


            // If no customers were returned
            if (SendRequest.Contains("There is an existing Customer with the same Client"))
            {
                throw new Exceptions.RecordAlreadyExistsException(
                   string.Format("A customer with the customer reference of {0} already exists within the client. Please change the customer reference and re-submit", CustomerReference)
                   );
            }
            else
            {
                // Pass the return string to the handler. This will throw an exception if it is not what we expect
                Handler.GenericExceptionCheck(SendRequest);

                // Get the JSON returned from EazyCustomerManager
                JObject SendRequestAsJson = JObject.Parse(SendRequest);
                // Get the list of Customers JSON objects
                var Customer = SendRequestAsJson;
                return string.Format("{0}", Customer);
            }
        }

        /// <summary>
        /// Create a new contract in EazyCustomerManager
        /// </summary>
        /// 
        /// <param name="Customer">The GUID of the customer the new contract will belong to</param>
        /// <param name="ScheduleName">The schedule name the new contract will belong to</param>
        /// <param name="StartDate">The desired start date of the new contract. This must be X working days in the future, where X is the number of working days agreed with EazyCollect.</param>
        /// <param name="GiftAid">Whether the new contract will be eligible for GiftAid or not</param>
        /// <param name="TerminationType">The method of termination for the new contract</param>
        /// <param name="AtTheEnd">What happens to the new contract after the termination event have been triggered</param>
        /// 
        /// <OptionalParams>
        /// <param name="NumberofDebits">The number of debits to be taken, this is mandatory if TerminationType has been set to "Collect certain number of debits"</param>
        /// <param name="Frequency">Mandatory if the new contract is not ad-hoc. This parameter allows you to skip periods (Ex. a value of 2 would collect every second month)</param>
        /// <param name="InitialAmount">Used if the first collection differs from the regular collection amount. Not to be used with ad-hoc contracts</param>
        /// <param name="ExtraInitialAmount">Used if there are any additional charges (Ex. Gym joining fee)</param>
        /// <param name="PaymentAmount">Mandatory if the contract is not ad-hoc. The regular collection amount of the new contract</param>
        /// <param name="FinalAmount">Used if the final collection amount differs from the regular collection amount. Not to be used with ad-hoc contracts</param>
        /// <param name="PaymentMonthInYear">Mandatory for annual contracts. The collection month for annual payments (1-12)</param>
        /// <param name="PaymentDayInMonth">Mandatory for annual and monthly contracts. The collection day of the month (1-28 || Last day of month)</param>
        /// <param name="PaymentDayInWeek">Mandatory for weekly contracts. The collection day of the week for weekly schedules (Monday .. Friday)</param>
        /// <param name="TerminationDate">Mandatory if termination type is set to "End on exact date". The termination date of a contract</param>
        /// <param name="AdditionalReference">An additional, search-able reference for the newly created contract</param>
        /// <param name="CustomDDReference">A custom DD reference for the new contract. This option is only available upon request to Eazy Collect </param>
        /// </OptionalParams>
        /// 
        /// <example>
        /// Contract("ab09362d-f88e-4ee8-be85-e27e1a6ce06a", "test_schedule", "2019-06-20", "False", "Until further notice", "Switch to further notice")
        /// </example>
        /// 
        /// <returns>
        /// Contract JSON formatted as string
        /// </returns>
        public string Contract(string Customer, string ScheduleName, string StartDate, bool GiftAid, string TerminationType, string AtTheEnd, string NumberofDebits = "", string Frequency = "", string InitialAmount = "",
            string ExtraInitialAmount = "", string PaymentAmount = "", string FinalAmount = "", string PaymentMonthInYear = "", string PaymentDayInMonth = "", string PaymentDayInWeek = "", string TerminationDate = "",
            string AdditionalReference = "", string CustomDDReference = "")
        {
            if (Customer == "" || ScheduleName == "" || StartDate == "" || TerminationType == "" || AtTheEnd == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            ContractChecks = new Utilities.ContractPostChecks();
            // Perform several basic checks to ensure the information provided for the customer is fit for use
            ContractChecks.CheckScheduleNameIsAvailable(ScheduleName, Settings);
            int TerminationTypeInt = ContractChecks.CheckTerminationTypeIsValid(TerminationType);
            int AtTheEndInt = ContractChecks.CheckAtTheEndIsValid(AtTheEnd);
            string StartDateDateString = ContractChecks.CheckStartDateIsValid(StartDate, Settings);
            bool AdHocBool = ContractChecks.CheckScheduleAdHocStatus(ScheduleName, Settings);

            if (AdHocBool)
            {
                if (bool.Parse(Settings.GetSection("contracts")["AutoFixTerminationTypeAdHoc"]))
                {
                    if (TerminationTypeInt != 1)
                    {
                        TerminationType = "Until further notice";
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TerminationTypeInt != 1)
                    {
                        throw new Exceptions.InvalidParameterException(string.Format("Termination type must be set to 'Until further notice' on ad-hoc contracts"));
                    }
                }

                if (bool.Parse(Settings.GetSection("contracts")["AutoFixAtTheEndAdHoc"]))
                {
                    if (AtTheEndInt != 1)
                    {
                        AtTheEnd = "Switch to further notice";
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (AtTheEndInt != 1)
                    {
                        throw new Exceptions.InvalidParameterException(string.Format("AtTheEnd must be set to 'Switch to further notice' on ad-hoc contracts"));
                    }
                }

                if (InitialAmount != "") { throw new Exceptions.ParameterNotAllowedException(string.Format("InitialAmount is not allowed on ad-hoc contracts")); }
                if (ExtraInitialAmount != "") { throw new Exceptions.ParameterNotAllowedException(string.Format("ExtraInitialAmount is not allowed on ad-hoc contracts")); }
                if (FinalAmount != "") { throw new Exceptions.ParameterNotAllowedException(string.Format("FinalAmount is not allowed on ad-hoc contracts")); }
            }
            else
            {
                if (Frequency == "") { throw new Exceptions.EmptyRequiredParameterException(string.Format("Frequency is mandatory on non-ad-hoc contracts")); }
                if (PaymentAmount == "") { throw new Exceptions.EmptyRequiredParameterException(string.Format("Payment amount is mandatory on non-ad-hoc contracts")); }

                int FrequencyType = ContractChecks.CheckFrequency(ScheduleName, Settings);

                if (FrequencyType == 0)
                {
                    if (PaymentDayInWeek == "")
                    {
                        throw new Exceptions.EmptyRequiredParameterException(string.Format("Payment day in week is mandatory on weekly contracts"));
                    }
                    else
                    {
                        ContractChecks.CheckPaymentDayInWeekIsValid(PaymentDayInWeek);
                    }
                }
                else if (FrequencyType == 1)
                {
                    if (PaymentDayInMonth == "")
                    {
                        throw new Exceptions.EmptyRequiredParameterException(string.Format("Payment day in month is mandatory on monthly contracts"));
                    }
                    else
                    {
                        if (StartDateDateString.Substring(8, 2) != PaymentDayInMonth.PadLeft(2, '0'))
                        {
                            if (bool.Parse(Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"]))
                            {
                                PaymentDayInMonth = StartDateDateString.Substring(8, 2);
                            }
                            else
                            {
                                PaymentDayInMonth = StartDateDateString.Substring(8, 2);
                                throw new Exceptions.InvalidParameterException(string.Format("PaymentDayInMonth must be set to {0} if the start date is {1}.", PaymentDayInMonth, StartDateDateString));
                            }
                        }
                        ContractChecks.CheckPaymentDayInMonthIsValid(PaymentDayInMonth);
                    }
                }
                else if (FrequencyType == 2)
                {
                    if (PaymentDayInMonth == "")
                    {
                        throw new Exceptions.EmptyRequiredParameterException(string.Format("Payment day in month is mandatory on annual contracts"));
                    }
                    else if (PaymentMonthInYear == "")
                    {
                        throw new Exceptions.EmptyRequiredParameterException(string.Format("Payment month in year is mandatory on annual contracts"));
                    }
                    else
                    {
                        if (StartDateDateString.Substring(5, 2) != PaymentMonthInYear.PadLeft(2, '0'))
                        {
                            if (bool.Parse(Settings.GetSection("contracts")["AutoFixPaymentMonthInYear"]))
                            {
                                PaymentMonthInYear = StartDateDateString.Substring(5, 2);
                            }
                            else
                            {
                                throw new Exceptions.InvalidParameterException(string.Format("PaymentMonthInYear must be set to {0} if the start date is {1}.", StartDateDateString.Substring(5, 2), StartDateDateString));
                            }
                        }
                        if (StartDateDateString.Substring(8, 2) != PaymentDayInMonth.PadLeft(2, '0'))
                        {
                            if (bool.Parse(Settings.GetSection("contracts")["AutoFixPaymentDayInMonth"]))
                            {
                                PaymentDayInMonth = StartDateDateString.Substring(8, 2);
                            }
                            else
                            {
                                throw new Exceptions.InvalidParameterException(string.Format("PaymentDayInMonth must be set to {0} if the start date is {1}.", StartDateDateString.Substring(8, 2), StartDateDateString));
                            }
                        }
                        ContractChecks.CheckPaymentDayInMonthIsValid(PaymentDayInMonth);
                        ContractChecks.CheckPaymentMonthInYearIsValid(PaymentMonthInYear);
                    }
                }

                if (TerminationTypeInt == 0)
                {
                    if (NumberofDebits == "")
                    {
                        throw new Exceptions.EmptyRequiredParameterException(string.Format("NumberOfDebits is mandatory if TerminationType is set to 'Take certain number of debits'."));
                    }
                    else
                    {
                        ContractChecks.CheckNumberOfDebitsIsValid(NumberofDebits);
                    }
                }
                else if (TerminationTypeInt == 1)
                {
                    if (AtTheEndInt != 1)
                    {
                        throw new Exceptions.InvalidParameterException(string.Format("AtTheEnd must be set to 'Switch to further notice' if TerminationType is set to 'Until further notice'."));
                    }
                }
                else if (TerminationTypeInt == 2)
                {
                    if (TerminationDate == "")
                    {
                        throw new Exceptions.EmptyRequiredParameterException(string.Format("TerminationDate is mandatory if TerminationType set to 'End on exact date'."));
                    }
                    else
                    {
                        ContractChecks.CheckTerminationDateIsAfterStartDate(TerminationDate, StartDate);
                    }
                }
            }
            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();
            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("scheduleName", ScheduleName);
                Parameters.Add("start", StartDateDateString);
                Parameters.Add("isGiftAid", GiftAid.ToString());
                Parameters.Add("terminationType", TerminationType);
                Parameters.Add("atTheEnd", AtTheEnd);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@accesspaysuite.com");
            }

            if (NumberofDebits != "") { Parameters.Add("numberOfDebits", NumberofDebits); }
            if (Frequency != "") { Parameters.Add("every", Frequency); }
            if (InitialAmount != "") { Parameters.Add("initialAmount", InitialAmount); }
            if (ExtraInitialAmount != "") { Parameters.Add("extraInitialAmount", ExtraInitialAmount); }
            if (PaymentAmount != "") { Parameters.Add("amount", PaymentAmount); }
            if (FinalAmount != "") { Parameters.Add("finalAmount", FinalAmount); }
            if (PaymentMonthInYear != "") { Parameters.Add("paymentMonthInYear", PaymentMonthInYear); }
            if (PaymentDayInMonth != "") { Parameters.Add("paymentDayInMonth", PaymentDayInMonth); }
            if (PaymentDayInWeek != "") { Parameters.Add("paymentDayInWeek", PaymentDayInWeek); }
            if (TerminationDate != "") { Parameters.Add("terminationDate", TerminationDate); }
            if (AdditionalReference != "") { Parameters.Add("additionalReference", AdditionalReference); }
            if (CustomDDReference != "") { Parameters.Add("customDirectDebitReference", CustomDDReference); }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Post(string.Format("customer/{0}/contract", Customer), Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);
            // Get the JSON returned from EazyCustomerManager
            JObject SendRequestAsJson = JObject.Parse(SendRequest);
            // Get the list of Contracts JSON objects
            JToken NewContract = SendRequestAsJson;
            return NewContract.ToString();
        }

        /// <summary>
        /// Cancel a Direct Debit within EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: Canceling a Direct Debit will not cancel the payment creation process.The reason being; There are two parts to a contract, the schedule and the Direct Debit.Cancelling the Direct Debit will 
        /// cease future payments to the bank, but it will generate payments on the system. These payments will return unpaid, though any ad-hoc payments must be manually deleted.
        /// </remarks>
        /// 
        /// <param name="Contract">The GUID of the contract to be archived</param>
        /// 
        /// <example>
        /// CancelDirectDebit("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        /// </example>
        /// 
        /// <returns>
        /// A confirmation string
        /// </returns>
        public string CancelDirectDebit(string Contract)
        {
            Session CreateRequest = Handler.Session(Settings);
            string SendRequest = CreateRequest.Post(string.Format("contract/{0}/cancel", Contract));

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("Contract not found"))
            {
                throw new Exceptions.ResourceNotFoundException(string.Format("The contract {0} could not be found.", Contract));
            }
            else
            {
                return SendRequest;
            }
        }

        /// <summary>
        /// Archive a contract within ECM3
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: Archiving a contract achieves different results to canceling a Direct Debit.First and most importantly, the process is irreversible.Once a contract is archived, it can not
        /// be unarchived.The process flow works like so; The Direct Debit is canceled, any arrears that are outstanding are written off, any future scheduled payments are canceled and finally, 
        /// the contract status is set to archived.Like canceling a Direct Debit, any ad_hoc payments must be manually canceled.
        /// </remarks>
        /// 
        /// <param name="Contract">The GUID of the contract owning the Direct Debit to be canceled</param>
        /// 
        /// <example>
        /// CancelDirectDebit("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        /// </example>
        ///
        /// <returns>
        /// A confirmation string
        /// </returns>
        public string ArchiveContract(string Contract)
        {
            Session CreateRequest = Handler.Session(Settings);
            string SendRequest = CreateRequest.Post(string.Format("contract/{0}/archive", Contract));

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("Contract is already archived"))
            {
                throw new Exceptions.RecordAlreadyExistsException(string.Format("The contract {0} is already archived.", Contract));
            }
            else
            {
                return SendRequest;
            }
        }

        /// <summary>
        /// Reactivate a Direct Debit within EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: Reactivating a contract changes the status of a contract from ‘Canceled’ to ‘Pending to activate’. This will sent a new instruction to the bank, generating an 0N charge.
        /// </remarks>
        /// 
        ///
        /// <param name="Contract"> GUID of the contract to be re-activated</param>
        /// 
        /// <example>
        /// ReactivateDirectDebit("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        /// </example>
        /// 
        /// <returns>
        /// Confirmation string
        /// </returns>

        public string ReactivateDirectDebit(string Contract)
        {
            Session CreateRequest = Handler.Session(Settings);
            string SendRequest = CreateRequest.Post(string.Format("contract/{0}/reactivate", Contract));

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);
            return SendRequest;
        }

        /// <summary>
        /// Restart a contract within EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: Restarting a contract is fundamentally different to reactivating a contract as it can only be performed if the following criteria have been met
        ///
        /// - The original contract was a fixed term which has expired
        /// - The payment schedule has met its end naturally, and the contract status has become 'Expired'
        ///
        /// This call adds a new contract onto the end of the previous contract, in effect 'recycling' the previous direct debit at the bank which can save on Direct Debit setup charges.
        /// </remarks>
        ///
        /// <param name="Contract">The GUID of the contract to be restated</param>
        /// <param name="TerminationType">The termination type of the restarted contract</param>
        /// <param name="AtTheEnd">What happens to the contract after the termination clause has been met</param>
        /// 
        /// <OptionalParams>
        /// <param name="PaymentAmount">Mandatory if the contract is not ad-hoc. The regular collection amount for the restated contract/param>
        /// <param name="InitialAmount">Used if the first collection amount is different from the rest.Not to be used on ad-hoc contracts.</param>
        /// <param name="FinalAmount">Used if the final collection amount is different from the rest.Not to be used on ad-hoc contracts.</param>
        /// <param name="PaymentDayInMonth">The collection day for monthly contracts.Accepts 1-28 or 'last day of month'</param>
        /// <param name="PaymentMonthInYear">The collection month for annual contracts. Accepts 1-12</param>
        /// <param name="AdditionalReference">An additional reference for the newly created contract</param>
        /// 
        /// </OptionalParams>
        /// <example>
        /// ReactivateDirectDebit("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        /// </example>
        /// 
        /// <returns>
        /// Confirmation string
        /// </returns>
        public string RestartContract(string Contract, string TerminationType, string AtTheEnd, string PaymentAmount = "", string InitialAmount = "", string FinalAmount = "", 
            string PaymentDayInMonth = "", string PaymentMonthInYear = "", string AditionalReference = "")
        {
            if (Contract == "" || TerminationType == "" || AtTheEnd == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();
            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("terminationType", TerminationType);
                Parameters.Add("atTheEnd", AtTheEnd);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@accesspaysuite.com");
            }

            if (PaymentAmount != "") { Parameters.Add("amount", PaymentAmount); }
            if (InitialAmount != "") { Parameters.Add("initialAmount", InitialAmount); }
            if (FinalAmount != "") { Parameters.Add("finalAmount", FinalAmount); }
            if (PaymentDayInMonth != "") { Parameters.Add("paymentDayInMonth", PaymentDayInMonth); }
            if (PaymentMonthInYear != "") { Parameters.Add("paymentMonthInYear", PaymentMonthInYear); }
            if (AditionalReference != "") { Parameters.Add("additionalReference", AditionalReference); }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Post(string.Format("contract/{0}/restart", Contract), Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);
            return SendRequest;
        }

        /// <summary>
        /// Create a new payment against an existing contract in EazyCustomerManager
        /// </summary>
        ///
        /// <param name="Contract">The GUID of the contact a payment will be made against</param>
        /// <param name="PaymentAmount">The total amount to be collected from the new payment</param>
        /// <param name="CollectionDate">The desired start date of the new contract. This must be x working days in the future, where x is the agreed amount of working days with Eazy Collect.</param>
        /// <param name="Comment">A comment related to the new payment/param>
        /// 
        /// <OptionalParams>
        /// <param name="IsCredit">If you have your own SUN and you have made prior arrangements with Eazy Collect, this may be passed to issue a credit to a customer.By default, it is set to false.</param>
        /// </OptionalParams>
        /// <example>
        /// Payment("42217d45-cf22-4430-ab02-acc1f8a2d020", "10.00", "2019-05-07", "A new payment")
        /// </example>
        /// 
        /// <returns>
        /// Confirmation string
        /// </returns>
        public string Payment(string Contract, string PaymentAmount, string CollectionDate, string Comment, bool IsCredit = false)
        {
            if (Contract == "" || PaymentAmount == "" || CollectionDate == "" || Comment == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();

            PaymentChecks = new Utilities.PaymentPostChecks();
            bool _PaymentAmountNotNegative = PaymentChecks.CheckPaymentAmount(PaymentAmount);
            string TrueCollectionDate = PaymentChecks.CheckPaymentDate(CollectionDate, Settings);
            bool IsCreditAllowed = PaymentChecks.CheckIfCreditIsAllowed(Settings);
            

            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("amount", PaymentAmount);
                Parameters.Add("date", TrueCollectionDate);
                Parameters.Add("comment", Comment);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@accesspaysuite.com");
            }

            if (!IsCreditAllowed)
            {
                if (IsCredit)
                {
                    throw new Exceptions.InvalidParameterException("Is Credit is not allowed.");
                }
            }
            else
            {
                if (IsCredit)
                {
                    Parameters.Add("isCredit", IsCredit.ToString());
                }
            }


            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Post(string.Format("contract/{0}/payment", Contract), Parameters);

            if (SendRequest.Contains("Contract not found"))
            {
                throw new Exceptions.InvalidParameterException("The specified contract could not be found in EazyCustomerManager");
            }

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);
            return SendRequest;
        }
    }
}