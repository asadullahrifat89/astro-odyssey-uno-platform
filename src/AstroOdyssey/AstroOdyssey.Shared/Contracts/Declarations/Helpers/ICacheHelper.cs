namespace AstroOdyssey
{
    public interface ICacheHelper
    {
        PlayerCredentials GetCachedPlayerCredentials();

        void SetCachedPlayerCredentials(string userName, string password);

        string GetCachedValue(string key);

        void SetCachedValue(string key, string value);

        void RemoveCachedValue(string key);

        bool WillAuthTokenExpireSoon();

        Session GetCachedSession();

        void SetCachedSession(Session session);

        bool WillSessionExpireSoon();

        bool HasSessionExpired();

        bool IsCookieAccepted();

        void SetCookieAccepted();
    }
}
