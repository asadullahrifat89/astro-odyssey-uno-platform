using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public class BackendService : IBackendService
    {
        #region Fields

        private readonly IHttpRequestService _httpRequestService;

        #endregion

        #region Ctor

        public BackendService(IHttpRequestService httpRequestService)
        {
            _httpRequestService = httpRequestService;
        }

        #endregion

        #region Methods

        #region Public

        public async Task<(bool IsSuccess, string Message)> SignupUser(
            string fullName,
            string userName,
            string email,
            string password,
            bool subscribedNewsletters)
        {
            ServiceResponse response = await Signup(
                   fullName: fullName,
                   userName: userName,
                   email: email,
                   password: password,
                   subscribedNewsletters: subscribedNewsletters);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                return (false, error);
            }

            // store game profile
            var gameProfile = ParseResult<GameProfile>(response.Result);
            GameProfileHelper.GameProfile = gameProfile;

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message)> AuthenticateUser(
            string userNameOrEmail,
            string password)
        {
            // authenticate
            ServiceResponse response = await Authenticate(
                userNameOrEmail: userNameOrEmail,
                password: password);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                return (false, error);
            }

            // hold auth token
            var authToken = ParseResult<AuthToken>(response.Result);
            AuthTokenHelper.AuthToken = authToken;

            PlayerCredentialsHelper.SetPlayerCredentials(
                userName: userNameOrEmail,
                password: password);

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message)> ValidateUserSession(Session session)
        {
            ServiceResponse response = await ValidateSession(Constants.GAME_ID, session.SessionId);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
                return (false, "ERROR");

            // store auth token
            var authToken = ParseResult<AuthToken>(response.Result);
            AuthTokenHelper.AuthToken = authToken;

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message)> GenerateUserSession()
        {
            ServiceResponse response = await GenerateSession(
                gameId: Constants.GAME_ID,
                userId: GameProfileHelper.GameProfile.User.UserId);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                return (false, error);
            }

            // store session
            var session = ParseResult<Session>(response.Result);
            SessionHelper.Session = session;

            if (CookieHelper.IsCookieAccepted())
                SessionHelper.SetCachedSession(session);

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message)> SubmitUserGameScore(double score)
        {
            ServiceResponse response = await SubmitGameScore(score);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                return (false, error);
            }

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message, GameProfile GameProfile)> GetUserGameProfile()
        {
            var recordResponse = await GetGameProfile();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                return (false, string.Join("\n", error), null);
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            GameProfileHelper.GameProfile = gameProfile;

            return (true, "OK", gameProfile);
        }

        public async Task<(bool IsSuccess, string Message, GameProfile[] GameProfiles)> GetUserGameProfiles(
            int pageIndex,
            int pageSize)
        {
            var recordsResponse = await GetGameProfiles(pageIndex: pageIndex, pageSize: pageSize);

            if (!recordsResponse.IsSuccess)
            {
                var error = recordsResponse.Errors.Errors;
                return (false, string.Join("\n", error), Array.Empty<GameProfile>());
            }

            var result = recordsResponse.Result;
            var count = recordsResponse.Result.Count;

            return count > 0 ? (true, "OK", result.Records) : (true, "OK", Array.Empty<GameProfile>());
        }

        public async Task<(bool IsSuccess, string Message, GameScore[] GameScores)> GetUserGameScores(
            int pageIndex,
            int pageSize)
        {
            var recordsResponse = await GetGameScores(pageIndex: pageIndex, pageSize: pageSize);

            if (!recordsResponse.IsSuccess)
            {
                var error = recordsResponse.Errors.Errors;
                return (false, string.Join("\n", error), Array.Empty<GameScore>());
            }

            var result = recordsResponse.Result;
            var count = recordsResponse.Result.Count;

            return count > 0 ? (true, "OK", result.Records) : (true, "OK", Array.Empty<GameScore>());
        }

        #endregion

        #region Private

        private T ParseResult<T>(object obj)
        {
            var result = JsonConvert.DeserializeObject<T>(obj.ToString(), new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return result;
        }

        private async Task<ServiceResponse> GenerateSession(
            string gameId,
            string userId)
        {
            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: Constants.GAME_API_BASEURL,
                path: Constants.Action_GenerateSession,
                httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
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

        private async Task<ServiceResponse> ValidateSession(
            string gameId,
            string sessionId)
        {
            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
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

        private async Task<ServiceResponse> Authenticate(
            string userNameOrEmail,
            string password)
        {
            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
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

        private async Task<bool> RefreshAuthToken()
        {
            // taking in account that user has already logged in and a session and auth token exists
            if (SessionHelper.WillSessionExpireSoon() || AuthTokenHelper.WillAuthTokenExpireSoon())
            {
                if (SessionHelper.GetCachedSession() is Session session)
                {
                    // validate session and get new auth token
                    if (!await ValidateSession(session.SessionId))
                        return false;

                    if (SessionHelper.WillSessionExpireSoon())
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
            var response = await GenerateSession(gameId: Constants.GAME_ID, userId: GameProfileHelper.GameProfile.User.UserId);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
                return false;

            var session = ParseResult<Session>(response.Result);
            SessionHelper.Session = session;

            if (CookieHelper.IsCookieAccepted())
                SessionHelper.SetCachedSession(session);

            return await ValidateSession(session.SessionId);
        }

        private async Task<bool> ValidateSession(string sessionId)
        {
            var response = await ValidateSession(
                gameId: Constants.GAME_ID,
                sessionId: sessionId);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
                return false;

            var authToken = ParseResult<AuthToken>(response.Result);
            AuthTokenHelper.AuthToken = authToken;

            return true;
        }

        private async Task<ServiceResponse> Signup(
            string fullName,
            string userName,
            string email,
            string password,
            bool subscribedNewsletters)
        {
            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_SignUp,
                 httpHeaders: new Dictionary<string, string>(),
                 httpMethod: HttpMethod.Post,
                 payload: new
                 {
                     Email = email,
                     FullName = fullName,
                     UserName = userName,
                     Password = password,
                     GameId = Constants.GAME_ID,
                     MetaData = new Dictionary<string, string>()
                     {
                         { "SubscribedNewsletters", subscribedNewsletters.ToString() }
                     },
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        private async Task<ServiceResponse> SubmitGameScore(double score)
        {
            if (!await RefreshAuthToken())
                return new ServiceResponse() { HttpStatusCode = HttpStatusCode.Conflict, ExternalError = "Failed to refresh token." };

            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: Constants.GAME_API_BASEURL,
                path: Constants.Action_SubmitGameScore,
                httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
                httpMethod: HttpMethod.Post,
                payload: new
                {
                    User = new AttachedUser()
                    {
                        UserEmail = GameProfileHelper.GameProfile.User.UserEmail,
                        UserName = GameProfileHelper.GameProfile.User.UserName,
                        UserId = GameProfileHelper.GameProfile.User.UserId,
                    },
                    Score = score,
                    GameId = Constants.GAME_ID,
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        private async Task<QueryRecordResponse<GameProfile>> GetGameProfile()
        {
            if (!await RefreshAuthToken())
                new QueryRecordResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

            var response = await _httpRequestService.SendRequest<QueryRecordResponse<GameProfile>, QueryRecordResponse<GameProfile>>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_GetGameProfile,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new QueryRecordResponse<GameProfile>()
                : response.ErrorResponse ?? new QueryRecordResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<QueryRecordsResponse<GameProfile>> GetGameProfiles(
            int pageIndex,
            int pageSize)
        {
            if (!await RefreshAuthToken())
                new QueryRecordsResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

            var response = await _httpRequestService.SendRequest<QueryRecordsResponse<GameProfile>, QueryRecordsResponse<GameProfile>>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_GetGameProfiles,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
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

        private async Task<QueryRecordsResponse<GameScore>> GetGameScores(
            int pageIndex,
            int pageSize)
        {
            if (!await RefreshAuthToken())
                new QueryRecordsResponse<GameScore>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

            var response = await _httpRequestService.SendRequest<QueryRecordsResponse<GameScore>, QueryRecordsResponse<GameScore>>(
                 baseUrl: Constants.GAME_API_BASEURL,
                 path: Constants.Action_GetGameScores,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     PageIndex = pageIndex,
                     PageSize = pageSize,
                     ScoreDay = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy"),
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse
                : response.ErrorResponse ?? new QueryRecordsResponse<GameScore>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        #endregion      

        #endregion
    }
}
