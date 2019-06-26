using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace EazySDK
{
    /// <summary>
    /// A collection of PATCH requests made to the EazyCustomerManager API
    /// </summary>
    public class Patch
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


        public Patch(IConfiguration _Settings)
        {
            // Get the Settings passed from the Client Handler
            Settings = _Settings;
            // Reference the Client Handler for sending a request
            Handler = new ClientHandler();
        }

        /// <summary>
        /// Modify a customer in EazyCustomerManager
        /// </summary>
        /// 
        /// <param name="Customer">The GUID of an existing customer in EazyCustomerManager</param>
        /// 
        /// <OptionalParams>
        /// <param name="Email">The new email address of the existing customer. This must be unique</param>
        /// <param name="Title">The new title of the existing customer</param>
        /// <param name="DateOfBirth">The new date of birth of the existing customer in YYYY-MM-DD format</param>
        /// <param name="FirstName">The new first name of the existing customer</param>
        /// <param name="Surname">The new surname of the existing customer</param>
        /// <param name="CompanyName">The new company name of the existing customer</param>
        /// <param name="Line1">The new first line of the existing customers address</param>
        /// <param name="Line2">The new second line of the existing customers address</param>
        /// <param name="Line3">The new third line of the existing customers address</param>
        /// <param name="Line4">The new fourth line of the existing customers address</param>
        /// <param name="PostCode">The new post code of the existing customers address</param>
        /// <param name="AccountNumber">The new account number of the existing customer</param>
        /// <param name="SortCode">The new sort code of the existing customer</param>
        /// <param name="AccountHolderName">The new name on the account of the existing customer</param>
        /// <param name="HomePhone">The new home phone number of the existing customer</param>
        /// <param name="MobilePhone">The new mobile phone number of the existing customer</param>
        /// <param name="WorkPhone">The new work phone number of the existing customer</param>
        /// <param name="Initials">The new initials of the existing customer</param>
        /// </OptionalParams>
        /// 
        /// <example>
        /// Customer("ab09362d-f88e-4ee8-be85-e27e1a6ce06a", Surname: "New surname");
        /// </example>
        /// 
        /// <returns>
        /// Confirmation string
        /// </returns>
        public string Customer(string Customer, string Email = "", string Title = "", string DateOfBirth = "", string FirstName = "", string Surname = "", string CompanyName = "", string Line1 = "",
            string Line2 = "", string Line3 = "", string Line4 = "", string PostCode = "", string AccountNumber = "", string SortCode = "", string AccountHolderName = "", string HomePhone = "",
            string MobilePhone = "", string WorkPhone = "", string Initials = "")
        {
            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();

            CustomerChecks = new Utilities.CustomerPostChecks();
            if (Email != "")
            {
                CustomerChecks.CheckEmailAddressIsCorrectlyFormatted(Email);
                Parameters.Add("email", Email);       
            }
            if (PostCode != "")
            {
                CustomerChecks.CheckPostCodeIsCorectlyFormatted(PostCode);
                Parameters.Add("postCode", PostCode);
            }
            if (AccountNumber != "")
            {
                CustomerChecks.CheckAccountNumberIsFormattedCorrectly(AccountNumber);
                Parameters.Add("accountNumber", AccountNumber);
            }
            if (SortCode != "")
            {
                CustomerChecks.CheckSortCodeIsFormattedCorrectly(SortCode);
                Parameters.Add("bankSortCode", SortCode);
            }
            if (AccountHolderName != "")
            {
                CustomerChecks.CheckAccountHolderNameIsFormattedCorrectly(AccountHolderName);
                Parameters.Add("accountHolderName", AccountHolderName);
            }
            if (Title != "") { Parameters.Add("title", Title); }
            if (FirstName != "") { Parameters.Add("firstName", FirstName); }
            if (Surname != "") { Parameters.Add("surname", Surname); }
            if (Line1 != "") { Parameters.Add("line1", Line1); }
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
            var SendRequest = CreateRequest.Patch(string.Format("customer/{0}", Customer), _Parameters: Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("Customer updated"))
            {
                return string.Format("The customer {0} has been updated successfully", Customer);
            }
            else
            {
                return string.Format("An unknown error has occurred. The customer {0} has not been updated", Customer);
            }
        }

        /// <summary>
        /// Modify the regular collection amount of a contract in EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// It is important to note that if the contract is already within the cut-off date for the next collection, the regular collection amount will not be amended until the following month.
        /// </remarks>
        /// 
        /// <param name="Contract">The GUID of an existing contract within EazyCustomerManager</param>
        /// <param name="PaymentAmount">The new regular collection amount the existing contract</param>
        /// <param name="Comment">A comment to describe the actions performed when modifying the regular collection amount of the existing contract</param>
        /// 
        /// <example>ContractAmount("36bb4f4f-9a7f-4ead-82dc-9295c6fb9e8b", "10.50", "Added x product");</example>
        /// <returns></returns>
        public string ContractAmount(string Contract, string PaymentAmount, string Comment)
        {
            if (PaymentAmount == "" || Comment == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                    );
            }

            PaymentChecks = new Utilities.PaymentPostChecks();
            // Perform several basic checks to ensure the information provided for the customer is fit for use
            PaymentChecks.CheckPaymentAmount(PaymentAmount);

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();

            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("amount", PaymentAmount);
                Parameters.Add("comment", Comment);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@eazycollect.co.uk");
            }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Patch(string.Format("contract/{0}/amount", Contract), _Parameters: Parameters);
            Handler.GenericExceptionCheck(SendRequest);

            // If no customers were returned
            if (SendRequest.Contains("Contract updated"))
            {
                return string.Format("The contract {0} has been updated with the new regular collection amount of {1}", Contract, PaymentAmount);
                 
            }
            else if (SendRequest.Contains("Contract not found"))
            {
                throw new Exceptions.ResourceNotFoundException(string.Format("The contract {0} could not be found within EazyCustomerManager", Contract));
            }
            else
            {
                return SendRequest;
            }
        }

        /// <summary>
        /// Modify the regular collection day of a weekly contract in EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// It is important to note that if the contract is already within the cut-off date for the next collection, the regular collection day will not be amended until the following week.
        /// </remarks>
        /// 
        ///
        /// <param name="Contract">The GUID of an existing contract within EazyCustomerManager</param>
        /// <param name="NewPaymentDay">The new regular collection day of the existing contract</param>
        /// <param name="Comment">A comment to describe the amendment made to the contract</param>
        /// <param name="AmmendNextPayment">Whether or not the next payment should be amended to account for the change in contract day</param>
        /// <param name="NextPaymentAmount">If the next payment amount is to be amended, dictate what the next payment amount should be</param>
        /// <returns></returns>
        public string ContractDayWeekly(string Contract, string NewPaymentDay, string Comment, bool AmmendNextPayment, string NextPaymentAmount = "")
        {
            if (NewPaymentDay == "" || Comment == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            ContractChecks = new Utilities.ContractPostChecks();
            // Perform several basic checks to ensure the information provided for the customer is fit for use
            ContractChecks.CheckPaymentDayInWeekIsValid(NewPaymentDay);
           
            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();
            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("day", NewPaymentDay);
                Parameters.Add("comment", Comment);
                Parameters.Add("patchNextPayment", AmmendNextPayment.ToString());
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@eazycollect.co.uk");
            }

            if (AmmendNextPayment) { Parameters.Add("nextPaymentPatchAmount", NextPaymentAmount); }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Patch(string.Format("contract/{0}/weekly", Contract), Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("Contract updated"))
            {
                return string.Format("The contract {0} has been updated with the new regular collection day of {1}", Contract, NewPaymentDay);
            }
            else if (SendRequest.Contains("Contract not found"))
            {
                throw new Exceptions.ResourceNotFoundException(string.Format("The contract {0} could not be found within EazyCustomerManager", Contract));
            }
            else
            {
                return SendRequest;
            }
        }

        /// <summary>
        /// Modify the regular collection day of a monthly contract in EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// It is important to note that if the contract is already within the cut-off date for the next collection, the regular collection day will not be amended until the following month.
        /// </remarks>
        /// 
        ///
        /// <param name="Contract">The GUID of an existing contract within EazyCustomerManager</param>
        /// <param name="NewPaymentDay">The new regular collection day of the existing contract</param>
        /// <param name="Comment">A comment to describe the amendment made to the contract</param>
        /// <param name="AmmendNextPayment">Whether or not the next payment should be amended to account for the change in contract day</param>
        /// <param name="NextPaymentAmount">If the next payment amount is to be amended, dictate what the next payment amount should be</param>
        /// <returns></returns>
        public string ContractDayMonthly(string Contract, string NewPaymentDay, string Comment, bool AmendNextPayment, string NextPaymentAmount = "")
        {
            if (NewPaymentDay == "" || Comment == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            if (AmendNextPayment)
            {
                if (NextPaymentAmount == "")
                {
                    throw new Exceptions.EmptyRequiredParameterException("NextPaymentAmount must be called if AmendNextPayment is set to true.");
                }
            }
            else
            {
                if (NextPaymentAmount != "")
                {
                    throw new Exceptions.ParameterNotAllowedException("NextPaymentAmount must not be called if AmendNextPayment is set to false.");
                }
            }


            ContractChecks = new Utilities.ContractPostChecks();
            // Perform several basic checks to ensure the information provided for the customer is fit for use
            ContractChecks.CheckPaymentDayInMonthIsValid(NewPaymentDay);

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();
            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("monthDay", NewPaymentDay);
                Parameters.Add("comment", Comment);
                Parameters.Add("patchNextPayment", AmendNextPayment.ToString());
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@eazycollect.co.uk");
            }

            if (AmendNextPayment) { Parameters.Add("nextPaymentPatchAmount", NextPaymentAmount); }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Patch(string.Format("contract/{0}/monthly", Contract), Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("Contract updated"))
            {
                return string.Format("The contract {0} has been updated with the new regular collection day of {1}", Contract, NewPaymentDay);
            }
            else if (SendRequest.Contains("Contract not found"))
            {
                throw new Exceptions.ResourceNotFoundException(string.Format("The contract {0} could not be found within EazyCustomerManager", Contract));
            }
            else
            {
                return SendRequest;
            }
        }

        /// <summary>
        /// Modify the regular collection day  and month of an annual contract in EazyCustomerManager
        /// </summary>
        /// 
        /// <remarks>
        /// It is important to note that if the contract is already within the cut-off date for the next collection, the regular collection day will not be amended until the following year.
        /// </remarks>
        /// 
        ///
        /// <param name="Contract">The GUID of an existing contract within EazyCustomerManager</param>
        /// <param name="NewPaymentDay">The new regular collection day of the existing contract</param>
        /// <param name="NewPaymentMonth">The new regular collection month of the existing contract</param>
        /// <param name="Comment">A comment to describe the amendment made to the contract</param>
        /// <param name="AmmendNextPayment">Whether or not the next payment should be amended to account for the change in contract day</param>
        /// <param name="NextPaymentAmount">If the next payment amount is to be amended, dictate what the next payment amount should be</param>
        /// <returns></returns>
        public string ContractDayAnnually(string Contract, string NewPaymentDay, string NewPaymentMonth, string Comment, bool AmendNextPayment, string NextPaymentAmount = "")
        {
            if (NewPaymentDay == "" || NewPaymentMonth == "" || Comment == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            if (AmendNextPayment)
            {
                if (NextPaymentAmount == "")
                {
                    throw new Exceptions.EmptyRequiredParameterException("NextPaymentAmount must be called if AmendNextPayment is set to true.");
                }
            }
            else
            {
                if (NextPaymentAmount != "")
                {
                    throw new Exceptions.ParameterNotAllowedException("NextPaymentAmount must not be called if AmendNextPayment is set to false.");
                }
            }

            ContractChecks = new Utilities.ContractPostChecks();
            // Perform several basic checks to ensure the information provided for the customer is fit for use
            ContractChecks.CheckPaymentDayInMonthIsValid(NewPaymentDay);
            ContractChecks.CheckPaymentMonthInYearIsValid(NewPaymentMonth);

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();
            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("monthDay", NewPaymentDay);
                Parameters.Add("month", NewPaymentMonth);
                Parameters.Add("comment", Comment);
                Parameters.Add("patchNextPayment", AmendNextPayment.ToString());
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@eazycollect.co.uk");
            }

            if (AmendNextPayment) { Parameters.Add("nextPaymentPatchAmount", NextPaymentAmount); }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Patch(string.Format("contract/{0}/annual", Contract), Parameters);

            // Pass the return string to the handler. This will throw an exception if it is not what we expect
            Handler.GenericExceptionCheck(SendRequest);

            if (SendRequest.Contains("Contract updated"))
            {
                return string.Format("The contract {0} has been updated with the new regular collection day of {1} and regular collection month of {2}", Contract, NewPaymentDay, NewPaymentMonth);
            }
            else if (SendRequest.Contains("Contract not found"))
            {
                throw new Exceptions.ResourceNotFoundException(string.Format("The contract {0} could not be found within EazyCustomerManager", Contract));
            }
            else
            {
                return SendRequest;
            }
        }

        /// <summary>
        /// Modify a payment within EazyCusotmerManager
        /// </summary>
        /// 
        /// <remarks>
        /// It is important to note that once a payment has been submitted to BACS, it is too late to amend the payment
        /// </remarks>
        /// 
        /// <param name="Contract">The GUID of an existing contract within EazyCustomerManager</param>
        /// <param name="Payment">The GUID of an existing payment within EazyCustomerManager</param>
        /// <param name="PaymentAmount">The new regular collection amount the existing contract</param>
        /// <param name="PaymentDay">The new collection day of the payment</param>
        /// <param name="Comment">A comment to describe the actions performed when amending the payment</param>
        /// 
        /// <returns></returns>
        public string Payment(string Contract, string Payment, string PaymentAmount, string PaymentDay, string Comment)
        {
            if (PaymentAmount == "" || PaymentDay == "" || Comment == "")
            {
                throw new Exceptions.EmptyRequiredParameterException(
                    "One or more required parameters are empty. Please double check all required parameters are filled and re-submit."
                );
            }

            // Create a new dictionary of parameters
            Parameters = new Dictionary<string, string>();

            PaymentChecks = new Utilities.PaymentPostChecks();
            PaymentChecks.CheckPaymentAmount(PaymentAmount);

            // Add method arguments to the parameters only if they are not empty
            try
            {
                Parameters.Add("amount", PaymentAmount);
                Parameters.Add("date", PaymentDay);
                Parameters.Add("comment", Comment);
            }
            catch (ArgumentException)
            {
                throw new Exceptions.InvalidParameterException("There was an error adding one or more parameters to the call. Please try again, or contact help@eazycollect.co.uk");
            }

            var CreateRequest = Handler.Session(Settings);
            var SendRequest = CreateRequest.Patch(string.Format("contract/{0}/payment/{1}", Contract, Payment), Parameters);

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