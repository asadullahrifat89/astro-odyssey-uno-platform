using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace AstroOdyssey
{
    public class AssetHelper : IAssetHelper
    {
        private readonly List<StorageFile> _assetFiles;

        public AssetHelper()
        {
            _assetFiles = new List<StorageFile>();
        }

        public async Task PreloadAssets()
        {
            if (_assetFiles.Count == 0)
            {
                #region Images

                foreach (var asset in GameObjectTemplates.PLAYER_RAGE_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.STAR_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.PLANET_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.ENEMY_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.METEOR_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.PLAYER_SHIP_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(asset.AssetUri, UriKind.RelativeOrAbsolute));
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.PLAYER_SHIP_THRUST_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.COLLECTIBLE_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.BOSS_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset.AssetUri);
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.GAME_MISC_IMAGE_TEMPLATES)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(asset);
                    _assetFiles.Add(file);
                }

                #endregion

                #region Sounds

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.MENU_SELECT_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.GAME_START_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.GAME_OVER_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.PLAYER_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.PLAYER_PLASMA_BOMB_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.PLAYER_BEAM_CANNON_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.PLAYER_SONIC_BLAST_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.ENEMY_ROUNDS_FIRE_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.METEOR_DESTRUCTION_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.ROUNDS_HIT_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.BOSS_DESTRUCTION_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.POWER_UP_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.POWER_DOWN_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.RAGE_UP_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.RAGE_DOWN_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.ENEMY_INCOMING_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.ENEMY_DESTRUCTION_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.HEALTH_GAIN_MUSIC_URL, UriKind.RelativeOrAbsolute)));
                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.HEALTH_LOSS_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                _assetFiles.Add(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + GameObjectTemplates.COLLECTIBLE_COLLECTED_MUSIC_URL, UriKind.RelativeOrAbsolute)));

                foreach (var asset in GameObjectTemplates.GAME_INTRO_MUSIC_URLS)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + asset, UriKind.RelativeOrAbsolute));
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.BOSS_APPEARANCE_MUSIC_URLS)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + asset, UriKind.RelativeOrAbsolute));
                    _assetFiles.Add(file);
                }

                foreach (var asset in GameObjectTemplates.BACKGROUND_MUSIC_URLS)
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + asset, UriKind.RelativeOrAbsolute));
                    _assetFiles.Add(file);
                }

                #endregion
            }
        }
    }
}
