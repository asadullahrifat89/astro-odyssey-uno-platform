using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public interface IBackendService
    {
        Task<(bool IsSuccess, string Message)> AuthenticateUser(string userNameOrEmail, string password);

        Task<(bool IsSuccess, string Message)> SignupUser(string fullName, string userName, string email, string password, bool subscribedNewsletters);

        Task<(bool IsSuccess, string Message)> SubmitUserGameScore(double score);

        Task<(bool IsSuccess, string Message)> GenerateUserSession();

        Task<(bool IsSuccess, string Message)> ValidateUserSession(Session session);

        Task<(bool IsSuccess, string Message, GameProfile GameProfile)> GetUserGameProfile();

        Task<(bool IsSuccess, string Message, GameScore[] GameScores)> GetUserGameScores(int pageIndex, int pageSize);

        Task<(bool IsSuccess, string Message, GameProfile[] GameProfiles)> GetUserGameProfiles(int pageIndex, int pageSize);

        Task<(bool IsSuccess, string Message)> CheckUserIdentityAvailability(string userName, string email);
    }
}
