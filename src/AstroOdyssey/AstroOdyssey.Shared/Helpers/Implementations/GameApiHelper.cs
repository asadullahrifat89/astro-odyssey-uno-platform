using AstroOdysseyCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public class GameApiHelper : IGameApiHelper
    {
        private readonly IHttpRequestHelper _httpRequestHelper;

        public GameApiHelper(IHttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
        }

        public async Task<ServiceResponse> Authenticate(string userNameOrEmail, string password)
        {
            var response = await _httpRequestHelper.SendRequest<ServiceResponse, string>(
                baseUrl: Constants.GAME_API_BASEURL,
                path: Constants.Action_Authenticate,
                httpHeaders: new Dictionary<string, string>(),
                httpMethod: HttpMethod.Post,
                payload: new
                {
                    UserName = userNameOrEmail,
                    Password = password,
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = response.ErrorResponse };
        }
    }
}
