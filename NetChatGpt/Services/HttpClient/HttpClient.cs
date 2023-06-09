using NetChatGptCLient.Models.HttpClient;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        /// <summary>
        /// Basic IHttpResponse realization
        /// </summary>
        private class RestHttpResponse : IHttpResponse
        {
            private readonly RestResponse _restResponse;

            public RestHttpResponse(RestResponse restResponse)
            {
                _restResponse = restResponse;
            }

            public int StatusCode => (int)_restResponse.StatusCode;
            public bool IsSuccess => _restResponse.IsSuccessful;
            public bool HasError => !IsSuccess;

            public Uri? RequestUrl => _restResponse.ResponseUri;
            public string? Content => _restResponse.Content;

            public IReadOnlyDictionary<string, string?>? Headers => _restResponse
                .Headers
                ?.ToDictionary(param => param.Name!, param => param.Value?.ToString());
            public string? ContentType => _restResponse.ContentType;

            public IReadOnlyDictionary<string, string>? Cookies => _restResponse
                .Cookies
                ?.ToDictionary(cookie => cookie.Name, cookie => cookie.Value);
            public bool IsEmptyCookies => (Cookies == null) || (Cookies.Count == 0);
        }

        /// <summary>
        /// The pnly IRestClient
        /// </summary>
        private readonly IRestClient _client;

        public Uri? BaseUrl 
        {
            get => _client.Options.BaseUrl;
        }

        public HttpClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public HttpClient(string baseUrl, string apiToken)
        {
            var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                apiToken, "Bearer"
            );

            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = authenticator
            };

            _client = new RestClient(options);
        }

        private RestRequest MakeRequest(string action,
            IReadOnlyDictionary<string, string>? customParams = null,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null)
        {
            var request = new RestRequest(action);

            if (customParams != null)
            {
                foreach (var param in customParams)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            if (queryParams != null)
            {
                foreach (var queryParam in queryParams)
                {
                    request.AddQueryParameter(queryParam.Key, queryParam.Value);
                }
            }

            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            return request;
        }

        public async Task<IHttpResponse?> GetAsync(string actionUrl,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null)
        {
            var request = MakeRequest(actionUrl,
                queryParams: queryParams,
                customHeaders: customHeaders);
            request.Method = Method.Get;

            var response = await _client.ExecuteAsync(request);
            return response != null
                ? new RestHttpResponse(response)
                : null;
        }

        public async Task<IHttpResponse?> PatchAsync(string actionUrl,
            IReadOnlyDictionary<string, string>? requestBody = null,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null)
        {
            var request = MakeRequest(actionUrl,
                customParams: requestBody,
                queryParams: queryParams,
                customHeaders: customHeaders);
            request.Method = Method.Patch;

            var response = await _client.ExecuteAsync(request);
            return response != null
                ? new RestHttpResponse(response)
                : null;
        }

        public async Task<IHttpResponse?> PatchJsonAsync(string actionUrl, object requestBody,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null)
        {
            var request = MakeRequest(actionUrl,
                queryParams: queryParams,
                customHeaders: customHeaders);
            request.Method = Method.Patch;

            if ((customHeaders != null) && (customHeaders.ContainsKey("Content-Type")))
            {
                request.AddJsonBody(requestBody, customHeaders["Content-Type"]);
            }
            else
            {
                request.AddJsonBody(requestBody);
            }

            var response = await _client.ExecuteAsync(request);
            return response != null
                ? new RestHttpResponse(response)
                : null;
        }

        public async Task<IHttpResponse?> PostAsync(string actionUrl,
            IReadOnlyDictionary<string, string>? requestBody = null,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null)
        {
            var request = MakeRequest(actionUrl,
                customParams: requestBody,
                queryParams: queryParams,
                customHeaders: customHeaders);
            request.Method = Method.Post;

            var response = await _client.ExecuteAsync(request);
            return response != null
                ? new RestHttpResponse(response)
                : null;
        }

        public async Task<IHttpResponse?> PostJsonAsync(string actionUrl, object requestBody,
            IReadOnlyDictionary<string, string>? queryParams = null,
            IReadOnlyDictionary<string, string>? customHeaders = null)
        {
            var request = MakeRequest(actionUrl,
                queryParams: queryParams,
                customHeaders: customHeaders);
            request.Method = Method.Post;

            if ((customHeaders != null) && (customHeaders.ContainsKey("Content-Type")))
            {
                request.AddJsonBody(requestBody, customHeaders["Content-Type"]);
            }
            else
            {
                request.AddJsonBody(requestBody);
            }

            var response = await _client.ExecuteAsync(request);
            return response != null
                ? new RestHttpResponse(response)
                : null;
        }
    }
}
