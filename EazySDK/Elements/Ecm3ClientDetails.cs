using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class Ecm3ClientDetails
    {
        private readonly IConfiguration _configuration;
        private string ApiKey { get; set; }
        private string ClientCode { get; set; }

        public Ecm3ClientDetails(IConfiguration config)
        {
            _configuration = config;
        }

        public string GetApiKey()
        {
            ApiKey = _configuration.GetSection("ecm3ClientDetails")["ApiKey"];
            return ApiKey;
        }

        public string GetClientCode()
        {
            ClientCode = _configuration.GetSection("ecm3ClientDetails")["ClientCode"];
            return ClientCode;
        }
    }
}

