using Microsoft.Extensions.DependencyInjection;
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
        #region Fields

        private readonly IAudioHelper _audioHelper; 

        #endregion

        #region Ctor

        public GameStartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;            
            _audioHelper = App.Container.GetService<IAudioHelper>();
        }

        #endregion

        #region Events

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.GAME_INTRO);

            SetLocalization();

            await this.PlayPageLoadedTransition();

            PreloadAssets();
        }

        private async void PreloadAssets()
        {
            if (AssetsPreloadGrid.Children is null || AssetsPreloadGrid.Children.Count == 0)
            {
                foreach (var asset in GameObjectTemplates.STAR_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.PLANET_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.ENEMY_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.METEOR_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.PLAYER_SHIP_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(new Uri(asset.AssetUri, UriKind.RelativeOrAbsolute));
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.PLAYER_SHIP_THRUST_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.COLLECTIBLE_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.PLAYER_RAGE_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                await Task.Delay(500);

                foreach (var asset in GameObjectTemplates.BOSS_TEMPLATES)
                {
                    Image content = new Image() { Stretch = Stretch.Uniform };
                    content.Source = new BitmapImage(asset.AssetUri);
                    AssetsPreloadGrid.Children.Add(content);
                }

                Image gameOver = new Image() { Stretch = Stretch.Uniform };
                gameOver.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/gameOver.png", UriKind.RelativeOrAbsolute));
                AssetsPreloadGrid.Children.Add(gameOver);
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

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

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameSignupPage));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameLoginPage));
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
            LocalizationHelper.SetLocalizedResource(ApplicationName_Header);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_RegisterButton);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
        }

        #endregion
    }
}
