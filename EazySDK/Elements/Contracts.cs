using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class Contracts
    {
        private readonly IConfiguration _configuration;
        private bool AutoFixStartDate { get; set; }
        private bool AutoFixTerminationTypeAdHoc { get; set; }
        private bool AutoFixAtTheEndAdHoc { get; set; }
        private bool AutoFixPaymentDayInMonth { get; set; }
        private bool AutoFixPaymentMonthInYear { get; set; }

        public Contracts(IConfiguration config)
        {
            _configuration = config;
        }

        public bool GetAutoFixStartDate()
        {
            AutoFixStartDate = bool.Parse(_configuration.GetSection("contracts")["AutoFixStartDate"]);
            return AutoFixStartDate;
        }

        public bool GetAutoFixTerminationTypeAdHoc()
        {
            AutoFixTerminationTypeAdHoc = bool.Parse(_configuration.GetSection("contracts")["AutoFixTerminationTypeAdHoc"]);
            return AutoFixTerminationTypeAdHoc;
        }

        public bool GetAutoFixAtTheEndAdHoc()
        {
            {
                AutoFixAtTheEndAdHoc = bool.Parse(_configuration.GetSection("contracts")["AutoFixAtTheEndAdHoc"]);
                return AutoFixAtTheEndAdHoc;
            }
        }

        public bool GetAutoFixPaymentDayInMonth()
        {
            {
                AutoFixPaymentDayInMonth = bool.Parse(_configuration.GetSection("contracts")["AutoFixPaymentDayInMonth"]);
                return AutoFixPaymentDayInMonth;
            }
        }


        public bool GetAutoFixPaymentMonthInYear()
        {
            {
                AutoFixPaymentMonthInYear = bool.Parse(_configuration.GetSection("contracts")["AutoFixPaymentMonthInYear"]);
                return AutoFixPaymentMonthInYear;
            }
        }

    }
}

