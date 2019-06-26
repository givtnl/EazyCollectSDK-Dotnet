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
