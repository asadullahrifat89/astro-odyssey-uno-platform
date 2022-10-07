using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public interface ILocalizationHelper
    {
        Task LoadLocalizationKeys();

        string GetLocalizedResource(string resourceKey);

        void SetLocalizedResource(UIElement uIElement);
    }
}
