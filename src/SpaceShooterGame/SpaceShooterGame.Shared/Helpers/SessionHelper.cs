using Newtonsoft.Json;
using System;

namespace SpaceShooterGame
{
    public static class SessionHelper
    {
        #region Properties
        
        public static Session Session { get; set; } 

        #endregion

        #region Methods

        public static Session GetCachedSession()
        {
            var localValue = CacheHelper.GetCachedValue(Constants.CACHE_SESSION_KEY);

            if (!localValue.IsNullOrBlank())
            {
                var session = JsonConvert.DeserializeObject<Session>(localValue);
                return session;
            }

            return null;
        }

        public static void SetSession(Session session)
        {
            Session = session;
        }

        public static void SetCachedSession(Session session)
        {
            CacheHelper.SetCachedValue(Constants.CACHE_SESSION_KEY, JsonConvert.SerializeObject(session));
        }

        public static void RemoveCachedSession() 
        {
            CacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);
            Session = null;
        }

        public static bool WillSessionExpireSoon()
        {
            if (Session is null)
                return true;

            if (DateTime.UtcNow.AddMinutes(1) > Session.ExpiresOn)
                return true;

            return false;
        }

        public static bool HasSessionExpired()
        {
            if (Session is null)
                return true;

            if (DateTime.UtcNow > Session.ExpiresOn)
                return true;

            return false;
        }

        public static void TryLoadSession() 
        {
            if (Session is null)
                Session = GetCachedSession();
        }

        #endregion
    }
}
