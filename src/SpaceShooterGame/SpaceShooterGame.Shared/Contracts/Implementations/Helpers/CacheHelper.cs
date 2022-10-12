using Newtonsoft.Json;
using System;
using Windows.Storage;

namespace SpaceShooterGame
{
    public class CacheHelper : ICacheHelper
    {
        public PlayerCredentials GetCachedPlayerCredentials()
        {
            if (App.AuthCredentials is not null)
            {
                App.AuthCredentials.Password = App.AuthCredentials.Password.Decrypt();
                return App.AuthCredentials;
            }

            return null;
        }

        public void SetCachedPlayerCredentials(string userName, string password)
        {
            App.AuthCredentials = new PlayerCredentials(
                userName: userName,
                password: password.Encrypt());
        }

        public string GetCachedValue(string key)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var localValue = localSettings.Values[key] as string;
            return localValue;
        }

        public void SetCachedValue(string key, string value)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = value;
        }

        public void RemoveCachedValue(string key)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(key))
                localSettings.Values.Remove(key);
        }

        public Session GetCachedSession()
        {
            var localValue = GetCachedValue(Constants.CACHE_SESSION_KEY);

            if (!localValue.IsNullOrBlank())
            {
                var session = JsonConvert.DeserializeObject<Session>(localValue);
                return session;
            }

            return null;
        }

        public bool WillAuthTokenExpireSoon()
        {
            if (DateTime.UtcNow.AddSeconds(20) > App.AuthToken.ExpiresOn)
                return true;

            return false;
        }

        public bool WillSessionExpireSoon()
        {
            if (App.Session is null)
                return true;

            if (DateTime.UtcNow.AddMinutes(1) > App.Session.ExpiresOn)
                return true;

            return false;
        }

        public bool HasSessionExpired()
        {
            if (App.Session is null)
                return true;

            if (DateTime.UtcNow > App.Session.ExpiresOn)
                return true;

            return false;
        }

        public void SetCachedSession(Session session)
        {
            SetCachedValue(Constants.CACHE_SESSION_KEY, JsonConvert.SerializeObject(session));
        }

        public bool IsCookieAccepted()
        {
            return GetCachedValue(Constants.COOKIE_KEY) is string cookie && cookie == Constants.COOKIE_ACCEPTED_KEY;
        }

        public void SetCookieAccepted()
        {
            SetCachedValue(Constants.COOKIE_KEY, Constants.COOKIE_ACCEPTED_KEY);
        }
    }
}
