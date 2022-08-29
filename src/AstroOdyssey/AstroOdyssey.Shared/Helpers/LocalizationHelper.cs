using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace AstroOdyssey
{
    public static class LocalizationHelper
    {
        private static IDictionary<string, string> _localizationResourceCache = new Dictionary<string, string>();
        private static string _currentlanguage;

        public static void SetAppLanguage(string name)
        {
            _currentlanguage = name;
            var culture = new System.Globalization.CultureInfo(name);

            System.Globalization.CultureInfo.CurrentUICulture = culture;
            System.Globalization.CultureInfo.CurrentCulture = culture;
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.IetfLanguageTag;
            Windows.ApplicationModel.Resources.ResourceLoader.DefaultLanguage = culture.IetfLanguageTag;


            _localizationResourceCache.Clear(); // clear the resources cache          
        }

        /// <summary>
        /// Gets the localization resource.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetLocalizedResource(string resourceKey)
        {
            if (_localizationResourceCache.ContainsKey(resourceKey))
            {
                return _localizationResourceCache[resourceKey];
            }
            else
            {
                var resourceValue = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(resourceKey);
                _localizationResourceCache.Add(resourceKey, resourceValue);
                return resourceValue;
            }
        }
    }
}
