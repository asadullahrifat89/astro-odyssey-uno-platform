using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.System;

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

        public async Task<ServiceResponse> SubmitGameScore(double score)
        {
            await RefreshAuthToken();

            var response = await _httpRequestHelper.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: Constants.GAME_API_BASEURL,
                path: Constants.Action_SubmitGameScore,
                httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {App.AuthToken.AccessToken}" } },
                httpMethod: HttpMethod.Post,
                payload: new
                {
                    User = new AttachedUser()
                    {
                        UserEmail = App.GameProfile.User.UserEmail,
                        UserName = App.GameProfile.User.UserName,
                        UserId = App.GameProfile.User.UserId,
                    },
                    Score = score,
                    GameId = Constants.GAME_ID,
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse;
        }

        public async Task<QueryRecordResponse<GameProfile>> GetGameProfile()
        {
            await RefreshAuthToken();

            var response = await _httpRequestHelper.SendRequest<QueryRecordResponse<GameProfile>, QueryRecordResponse<GameProfile>>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_GetGameProfile,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {App.AuthToken.AccessToken}" } },
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse
                : response.ErrorResponse;
        }

        public async Task<QueryRecordsResponse<GameProfile>> GetGameProfiles(
            int pageIndex,
            int pageSize)
        {
            await RefreshAuthToken();

            var response = await _httpRequestHelper.SendRequest<QueryRecordsResponse<GameProfile>, QueryRecordsResponse<GameProfile>>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_GetGameProfiles,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {App.AuthToken.AccessToken}" } },
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     PageIndex = pageIndex,
                     PageSize = pageSize,
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse
                : response.ErrorResponse;
        }

        public async Task<QueryRecordsResponse<GameScore>> GetGameScores(
            int pageIndex,
            int pageSize)
        {
            await RefreshAuthToken();

            var response = await _httpRequestHelper.SendRequest<QueryRecordsResponse<GameScore>, QueryRecordsResponse<GameScore>>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_GetGameScores,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {App.AuthToken.AccessToken}" } },
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     PageIndex = pageIndex,
                     PageSize = pageSize,
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse
                : response.ErrorResponse;
        }

        private async Task RefreshAuthToken()
        {
            // if token has expired or will expire in 10 secs, get a new token
            if (AuthCredentialsCacheHelper.GetCachedSession() is Session session && App.AuthToken is not null && DateTime.UtcNow.AddSeconds(10) > App.AuthToken.ExpiresOn)
            {
                var response = await ValidateSession(
                    gameId: Constants.GAME_ID,
                    sessionId: session.SessionId);

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    var authToken = ParseResult<AuthToken>(response.Result);
                    App.AuthToken = authToken;
                }
            }
        }

        public async Task<ServiceResponse> GenerateSession(string gameId, string userId)
        {
            var response = await _httpRequestHelper.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: Constants.GAME_API_BASEURL,
                path: Constants.Action_GenerateSession,
                httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {App.AuthToken.AccessToken}" } },
                httpMethod: HttpMethod.Post,
                payload: new
                {
                    GameId = gameId,
                    UserId = userId,
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse;
        }

        public async Task<ServiceResponse> ValidateSession(string gameId, string sessionId)
        {
            var response = await _httpRequestHelper.SendRequest<ServiceResponse, ServiceResponse>(
            baseUrl: Constants.GAME_API_BASEURL,
            path: Constants.Action_ValidateSession,
            httpHeaders: new Dictionary<string, string>(),
            httpMethod: HttpMethod.Post,
            payload: new
            {
                GameId = gameId,
                SessionId = sessionId,
            });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse;
        }

        #endregion
    }
}
