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
        private readonly IGameApiHelper _gameApiHelper;

        private readonly ProgressBar _progressBar;
        private readonly TextBlock _errorContainer;
        private readonly Button[] _actionButtons;

        #endregion

        #region Ctor

        public GameStartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;

            _audioHelper = App.Container.GetService<IAudioHelper>();
            _gameApiHelper = App.Container.GetService<IGameApiHelper>();

            _progressBar = GameStartPage_ProgressBar;
            _errorContainer = GameStartPage_ErrorText;
            _actionButtons = new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton, GameStartPage_LogoutButton };
        }

        #endregion

        #region Events

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.RunProgressBar(
                progressBar: _progressBar,
                errorContainer: _errorContainer,
                actionButtons: _actionButtons);

            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.GAME_INTRO);

            if (CacheHelper.GetCachedValue(Constants.CACHE_LANGUAGE_KEY) is string language)
                App.CurrentCulture = language;

            SetLocalization();

            await this.PlayPageLoadedTransition();

            // check for session
            if (CacheHelper.GetCachedSession() is Session session && await ValidateSession(session))
            {
                await GetGameProfile();

                // make logout button visible
                GameStartPage_LogoutButton.Visibility = Visibility.Visible;
                GameLoginPage_LoginButton.Visibility = Visibility.Collapsed;
                GameLoginPage_RegisterButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // if session is not valid then remove it
                CacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);

                // make login button visible
                GameStartPage_LogoutButton.Visibility = Visibility.Collapsed;
                GameLoginPage_LoginButton.Visibility = Visibility.Visible;
                GameLoginPage_RegisterButton.Visibility = Visibility.Visible;
            }

            this.StopProgressBar(
                progressBar: _progressBar,
                actionButtons: _actionButtons);

            PreloadAssets();
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
            App.EnterFullScreen(true);
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameSignupPage));
            App.EnterFullScreen(true);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameLoginPage));
            App.EnterFullScreen(true);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            // delete session
            CacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);
            GameStartPage_LogoutButton.Visibility = Visibility.Collapsed;
            GameLoginPage_LoginButton.Visibility = Visibility.Visible;
            GameLoginPage_RegisterButton.Visibility = Visibility.Visible;
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tag)
            {
                App.CurrentCulture = tag;
                SetLocalization();
                CacheHelper.SetCachedValue(Constants.CACHE_LANGUAGE_KEY, tag);
            }
        }

        #endregion

        #region Methods

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

        private async Task<bool> ValidateSession(Session session)
        {
            ServiceResponse response = await _gameApiHelper.ValidateSession(Constants.GAME_ID, session.SessionId);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return false;

            // store auth token
            var authToken = _gameApiHelper.ParseResult<AuthToken>(response.Result);
            App.AuthToken = authToken;

            return true;
        }

        private async Task<bool> GetGameProfile()
        {
            // get game profile
            var recordResponse = await _gameApiHelper.GetGameProfile();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                this.ShowError(
                  progressBar: GameStartPage_ProgressBar,
                  errorContainer: GameStartPage_ErrorText,
                  error: string.Join("\n", error),
                  GameLoginPage_LoginButton,
                  GameLoginPage_RegisterButton,
                  GameStartPage_LogoutButton);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            return true;
        }

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
            LocalizationHelper.SetLocalizedResource(GameStartPage_LogoutButton);
        }

        #endregion
    }
}
