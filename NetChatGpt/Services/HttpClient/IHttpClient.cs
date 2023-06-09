using NetChatGptCLient.Models.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.HttpClient
{
    internal interface IHttpClient
    {
        Uri? BaseUrl { get; }

        Task<IHttpResponse?> GetAsync(string actionUrl,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null);
        Task<IHttpResponse?> PostAsync(string actionUrl,
            IReadOnlyDictionary<string, string>? requestBody = null,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null);
        Task<IHttpResponse?> PostJsonAsync(string actionUrl, object requestBody,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null);

        Task<IHttpResponse?> PatchAsync(string actionUrl,
            IReadOnlyDictionary<string, string>? requestBody = null,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null);
        Task<IHttpResponse?> PatchJsonAsync(string actionUrl, object requestBody,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null);
    }
}
