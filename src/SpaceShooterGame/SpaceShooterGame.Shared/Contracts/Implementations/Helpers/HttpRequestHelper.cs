using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public class HttpRequestHelper : IHttpRequestHelper
    {
        private readonly IHttpService _httpService;

        public HttpRequestHelper(IHttpService httpService)
        {
            _httpService = httpService;
        }

        #region Methods

        #region Public Methods

        public async Task<(TResponse SuccessResponse, TErrorResponse ErrorResponse, HttpStatusCode StatusCode)> SendRequest<TResponse, TErrorResponse>(
            string baseUrl,
            string path,
            Dictionary<string, string> httpHeaders,
            HttpMethod httpMethod,
            string contentType = "application/json",
            object payload = null,
            IEnumerable<KeyValuePair<string, string>> formUrlEncodedContent = null)
        {
            dynamic response = null;
            dynamic errResponse = null;

            HttpStatusCode statusCode = HttpStatusCode.OK;

            var uri = new Uri(baseUrl.Trim('/') + path + BuildQueryString(httpMethod, payload));

            try
            {
                using (var httpRequestMessage = new HttpRequestMessage(httpMethod, uri))
                {
                    foreach (var header in httpHeaders)
                    {
                        httpRequestMessage.Headers.Add(header.Key, header.Value);
                    }

                    switch (contentType)
                    {
                        case "application/json":
                            {
                                if (httpMethod != HttpMethod.Get && payload is not null)
                                {
                                    var stringifiedPayload = JsonConvert.SerializeObject(payload);
                                    var bodyData = new StringContent(stringifiedPayload, Encoding.UTF8, contentType);
                                    httpRequestMessage.Content = bodyData;
                                }
                            }
                            break;
                        case "application/x-www-form-urlencoded":
                            {
                                if (httpMethod != HttpMethod.Get && formUrlEncodedContent is not null)
                                {
                                    var bodyData = new FormUrlEncodedContent(formUrlEncodedContent);
                                    httpRequestMessage.Content = bodyData;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    var httpResponseMessage = await _httpService.SendAsync(httpRequestMessage);

                    statusCode = httpResponseMessage.StatusCode;

                    var content = await httpResponseMessage.Content.ReadAsStringAsync();

#if DEBUG
                    Console.WriteLine($"RequestUri: {httpRequestMessage.RequestUri}\n " +
                        $"Payload = {(payload is null ? "NA" : JsonConvert.SerializeObject(payload, Formatting.Indented))}\n " +
                        $"StatusCode:{httpResponseMessage.StatusCode}\n " +
                        $"ReasonPhrase:{httpResponseMessage.ReasonPhrase}");
#endif

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response = ParseResponse<TResponse>(content);
                    }
                    else
                    {
                        errResponse = ParseResponse<TErrorResponse>(content);
                    }
                }

                return (response, errResponse, statusCode);
            }
            catch (
#if DEBUG
            Exception ex
#else
            Exception
#endif
            )
            {
#if DEBUG
                Console.WriteLine($"RequestUri: {uri.OriginalString}\n" + $" Exception:{ex.Message}");
#endif
                return (response, errResponse, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            return await _httpService.SendAsync(httpRequestMessage);
        }

        #endregion

        #region Private Methods

        private dynamic ParseResponse<TResponse>(string content)
        {
            dynamic response;
            var responseType = typeof(TResponse);

            if (IsPrimitiveType(responseType))
            {
                response = content;
#if DEBUG
                Console.WriteLine($"Response = {response}");
#endif
            }
            else
            {
                response = JsonConvert.DeserializeObject<TResponse>(content);
#if DEBUG
                Console.WriteLine($"Response = {JsonConvert.SerializeObject(response, Formatting.Indented)}");
#endif
            }

            return response;
        }

        private string BuildQueryString(HttpMethod httpMethod, object payload)
        {
            var queryString = string.Empty;

            if (httpMethod == HttpMethod.Get && payload is not null)
            {
                var parameters = from p in payload.GetType().GetProperties()
                                 where p.GetValue(payload, null) is not null
                                 select $"{p.Name}={WebUtility.UrlEncode(p.GetValue(payload, null).ToString())}";

                queryString = "?" + string.Join("&", parameters);
            }

            return queryString;
        }

        private bool IsPrimitiveType(Type listType)
        {
            return listType == typeof(string)
                            || listType == typeof(int)
                            || listType == typeof(long)
                            || listType == typeof(decimal)
                            || listType == typeof(double)
                            || listType == typeof(float)
                            || listType == typeof(DateTime);
        }

        #endregion

        #endregion
    }
}
