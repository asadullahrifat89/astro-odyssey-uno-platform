using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameLoginPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;
        private readonly IAudioHelper _audioHelper;

        #endregion

        #region Ctor

        public GameLoginPage()
        {
            InitializeComponent();
            Loaded += GameLoginPage_Loaded;

            _gameApiHelper = App.Container.GetService<IGameApiHelper>();
            _audioHelper = App.Container.GetService<IAudioHelper>();
        }

        #endregion

        #region Events

        private async void GameLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            if (AuthCredentialsCacheHelper.GetCachedAuthCredentials() is PlayerAuthCredentials authCredentials && !authCredentials.UserName.IsNullOrBlank() && !authCredentials.Password.IsNullOrBlank())
            {
                GameLoginPage_UserNameBox.Text = authCredentials.UserName;
                GameLoginPage_PasswordBox.Text = authCredentials.Password;
            }
            else
            {
                GameLoginPage_UserNameBox.Text = null;
                GameLoginPage_PasswordBox.Text = null;
            }

            await this.PlayPageLoadedTransition();
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

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameSignupPage));
        }

        #endregion

        #region Methods

        private async Task PerformLogin()
        {
            this.RunProgressBar(
                progressBar: GameLoginPage_ProgressBar,
                errorContainer: GameLoginPage_ErrorText,
                actionButtons: new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton });

            if (!await Authenticate())
                return;

            if (!await GetGameProfile())
                return;

            if (!await GenerateSession())
                return;

            if (App.GameScoreSubmissionPending)
            {
                if (!await SubmitScore())
                    return;

                App.GameScoreSubmissionPending = false;
            }

            this.StopProgressBar(
                progressBar: GameLoginPage_ProgressBar,
                actionButtons: new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton });

            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameLeaderboardPage));
        }

        private async Task<bool> Authenticate()
        {
            // authenticate
            ServiceResponse response = await _gameApiHelper.Authenticate(
                userNameOrEmail: GameLoginPage_UserNameBox.Text.Trim(),
                password: GameLoginPage_PasswordBox.Text.Trim());

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response.ExternalError;
                this.ShowError(
                    progressBar: GameLoginPage_ProgressBar,
                    errorContainer: GameLoginPage_ErrorText,
                    error: error,
                    GameLoginPage_LoginButton,
                    GameLoginPage_RegisterButton);

                return false;
            }

            // store auth token
            var authToken = _gameApiHelper.ParseResult<AuthToken>(response.Result);
            App.AuthToken = authToken;

            AuthCredentialsCacheHelper.SetCachedAuthCredentials(
                userName: GameLoginPage_UserNameBox.Text.Trim(),
                password: GameLoginPage_PasswordBox.Text.Trim());

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
                  progressBar: GameLoginPage_ProgressBar,
                  errorContainer: GameLoginPage_ErrorText,
                  error: string.Join("\n", error),
                  GameLoginPage_LoginButton,
                  GameLoginPage_RegisterButton);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            return true;
        }

        private async Task<bool> SubmitScore()
        {
            this.RunProgressBar(
                progressBar: GameLoginPage_ProgressBar,
                errorContainer: GameLoginPage_ErrorText,
                actionButtons: new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton });

            ServiceResponse response = await _gameApiHelper.SubmitGameScore(App.GameScore.Score);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response.ExternalError;
                this.ShowError(
                     progressBar: GameLoginPage_ProgressBar,
                     errorContainer: GameLoginPage_ErrorText,
                     error: error,
                     GameLoginPage_LoginButton,
                     GameLoginPage_RegisterButton);

                return false;
            }

            return true;
        }

        private async Task<bool> GenerateSession()
        {
            ServiceResponse response = await _gameApiHelper.GenerateSession(
                gameId: Constants.GAME_ID,
                userId: App.GameProfile.User.UserId);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response.ExternalError;
                this.ShowError(
                    progressBar: GameLoginPage_ProgressBar,
                    errorContainer: GameLoginPage_ErrorText,
                    error: error,
                    GameLoginPage_LoginButton,
                    GameLoginPage_RegisterButton);

                return false;
            }

            // store session
            var session = _gameApiHelper.ParseResult<Session>(response.Result);
            AuthCredentialsCacheHelper.SetCachedSession(session);

            return true;
        }

        private void EnableLoginButton()
        {
            GameLoginPage_LoginButton.IsEnabled = !GameLoginPage_UserNameBox.Text.IsNullOrBlank() && !GameLoginPage_PasswordBox.Text.IsNullOrBlank();
        }

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(ApplicationName_Header);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_UserNameBox);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_PasswordBox);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_RegisterButton);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
        }

        #endregion
    }
}
