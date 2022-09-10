using AstroOdysseyCore;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public interface IGameApiHelper
    {
        T ParseResult<T>(object obj);

        Task<ServiceResponse> Authenticate(string userNameOrEmail, string password);

        Task<ServiceResponse> Signup(string userName, string email, string password);
    }
}
