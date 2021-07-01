using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EazySDK.Utilities
{
    public class ContractPostChecks
    {
        /// <summary>
        /// Read a file containing available schedule names and return an error if the given schedule could not be found
        /// </summary>
        /// 
        /// 
        /// <param name="ScheduleName">A schedule name provided by an external function</param>
        /// <param name="Settings">Settings configuration used for making a call to EazyCustomerManager</param>
        /// 
        /// <example>
        /// CheckScheduleNameIsAvailable("TestSchedule")
        /// </example>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckScheduleNameIsAvailable(string ScheduleName, IConfiguration Settings)
        {
            SchedulesReader reader = new SchedulesReader();
            JObject RootJson = reader.ReadSchedulesFile(Settings);
            JObject SchedulesJson = RootJson["Schedules"].ToObject<JObject>();
            List<string> ScheduleNamesList = new List<string>();

            foreach (JProperty property in SchedulesJson.Properties())
            {
                ScheduleNamesList.Add(property.Name.ToLower());
            }

            if (ScheduleNamesList.Contains(ScheduleName, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid schedule name. The available schedule names are: {1}", ScheduleName, string.Join(", ", ScheduleNamesList)));
            }
        }

        /// <summary>
        /// Check that the TerminationType provided is valid, and return an integer to be used by other functions
        /// </summary>
        /// 
        /// <param name="TerminationType"> A TerminationType provided by an external function</param>
        /// 
        /// <returns>
        /// int(0-2)
        /// </returns>
        public int CheckTerminationTypeIsValid(string TerminationType)
        {
            Dictionary<string, int> ValidTerminationTypes = new Dictionary<string, int>
            {
                { "take certain number of debits", 0 },
                { "until further notice", 1 },
                { "end on exact date", 2 }
            };

            if (!ValidTerminationTypes.ContainsKey(TerminationType.ToLower()))
            {
                throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid TerminationType. The available TerminationTypes are: {1}", TerminationType, string.Join(", ", ValidTerminationTypes.Keys)));
            }
            else
            {
                return ValidTerminationTypes[TerminationType.ToLower()];
            }
        }

        /// <summary>
        /// Check that the AtTheEnd provided is valid, and return an integer to be used by other functions
        /// </summary>
        /// 
        /// <param name="AtTheEnd"> A TerminationType provided by an external function</param>
        /// 
        /// <returns>
        /// int(0-1)
        /// </returns>
        public int CheckAtTheEndIsValid(string AtThEnd)
        {
            Dictionary<string, int> ValidAtTheEnds = new Dictionary<string, int>
            {
                { "expire", 0 },
                { "switch to further notice", 1 },
            };

            if (!ValidAtTheEnds.ContainsKey(AtThEnd.ToLower()))
            {
                throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid AtTheEnd. The available AtTheEnds are: {1}", AtThEnd, string.Join(", ", ValidAtTheEnds.Keys)));
            }
            else
            {
                return ValidAtTheEnds[AtThEnd.ToLower()];
            }
        }

        /// <summary>
        /// Check that the PaymentDayInWeek provided is valid, and return a bool if it is.
        /// </summary>
        /// 
        /// <param name="PaymentDayInWeek"> A PaymentDayInWeek provided by an external function</param>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckPaymentDayInWeekIsValid(string PaymentDayInWeek)
        {
            List<string> ValidPaymentDayInWeeks = new List<string>
            {
                { "1" },
                { "2" },
                { "3" },
                { "4" },
                { "5" }
            };

            if (!ValidPaymentDayInWeeks.Contains(PaymentDayInWeek.ToLower()))
            {
                throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid PaymentDayInWeek. The available PaymentDayInWeeks are: Monday, Tuesday, Wednesday, Thursday and Friday.", PaymentDayInWeek));
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check that the PaymentDayInMonth provided is valid, and return a bool if it is.
        /// </summary>
        /// 
        /// <param name="PaymentDayInMonth"> A PaymentDayInMonth provided by an external function</param>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckPaymentDayInMonthIsValid(string PaymentDayInMonth)
        {
            try
            {
                if (int.Parse(PaymentDayInMonth) >= 1 && int.Parse(PaymentDayInMonth) <= 28)
                {
                    return true;
                }
                else
                {
                    throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid PaymentDayInMonth. The PaymentDayInMonth must be between 1 and 28 or be set to 'last day of month'", PaymentDayInMonth));
                }
            }
            catch (FormatException)
            {
                if (PaymentDayInMonth.ToLower() == "last day of month")
                {
                    return true;
                } else
                {
                    throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid PaymentDayInMonth. The PaymentDayInMonth must be between 1 and 28 or be set to 'last day of month'", PaymentDayInMonth));
                }
            }
        }

        /// <summary>
        /// Check that the PaymentMonthInYear provided is valid, and return a bool if it is.
        /// </summary>
        /// 
        /// <param name="PaymentMonthInYear"> A PaymentMonthInYear provided by an external function</param>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckPaymentMonthInYearIsValid(string PaymentMonthInYear)
        {
            try
            {
                if (int.Parse(PaymentMonthInYear) >= 1 && int.Parse(PaymentMonthInYear) <= 12)
                {
                    return true;
                }
                else
                {
                    throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid PaymentMonthInYear. The PaymentMonthInYear must be between 1 and 12.", PaymentMonthInYear));
                }
            }
            catch
            {
                throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid PaymentMonthInYear. The PaymentMonthInYear must be between 1 and 12.", PaymentMonthInYear));
            }
        }

        /// <summary>
        /// Check that the NumberOfDebits provided is valid, and return a bool if it is.
        /// </summary>
        /// 
        /// <param name="NumberOfDebits"> A NumberOfDebits provided by an external function</param>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckNumberOfDebitsIsValid(string NumberOfDebits)
        {
            try
            {
                if (int.Parse(NumberOfDebits) >= 1 && int.Parse(NumberOfDebits) <= 99)
                {
                    return true;
                }
                else
                {
                    throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid NumberOfDebits. The NumberOfDebits must be between 1 and 99.", NumberOfDebits));
                }
            }
            catch
            {
                throw new Exceptions.InvalidParameterException(string.Format("{0} is not a valid NumberOfDebits. The NumberOfDebits must be between 1 and 99.", NumberOfDebits));
            }
        }


        /// <summary>
        /// Check that the TerminationDate provided is valid, and is later than the StartDate.
        /// </summary>
        /// 
        /// <param name="TerminationDate"> A TerminationDate provided by an external function</param>
        /// <param name="StartDate"> A StartDate provided by an external function</param>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckTerminationDateIsAfterStartDate(string TerminationDate, string StartDate)
        {
            DateTime TermDate = new DateTime();
            DateTime SDate = new DateTime();
            try
            {
                TermDate = DateTime.Parse(TerminationDate);
                SDate = DateTime.Parse(StartDate);
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidParameterException("Either the termination date or the start date are not valid ISO dates.", e);
            }

            if (TermDate <= SDate)
            {
                throw new Exceptions.InvalidParameterException(string.Format("The Termination date of {0} is too early. It must be after the date {1}", TerminationDate, StartDate));
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check whether or not the schedule is Ad-Hoc, and return true if it is.
        /// </summary>
        /// 
        /// <param name="ScheduleName"> A ScheduleName provided by an external function</param>
        /// <param name="Settings">An IConfiguration provided by an external function</param>
        /// 
        /// <returns>
        /// bool
        /// </returns>
        public bool CheckScheduleAdHocStatus(string ScheduleName, IConfiguration Settings)
        {
            SchedulesReader reader = new SchedulesReader();
            JObject RootJson = reader.ReadSchedulesFile(Settings);
            JObject SchedulesJson = RootJson["Schedules"].ToObject<JObject>();
            JToken Schedule = SchedulesJson.GetValue(ScheduleName, StringComparison.CurrentCultureIgnoreCase);
            bool AdHoc = new bool();

            try
            {
                AdHoc = bool.Parse(Schedule["ScheduleAdHoc"].ToString());
            }
            catch (NullReferenceException)
            {
                throw new Exceptions.InvalidParameterException("The schedule {0} does not exist for the given client", ScheduleName);
            }

            if (AdHoc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check that the StartDate provided is valid, and return a string.
        /// </summary>
        /// 
        /// <param name="StartDate"> A start date provided by an external function</param>
        /// 
        /// <returns>
        /// string
        /// </returns>
        public string CheckStartDateIsValid(string StartDate, IConfiguration Settings)
        {
            DateTime InitialDate = new DateTime();
            int InitialProcessingDays = new int();
            bool AutoFixStartDate = new bool();

            try
            {
                InitialDate = DateTime.Parse(StartDate);
            }
            catch (FormatException)
            {
                throw new Exceptions.InvalidStartDateException(string.Format("The start date {0} is not valid. Please re-submit in ISO format (yyyy-mm-dd)", StartDate));
            }

            try
            {
                InitialProcessingDays = int.Parse(Settings.GetSection("directDebitProcessingDays")["InitialProcessingDays"]);
            }
            catch (FormatException)
            {
                string InvalidIPD = Settings.GetSection("directDebitProcessingDays")["InitialProcessingDays"];
                throw new Exceptions.InvalidSettingsConfigurationException("The InitialProcessingDays setting is misconfigured. It was expecting an integer, and received '{0}' instead", InvalidIPD);
            }

            try
            {
                AutoFixStartDate = bool.Parse(Settings.GetSection("contracts")["AutoFixStartDate"]);
            }
            catch (FormatException)
            {
                string InvalidAFSD = Settings.GetSection("contracts")["AutoFixStartDate"];
                throw new Exceptions.InvalidSettingsConfigurationException("The AutoFixStartDate setting is misconfigured. It was expecting a boolean, and received '{0}' instead", InvalidAFSD);
            }

            CheckWorkingDays WorkingDays = new CheckWorkingDays();
            DateTime FirstAvailableDate = WorkingDays.CheckWorkingDaysInFuture(InitialProcessingDays);
            

            if (InitialDate < FirstAvailableDate)
            {
                if (AutoFixStartDate)
                {
                    return FirstAvailableDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    throw new Exceptions.InvalidStartDateException(string.Format("The start date of {0} is too soon. The earliest possible start date for this contract is {1}", StartDate, FirstAvailableDate.ToString("yyyy-MM-dd")));
                }
            }
            else
            {
                return InitialDate.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Check that the Frequency provided is valid, and return a string.
        /// </summary>
        /// 
        /// <param name="ScheduleName"> A schedule name provided by an external function</param>
        /// 
        /// <returns>
        /// int (-1,2)
        /// </returns>
        public int CheckFrequency(string ScheduleName, IConfiguration Settings)
        {
            SchedulesReader Reader = new SchedulesReader();
            JObject SchedulesList = Reader.ReadSchedulesFile(Settings);
            string Frequency = "";

            try
            {
                JToken ScheduleToken = SchedulesList.SelectToken("Schedules." + ScheduleName);
                Frequency = ScheduleToken.Value<string>("ScheduleFrequency");
            }
            catch (Newtonsoft.Json.JsonException)
            {
                throw new Exceptions.InvalidIOConfiguration("The schedule {0} could not be found.", ScheduleName);
            }
            
            if (Frequency == "Weekly")
            {
                return 0;
            }
            else if (Frequency == "Monthly")
            {
                return 1;
            }
            else if (Frequency == "Annually")
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
    }
}
