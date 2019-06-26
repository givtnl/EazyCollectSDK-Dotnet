using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace EazySDK.Utilities
{
    public class SchedulesReader
    {
        private static string Schedules { get; set; }
        private static string Environment { get; set; }

        private static DateTime date { get; set; }
        private static JObject SchedulesJson { get; set; }
        private static JToken SchedulesList { get; set; }
        private static SchedulesWriter Writer { get; set; }

        /// <summary>
        /// Read the given schedules file and return a JSON object with the schedules data. If the file needs updating, send the JObject to the schedule writer.
        /// </summary>
        /// 
        /// <returns>schedules JObject</returns>
        public JObject ReadSchedulesFile(IConfiguration Settings)
        {
            Environment = Settings.GetSection("currentEnvironment")["Environment"].ToLower();

            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Includes") || (!File.Exists(Directory.GetCurrentDirectory() + @"\Includes\" + Environment + "scheduleslist.json")))
            {
                Writer = new SchedulesWriter();
                SchedulesJson = Writer.ScheduleWriter(Settings);
                return SchedulesJson;
            }
            else
            {
                try
                {
                    SchedulesJson = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + @"\Includes\" + Environment + "scheduleslist.json"));
                    return SchedulesJson;
                }
                catch
                {
                    Writer = new SchedulesWriter();
                    SchedulesJson = Writer.ScheduleWriter(Settings);
                    return SchedulesJson;
                }
            }
        }
    }
}