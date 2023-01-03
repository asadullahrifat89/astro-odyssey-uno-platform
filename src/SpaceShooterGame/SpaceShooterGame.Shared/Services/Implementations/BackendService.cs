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
            string city,
            string userName,
            string email,
            string password,
            bool subscribedNewsletters)
        {
            ServiceResponse response = await Signup(
                   fullName: fullName,
                   city: city,
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
            SetAuthTokenAndRefreshToken(response);

            PlayerCredentialsHelper.SetPlayerCredentials(
                userName: userNameOrEmail,
                password: password);

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

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message, GamePlayResult GamePlayResult)> SubmitUserGameScore(double score)
        {
            ServiceResponse response = await SubmitGameScore(score);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                return (false, error, null);
            }

            return (true, "OK", ParseResult<GamePlayResult>(response.Result));
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

        public async Task<(bool IsSuccess, string Message)> CheckUserIdentityAvailability(
            string userName,
            string email)
        {
            var recordResponse = await CheckIdentityAvailability(
                userName: userName,
                email: email);

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                return (false, string.Join("\n", error));
            }

            return (true, "OK");
        }

        public async Task<(bool IsSuccess, string Message, Season Season)> GetGameSeason()
        {
            var recordResponse = await GetSeason();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                return (false, string.Join("\n", error), null);
            }

            return (true, "OK", recordResponse.Result);
        }

        public async Task<(bool IsSuccess, string Message, GamePrizeOfTheDay GamePrize)> GetGameDailyPrize()
        {
            var recordResponse = await GetGamePrize();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                return (false, string.Join("\n", error), null);
            }

            return (true, "OK", recordResponse.Result);
        }

        public async Task<(bool IsSuccess, string Message, Company Company)> GetCompanyBrand()
        {
            var recordResponse = await GetCompany();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                return (false, string.Join("\n", error), null);
            }

            return (true, "OK", recordResponse.Result);
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
            if (!await RefreshAuthToken())
                return new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Failed to refresh token." };

            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
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

        private async Task<ServiceResponse> ValidateToken(string refreshToken)
        {
            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                path: Constants.Action_ValidateToken,
                httpHeaders: new Dictionary<string, string>(),
                httpMethod: HttpMethod.Post,
                payload: new
                {
                    CompanyId = Constants.COMPANY_ID,
                    RefreshToken = refreshToken,
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        private async Task<ServiceResponse> Authenticate(
            string userNameOrEmail,
            string password)
        {
            var payload = new
            {
                UserName = userNameOrEmail,
                Password = password,
                CompanyId = Constants.COMPANY_ID,
            };

            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
               baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
               path: Constants.Action_Authenticate,
               httpHeaders: new Dictionary<string, string>(),
               httpMethod: HttpMethod.Post,
               contentType: "application/x-www-form-urlencoded",
               formUrlEncodedContent: ObjectExtensions.GetProperties(payload));

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        private async Task<ServiceResponse> Signup(
            string fullName,
            string city,
            string userName,
            string email,
            string password,
            bool subscribedNewsletters)
        {
            var payload = new
            {
                Email = email,
                FullName = fullName,
                UserName = userName,
                Password = password,
                City = city,
                GameId = Constants.GAME_ID,
                CompanyId = Constants.COMPANY_ID,
                SubscribedNewsletters = subscribedNewsletters.ToString(),
            };

            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                 path: Constants.Action_SignUp,
                 httpHeaders: new Dictionary<string, string>(),
                 httpMethod: HttpMethod.Post,
                 contentType: "application/x-www-form-urlencoded",
                 formUrlEncodedContent: ObjectExtensions.GetProperties(payload));

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        private async Task<ServiceResponse> SubmitGameScore(double score)
        {
            if (!await RefreshAuthToken())
                return new ServiceResponse() { HttpStatusCode = HttpStatusCode.Conflict, ExternalError = "Failed to refresh token." };

            var response = await _httpRequestService.SendRequest<ServiceResponse, ServiceResponse>(
                baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                path: Constants.Action_SubmitGameScore,
                httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
                httpMethod: HttpMethod.Post,
                payload: new
                {
                    User = new AttachedUser()
                    {
                        UserName = GameProfileHelper.GameProfile.User.UserName,
                        UserId = GameProfileHelper.GameProfile.User.UserId,
                    },
                    Score = score,
                    GameId = Constants.GAME_ID,
                    SessionId = SessionHelper.Session.SessionId.UnBitShift()
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.OK }
                : response.ErrorResponse ?? new ServiceResponse() { HttpStatusCode = HttpStatusCode.InternalServerError, ExternalError = "Internal server error." };
        }

        private async Task<QueryRecordResponse<GameProfile>> GetGameProfile()
        {
            if (!await RefreshAuthToken())
                return new QueryRecordResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

            var response = await _httpRequestService.SendRequest<QueryRecordResponse<GameProfile>, QueryRecordResponse<GameProfile>>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
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
                return new QueryRecordsResponse<GameProfile>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

            var response = await _httpRequestService.SendRequest<QueryRecordsResponse<GameProfile>, QueryRecordsResponse<GameProfile>>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
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
                return new QueryRecordsResponse<GameScore>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "Failed to refresh token." } });

            var response = await _httpRequestService.SendRequest<QueryRecordsResponse<GameScore>, QueryRecordsResponse<GameScore>>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                 path: Constants.Action_GetGameScoresOfTheDay,
                 httpHeaders: new Dictionary<string, string>() { { "Authorization", $"bearer {AuthTokenHelper.AuthToken.AccessToken}" } },
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     PageIndex = pageIndex,
                     PageSize = pageSize,
                     //ScoreDay = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy"),
                     GameId = Constants.GAME_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse
                : response.ErrorResponse ?? new QueryRecordsResponse<GameScore>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<QueryRecordResponse<bool>> CheckIdentityAvailability(
            string userName,
            string email)
        {
            var payload = new
            {
                Email = email,
                UserName = userName,
                GameId = Constants.GAME_ID,
                CompanyId = Constants.COMPANY_ID,
            };

            var response = await _httpRequestService.SendRequest<QueryRecordResponse<bool>, QueryRecordResponse<bool>>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                 path: Constants.Action_CheckIdentityAvailability,
                 httpHeaders: new Dictionary<string, string>(),
                 httpMethod: HttpMethod.Post,
                 contentType: "application/x-www-form-urlencoded",
                 formUrlEncodedContent: ObjectExtensions.GetProperties(payload));

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new QueryRecordResponse<bool>()
                : response.ErrorResponse ?? new QueryRecordResponse<bool>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<QueryRecordResponse<Season>> GetSeason()
        {
            var response = await _httpRequestService.SendRequest<QueryRecordResponse<Season>, QueryRecordResponse<Season>>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                 path: Constants.Action_GetSeason,
                 httpHeaders: new Dictionary<string, string>(),
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     CompanyId = Constants.COMPANY_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new QueryRecordResponse<Season>()
                : response.ErrorResponse ?? new QueryRecordResponse<Season>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<QueryRecordResponse<GamePrizeOfTheDay>> GetGamePrize()
        {
            //_ = int.TryParse(DateTime.UtcNow.ToString("dd-MMM-yyyy").Split('-')[0], out int day); // take the day part

            var response = await _httpRequestService.SendRequest<QueryRecordResponse<GamePrizeOfTheDay>, QueryRecordResponse<GamePrizeOfTheDay>>(
                baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                path: Constants.Action_GetGamePrizeOfTheDay,
                httpHeaders: new Dictionary<string, string>(),
                httpMethod: HttpMethod.Get,
                payload: new
                {
                    Culture = LocalizationHelper.CurrentCulture,
                    //Day = day,
                    GameId = Constants.GAME_ID,
                    CompanyId = Constants.COMPANY_ID,
                });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new QueryRecordResponse<GamePrizeOfTheDay>()
                : response.ErrorResponse ?? new QueryRecordResponse<GamePrizeOfTheDay>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<QueryRecordResponse<Company>> GetCompany()
        {
            var response = await _httpRequestService.SendRequest<QueryRecordResponse<Company>, QueryRecordResponse<Company>>(
                 baseUrl: AppSettingsHelper.AppSettings.BackendApiBaseUrl,
                 path: Constants.Action_GetCompany,
                 httpHeaders: new Dictionary<string, string>(),
                 httpMethod: HttpMethod.Get,
                 payload: new
                 {
                     CompanyId = Constants.COMPANY_ID,
                 });

            return response.StatusCode == HttpStatusCode.OK
                ? response.SuccessResponse ?? new QueryRecordResponse<Company>()
                : response.ErrorResponse ?? new QueryRecordResponse<Company>().BuildErrorResponse(new ErrorResponse() { Errors = new string[] { "No data found." } });
        }

        private async Task<bool> RefreshAuthToken()
        {
            if (AuthTokenHelper.WillAuthTokenExpireSoon())
                return await ValidateToken();

            return true;
        }

        private async Task<bool> ValidateToken()
        {
            var response = await ValidateToken(refreshToken: AuthTokenHelper.RefreshToken);

            if (response is null || response.HttpStatusCode != HttpStatusCode.OK)
                return false;

            SetAuthTokenAndRefreshToken(response);

            return true;
        }

        private void SetAuthTokenAndRefreshToken(ServiceResponse response)
        {
            var authToken = ParseResult<AuthToken>(response.Result);
            AuthTokenHelper.AuthToken = authToken;
            AuthTokenHelper.RefreshToken = authToken.RefreshToken;

            if (CookieHelper.IsCookieAccepted())
                AuthTokenHelper.SetCachedRefreshToken(authToken.RefreshToken);
        }


        #endregion

        #endregion
    }
}
