using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
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
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        public async Task<ServiceResponse> SubmitGameScore(double score)
        {
            if (!await RefreshAuthToken())
                return new ServiceResponse() { HttpStatusCode = HttpStatusCode.Conflict, ExternalError = "Failed to refresh token." };

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
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
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
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
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
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        public async Task<QueryRecordResponse<GameProfile>> GetGameProfile()
        {
            if (!await RefreshAuthToken())
                new QueryRecordResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

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
                ? response.SuccessResponse ?? new QueryRecordResponse<GameProfile>()
                : response.ErrorResponse ?? new QueryRecordResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        public async Task<QueryRecordsResponse<GameProfile>> GetGameProfiles(int pageIndex, int pageSize)
        {
            if (!await RefreshAuthToken())
                new QueryRecordsResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

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
                : response.ErrorResponse ?? new QueryRecordsResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        public async Task<QueryRecordsResponse<GameScore>> GetGameScores(int pageIndex, int pageSize)
        {
            if (!await RefreshAuthToken())
                new QueryRecordsResponse<GameScore>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

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
                : response.ErrorResponse ?? new QueryRecordsResponse<GameScore>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<bool> RefreshAuthToken()
        {
            if (CacheHelper.WillSessionExpireSoon() || CacheHelper.WillAuthTokenExpireSoon())
            {
                if (CacheHelper.GetCachedSession() is Session session)
                {
                    // validate session and get new auth token
                    if (!await ValidateSession(session.SessionId))
                        return false;

                    if (CacheHelper.WillSessionExpireSoon())
                    {
                        // with new auth token generate a new session and validate it, get new auth token for new session
                        if (!await GenerateAndValidateSession())
                            return false;
                    }
                }
            }

            return true;
        }

        private async Task<bool> GenerateAndValidateSession()
        {
            var response = await GenerateSession(gameId: Constants.GAME_ID, userId: App.GameProfile.User.UserId);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return false;

            var session = ParseResult<Session>(response.Result);
            CacheHelper.SetCachedSession(session);

            return await ValidateSession(session.SessionId);
        }

        private async Task<bool> ValidateSession(string sessionId)
        {
            var response = await ValidateSession(
                gameId: Constants.GAME_ID,
                sessionId: sessionId);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return false;

            var authToken = ParseResult<AuthToken>(response.Result);
            App.AuthToken = authToken;

            return true;
        }

        #endregion
    }
}
