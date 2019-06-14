using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace EazySDK
{
    class HttpController
    {

        public HttpClient HttpClient()
        {
            HttpClient client = new HttpClient();
            return client;
        }
    }
}
