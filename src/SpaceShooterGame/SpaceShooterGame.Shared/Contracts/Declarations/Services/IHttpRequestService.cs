using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public interface IHttpRequestService
    {
        Task<(TResponse SuccessResponse, TErrorResponse ErrorResponse, HttpStatusCode StatusCode)> SendRequest<TResponse, TErrorResponse>(
            string baseUrl,
            string path,
            Dictionary<string, string> httpHeaders,
            HttpMethod httpMethod,
            string contentType = "application/json",
            object payload = null,
            IEnumerable<KeyValuePair<string, string>> formUrlEncodedContent = null);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
    }
}
