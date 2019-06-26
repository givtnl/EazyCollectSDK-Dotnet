using System;
using Microsoft.Extensions.Configuration;

namespace EazySDK.Utilities
{
    public class PaymentPostChecks
    {
        /// <summary>
        /// Check that the collection_date argument is a valid ISO date and is at least x working days in the future, where x is the predetermined OngoingProcessingDays setting.
        /// Throw an error if this is not the case.
        /// </summary>
        /// 
        /// <param name="CollectionDate">A collection date provided by an external function</param>
        /// <param name="Settings">Settings configuration used for making a call to EazyCustomerManager</param>
        /// 
        /// <example>
        /// CheckPaymentDate("2019-06-24", Settings)
        /// </example>
        /// 
        /// <returns>
        /// date formatted as string
        /// </returns>
        public string CheckPaymentDate(string CollectionDate, IConfiguration Settings)
        {
            DateTime InitialDate = new DateTime();
            int OngoingProcessingDays = new int();
            bool AutoFixStartDate = new bool();

            try
            {
                InitialDate = DateTime.Parse(CollectionDate);
            }
            catch (FormatException)
            {
                throw new Exceptions.InvalidPaymentDateException(string.Format("The payment date {0} is not valid. Please re-submit in ISO format (yyyy-mm-dd)", CollectionDate));
            }

            try
            {
                OngoingProcessingDays = int.Parse(Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"]);
            }
            catch (FormatException)
            {
                string InvalidOPD = Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"];
                throw new Exceptions.InvalidSettingsConfigurationException(string.Format("The ongoingProcessingDays requires an integer. '{0}' is not a valid value for this setting.", InvalidOPD));
            }

            try
            {
                AutoFixStartDate = bool.Parse(Settings.GetSection("payments")["AutoFixPaymentDate"]);
            }
            catch (FormatException)
            {
                string InvalidAFSD = Settings.GetSection("contracts")["AutoFixStartDate"];
                throw new Exceptions.InvalidSettingsConfigurationException(string.Format("The AutoFixStartDate requires a boolean. '{0}' is not a valid value for this setting.", InvalidAFSD));
            }

            CheckWorkingDays WorkingDays = new CheckWorkingDays();
            DateTime FirstAvailableDate = WorkingDays.CheckWorkingDaysInFuture(OngoingProcessingDays);

            if (InitialDate < FirstAvailableDate)
            {
                if (AutoFixStartDate)
                {
                    return FirstAvailableDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    throw new Exceptions.InvalidPaymentDateException(string.Format("The payment date of {0} is too soon. The earliest possible payment date is {1}", CollectionDate, FirstAvailableDate.ToString("yyyy-MM-dd")));
                }
            }
            else
            {
                return InitialDate.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Check whether or not is_credit is enabled for a client. This function does not check whether is_credit is enabled through EazyCustomerManager, 
        /// instead it checks the setting in AppSettings.json. Regardless of the setting, IsCredit will fail if it is  disallowed on EazyCustomerManager.
        /// </summary>
        /// 
        /// <param name="Settings">Settings configuration used for making a call to EazyCustomerManager</param>
        /// 
        /// <example>
        /// CheckIfCreditIsAllowed(false, Settings)
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckIfCreditIsAllowed(IConfiguration Settings)
        {
            bool IsCreditAllowed = new bool();

            try
            {
                IsCreditAllowed = bool.Parse(Settings.GetSection("payments")["IsCreditAllowed"]);
            }
            catch (FormatException)
            {
                string InvalidCredit = Settings.GetSection("payments")["IsCreditAllowed"];
                throw new Exceptions.InvalidSettingsConfigurationException(string.Format("The isCreditAllowed setting is misconfigured. It's value is '{0}', though it should be either TRUE or FALSE.", InvalidCredit));
            }

            if (IsCreditAllowed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check that the PaymentAmount is above 0.00. If this is not the case, throw an error.
        /// </summary>
        /// 
        /// <param name="PaymentAmount">A PaymentAmount provided by an external function</param>
        /// 
        /// <example>
        /// CheckPaymentAmount("123.45")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckPaymentAmount(string PaymentAmount)
        {
            float fPaymentAmount = new float();

            try
            {
                fPaymentAmount = float.Parse(PaymentAmount);
            }
            catch (FormatException)
            {
                throw new Exceptions.InvalidParameterException("The payment amount must be formatted as GBP currency, using x.xx formatting. " + PaymentAmount + " does not conform to this formatting.");
            }

            if (fPaymentAmount >= 0.01)
            {
                return true;
            }
            else
            {
                throw new Exceptions.InvalidParameterException("The payment amount must be at least £0.01. £" + PaymentAmount + " is too low.");
            }
        }
    }
}
