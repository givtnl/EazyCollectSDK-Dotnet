using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class Payments
    {
        private readonly IConfiguration _configuration;
        private bool AutoFixPaymentDate { get; set; }
        private bool IsCreditAllowed { get; set; }

        public Payments(IConfiguration config)
        {
            _configuration = config;
        }

        public bool GetAutoFixPaymentDate()
        {
            AutoFixPaymentDate = bool.Parse(_configuration.GetSection("payments")["AutoFixPaymentDate"]);
            return AutoFixPaymentDate;
        }

        public bool GetClientCode()
        {
            IsCreditAllowed = bool.Parse(_configuration.GetSection("payments")["IsCreditAllowed"]);
            return IsCreditAllowed;
        }
    }
}

