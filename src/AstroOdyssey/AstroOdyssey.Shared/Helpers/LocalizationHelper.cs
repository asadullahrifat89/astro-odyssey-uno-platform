using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AstroOdyssey
{
    public static class LocalizationHelper
    {
        //private static IDictionary<string, string> _localizationResourceCache = new Dictionary<string, string>();

        //public static void SetAppCulture(string name)
        //{
        //    //var culture = new System.Globalization.CultureInfo(name);

        //    //System.Globalization.CultureInfo.CurrentUICulture = culture;
        //    //System.Globalization.CultureInfo.CurrentCulture = culture;
        //    //Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.IetfLanguageTag;
        //    //Windows.ApplicationModel.Resources.ResourceLoader.DefaultLanguage = culture.IetfLanguageTag;


        //    //_localizationResourceCache.Clear(); // clear the resources cache          
        //}

        /// <summary>
        /// Gets the localization resource.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetLocalizedResource(string resourceKey)
        {
            var localizationTemplate = Constants.LOCALIZATION_TEMPLATES.FirstOrDefault(x => x.Key == resourceKey);

            return localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;

            //if (_localizationResourceCache.ContainsKey(resourceKey))
            //{
            //    return _localizationResourceCache[resourceKey];
            //}
            //else
            //{
            //    var resourceValue = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(resourceKey);
            //    _localizationResourceCache.Add(resourceKey, resourceValue);
            //    return resourceValue;
            //}
        }

        public static void SetLocalizedResource(UIElement uIElement)
        {
            var localizationTemplate = Constants.LOCALIZATION_TEMPLATES.FirstOrDefault(x => x.Key == uIElement.Name);

            var value = localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;

            if (uIElement is TextBlock textBlock)
                textBlock.Text = value;
            else if (uIElement is Button button)
                button.Content = value;

            //if (_localizationResourceCache.ContainsKey(resourceKey))
            //{
            //    return _localizationResourceCache[resourceKey];
            //}
            //else
            //{
            //    var resourceValue = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(resourceKey);
            //    _localizationResourceCache.Add(resourceKey, resourceValue);
            //    return resourceValue;
            //}
        }
    }
}
