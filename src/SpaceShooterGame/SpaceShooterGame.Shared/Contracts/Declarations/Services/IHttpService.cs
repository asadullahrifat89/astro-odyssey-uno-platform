using System.Net.Http;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);

        Task<Response> SendAsync<Response>(
            HttpMethod httpMethod,
            string baseUri,
            string actionUri,
            object payload = null,
            string accessToken = null) where Response : class;

        Task<Response> SendAsync<Response>(HttpRequestMessage httpRequestMessage) where Response : class;
    }
}
