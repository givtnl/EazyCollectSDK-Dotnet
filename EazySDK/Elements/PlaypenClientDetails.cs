using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class PlaypenClientDetails
    {
        private readonly IConfiguration _configuration;
        private string ApiKey { get; set; }
        private string ClientCode { get; set; }

        public PlaypenClientDetails(IConfiguration config)
        {
            _configuration = config;
        }

        public string GetApiKey()
        {
            ApiKey = _configuration.GetSection("playpenClientDetails")["ApiKey"];
            return ApiKey;
        }

        public string GetClientCode()
        {
            ClientCode = _configuration.GetSection("playpenClientDetails")["ClientCode"];
            return ClientCode;
        }
    }
}

