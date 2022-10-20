using Windows.Storage;

namespace SpaceShooterGame
{
    public static class CacheHelper
    {
        #region Methods

        public static string GetCachedValue(string key)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var localValue = localSettings.Values[key] as string;
            return localValue;
        }

        public static void SetCachedValue(string key, string value)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = value;
        }

        public static void RemoveCachedValue(string key)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(key))
                localSettings.Values.Remove(key);
        }

        #endregion
    }
}
