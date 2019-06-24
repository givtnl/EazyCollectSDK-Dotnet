using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EazySDK.Utilities
{
    public class WorkingDaysReader
    {
        private ClientHandler Handler { get; set; }
        private IConfiguration Settings { get; set; }
        private int UpdateDays { get; set; }
        private static DateTime date { get; set; }
        private static List<string> WorkingDaysList { get; set; }
        private static IEnumerable<string> FileLines { get; set; }
        private static DateTime LastUpdateDate { get; set; }


        /// <summary>
        /// Read the working days file and return a List object with the working days data. If the file needs updating, send the updating or it doesn't exist, write the file..
        /// </summary>
        /// 
        /// <returns>bank holidays list</returns>
        public List<string> ReadWorkingDaysFile()
        {
            Handler = new ClientHandler();
            Settings = Handler.Settings();
            UpdateDays = int.Parse(Settings.GetSection("other")["BankHolidayUpdateDays"]);

            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"..\..\Includes") || (!File.Exists(Directory.GetCurrentDirectory() + @"..\..\Includes\bankholidays.json")))
            {
                WorkingDaysWriter writer = new WorkingDaysWriter();
                WorkingDaysList = writer.WorkingDayWriter();
                return WorkingDaysList;
            }
            else
            {
                WorkingDaysList = new List<string>();
                FileLines = File.ReadLines(Directory.GetCurrentDirectory() + @"..\..\Includes\bankholidays.json");

                LastUpdateDate = DateTime.Parse(FileLines.First());

                if ((DateTime.UtcNow.Date - LastUpdateDate).TotalDays >= UpdateDays) {
                    WorkingDaysWriter writer = new WorkingDaysWriter();
                    WorkingDaysList = writer.WorkingDayWriter();
                    return WorkingDaysList;
                }
                else
                {
                    foreach (string Line in FileLines)
                    {
                        WorkingDaysList.Add(Line);
                    }
                    WorkingDaysList.RemoveAt(0);
                    return WorkingDaysList;
                }
            }
        }
    }
}