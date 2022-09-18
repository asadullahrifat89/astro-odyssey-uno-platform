using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace AstroOdyssey
{
    public class AssetHelper : IAssetHelper
    {
        #region Ctor

        public AssetHelper()
        {

        }

        #endregion

        #region Methods

        public async Task PreloadAssets(ProgressBar progressBar, TextBlock messageBlock)
        {
            progressBar.IsIndeterminate = false;
            progressBar.ShowPaused = false;
            progressBar.Value = 0;
            progressBar.Minimum = 0;

            messageBlock.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            messageBlock.Foreground = new SolidColorBrush(Colors.White);
            messageBlock.Text = "Loading game assets...";

            var maximum = 0;

            foreach (var fieldInfo in typeof(GameObjectTemplates).GetFields())
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

            foreach (var asset in GameObjectTemplates.PLAYER_RAGE_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var asset in GameObjectTemplates.STAR_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var asset in GameObjectTemplates.PLANET_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var asset in GameObjectTemplates.ENEMY_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var asset in GameObjectTemplates.METEOR_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var asset in GameObjectTemplates.PLAYER_SHIP_TEMPLATES)
            {
                await GetFileAsync(new Uri(asset.AssetUri, UriKind.RelativeOrAbsolute), progressBar);
            }

            foreach (var asset in GameObjectTemplates.PLAYER_SHIP_THRUST_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var assetUri in GameObjectTemplates.COLLECTIBLE_TEMPLATES)
            {
                await GetFileAsync(assetUri, progressBar);
            }

            foreach (var asset in GameObjectTemplates.BOSS_TEMPLATES)
            {
                await GetFileAsync(asset.AssetUri, progressBar);
            }

            foreach (var assetUri in GameObjectTemplates.GAME_MISC_IMAGE_TEMPLATES)
            {
                await GetFileAsync(assetUri, progressBar);
            }

            #endregion

            #region Sounds

            var prefix = "ms-appx:///";

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.MENU_SELECT_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.GAME_START_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.GAME_OVER_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.PLAYER_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.PLAYER_PLASMA_BOMB_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.PLAYER_BEAM_CANNON_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.PLAYER_SONIC_BLAST_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.ENEMY_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.METEOR_DESTRUCTION_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.ROUNDS_HIT_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.BOSS_DESTRUCTION_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.POWER_UP_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.POWER_DOWN_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.RAGE_UP_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.RAGE_DOWN_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.ENEMY_INCOMING_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.ENEMY_DESTRUCTION_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);


            await GetFileAsync(new Uri(prefix + GameObjectTemplates.HEALTH_GAIN_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.HEALTH_LOSS_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            await GetFileAsync(new Uri(prefix + GameObjectTemplates.COLLECTIBLE_COLLECTED_MUSIC_URL, UriKind.RelativeOrAbsolute), progressBar);

            foreach (var asset in GameObjectTemplates.GAME_INTRO_MUSIC_URLS)
            {
                await GetFileAsync(new Uri(prefix + asset, UriKind.RelativeOrAbsolute), progressBar);
            }

            foreach (var asset in GameObjectTemplates.BOSS_APPEARANCE_MUSIC_URLS)
            {
                await GetFileAsync(new Uri(prefix + asset, UriKind.RelativeOrAbsolute), progressBar);
            }

            foreach (var asset in GameObjectTemplates.BACKGROUND_MUSIC_URLS)
            {
                await GetFileAsync(new Uri(prefix + asset, UriKind.RelativeOrAbsolute), progressBar);
            }

            messageBlock.Text = string.Empty;
            messageBlock.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;

            #endregion            
        }

        private async Task GetFileAsync(Uri uri, ProgressBar progressBar)
        {
            await StorageFile.GetFileFromApplicationUriAsync(uri);
            progressBar.Value++;
        }

        #endregion
    }
}
