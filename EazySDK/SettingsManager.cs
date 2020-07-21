using System;
using System.Collections.Generic;
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

        public static IConfiguration CreateSettings(string filename)
        {
            if (System.String.IsNullOrEmpty(filename)) { filename = "appSettings.json"; }

            if (!File.Exists(Directory.GetCurrentDirectory() + "/" + filename))
            {
                SettingsWriter writer = new SettingsWriter();
            }

            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(filename, optional: false, reloadOnChange: true)
                    .Build();
                return configuration;
            }
            catch(FormatException)
            {
                throw new Exceptions.InvalidSettingsConfigurationException(
                    $"The selected settings file {filename} is not formatted correctly. You can either re-create it or default to appSettings.json by calling CreateSettings() without any overflows."    
                );
            }
            catch(FileNotFoundException)
            {
                throw new Exceptions.InvalidSettingsFileException(
                    $"The settings file {filename} does not exist. You can either create it or default to appSettings.json by calling CreateSettings() without any overflows."
                );
            }

            
        }

        public static IConfiguration CreateSettingsInMemory()
        {
            IConfiguration configuration = GetDefaultSettings();
            return configuration;
        }

        public static IConfiguration GetDefaultSettings()
        {
            var defaultOptions = new Dictionary<string, string>
            {
                { "currentEnvironment:Environment","ecm3" },

                { "sandboxClientDetails:ApiKey","" },
                { "sandboxClientDetails:ClientCode","" },

                { "ecm3ClientDetails:ApiKey","" },
                { "ecm3ClientDetails:ClientCode","" },

                { "directDebitProcessingDays:InitialProcessingDays","10" },
                { "directDebitProcessingDays:OngoingProcessingDays","5" },

                { "contracts:AutoFixStartDate","false" },
                { "contracts:AutoFixTerminationTypeAdHoc","false" },
                { "contracts:AutoFixAtTheEndAdHoc","false" },
                { "contracts:AutoFixPaymentDayInMonth","false" },
                { "contracts:AutoFixPaymentMonthInYear","false" },

                { "payments:AutoFixPaymentDate","false" },
                { "payments:IsCreditAllowed","false" },

                { "warnings:CustomerSearchWarning","false" },


                { "other:BankHolidayUpdateDays","30" },
                { "other:ForceUpdateSchedulesOnRun","false" },
            };
            IConfiguration Settings = new ConfigurationBuilder().AddInMemoryCollection(defaultOptions).Build();
            return Settings;
        }
    }
}
