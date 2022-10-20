namespace SpaceShooterGame
{
    public static class CookieHelper
    {
        #region Methods

        public static bool IsCookieAccepted()
        {
            return CacheHelper.GetCachedValue(Constants.COOKIE_KEY) is string cookie && cookie == Constants.COOKIE_ACCEPTED_KEY;
        }

        public static void SetCookieAccepted()
        {
            CacheHelper.SetCachedValue(Constants.COOKIE_KEY, Constants.COOKIE_ACCEPTED_KEY);
        }

        public static void SetCookieDeclined()
        {
            CacheHelper.RemoveCachedValue(Constants.COOKIE_KEY);
        }


        #endregion
    }
}
