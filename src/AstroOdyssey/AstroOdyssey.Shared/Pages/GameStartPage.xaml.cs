using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;

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

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.GAME_INTRO);

            SetLocalization();

            await this.PlayPageLoadedTransition();

            PreloadAssets();
        }

        private async void PreloadAssets()
        {
            if (AssetsPreloadGrid.Children is null || AssetsPreloadGrid.Children.Count == 0)
            {
                foreach (var asset in Constants.STAR_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in Constants.PLANET_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in Constants.ENEMY_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in Constants.METEOR_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in Constants.PLAYER_SHIP_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(new Uri(asset.AssetUri, UriKind.RelativeOrAbsolute));
                    AssetsPreloadGrid.Children.Add(content);
                }

                Image gameOver = new Image() { Stretch = Stretch.Uniform };
                gameOver.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/gameOver.png", UriKind.RelativeOrAbsolute));
                AssetsPreloadGrid.Children.Add(gameOver);

                Image space_thrust1 = new Image() { Stretch = Stretch.Uniform };
                gameOver.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/space_thrust1.png", UriKind.RelativeOrAbsolute));
                AssetsPreloadGrid.Children.Add(gameOver);

                Image space_thrust2 = new Image() { Stretch = Stretch.Uniform };
                gameOver.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/space_thrust2.png", UriKind.RelativeOrAbsolute));
                AssetsPreloadGrid.Children.Add(gameOver);

                Image space_thrust3 = new Image() { Stretch = Stretch.Uniform };
                gameOver.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/space_thrust3.png", UriKind.RelativeOrAbsolute));
                AssetsPreloadGrid.Children.Add(gameOver);
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
            App.EnterFullScreen(true);
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tag)
            {
                App.CurrentCulture = tag;
                SetLocalization();
            }
        }

        #endregion

        #region Methods

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(GameStartPage_EnglishButton);
            LocalizationHelper.SetLocalizedResource(GameStartPage_FrenchButton);
            LocalizationHelper.SetLocalizedResource(GameStartPage_DeutschButton);
            LocalizationHelper.SetLocalizedResource(GameStartPage_BanglaButton);
            LocalizationHelper.SetLocalizedResource(GameStartPage_Tagline);
            LocalizationHelper.SetLocalizedResource(GameStartPage_PlayButton);
            LocalizationHelper.SetLocalizedResource(GameStartPage_DeveloperProfileButton);
            LocalizationHelper.SetLocalizedResource(GameStartPage_AssetsCreditButton);
        }

        #endregion
    }
}
