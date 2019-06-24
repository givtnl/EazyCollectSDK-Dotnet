using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK.Utilities
{
    public class PaymentPostChecks
    {
        /// <summary>
        /// Check that the collection_date argument is a valid ISO date and is at least x working days in the future, where x is the pre-determined OngoingProcessingDays setting.
        /// Throw an error if this is not the case.
        /// </summary>
        /// 
        /// <param name="CollectionDate">A colelction date provided by an external function</param>
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
            DateTime InitialDate = DateTime.Parse(CollectionDate);
            int OngoingProcessingDays = int.Parse(Settings.GetSection("directDebitProcessingDays")["OngoingProcessingDays"]);
            CheckWorkingDays WorkingDays = new CheckWorkingDays();

            DateTime FirstAvailableDate = WorkingDays.CheckWorkingDaysInFuture(OngoingProcessingDays);
            bool AutoFixStartDate = bool.Parse(Settings.GetSection("contracts")["AutoFixStartDate"]);

            if (InitialDate < FirstAvailableDate)
            {
                if (AutoFixStartDate)
                {
                    return FirstAvailableDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    throw new Exceptions.InvalidStartDateException(string.Format("The payment date of {0} is too soon. The earliest possible payment date is {1}", CollectionDate, FirstAvailableDate.ToString("yyyy-MM-dd")));
                }
            }
            else
            {
                return CollectionDate;
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
            bool IsCreditAllowed = bool.Parse(Settings.GetSection("payments")["IsCreditAllowed"]);
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
            if (float.Parse(PaymentAmount) >= 0.01)
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
