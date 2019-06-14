using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class SandBoxClientDetails
    {
        private readonly IConfiguration _configuration;
        private string ApiKey { get; set; }
        private string ClientCode { get; set; }

        public SandBoxClientDetails(IConfiguration config)
        {
            _configuration = config;
        }

        public string GetApiKey()
        {
            ApiKey = _configuration.GetSection("sandboxClientDetails")["ApiKey"];
            return ApiKey;
        }

        public string GetClientCode()
        {
            ClientCode = _configuration.GetSection("sandboxClientDetails")["ClientCode"];
            return ClientCode;
        }
    }
}

