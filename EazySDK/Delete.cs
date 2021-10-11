using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EazySDK
{
    /// <summary>
    /// A collection of DELETE requests made to the EazyCustomerManager API
    /// </summary>
    public class Delete
    {
        // The client object which manages the Session and Settings
        private ClientHandler Handler { get; set; }
        // The settings object, derives from ClientHandler
        private IConfiguration Settings { get; set; }
        // The parameters passed from a function, if there are any
        private Dictionary<string, string> Parameters { get; set; }


        public Delete(IConfiguration _Settings)
        {
            // Get the Settings passed from the Client Handler
            Settings = _Settings;
            // Reference the Client Handler for sending a request
            Handler = new ClientHandler();
        }

        /// <summary>
        /// Delete the currently selected callback URL for EazyCustomerManager
        /// </summary>
        /// 
        /// <example>
        /// CallbackUrl();
        /// </example>
        /// 
        /// <returns>
        /// Confirmation string
        /// </returns>
        public string CallbackUrl(string entity)
        {
            string[] validEntities = { "contract", "customer", "payment" };

            if (!validEntities.Contains(entity.ToLower()))
            {
                throw new Exceptions.InvalidParameterException($"{entity} is not a valid entity; must be one of either 'contract', 'customer' or 'payment'.");
            }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Delete($"BACS/{entity}/callback");

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("null"))
            {
                throw new Exceptions.ResourceNotFoundException("A callback URL has not been set.");
            }
            else
            {
                return "Callback URL deleted";
            }
        }

        /// <summary>
        /// Delete a payment from EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// Once a payment has been uploaded to BACS, it is too late to delete it
        /// </remarks>
        /// 
        /// <param name="Contract">The GUID of an existing contract within EazyCustomerManager</param>
        /// <param name="Payment">The GUID of an existing payment within EazyCustomerManager</param>
        /// <param name="Comment">A comment to describe the actions performed when modifying the regular collection amount of the existing contract</param>
        /// 
        /// <example>
        /// Payment("36bb4f4f-9a7f-4ead-82dc-9295c6fb9e8b", "36bb4f4f-9a7f-4ead-82dc-9295c6fb9e8b", "Duplicate payment");
        /// </example>
        /// 
        /// <returns>
        /// Confirmation string
        /// </returns>
        public string Payment(string Contract, string Payment, string Comment)
        {
            if (Comment == "")
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
                Parameters.Add("comment", Comment);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@accesspaysuite.com");
            }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Delete(string.Format("contract/{0}/payment/{1}", Contract, Payment), _Parameters: Parameters);
            Handler.GenericExceptionCheck(SendRequest);

            // If no customers were returned
            if (SendRequest.Contains("Payment not found"))
            {
                return string.Format("The payment {0} either does not exist or has already been deleted", Payment);

            }
            else
            {
                return string.Format("Payment {0} deleted", Payment);
            }
        }
    }
}