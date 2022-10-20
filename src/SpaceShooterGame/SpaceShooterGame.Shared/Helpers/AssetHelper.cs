using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SpaceShooterGame
{
    public static class AssetHelper
    {
        #region Fields

        private static string _baseUrl;

        private static bool _assetsPreloaded;

        #endregion

        #region Properties

        public static (ImageType ImageType, Uri AssetUri, double Size)[] STAR_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] PLANET_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] ENEMY_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] METEOR_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] BOSS_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] COLLECTIBLE_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] PLAYER_SHIP_TEMPLATES { get; set; }        

        public static (ImageType ImageType, Uri AssetUri, double Size)[] PLAYER_RAGE_TEMPLATES { get; set; }

        public static (ImageType ImageType, Uri AssetUri, double Size)[] PLAYER_SHIP_THRUST_TEMPLATES { get; set; }

        #endregion

        #region Methods

        public static string GetBaseUrl()
        {
            if (_baseUrl.IsNullOrBlank())
            {
                var indexUrl = Uno.Foundation.WebAssemblyRuntime.InvokeJS("window.location.href;");
                var appPackage = Environment.GetEnvironmentVariable("UNO_BOOTSTRAP_APP_BASE");
                _baseUrl = $"{indexUrl}{appPackage}";

#if DEBUG
                Console.WriteLine(_baseUrl);
#endif 
            }

            return _baseUrl;
        }

        public static async void PreloadAssets(ProgressBar progressBar, TextBlock messageBlock)
        {
            if (!_assetsPreloaded)
            {
                progressBar.IsIndeterminate = false;
                progressBar.ShowPaused = false;
                progressBar.Value = 0;
                progressBar.Minimum = 0;

                messageBlock.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                messageBlock.Foreground = new SolidColorBrush(Colors.White);
                messageBlock.Text = LocalizationHelper.GetLocalizedResource("LOADING_GAME_ASSETS");

                var maximum = 0;

                foreach (var fieldInfo in typeof(Constants).GetFields().Where(x => x.Name.EndsWith("TEMPLATES")))
                {
                    if (fieldInfo.FieldType.IsArray)
                    {
                        var value = fieldInfo.GetValue(fieldInfo);
                        var array = value as dynamic;

                        maximum += array.Length;
                    }
                    else
                    {
                        maximum++;
                    }
                }

                progressBar.Maximum = maximum;

                #region Images

                STAR_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.STAR).ToArray();
                PLANET_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.PLANET).ToArray();

                ENEMY_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.ENEMY).ToArray();
                METEOR_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.METEOR).ToArray();

                BOSS_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.BOSS).ToArray();

                COLLECTIBLE_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.COLLECTIBLE).ToArray();

                PLAYER_SHIP_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.PLAYER_SHIP).ToArray();                
                PLAYER_RAGE_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.PLAYER_RAGE).ToArray();
                PLAYER_SHIP_THRUST_TEMPLATES = Constants.IMAGE_TEMPLATES.Where(x => x.ImageType == ImageType.PLAYER_SHIP_THRUST).ToArray();

                foreach (var asset in Constants.IMAGE_TEMPLATES)
                {
                    await GetFileAsync(asset.AssetUri, progressBar);
                }

                #endregion

                #region Sounds

                foreach (var uri in Constants.SOUND_TEMPLATES.Select(x => x.Value).ToArray())
                {
                    await GetFileAsync(new Uri($"ms-appx:///{uri}"), progressBar);
                }

                messageBlock.Text = string.Empty;
                messageBlock.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;

                #endregion

                _assetsPreloaded = true;
            }
        }

        private static async Task GetFileAsync(Uri uri, ProgressBar progressBar)
        {
            await StorageFile.GetFileFromApplicationUriAsync(uri);
            progressBar.Value++;
        }

        #endregion
    }
}
