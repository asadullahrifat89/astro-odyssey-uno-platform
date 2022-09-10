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
    }
}
