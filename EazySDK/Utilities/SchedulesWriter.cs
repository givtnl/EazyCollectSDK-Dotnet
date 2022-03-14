using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace EazySDK.Utilities
{
    public class SchedulesWriter
    {
        private static string ScheduleString { get; set; }
        private static JObject SchedulesJson { get; set; }
        private JToken SchedulesToken { get; set; }
        private JToken ServicesListToken { get; set; }
        private JToken SchedulesListToken { get; set; }
        private Get Get { get; set; }
        private static JObject SchedulesObject { get; set; }
        private static JObject RootObject { get; set; }
        private static JObject LastUpdatedObject { get; set; }
        private static string Environment { get; set; }

        /// <summary>
        /// Get the available schedules from the Get().Schedules() command and return the schedules as a JSON object
        /// </summary>
        /// 
        /// <param name="Settings">
        /// A settings object passed on from the SchedulesReader 
        /// </param>
        /// 
        /// <returns>
        /// JSON schedules list
        /// </returns>
        public JObject ScheduleWriter(IConfiguration Settings)
        {
            // We will use Get to call Get.Schedules()
            Get = new Get(Settings);
            ScheduleString = Get.Schedules();

            // Parse SchedulesString into a Json object
            SchedulesJson = JObject.Parse(ScheduleString);
            // Get a token of the Services property of the SchedulesJson
            SchedulesToken = SchedulesJson["Services"];

            // Create the objects to format the JSON to be written
            RootObject = new JObject();
            SchedulesObject = new JObject();
            LastUpdatedObject = new JObject();

            // There can be multiple lists of services, though there is usually only one
            foreach (var ServicesListToken in SchedulesToken)
            {
                // Get the list of schedules within a given service
                foreach (var ScheduleObject in ServicesListToken["Schedules"])
                {
                    // Build a schedule object
                    Models.Schedules.ScheduleName = ScheduleObject["Name"].ToString();
                    if (ScheduleObject.ToString().Contains("AD-HOC Payments"))
                    {
                        Models.Schedules.ScheduleAdHoc = true;
                    }
                    else
                    {
                        Models.Schedules.ScheduleAdHoc = false;
                    }
                    Models.Schedules.ScheduleFrequency = ScheduleObject["Frequency"].ToString();

                    // We use a temp object to prepare the JObject. We could just add three entries to the Schedule object, but this ensures correct formatting
                    JObject _TempObject = new JObject();
                    _TempObject.Add(nameof(Models.Schedules.ScheduleAdHoc), Models.Schedules.ScheduleAdHoc);
                    _TempObject.Add(nameof(Models.Schedules.ScheduleFrequency), Models.Schedules.ScheduleFrequency);
                    SchedulesObject.Add(Models.Schedules.ScheduleName, _TempObject);
                } 
            }
       
            RootObject.Add("Schedules", SchedulesObject);
            RootObject.Add("LastUpdated", DateTime.Today.Date.ToString("yyyy-MM-dd"));
            return RootObject;
        }
    }
}
