using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EazySDK.Utilities
{
    public class WorkingDaysWriter
    {
        private static HttpRequestMessage RequestMessage { get; set; }
        private static HttpResponseMessage ResponseMessage { get; set; }
        private static Stream ResponseStream { get; set; }
        private static HttpClient HttpClient { get; set; }
        private static string RequestUri { get; set; }
        private static string ResponseAsString { get; set; }
        private static StreamReader Reader { get; set; }
        private static JObject ResponseAsJson { get; set; }
        private static List<string> DatesList { get; set; }

        /// <summary>
        /// Get the UK bank holidays from the Gov.UK API, write the result to a file and return a list of bank holidays
        /// </summary>
        /// 
        /// <returns>
        /// List of JSON dates
        /// </returns>
        public List<string> WorkingDayWriter()
        {
            RequestUri = @"https://www.gov.uk/bank-holidays.json";

            RequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(RequestUri),
                Method = new HttpMethod("GET")
            };

            HttpClient = new HttpClient();
            ResponseMessage = HttpClient.SendAsync(RequestMessage).Result;

            ResponseStream = ResponseMessage.Content.ReadAsStreamAsync().Result;

            Reader = new StreamReader(ResponseStream);

            ResponseAsString = Reader.ReadToEnd();
            ResponseAsJson = JObject.Parse(ResponseAsString);
            DatesList = new List<string>();

            foreach (JToken i in ResponseAsJson["england-and-wales"]["events"])
            {
                if (DateTime.Parse(i["date"].ToString()).Year >= DateTime.Now.Year)
                {
                    DatesList.Add(i["date"].ToString());
                }
            }
            // We will use the first line as a time check when attempting to read the file
            DatesList.Insert(0, DateTime.Today.Date.ToString("yyyy-MM-dd"));
            File.WriteAllText(Directory.GetCurrentDirectory() + @"..\..\Includes\bankholidays.json", string.Join("\n", DatesList.ToArray()));
            return DatesList;
        }
    }
}
