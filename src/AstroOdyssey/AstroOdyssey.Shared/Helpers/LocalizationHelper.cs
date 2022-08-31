using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Linq;

namespace AstroOdyssey
{
    public static class LocalizationHelper
    {
        /// <summary>
        /// Gets the localization resource.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetLocalizedResource(string resourceKey)
        {
            var localizationTemplate = Constants.LOCALIZATION_TEMPLATES.FirstOrDefault(x => x.Key == resourceKey);
            return localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;
        }

        /// <summary>
        /// Sets a localized value on the provided ui element.
        /// </summary>
        /// <param name="uIElement"></param>
        public static void SetLocalizedResource(UIElement uIElement)
        {
            var localizationTemplate = Constants.LOCALIZATION_TEMPLATES.FirstOrDefault(x => x.Key == uIElement.Name);

            var value = localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;

            if (uIElement is TextBlock textBlock)
                textBlock.Text = value;
            else if (uIElement is Button button)
                button.Content = value;
            else if (uIElement is HyperlinkButton hyperlinkButton)
                hyperlinkButton.Content = value;
        }
    }
}
