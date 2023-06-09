using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.HttpClient
{
    internal static class HttpClientFactory
    {
        public static IHttpClient CreateHttpClient(string baseUrl)
        {
            return new HttpClient(baseUrl);
        }

        public static IHttpClient CreateHttpClient(string baseUrl, string apiToken)
        {
            return new HttpClient(baseUrl, apiToken);
        }
    }
}
