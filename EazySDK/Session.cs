using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace EazySDK
{
    /// <summary>
    /// The Session class contains the core logic of sending HTTP requests to EazyCustomerManager
    /// </summary>
    public class Session
    {
        #region Client objects
        // The settings object, derives from ClientHandler
        private static IConfiguration Settings { get; set; }

        #endregion Client objects

        #region HTTP objects
        // HttpController returns a HTTP client
        private static HttpController HttpController { get; set; }
        // This HttpClient will inherit from HttpController
        private static HttpClient HttpClient { get; set; }
        // The HTTP method must be encoded, and we will use a HttpMethod object to achieve this
        private static HttpMethod HttpEncodedMethod { get; set; }
        // We will use the ParseQueryString() function from HttpUtility to build our query strings
        private static HttpUtility HttpUtility { get; set; }
        // We use HttpRequestMessage to build the request to be sent to EazyCustomerManager
        private static HttpRequestMessage RequestMessage { get; set; }
        #endregion HTTP objects

        #region String objects
        // We use the StreamReader to read the results of the HttpRequestMessage
        private static StreamReader Reader { get; set; }
        // The environment used when communicating with EazyCustomerManager
        private static string Environment { get; set; }
        // The ApiKey passed into the headers of requests sent to EazyCustomerManager
        private static string ApiKey { get; set; }
        // The client code of the client sending requests to EazyCustomerManager
        private static string ClientCode { get; set; }
        // The base Uri of requests sent to EazyCustomerManager. This relies on the Environment and the Client Code
        private static string BaseUri { get; set; }
        // The target Uri endpoint of requests sent to ECM3
        private static string UriEndpoint { get; set; }
        // The HttpMethod of requests sent to ECM3. This will be passed into HttpEncodedMethod.
        private static string HttpMethod { get; set; }
        // The full request Uri to be sent to EazyCustomerManager
        private static string RequestUri { get; set; }
        // The response string to be returns from the request sent to EazyCustomerManager
        private static string ResponseAsText { get; set; }
        #endregion String objects

        #region Dictionary objects
        // The parameters to be sent to EazyCustomerManager along with the request
        private static Dictionary<string, string> Parameters { get; set; }
        #endregion

        /// <summary>
        /// Instantiate the Session object
        /// </summary>
        /// <param name="_Settings">_Settings is called from ClientHandler as a static entity</param>
        public Session(IConfiguration _Settings)
        {
            // Reference the HttpController to get the HttpClient object
            HttpController = new HttpController();
            HttpClient = HttpController.HttpClient();

            // Get the Settings from the ClientHandler
            Settings = _Settings;
            try
            {
                // Get the current value of the Environment
                Environment = Settings.GetSection("currentEnvironment")["Environment"];
            }
            catch (FormatException)
            {
                throw new Exceptions.InvalidEnvironmentException("The environment setting could not be read. Please ensure this is formatted as a string.");
            }
            
            // Ensure the Environment is valid
            HashSet<string> AcceptableEnvironments = new HashSet<string> { "playpen", "ecm3" };
            if (!AcceptableEnvironments.Contains(Environment.ToLower()))
            {
                throw new Exception(string.Format("{0} is not an acceptable environment.", Environment));
            }


            // If the environment is valid, get the client settings
            else if (Environment.ToLower() == "ecm3")
            {
                ApiKey = Settings.GetSection("ecm3ClientDetails")["ApiKey"];
                ClientCode = Settings.GetSection("ecm3ClientDetails")["ClientCode"];

            }
            else
            {
                ApiKey = Settings.GetSection("playpenClientDetails")["ApiKey"];
                ClientCode = Settings.GetSection("playpenClientDetails")["ClientCode"];
            }
            }

            /// <summary>
            /// Send a Request to ECM3. This function is never directly called, and is instead handled by the Get(), Post(), Patch() or Delete() functions
            /// </summary>
            /// <param name="_method">The HTTP method passed from a HTTP function</param>
            /// <param name="_endpoint">The URI endpoint passed from a HTTP function</param>
            /// <param name="_parameters">The parameters, if there are any, passed from a HTTP function</param>
            /// <returns></returns>
            public static string Request(string _method, string _endpoint, Dictionary<string, string> _parameters = null)
            {
                // The base Uri for all requests sent to EazyCustomerManager
                BaseUri = string.Format(@"https://{0}.eazycollect.co.uk/api/v3/client/{1}/", Environment, ClientCode);
                // The endpoint for the current request to be sent to EazyCustomerManager
                UriEndpoint = _endpoint;
                // The base Uri for the current request
                RequestUri = BaseUri + UriEndpoint;
                // The HTTP method for the current request
                HttpMethod = _method;
                // The parameters for the current request
                Parameters = _parameters;

                // Ensure the HTTP method is allowed by EazyCustomerManager
                HashSet<string> AcceptableHTTPMethods = new HashSet<string> { "POST", "PATCH", "DELETE", "GET" };
                if (!AcceptableHTTPMethods.Contains(HttpMethod))
                {
                    // Throw an exception if the HTTP method is disallowed
                    throw new Exception(string.Format("{0} isn't a HTTP method supported by EazySDK. The available methods are GET, POST, PATCH and DELETE.", HttpMethod.ToUpper()));
                }

                // Attempt to create the full request URL
                try
                {
                    // Get a Uri object from the request Uri
                    var uriBuilder = new UriBuilder(RequestUri);
                    // Get the query from the Uri object
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                    // Iterate over the Parameters and add them into the query object
                    foreach (KeyValuePair<string, string> param in Parameters)
                    {
                        query[param.Key] = param.Value;
                    }
                    // Update the Request Uri with the new query
                    uriBuilder.Query = query.ToString();
                    RequestUri = uriBuilder.ToString();
                }
                // If no parameters exist, continue
                catch (NullReferenceException)
                {
                }

                // Encode the HTTP method before sending it with the request to EazyCustomerManager
                HttpEncodedMethod = new HttpMethod(HttpMethod);

                // Create the HTTP message to be sent to EazyCustomerManager
                RequestMessage = new HttpRequestMessage {
                    // Instead of passing Content, we pass the query into the RequestUri
                    RequestUri = new Uri(RequestUri),
                    Method = HttpEncodedMethod
                };
                // Add the Headers to the request
                RequestMessage.Headers.Add("ApiKey", ApiKey);

                // Get the result of the request sent to EazyCustomerManager
                var RequestResponse = HttpClient.SendAsync(RequestMessage).Result;

                // Get a stream of the response for  easier reading
                var ResponseStream = RequestResponse.Content.ReadAsStreamAsync().Result;

                // Read the Response as a stream
                Reader = new StreamReader(ResponseStream);

                // Read, then return the entire response
                string ResponseAsString = Reader.ReadToEnd();
                return ResponseAsString;
            }
        
        /// <summary>
        /// Send a GET request to EazyCustomerManager
        /// </summary>
        public string Get(string _Endpoint, Dictionary<string, string> _Parameters = null)
        {
            return Request("GET", _Endpoint, _parameters: _Parameters);
        }

        /// <summary>
        /// Send a POST request to EazyCustomerManager
        /// </summary>
        public string Post(string _Endpoint, Dictionary<string, string> _Parameters = null)
        {
            return Request("POST", _Endpoint, _parameters: _Parameters);
        }

        /// <summary>
        /// Send a PATCH request to EazyCustomerManager
        /// </summary>
        public string Patch(string _Endpoint, Dictionary<string, string> _Parameters = null)
        {

            return Request("PATCH", _Endpoint, _parameters: _Parameters);
        }

        /// <summary>
        /// Send a DELETE request to EazyCustomerManager
        /// </summary>
        public string Delete(string _Endpoint, Dictionary<string, string> _Parameters = null)
        {

            return Request("DELETE", _Endpoint, _parameters: _Parameters);
        }

    }
}
