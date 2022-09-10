using AstroOdysseyCore;
using Newtonsoft.Json;
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
        #region Fields

        private readonly IHttpRequestHelper _httpRequestHelper;

        #endregion

        #region Ctor

        public GameApiHelper(IHttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
        }

        #endregion

        #region Methods

        public T ParseResult<T>(object obj)
        {
            var result = JsonConvert.DeserializeObject<T>(obj.ToString(), new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return result;
        }

        public async Task<ServiceResponse> Authenticate(string userNameOrEmail, string password)
        {
            var response = await _httpRequestHelper.SendRequest<ServiceResponse, ServiceResponse>(
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
                : response.ErrorResponse;
        }

        public async Task<ServiceResponse> Signup(string userName, string email, string password)
        {
            var response = await _httpRequestHelper.SendRequest<ServiceResponse, ServiceResponse>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_SignUp,
                 httpHeaders: new Dictionary<string, string>(),
                 httpMethod: HttpMethod.Post,
                 payload: new
                 {
                     Email = email,
                     UserName = userName,
                     Password = password,
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse;
        }

        #endregion
    }
}
