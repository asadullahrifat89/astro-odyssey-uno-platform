using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameStartPage : Page
    {
        #region Fields

        private readonly IBackendService _backendService;

        #endregion

        #region Ctor

        public GameStartPage()
        {
            InitializeComponent();

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();

            Loaded += StartPage_Loaded;
        }

        #endregion

        #region Events

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            LocalizationHelper.CheckLocalizationCache();
            await LocalizationHelper.LoadLocalizationKeys(() =>
            {
                this.SetLocalization();
            });

            //await this.PlayLoadedTransition();

            AudioHelper.LoadGameSounds(() =>
            {
                AudioHelper.StopSound();
                AudioHelper.PlaySound(SoundType.INTRO);
                AssetHelper.PreloadAssets(progressBar: ProgressBar, messageBlock: ProgressBarMessageBlock);
            });

            await CheckUserSession();
        }

        private void HowToPlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameInstructionsPage));
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ShipSelectionPage));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameSignupPage));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLoginPage));
        }

        private void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLeaderboardPage));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            PerformLogout();
        }

        private void PerformLogout()
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            SessionHelper.RemoveCachedSession();
            AuthTokenHelper.AuthToken = null;
            GameProfileHelper.GameProfile = null;
            PlayerScoreHelper.PlayerScore = null;
            App.Ship = null;

            SetLoginContext();
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string tag)
            {
                AudioHelper.PlaySound(SoundType.MENU_SELECT);

                LocalizationHelper.CurrentCulture = tag;

                if (CookieHelper.IsCookieAccepted())
                    LocalizationHelper.SaveLocalizationCache(tag);

                this.SetLocalization();
            }
        }

        private void GameStartPage_CookieAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            CookieHelper.SetCookieAccepted();
            CookieToast.Visibility = Visibility.Collapsed;
        }

        private void GameStartPage_CookieDeclineButton_Click(object sender, RoutedEventArgs e)
        {
            CookieHelper.SetCookieDeclined();
            CookieToast.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Methods

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
            App.EnterFullScreen(true);
        }

        private async Task CheckUserSession()
        {
            SessionHelper.TryLoadSession();

            if (GameProfileHelper.HasUserLoggedIn())
            {
                if (SessionHelper.HasSessionExpired())
                {
                    SessionHelper.RemoveCachedSession();
                    SetLoginContext();
                }
                else
                {
                    SetLogoutContext();
                }
            }
            else
            {
                if (SessionHelper.HasSessionExpired())
                {
                    SessionHelper.RemoveCachedSession();
                    SetLoginContext();
                    ShowCookieToast();
                }
                else
                {
                    if (SessionHelper.GetCachedSession() is Session session
                        && await ValidateSession(session)
                        && await GetGameProfile())
                    {
                        SetLogoutContext();
                        ShowWelcomeBackToast();
                    }
                    else
                    {
                        SetLoginContext();
                        ShowCookieToast();
                    }
                }
            }
        }

        private async void ShowWelcomeBackToast()
        {
            AudioHelper.PlaySound(SoundType.POWER_UP);
            GameStartPage_UserName.Text = GameProfileHelper.GameProfile.User.UserName;

            WelcomeBackToast.Opacity = 1;
            await Task.Delay(TimeSpan.FromSeconds(5));
            WelcomeBackToast.Opacity = 0;
        }

        private void ShowCookieToast()
        {
            if (!CookieHelper.IsCookieAccepted())
                CookieToast.Visibility = Visibility.Visible;
        }

        private void SetLogoutContext()
        {
            GameStartPage_LogoutButton.Visibility = Visibility.Visible;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
            GameLoginPage_LoginButton.Visibility = Visibility.Collapsed;
            GameLoginPage_RegisterButton.Visibility = Visibility.Collapsed;
        }

        private void SetLoginContext()
        {
            GameStartPage_LogoutButton.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
            GameLoginPage_LoginButton.Visibility = Visibility.Visible;
            GameLoginPage_RegisterButton.Visibility = Visibility.Visible;
        }

        private async Task<bool> ValidateSession(Session session)
        {
            var (IsSuccess, _) = await _backendService.ValidateUserSession(session);
            return IsSuccess;
        }

        private async Task<bool> GetGameProfile()
        {
            (bool IsSuccess, string Message, _) = await _backendService.GetUserGameProfile();

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        #endregion
    }
}
