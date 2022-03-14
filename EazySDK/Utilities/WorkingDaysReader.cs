using System;
using System.IO;
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

        public WorkingDaysReader(IConfiguration settings)
        {
            Settings = settings;
        }


        /// <summary>
        /// Read the working days file and return a List object with the working days data. If the file needs updating, send the updating or it doesn't exist, write the file..
        /// </summary>
        /// 
        /// <returns>bank holidays list</returns>
        public List<string> ReadWorkingDaysFile()
        {
            Handler = new ClientHandler();

            WorkingDaysWriter writer = new WorkingDaysWriter();
            WorkingDaysList = writer.WorkingDayWriter();
            return WorkingDaysList;
        }
    }
}