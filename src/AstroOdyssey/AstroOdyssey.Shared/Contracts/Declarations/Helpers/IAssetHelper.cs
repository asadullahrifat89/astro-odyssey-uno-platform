using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public interface IAssetHelper
    {
        Task PreloadAssets(ProgressBar progressBar, TextBlock messageBlock);
    }
}
