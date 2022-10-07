using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uno;
using Windows.Storage;

namespace AstroOdyssey
{
    public class LocalizationHelper : ILocalizationHelper
    {
        #region Fields

        private LocalizationKey[] LOCALIZATION_KEYS;

        #endregion

        #region Ctor

        public LocalizationHelper()
        {
            var localization = JsonConvert.SerializeObject(LOCALIZATION_KEYS);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["LOCALIZATION_KEYS"] = localization;
        }

        #endregion

        #region Methods

        public async Task LoadLocalizationKeys()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/localization.json"));
            var localizationJson = await FileIO.ReadTextAsync(file);
            LOCALIZATION_KEYS = JsonConvert.DeserializeObject<LocalizationKey[]>(localizationJson);

#if DEBUG
            Console.WriteLine("Localization Keys Count:" + LOCALIZATION_KEYS.Length);

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["LOCALIZATION_KEYS"] = localizationJson;
#endif
        }

        /// <summary>
        /// Gets the localization resource.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public string GetLocalizedResource(string resourceKey)
        {
            var localizationTemplate = LOCALIZATION_KEYS.FirstOrDefault(x => x.Key == resourceKey);
            return localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;
        }

        /// <summary>
        /// Sets a localized value on the provided ui element.
        /// </summary>
        /// <param name="uIElement"></param>
        public void SetLocalizedResource(UIElement uIElement)
        {
            var localizationTemplate = LOCALIZATION_KEYS.FirstOrDefault(x => x.Key == uIElement.Name);

            if (localizationTemplate is not null)
            {
                var value = localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;

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

        #endregion        
    }
}
