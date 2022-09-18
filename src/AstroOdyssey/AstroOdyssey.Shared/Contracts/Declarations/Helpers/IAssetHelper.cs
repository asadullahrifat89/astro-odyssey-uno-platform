using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public interface IAssetHelper
    {
        void PreloadAssets(ProgressBar progressBar, TextBlock messageBlock);
    }
}
