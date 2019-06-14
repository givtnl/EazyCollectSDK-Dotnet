using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class Warnings
    {
        private readonly IConfiguration _configuration;
        private bool CustomerSearchWarning { get; set; }

        public Warnings(IConfiguration config)
        {
            _configuration = config;
        }

        public bool GetCustomerSearchWarning()
        {
            CustomerSearchWarning = bool.Parse(_configuration.GetSection("warnings")["CustomerSearchWarning"]);
            return CustomerSearchWarning;
        }
    }
}

