using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameLoginPage : Page
    {
        #region Fields

        private readonly IBackendService _backendService;

        #endregion

        #region Ctor

        public GameLoginPage()
        {
            InitializeComponent();
            Loaded += GameLoginPage_Loaded;

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();
        }

        #endregion

        #region Events

        private void GameLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();

            // if user was already logged in or came here after sign up
            if (PlayerCredentialsHelper.GetCachedPlayerCredentials() is PlayerCredentials authCredentials && !authCredentials.UserName.IsNullOrBlank() && !authCredentials.Password.IsNullOrBlank())
            {
                GameLoginPage_UserNameBox.Text = authCredentials.UserName;
                GameLoginPage_PasswordBox.Text = authCredentials.Password;
            }
            else
            {
                GameLoginPage_UserNameBox.Text = null;
                GameLoginPage_PasswordBox.Text = null;
            }
        }

        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && GameLoginPage_LoginButton.IsEnabled)
                await PerformLogin();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameLoginPage_LoginButton.IsEnabled)
                await PerformLogin();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameSignupPage));
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameStartPage));
        }

        #endregion

        #region Methods

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        private async Task PerformLogin()
        {
            this.RunProgressBar();

            if (await Authenticate() && await GetGameProfile() && await GenerateSession())
            {
                if (PlayerScoreHelper.GameScoreSubmissionPending)
                {
                    if (await SubmitScore())
                        PlayerScoreHelper.GameScoreSubmissionPending = false;
                }

                this.StopProgressBar();
                NavigateToPage(typeof(GameLeaderboardPage));
            }
        }

        private async Task<bool> Authenticate()
        {
            (bool IsSuccess, string Message) = await _backendService.AuthenticateUser(
                userNameOrEmail: GameLoginPage_UserNameBox.Text.Trim(),
                password: GameLoginPage_PasswordBox.Text.Trim());

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
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

        private async Task<bool> GenerateSession()
        {
            (bool IsSuccess, string Message) = await _backendService.GenerateUserSession();

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private async Task<bool> SubmitScore()
        {
            (bool IsSuccess, string Message) = await _backendService.SubmitUserGameScore(PlayerScoreHelper.PlayerScore.Score);

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private void EnableLoginButton()
        {
            GameLoginPage_LoginButton.IsEnabled = !GameLoginPage_UserNameBox.Text.IsNullOrBlank() && !GameLoginPage_PasswordBox.Text.IsNullOrBlank();
        }

        #endregion
    }
}
