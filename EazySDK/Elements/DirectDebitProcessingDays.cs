using Microsoft.Extensions.Configuration;

namespace EazySDK.Elements
{
    public class DirectDebitProcessingDays
    {
        private readonly IConfiguration _configuration;
        private int InitialProcessingDays { get;  set; }
        private int OngoingProcessingDays { get;  set; }

        public DirectDebitProcessingDays(IConfiguration config)
        {
            _configuration = config;
        }

        public int GetInitialProcessingDays()
        {
            InitialProcessingDays = int.Parse(_configuration.GetSection("directDebitProcessingDays")["InitialProcessingDays"]);
            return InitialProcessingDays;
        }

        public int GetOngoingProcessingDays()
        {
            OngoingProcessingDays = int.Parse(_configuration.GetSection("directDebitProcessingDays")["OngoingProcessingDays"]);
            return OngoingProcessingDays;
        }
    }
}

