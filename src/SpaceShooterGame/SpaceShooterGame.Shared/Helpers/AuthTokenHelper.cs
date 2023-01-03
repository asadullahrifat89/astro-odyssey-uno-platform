using Newtonsoft.Json;
using System;

namespace SpaceShooterGame
{
    public static class AuthTokenHelper
    {
        #region Properties

        public static AuthToken AuthToken { get; set; }

        public static string RefreshToken { get; set; }

        #endregion

        #region Methods

        public static bool WillAuthTokenExpireSoon()
        {
            if (AuthToken is null || DateTime.UtcNow.AddSeconds(20) > AuthToken.ExpiresOn)
                return true;

            return false;
        }

        public static string GetCachedRefreshToken()
        {
            var localValue = CacheHelper.GetCachedValue(Constants.CACHE_REFRESH_TOKEN_KEY);

            return localValue;
        }

        public static void SetCachedRefreshToken(string refreshToken)
        {
            CacheHelper.SetCachedValue(Constants.CACHE_REFRESH_TOKEN_KEY, refreshToken);
        }

        public static void RemoveCachedRefreshToken()
        {
            CacheHelper.RemoveCachedValue(Constants.CACHE_REFRESH_TOKEN_KEY);
        }

        public static void TryLoadRefreshToken()
        {
            RefreshToken ??= GetCachedRefreshToken();
        }

        #endregion
    }
}
