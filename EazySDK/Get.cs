using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK
{
    /// <summary>
    /// A collection of GET requests made to the EazyCustomerManager API
    /// </summary>
    public class Get
    {
        #region Client objects
        // The client object which manages the Session and Settings
        private ClientHandler Handler { get; set; }
        // The settings object, derives from ClientHandler
        private IConfiguration Settings { get; set; }
        #endregion Client objects

        #region Dictionary objects
        // The parameters passed from a function, if there are any
        private Dictionary<string, string> Parameters { get; set; }
        #endregion


        public Get(IConfiguration _Settings)
        {
            // Get the Settings passed from the Client Handler
            Settings = _Settings;
            // Reference the Client Handler for sending a request
            Handler = new ClientHandler();
        }

        /// <summary>
        /// Get the current callback URL for the given entity from EazyCustomerManager
        /// </summary>
        /// 
        /// <param name="entity">The entity for which to receive BACS messages. Valid choices: "contract", "customer", "payment", "schedule"</param>
        /// 
        /// <example>
        /// CallbackUrl("contract");
        /// </example>
        /// 
        /// <returns>
        /// "The callback URL is example.com" 
        /// </returns>
        public string CallbackUrl(string entity)
        {
            string[] validEntities = { "contract", "customer", "payment", "schedule" };
            
            if (!validEntities.Contains(entity.ToLower()))
            {
                throw new Exceptions.InvalidParameterException($"{entity} is not a valid entity; must be one of either 'contract', 'customer', 'payment' or 'schedule'.");
            }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Get($"BACS/{entity}/callback");

            // Null will be returned if a callback URL currently does not exist
            if (SendRequest == "{\"Message\":null)")
            {
                return "A callback URI has not yet been set";
            }
            else
            {
                // Pass the return string to the handler. This will throw an exception if it is not what we expect
                Handler.GenericExceptionCheck(SendRequest);

                // Get the JSON returned from EazyCustomerManager
                JObject SendRequestAsJson = JObject.Parse(SendRequest);
                // Get the Message object from JSON
                var CallbackUri = SendRequestAsJson["Message"];
                return string.Format("The callback URL is {0}", CallbackUri);
            }
        }

        /// <summary>
        /// Search for an individual or a group of customers in EazyCustomerManager
        /// </summary>
        /// <remarks>
        /// NOTE: While searching for customers without any parameters, a warning will be rendered. This can be disabled by setting the warnings["customerSearch"] setting to false.
        /// </remarks>
        /// 
        /// <optionalParameters>
        /// <param name="Email">The full email address of a customer in EazyCustomerManager</param>
        /// <param name="Title">The title of a single customer or a group of customers in EazyCustomerManager</param>
        /// <param name="SearchFrom">Search for customers created in EazyCustomerManager after a given date</param>
        /// <param name="SearchTo">Search for a customer created in EazyCustomerManager before a given date</param>
        /// <param name="DateOfBirth">The full date of birth of a customer or a set of customers in EazyCustomerManager</param>
        /// <param name="CustomerReference">The full customer reference of a customer in EazyCustomerManager</param>
        /// <param name="FirstName">The full or partial first name of a customer or a set of customer in EazyCustomerManagers</param>
        /// <param name="Surname">The full or partial surname of a customer or group of customers in EazyCustomerManager</param>
        /// <param name="CompanyName">The full or partial company name of a customer or a set of customers in EazyCustomerManager</param>
        /// <param name="PostCode">The full or partial post code of a customer or a set of customers in EazyCustomerManager</param>
        /// <param name="AccountNumber">The full account number of a customer in EazyCustomerManager</param>
        /// <param name="SortCode">The full sort code of a customer or a group of customers in EazyCustomerManager</param>
        /// <param name="AccountHolderName">The full account holder name of a customer in EazyCustomerManager</param>
        /// <param name="HomePhone">The full or partial home phone number of a customer or group of customers in EazyCustomerManager</param>
        /// <param name="MobilePhone">The full or partial mobile phone number of a customer or group of customers in EazyCustomerManager</param>
        /// <param name="WorkPhone">The full or partial work phone number of a customer or group of customers in EazyCustomerManager</param>
        /// </optionalParameters>
        /// 
        /// <example>
        /// Customers(PostCode: "GL52 2NF")
        /// </example>
        /// 
        /// <returns>
        /// A string of customer JSON objects
        /// </returns>
        public string Customers(string Email = "", string Title = "", string SearchFrom = "", string SearchTo = "", string DateOfBirth = "", string CustomerReference = "", string FirstName = "", string Surname = "", 
            string CompanyName = "", string PostCode = "", string AccountNumber = "", string SortCode = "", string AccountHolderName = "", string HomePhone = "", string MobilePhone = "", string WorkPhone = "")
        {
            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();

            // Add method arguments to the parameters only if they are not empty
            if (Email != "") { Parameters.Add("email", Email); }
            if (Title != "") { Parameters.Add("title", Title); }
            if (SearchFrom != "") { Parameters.Add("from", SearchFrom); }
            if (SearchTo != "") { Parameters.Add("to", SearchTo); }
            if (DateOfBirth != "") { Parameters.Add("dateOfBirth", DateOfBirth); }
            if (CustomerReference != "") { Parameters.Add("customerRef", CustomerReference); }
            if (FirstName != "") { Parameters.Add("firstName", FirstName); }
            if (Surname != "") { Parameters.Add("surname", Surname); }
            if (CompanyName != "") { Parameters.Add("companyName", CompanyName); }
            if (PostCode != "") { Parameters.Add("postCode", PostCode); }
            if (AccountNumber != "") { Parameters.Add("accountNumber", AccountNumber); }
            if (SortCode != "") { Parameters.Add("bankSortCode", SortCode); }
            if (AccountHolderName != "") { Parameters.Add("accountHolderName", AccountHolderName); }
            if (HomePhone != "") { Parameters.Add("homePhoneNumber", HomePhone); }
            if (MobilePhone != "") { Parameters.Add("mobilePhoneNumber", MobilePhone); }
            if (WorkPhone != "") { Parameters.Add("workPhoneNumber", WorkPhone); }

            // If no parameters have been passed and the customerSearch warning is enabled, warn the user
            if (bool.Parse(Settings.GetSection("warnings")["CustomerSearchWarning"]) == true && Parameters.Count == 0)
            {
                Console.WriteLine("Searching for customers without passing any parameters may take some time.");
            }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Get("customer", _Parameters: Parameters);

            // If no customers were returned
            if (SendRequest == "{\"Customers\":[]}")
            {
                return "No customers could be found with the provided parameters";
            }
            else
            {
                // Pass the return string to the handler. This will throw an exception if it is not what we expect
                Handler.GenericExceptionCheck(SendRequest);

                // Get the JSON returned from EazyCustomerManager
                JObject SendRequestAsJson = JObject.Parse(SendRequest);
                // Get the list of Customers JSON objects
                var Customers = SendRequestAsJson["Customers"];
                return string.Format("{0}", Customers);
            }
        }

        /// <summary>
        /// Return all contracts belonging to a provided customer
        /// </summary>
        /// 
        /// <param name="Customer">The GUID of the customer to be searched against</param>
        /// 
        /// <example>
        /// Contracts("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        /// </example>
        /// 
        /// <returns>
        /// A string of contract JSON objects
        /// </returns>
        public string Contracts(string Customer)
        {
            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Get(string.Format("customer/{0}/contract", Customer));

            // If no contracts were returned
            if (SendRequest.Contains("\"Contracts\":[]"))
            {
                return string.Format("No contracts could be associated with the customer {0}", Customer);
            }
            else
            {
                // Pass the return string to the handler. This will throw an exception if it is not what we expect
                Handler.GenericExceptionCheck(SendRequest);
                // Get the JSON returned from EazyCustomerManager
                JObject SendRequestAsJson = JObject.Parse(SendRequest);
                // Get the list of Contracts JSON objects
                var Contracts = SendRequestAsJson["Contracts"];
                return Contracts.ToString();
            }
        }

        /// <summary>
        /// Return all payments belonging to a specific contract
        /// </summary>
        /// 
        /// <param name="Contract">The GUID of the contract to be searched against</param>
        /// 
        /// <optionalParams>
        /// <param name="NumberOfRows">The number of payments to be returned. By default, this is set to 100</param>
        /// </optionalParams>
        /// 
        /// <example>
        /// Payments("ab09362d-f88e-4ee8-be85-e27e1a6ce06a")
        /// </example>
        /// 
        /// <returns>
        /// A string of payment JSON objects
        /// </returns>
        public string Payments(string Contract, int NumberOfRows=100)
        {
            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>
            {
                { "rows", NumberOfRows.ToString() }
            };

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Get(string.Format("contract/{0}/payment", Contract), _Parameters: Parameters);

            // If no payments were returned
            if (SendRequest == "{\"Payments\":[]}")
            {
                return string.Format("No payments could be associated with the contract {0}", Contract);
            }
            else
            {
                // Pass the return string to the handler. This will throw an exception if it is not what we expect
                Handler.GenericExceptionCheck(SendRequest);
                // Get the JSON returned from EazyCustomerManager
                JObject SendRequestAsJson = JObject.Parse(SendRequest);
                // Get the list of payment JSON objects
                var Payments = SendRequestAsJson["Payments"];
                return string.Format("{0}", Payments);
            }
        }

        /// <summary>
        /// Return a single payment from a given contract
        /// </summary>
        /// 
        /// <param name="Contract">The GUID of the contract to be searched against</param>
        /// <param name="Payment">The GUID of the payment to be searched against</param>
        /// 
        /// <example>
        /// PaymentsSingle("ab09362d-f88e-4ee8-be85-e27e1a6ce06a", "36bb4f4f-9a7f-4ead-82dc-9295c6fb9e8b")
        /// </example>
        /// 
        /// <returns>
        /// payment json object
        /// </returns>
        public string PaymentsSingle(string Contract, string Payment)
        {
            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Get(string.Format("contract/{0}/payment/{1}", Contract, Payment));

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            return SendRequest;
        }

        /// <summary>
        /// Return all available schedules from EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: You should not need to run this command manually.The SDK will automatically get a list of available schedules when first ran, 
        /// and place them in the includes folder, named sandbox.csv and ecm3.csv respectively.
        /// </remarks>
        /// 
        /// <returns>
        /// schedules json objects
        /// </returns>
        public string Schedules()
        {
            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Get("schedules");

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            return SendRequest;
        }
    }
}
