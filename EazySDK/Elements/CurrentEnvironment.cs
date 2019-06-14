using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class CurrentEnvironment
    {
        private readonly IConfiguration _configuration;
        private string Environment { get; set; }

        public CurrentEnvironment(IConfiguration config)
        {
            _configuration = config;
        }

        public string GetEnvironment()
        {
            Environment = _configuration.GetSection("currentEnvironment")["Environment"];
            return Environment;
        }
    }
}

