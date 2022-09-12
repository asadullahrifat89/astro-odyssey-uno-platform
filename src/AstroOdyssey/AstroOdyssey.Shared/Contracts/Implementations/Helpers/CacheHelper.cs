using Newtonsoft.Json;
using System;
using Windows.Storage;

namespace AstroOdyssey
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
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var localValue = localSettings.Values[Constants.CACHE_SESSION_KEY] as string;

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
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var localValue = localSettings.Values[Constants.CACHE_SESSION_KEY] as string;

            if (localValue.IsNullOrBlank())
                return true;

            var session = JsonConvert.DeserializeObject<Session>(localValue);
            if (DateTime.UtcNow.AddMinutes(1) > session.ExpiresOn)
                return true;

            return false;
        }

        public bool HasSessionExpired()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var localValue = localSettings.Values[Constants.CACHE_SESSION_KEY] as string;

            if (localValue.IsNullOrBlank())
                return true;

            var session = JsonConvert.DeserializeObject<Session>(localValue);
            if (DateTime.UtcNow > session.ExpiresOn)
                return true;

            return false;
        }

        public void SetCachedSession(Session session)
        {
            // save in browser cache
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[Constants.CACHE_SESSION_KEY] = JsonConvert.SerializeObject(session);
        }
    }
}
