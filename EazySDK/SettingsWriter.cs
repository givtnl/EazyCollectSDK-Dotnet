using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;

namespace EazySDK
{
    public class SettingsWriter
    {
        private static string JsonArray { get; set; }
        private static JObject RootObject { get; set; }
        private static JObject _currentEnvironment { get; set; }
        private static JObject _sandboxClientDetails { get; set; }
        private static JObject _ecm3ClientDetails { get; set; }
        private static JObject _directDebitProcessingDays { get; set; }
        private static JObject _contracts { get; set; }
        private static JObject _payments { get; set; }
        private static JObject _warnings { get; set; }
        private static JObject _other { get; set; }

        public SettingsWriter()
        {
            List<Models.CurrentEnvironment> currentEnvironment = new List<Models.CurrentEnvironment>
            {
                new Models.CurrentEnvironment
                {
                    Environment = "sandbox",
                }
            };

            List<Models.SandboxClientDetails> sandboxClientDetails = new List<Models.SandboxClientDetails>
            {
                new Models.SandboxClientDetails
                {
                    ApiKey = "",
                    ClientCode = ""
                }
            };

            List<Models.Ecm3ClientDetails> ecm3ClientDetails = new List<Models.Ecm3ClientDetails>
            {
                new Models.Ecm3ClientDetails
                {
                    ApiKey = "",
                    ClientCode = ""
                }
            };

            List<Models.DirectDebitProcessingDays> directDebitProcessingDays = new List<Models.DirectDebitProcessingDays>
            {
                new Models.DirectDebitProcessingDays
                {
                    InitialProcessingDays = 10,
                    OngoingProcessingDays = 5
                }
            };

            List<Models.Contracts> contracts = new List<Models.Contracts>
            {
                new Models.Contracts
                {
                    AutoFixStartDate = false,
                    AutoFixTerminationTypeAdHoc = false,
                    AutoFixAtTheEndAdHoc = false,
                    AutoFixPaymentDayInMonth = false,
                    AutoFixPaymentMonthInYear = false
                }
            };

            List<Models.Payments> list = new List<Models.Payments>
            {
                new Models.Payments
                {
                    AutoFixPaymentDate = false,
                    IsCreditAllowed = false
                }
            };
            List<Models.Payments> payments = list;

            List<Models.Warnings> warnings = new List<Models.Warnings>
            {
                new Models.Warnings
                {
                    CustomerSearchWarning = false
                }
            };

            List<Models.Other> other = new List<Models.Other>
            {
                new Models.Other
                {
                    BankHolidayUpdateDays = 30,
                    ForceUpdateSchedulesOnRun = false
                }
            };

            RootObject = new JObject();
            _currentEnvironment = new JObject();
            _sandboxClientDetails = new JObject();
            _ecm3ClientDetails = new JObject();
            _directDebitProcessingDays = new JObject();
            _contracts = new JObject();
            _payments = new JObject();
            _warnings = new JObject();
            _other = new JObject();

            _currentEnvironment.Add(new JProperty(nameof(Models.CurrentEnvironment.Environment), currentEnvironment[0].Environment.ToString()));

            _sandboxClientDetails.Add(new JProperty(nameof(Models.SandboxClientDetails.ApiKey), sandboxClientDetails[0].ApiKey.ToString()));
            _sandboxClientDetails.Add(new JProperty(nameof(Models.SandboxClientDetails.ClientCode), sandboxClientDetails[0].ClientCode.ToString()));

            _ecm3ClientDetails.Add(new JProperty(nameof(Models.Ecm3ClientDetails.ApiKey), ecm3ClientDetails[0].ApiKey.ToString()));
            _ecm3ClientDetails.Add(new JProperty(nameof(Models.Ecm3ClientDetails.ClientCode), ecm3ClientDetails[0].ClientCode.ToString()));

            _directDebitProcessingDays.Add(new JProperty(nameof(Models.DirectDebitProcessingDays.InitialProcessingDays), directDebitProcessingDays[0].InitialProcessingDays.ToString()));
            _directDebitProcessingDays.Add(new JProperty(nameof(Models.DirectDebitProcessingDays.OngoingProcessingDays), directDebitProcessingDays[0].OngoingProcessingDays.ToString()));

            _contracts.Add(new JProperty(nameof(Models.Contracts.AutoFixStartDate), contracts[0].AutoFixStartDate.ToString()));
            _contracts.Add(new JProperty(nameof(Models.Contracts.AutoFixTerminationTypeAdHoc), contracts[0].AutoFixTerminationTypeAdHoc.ToString()));
            _contracts.Add(new JProperty(nameof(Models.Contracts.AutoFixAtTheEndAdHoc), contracts[0].AutoFixAtTheEndAdHoc.ToString()));
            _contracts.Add(new JProperty(nameof(Models.Contracts.AutoFixPaymentDayInMonth), contracts[0].AutoFixPaymentDayInMonth.ToString()));
            _contracts.Add(new JProperty(nameof(Models.Contracts.AutoFixPaymentMonthInYear), contracts[0].AutoFixPaymentMonthInYear.ToString()));

            _payments.Add(new JProperty(nameof(Models.Payments.AutoFixPaymentDate), payments[0].AutoFixPaymentDate.ToString()));
            _payments.Add(new JProperty(nameof(Models.Payments.IsCreditAllowed), payments[0].IsCreditAllowed.ToString()));

            _warnings.Add(new JProperty(nameof(Models.Warnings.CustomerSearchWarning), warnings[0].CustomerSearchWarning.ToString()));

            _other.Add(new JProperty(nameof(Models.Other.BankHolidayUpdateDays), other[0].BankHolidayUpdateDays.ToString()));
            _other.Add(new JProperty(nameof(Models.Other.ForceUpdateSchedulesOnRun), other[0].ForceUpdateSchedulesOnRun.ToString()));

            RootObject.Add("currentEnvironment", _currentEnvironment);
            RootObject.Add("sandboxClientDetails", _sandboxClientDetails);
            RootObject.Add("ecm3ClientDetails", _ecm3ClientDetails);
            RootObject.Add("directDebitProcessingDays", _directDebitProcessingDays);
            RootObject.Add("contracts", _contracts);
            RootObject.Add("payments", _payments);
            RootObject.Add("warnings", _warnings);
            RootObject.Add("other", _other);


            File.WriteAllText(Directory.GetCurrentDirectory() + "/appSettings.json", RootObject.ToString());
        }

    }
}
