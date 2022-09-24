using Microsoft.UI.Xaml.Controls;

namespace AstroOdyssey
{
    public interface IAssetHelper
    {
        void PreloadAssets(ProgressBar progressBar, TextBlock messageBlock);
    }
}
