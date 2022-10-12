using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public interface ILocalizationHelper
    {
        Task LoadLocalizationKeys();

        string GetLocalizedResource(string resourceKey);

        void SetLocalizedResource(UIElement uIElement);
    }
}
