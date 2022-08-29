using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameStartPage : Page
    {
        #region Ctor

        public GameStartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;
        }

        #endregion

        #region Events  

        private void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.GAME_INTRO);

            if (AssetsPreloadGrid.Children is null || AssetsPreloadGrid.Children.Count == 0)
            {
                foreach (var asset in Constants.STAR_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                foreach (var asset in Constants.PLANET_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                foreach (var asset in Constants.ENEMY_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                foreach (var asset in Constants.METEOR_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                foreach (var asset in Constants.PLAYER_SHIP_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(new Uri(asset.AssetUri, UriKind.RelativeOrAbsolute));
                    AssetsPreloadGrid.Children.Add(content);
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(typeof(ShipSelectionPage));
            App.EnterFullScreen(true);
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tag)
            {
                App.CurrentCulture = tag;
                App.Refresh();
            }
        }

        #endregion
    }
}
