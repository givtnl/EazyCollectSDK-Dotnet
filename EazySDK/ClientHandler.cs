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

        public string GenericExceptionCheck(string _result)
        {
            string result = _result;

            if (result.Contains("not supported"))
            {
                throw new Exceptions.UnsupportedHTTPMethodException(
                    string.Format("This is a generic error. This error can be caused by several events, including but not limited to:" +
                    "\n- The incorrect HTTP method is being used. For example, you cannot pass DELETE on a Customer object." +
                    "\n- The correct HTTP method is being used, but a mandatory parameter has been missed."));
            }
            else if (result.Contains("API not enabled"))
            {
                throw new Exceptions.SDKNotEnabledException(
                    string.Format("This is a generic error. This error can be caused by several events, including but not limited to:" +
                    "\n- The API key is either not valid or is not correct." +
                    "\n- The API key is correct and valid, but the incorrect client code is being used." +
                    "\n - The record you are attempting to access does not exist."));
            }
            else if (result.Contains("IIS 8.5 Detailed Error - 404.0 - Not Found"))
            {
                throw new Exceptions.ResourceNotFoundException(
                    string.Format("This is a generic error. This error can be caused by several events, including but not limited to:" +
                    "\n- You are searching against a record that does not exist." +
                    "\n- The record you are searching for does exist, but a mandatory parameter has been missed." +
                    "\n - The provided data is malformed."));
            }
            else
            {
                return result;
            }
        }
    }
}
