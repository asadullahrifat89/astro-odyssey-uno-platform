using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public sealed partial class GameStartPage : Page
    {
        #region Fields

        private readonly IAudioHelper _audioHelper;
        private readonly IGameApiHelper _gameApiHelper;
        private readonly ILocalizationHelper _localizationHelper;
        private readonly ICacheHelper _cacheHelper;
        private readonly IAssetHelper _assetHelper;

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
            _assetHelper = App.Container.GetService<IAssetHelper>();

            _progressBar = GameStartPage_ProgressBar;
            _errorContainer = GameStartPage_ErrorText;
            _actionButtons = new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton, GameStartPage_LogoutButton, GameStartPage_PlayButton };
        }

        #endregion

        #region Events

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            RunProgressBar();

            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.GAME_INTRO);

            CheckLocalizationCache();
            SetLocalization();

            await this.PlayLoadedTransition();

            await CheckLoginSession();

            StopProgressBar();

            await Task.Delay(500);

            _assetHelper.PreloadAssets(progressBar: _progressBar, messageBlock: _errorContainer);
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));

            await Task.Delay(1000);

            App.EnterFullScreen(true);
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameSignupPage));

            await Task.Delay(1000);

            App.EnterFullScreen(true);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameLoginPage));

            await Task.Delay(1000);

            App.EnterFullScreen(true);
        }

        private async void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameLeaderboardPage));

            await Task.Delay(1000);

            App.EnterFullScreen(true);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

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
            App.PlayerScore = null;
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

        private void CheckLocalizationCache()
        {
            if (_cacheHelper.GetCachedValue(Constants.CACHE_LANGUAGE_KEY) is string language)
                App.CurrentCulture = language;
        }

        private async Task CheckLoginSession()
        {
            if (App.Session is null)
                App.Session = _cacheHelper.GetCachedSession();

            if (App.HasUserLoggedIn)
            {
                if (_cacheHelper.HasSessionExpired())
                {
                    _cacheHelper.RemoveCachedValue(Constants.CACHE_SESSION_KEY);
                    App.Session = null;

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
                    App.Session = null;

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
                    messageBlock: _errorContainer,
                    message: string.Join("\n", error),
                    actionButtons: _actionButtons);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            return true;
        }

        private void RunProgressBar()
        {
            this.RunProgressBar(
                progressBar: _progressBar,
                messageBlock: _errorContainer,
                actionButtons: _actionButtons);
        }

        private void StopProgressBar()
        {
            this.StopProgressBar(
                progressBar: _progressBar,
                actionButtons: _actionButtons);
        }

        private void SetLocalization()
        {
            _localizationHelper.SetLocalizedResource(GameStartPage_EnglishButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_BanglaButton);

            _localizationHelper.SetLocalizedResource(GameStartPage_Tagline);
            _localizationHelper.SetLocalizedResource(GameStartPage_PlayButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_BrandProfileButton);
            _localizationHelper.SetLocalizedResource(ApplicationName_Header);
            _localizationHelper.SetLocalizedResource(GameLoginPage_RegisterButton);
            _localizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_LogoutButton);
            _localizationHelper.SetLocalizedResource(GameStartPage_WelcomeBackText);
            _localizationHelper.SetLocalizedResource(GameOverPage_LeaderboardButton);
        }

        #endregion
    }
}
