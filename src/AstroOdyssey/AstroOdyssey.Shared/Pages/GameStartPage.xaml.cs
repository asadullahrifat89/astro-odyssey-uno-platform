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
        private readonly ILocalizationHelper _localizationHelper;
        private readonly ICacheHelper _cacheHelper;

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
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
            _cacheHelper = App.Container.GetService<ICacheHelper>();

            _progressBar = GameStartPage_ProgressBar;
            _errorContainer = GameStartPage_ErrorText;
            _actionButtons = new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton, GameStartPage_LogoutButton, GameStartPage_PlayButton };
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

            CheckCachedLocalization();
            SetLocalization();

            await this.PlayLoadedTransition();

            await SetLoginControls();

            this.StopProgressBar(
                progressBar: _progressBar,
                actionButtons: _actionButtons);

            PreloadAssets();
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
            App.EnterFullScreen(true);
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(GameSignupPage));
            App.EnterFullScreen(true);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(GameLoginPage));
            App.EnterFullScreen(true);
        }

        private async void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(GameLeaderboardPage));
            App.EnterFullScreen(true);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            // delete session
            PerformLogout();
            GameStartPage_LogoutButton.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
            GameLoginPage_LoginButton.Visibility = Visibility.Visible;
            GameLoginPage_RegisterButton.Visibility = Visibility.Visible;
        }

        private void PerformLogout()
        {
            _cacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);
            App.AuthToken = null;
            App.GameProfile = null;
            App.GameScore = null;
            App.Ship = null;
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tag)
            {
                App.CurrentCulture = tag;
                SetLocalization();
                _cacheHelper.SetCachedValue(Constants.CACHE_LANGUAGE_KEY, tag);
            }
        }

        #endregion

        #region Methods

        private void CheckCachedLocalization()
        {
            if (_cacheHelper.GetCachedValue(Constants.CACHE_LANGUAGE_KEY) is string language)
                App.CurrentCulture = language;
        }

        private async Task SetLoginControls()
        {
            if (App.HasUserLoggedIn)
            {
                if (_cacheHelper.HasSessionExpired())
                {
                    _cacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);
                    MakeLoginControlsVisible();
                }
                else
                {
                    MakeLogoutControlsVisible();
                }
            }
            else
            {
                if (_cacheHelper.HasSessionExpired())
                {
                    _cacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);
                    MakeLoginControlsVisible();
                }
                else // if a non expired session exists then validate it, get a new auth token, and get game profile
                {
                    if (_cacheHelper.GetCachedSession() is Session session && await ValidateSession(session) && await GetGameProfile())
                    {
                        MakeLogoutControlsVisible();
                        ShowWelcomeBackToast();
                    }
                    else
                    {
                        MakeLoginControlsVisible();
                    }
                }
            }
        }

        private async void ShowWelcomeBackToast()
        {
            _audioHelper.PlaySound(SoundType.POWER_UP);
            GameStartPage_UserName.Text = App.GameProfile.User.UserName;
            await WelcomeBackToast.PlayLoadedTransition();
            await Task.Delay(TimeSpan.FromSeconds(5));
            await WelcomeBackToast.PlayUnLoadedTransition();
        }

        private void MakeLogoutControlsVisible()
        {
            // make logout button visible
            GameStartPage_LogoutButton.Visibility = Visibility.Visible;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
            GameLoginPage_LoginButton.Visibility = Visibility.Collapsed;
            GameLoginPage_RegisterButton.Visibility = Visibility.Collapsed;
        }

        private void MakeLoginControlsVisible()
        {
            // make login button visible
            GameStartPage_LogoutButton.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
            GameLoginPage_LoginButton.Visibility = Visibility.Visible;
            GameLoginPage_RegisterButton.Visibility = Visibility.Visible;
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
            var recordResponse = await _gameApiHelper.GetGameProfile();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                this.ShowError(
                  progressBar: _progressBar,
                  errorContainer: _errorContainer,
                  error: string.Join("\n", error),
                  actionButtons: _actionButtons);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            return true;
        }

        private void SetLocalization()
        {
            _localizationHelper.SetLocalizedResource(GameStartPage_EnglishButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_FrenchButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_DeutschButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_BanglaButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_Tagline);
            _localizationHelper.SetLocalizedResource(GameStartPage_PlayButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_DeveloperProfileButton);
            _localizationHelper.SetLocalizedResource(ApplicationName_Header);
            _localizationHelper.SetLocalizedResource(GameLoginPage_RegisterButton);
            _localizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_LogoutButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_WelcomeBackText);
            _localizationHelper.SetLocalizedResource(GameOverPage_LeaderboardButton);
            //_localizationHelper.SetLocalizedResource(GameStartPage_AssetsCreditButton);
        }

        #endregion
    }
}
