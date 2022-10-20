using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SpaceShooterGame
{
    public static class LocalizationHelper
    {
        #region Fields

        private static LocalizationKey[] LOCALIZATION_KEYS;
        private static string _localizationJson;

        #endregion

        #region Properties
        public static string CurrentCulture { get; set; }

        #endregion

        #region Methods

        public static async Task LoadLocalizationKeys(Action completed = null)
        {
            if (_localizationJson.IsNullOrBlank())
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/localization.json"));
                _localizationJson = await FileIO.ReadTextAsync(file);

                LOCALIZATION_KEYS = JsonConvert.DeserializeObject<LocalizationKey[]>(_localizationJson);

                if (LOCALIZATION_KEYS is null || LOCALIZATION_KEYS.Length == 0)
                    Console.WriteLine("LOCALIZATION NOT LOADED.");
                else
                    Console.WriteLine("LOCALIZATION LOADED.");

                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["LOCALIZATION_KEYS"] = _localizationJson;
                Console.WriteLine("Localization Keys Count:" + LOCALIZATION_KEYS?.Length);

                completed?.Invoke();
            }
            else
            {
                completed?.Invoke();
            }
        }

        public static string GetLocalizedResource(string resourceKey)
        {
            var localizationTemplate = LOCALIZATION_KEYS?.FirstOrDefault(x => x.Key == resourceKey);
            return localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == CurrentCulture).Value;
        }

        public static void SetLocalizedResource(UIElement uIElement)
        {
            var localizationTemplate = LOCALIZATION_KEYS?.FirstOrDefault(x => x.Key == uIElement.Name);

            if (localizationTemplate is not null)
            {
                var value = localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == CurrentCulture).Value;

                if (uIElement is TextBlock textBlock)
                    textBlock.Text = value;
                else if (uIElement is TextBox textBox)
                    textBox.Header = value;
                else if (uIElement is PasswordBox passwordBox)
                    passwordBox.Header = value;
                else if (uIElement is Button button)
                    button.Content = value;
                else if (uIElement is ToggleButton toggleButton)
                    toggleButton.Content = value;
                else if (uIElement is HyperlinkButton hyperlinkButton)
                    hyperlinkButton.Content = value;
                else if (uIElement is CheckBox checkBox)
                    checkBox.Content = value;
            }
        }

        public static void CheckLocalizationCache()
        {
            if (CacheHelper.GetCachedValue(Constants.CACHE_LANGUAGE_KEY) is string language)
                CurrentCulture = language;
        }

        public static void SaveLocalizationCache(string tag)
        {
            if (CacheHelper.GetCachedValue(Constants.COOKIE_KEY) is string cookie && cookie == "Accepted")
                CacheHelper.SetCachedValue(Constants.CACHE_LANGUAGE_KEY, tag);
        }

        public static bool HasLocalizationKey(string resourceKey)
        {
            return !resourceKey.IsNullOrBlank() && LOCALIZATION_KEYS is not null && LOCALIZATION_KEYS.Any(x => x.Key == resourceKey);
        }

        #endregion        
    }
}
