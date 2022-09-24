namespace AstroOdyssey
{
    public interface ICacheHelper
    {
        PlayerCredentials GetCachedPlayerCredentials();

        void SetCachedPlayerCredentials(string userName, string password);

        string GetCachedValue(string key);

        void SetCachedValue(string key, string value);

        void RemoveCachedValue(string key);

        Session GetCachedSession();

        bool WillAuthTokenExpireSoon();

        bool WillSessionExpireSoon();

        bool HasSessionExpired();

        void SetCachedSession(Session session);
    }
}
