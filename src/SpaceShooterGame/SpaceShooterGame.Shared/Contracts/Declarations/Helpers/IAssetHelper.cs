using Microsoft.UI.Xaml.Controls;

namespace SpaceShooterGame
{
    public interface IAssetHelper
    {
        void PreloadAssets(ProgressBar progressBar, TextBlock messageBlock);
    }
}
