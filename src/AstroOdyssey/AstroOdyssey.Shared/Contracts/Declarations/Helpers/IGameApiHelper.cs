using System;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public interface IGameApiHelper
    {
        T ParseResult<T>(object obj);

        Task<ServiceResponse> Authenticate(string userNameOrEmail, string password);

        Task<ServiceResponse> Signup(string userName, string email, string password);

        Task<ServiceResponse> SubmitGameScore(double score);

        Task<QueryRecordResponse<GameProfile>> GetGameProfile();

        Task<QueryRecordsResponse<GameScore>> GetGameScores(int pageIndex, int pageSize);

        Task<QueryRecordsResponse<GameProfile>> GetGameProfiles(int pageIndex, int pageSize);
                
        Task<ServiceResponse> GenerateSession(string gameId, string userId);

        Task<ServiceResponse> ValidateSession(string gameId, string sessionId);        
    }
}
