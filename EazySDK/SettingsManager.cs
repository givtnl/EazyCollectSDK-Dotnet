using System.IO;
using Microsoft.Extensions.Configuration;


namespace EazySDK
{
    public class SettingsManager 
    {
        public static IConfiguration CreateSettings()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/appSettings.json"))
            {
                SettingsWriter writer = new SettingsWriter();
            }


            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .Build();
            return configuration;
        }

    }
}
