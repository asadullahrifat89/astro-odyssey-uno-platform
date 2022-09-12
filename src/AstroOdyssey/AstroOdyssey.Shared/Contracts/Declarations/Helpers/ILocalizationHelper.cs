using Microsoft.UI.Xaml;

namespace AstroOdyssey
{
    public interface ILocalizationHelper 
    {
        string GetLocalizedResource(string resourceKey);

        void SetLocalizedResource(UIElement uIElement);
    }
}
