using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace EazySDK.Utilities
{
    public class SchedulesReader
    {
        private static string WorkingDirectory = Directory.GetParent(Path.GetDirectoryName(Directory.GetCurrentDirectory() + @"\Includes")).FullName;
        private static string Sandbox_schedules_file = @"..\Includes\Sandbox.json";
        private static string Ecm3_schedules_file = @"..\Includes\Ecm3.json";
        private static string Schedules { get; set; }
        private static string Environment { get; set; }
        private static string ScheduleName { get; set; }
        private static bool ScheduleAdHoc { get; set; }
        private static int ScheduleFrequency { get; set; }
        private static DateTime date { get; set; }
        private static JObject SchedulesJson { get; set; }
        private static JToken SchedulesList { get; set; }

        /// <summary>
        /// Read the given schedules file and return a JSON object with the schedules data. If the file needs updating, send the JObject to the schedule writer.
        /// </summary>
        /// 
        /// <returns>schedules JObject</returns>
        public JObject ReadSchedulesFile(IConfiguration Settings)
        {
            Environment = Settings.GetSection("currentEnvironment")["Environment"].ToLower();

            if (!Directory.Exists(WorkingDirectory))
            {
                Directory.CreateDirectory(WorkingDirectory);
                File.Create(WorkingDirectory + @"/sandbox.json");
                File.Create(WorkingDirectory + @"/ecm3.json");
            }

            if (!File.Exists(WorkingDirectory + @"/" + Environment + ".json"))
            {
                File.Create(WorkingDirectory + @"/" + Environment + ".json");
            }

            try
            {
                using (StreamReader Reader = new StreamReader(WorkingDirectory + @"/" + Environment + ".json"))
                {
                    JObject json = JObject.Parse(Reader.ReadToEnd());

                    foreach (var i in json)
                    {
                        SchedulesList = json[i]["Schedules"];

                        foreach (var Schedule in SchedulesList)
                        {
                            ScheduleName = Schedule["Name"].ToString();
                            ScheduleFrequency = int.Parse(Schedule["Frequency"].ToString());

                            if (Schedule["Description"].ToString().Contains("AD-HOC Payments"))
                            {
                                ScheduleAdHoc = false;
                            }
                            else
                            {
                                ScheduleAdHoc = true;
                            }
                            SchedulesJson["Schedule"]["Name"] = ScheduleName;
                            SchedulesJson["Schedule"]["AdHoc"] = ScheduleAdHoc;
                            SchedulesJson["Schedule"]["Frequency"] = ScheduleFrequency;
                        }
                    }
                    return SchedulesJson;
                }
            }
            catch
            {
                Get Get = new Get(Settings);
                Schedules = Get.Schedules();
                JObject json = JObject.Parse(Schedules);
                
                foreach (var i in json["Services"])
                {
                    SchedulesList = json[i]["Schedules"];

                    foreach (var Schedule in SchedulesList)
                    {
                        ScheduleName = Schedule["Name"].ToString();
                        ScheduleFrequency = int.Parse(Schedule["Frequency"].ToString());

                        if (Schedule["Description"].ToString().Contains("AD-HOC Payments"))
                        {
                            ScheduleAdHoc = false;
                        }
                        else
                        {
                            ScheduleAdHoc = true;
                        }

                        SchedulesJson["Schedule"]["Name"] = ScheduleName;
                        SchedulesJson["Schedule"]["AdHoc"] = ScheduleAdHoc;
                        SchedulesJson["Schedule"]["Frequency"] = ScheduleFrequency;
                    }
                }
                return SchedulesJson;
            }
        }
    }
}