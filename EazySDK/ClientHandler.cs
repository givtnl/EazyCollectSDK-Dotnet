using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EazySDK
{
    public class ClientHandler
    {
        public IConfiguration Settings()
        {
            var GetSettings = SettingsManager.CreateSettings();
            return GetSettings;
        }

        public Session Session(IConfiguration Settings)
        {
            var x = new Session(Settings);
            return x;
        }
    }
}
