using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK
{
    /// <summary>
    /// A collection of POST requests made to the EazyCustomerManager API
    /// </summary>
    public class Post
    {
        #region Client objects
        // The client object which manages the Session and Settings
        private ClientHandler Handler { get; set; }
        // The settings object, derives from ClientHandler
        private IConfiguration Settings { get; set; }
        private Utilities.CustomerPostChecks CustomerChecks { get; set; }
        #endregion Client objects

        #region Dictionary objects
        // The parameters passed from a function, if there are any
        private Dictionary<string, string> Parameters { get; set; }
        #endregion


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
        /// <example>
        /// CallbackUrl("https://test.com");
        /// </example>
        /// 
        /// <returns>
        /// "The new callback URL is https://test.com" 
        /// </returns>
        public string CallbackUrl(string _CallbackUrl)
        {
            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>
            {
                { "url", _CallbackUrl }
            };

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Post("BACS/callback", _Parameters: Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            return string.Format("The new callback URL is {0}", _CallbackUrl);
        }

        /// <summary>
        /// Create a new customer in ECM3
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
        /// Customer JSON object
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
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@eazycollect.co.uk");
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

        ///// <summary>
        ///// Return all contracts belonging to a provided customer
        ///// </summary>
        ///// 
        ///// <param name="Customer">The GUID of the customer to be searched against</param>
        ///// 
        ///// <example>
        ///// Contracts("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        ///// </example>
        ///// 
        ///// <returns>
        ///// A string of contract JSON objects
        ///// </returns>
        //public string Contracts(string Customer)
        //{
        //    var CreateRequest = Handler.Session(Settings);
        //    var SendRequest = CreateRequest.Get(string.Format("customer/{0}/contract", Customer));

        //    // If no contracts were returned
        //    if (SendRequest.Contains("\"Contracts\":[]"))
        //    {
        //        return string.Format("No contracts could be associated with the customer {0}", Customer);
        //    }
        //    else
        //    {
        //        // Pass the return string to the handler. This will throw an exception if it is not what we expect
        //        Handler.GenericExceptionCheck(SendRequest);
        //        // Get the JSON returned from EazyCustomerManager
        //        JObject SendRequestAsJson = JObject.Parse(SendRequest);
        //        // Get the list of Contracts JSON objects
        //        var Contracts = SendRequestAsJson["Contracts"];
        //        return Contracts.ToString();
        //    }
        //}

        ///// <summary>
        ///// Return all payments belonging to a specific contract
        ///// </summary>
        ///// 
        ///// <param name="Contract">The GUID of the contract to be searched against</param>
        ///// 
        ///// <optionalParams>
        ///// <param name="NumberOfRows">The number of payments to be returned. By default, this is set to 100</param>
        ///// </optionalParams>
        ///// 
        ///// <example>
        ///// Payments("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        ///// </example>
        ///// 
        ///// <returns>
        ///// A string of payment JSON objects
        ///// </returns>
        //public string Payments(string Contract, int NumberOfRows = 100)
        //{
        //    // Create a new dictionary of parameters
        //    Parameters = new Dictionary<string, string>
        //    {
        //        { "rows", NumberOfRows.ToString() }
        //    };

        //    var CreateRequest = Handler.Session(Settings);
        //    var SendRequest = CreateRequest.Get(string.Format("contract/{0}/payment", Contract), _Parameters: Parameters);

        //    // If no payments were returned
        //    if (SendRequest == "{\"Payments\":[]}")
        //    {
        //        return string.Format("No payments could be associated with the contract {0}", Contract);
        //    }
        //    else
        //    {
        //        // Pass the return string to the handler. This will throw an exception if it is not what we expect
        //        Handler.GenericExceptionCheck(SendRequest);
        //        // Get the JSON returned from EazyCustomerManager
        //        JObject SendRequestAsJson = JObject.Parse(SendRequest);
        //        // Get the list of payment JSON objects
        //        var Payments = SendRequestAsJson["Payments"];
        //        return string.Format("{0}", Payments);
        //    }
        //}

        ///// <summary>
        ///// Return a single payment from a given contract
        ///// </summary>
        ///// 
        ///// <param name="Contract">The GUID of the contract to be searched against</param>
        ///// <param name="Payment">The GUID of the payment to be searched against</param>
        ///// 
        ///// <example>
        ///// PaymentsSingle("ab09362d-f88e-4ee8-be85-e27e1a6ce06a", "36bb4f4f-9a7f-4ead-82dc-9295c6fb9e8b")
        ///// </example>
        ///// 
        ///// <returns>
        ///// payment json object
        ///// </returns>
        //public string PaymentsSingle(string Contract, string Payment)
        //{
        //    var CreateRequest = Handler.Session(Settings);
        //    var SendRequest = CreateRequest.Get(string.Format("contract/{0}/payment/{1}", Contract, Payment));

        //    // Pass the return string to the handler. This will throw an exception if it is not what we expect
        //    Handler.GenericExceptionCheck(SendRequest);

        //    return SendRequest;
        //}

        ///// <summary>
        ///// Return all available schedules from EazyCustomerManager
        ///// </summary>
        ///// 
        ///// <remarks>
        ///// NOTE: You should not need to run this command manually.The SDK will automatically get a list of available schedules when first ran, 
        ///// and place them in the includes folder, named sandbox.csv and ecm3.csv respectively.
        ///// </remarks>
        ///// 
        ///// <returns>
        ///// schedules json objects
        ///// </returns>
        //public string Schedules()
        //{
        //    var CreateRequest = Handler.Session(Settings);
        //    var SendRequest = CreateRequest.Get(string.Format("schedules"));

        //    // Pass the return string to the handler. This will throw an exception if it is not what we expect
        //    Handler.GenericExceptionCheck(SendRequest);

        //    return SendRequest;
        //}
    }
}