using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class Other
    {
        private readonly IConfiguration _configuration;
        private int BankHolidayUpdateDays { get; set; }
        private bool ForceUpdateSchedulesOnRun { get; set; }

        public Other(IConfiguration config)
        {
            _configuration = config;
        }

        public int GetBankHolidayUpdateDays()
        {
            BankHolidayUpdateDays = int.Parse(_configuration.GetSection("other")["BankHolidayUpdateDays"]);
            return BankHolidayUpdateDays;
        }

        public bool GetForceUpdateSchedulesOnRun()
        {
            ForceUpdateSchedulesOnRun = bool.Parse(_configuration.GetSection("other")["ForceUpdateSchedulesOnRun"]);
            return ForceUpdateSchedulesOnRun;
        }
    }
}

