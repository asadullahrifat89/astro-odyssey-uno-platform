using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    public sealed partial class GameLoginPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;
        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;
        private readonly ICacheHelper _cacheHelper;

        private readonly ProgressBar _progressBar;
        private readonly TextBlock _errorContainer;
        private readonly Button[] _actionButtons;

        #endregion

        #region Ctor

        public GameLoginPage()
        {
            InitializeComponent();
            Loaded += GameLoginPage_Loaded;

            _gameApiHelper = App.Container.GetService<IGameApiHelper>();
            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
            _cacheHelper = App.Container.GetService<ICacheHelper>();

            _progressBar = GameLoginPage_ProgressBar;
            _errorContainer = GameLoginPage_ErrorText;
            _actionButtons = new[] { GameLoginPage_LoginButton, GameLoginPage_RegisterButton };
        }

        #endregion

        #region Events

        private async void GameLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            // if user was already logged in or came here after sign up
            if (_cacheHelper.GetCachedPlayerCredentials() is PlayerCredentials authCredentials && !authCredentials.UserName.IsNullOrBlank() && !authCredentials.Password.IsNullOrBlank())
            {
                GameLoginPage_UserNameBox.Text = authCredentials.UserName;
                GameLoginPage_PasswordBox.Text = authCredentials.Password;
            }
            else
            {
                GameLoginPage_UserNameBox.Text = null;
                GameLoginPage_PasswordBox.Text = null;
            }

            await this.PlayLoadedTransition();
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
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(GameSignupPage));
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameStartPage));
        }


        #endregion

        #region Methods

        private async Task PerformLogin()
        {
            RunProgressBar();

            if (!await Authenticate())
                return;

            if (!await GetGameProfile())
                return;

            if (!await GenerateSession())
                return;

            if (App.GameScoreSubmissionPending)
            {
                if (await SubmitScore())
                    App.GameScoreSubmissionPending = false;
            }

            StopProgressBar();

            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(GameLeaderboardPage));
        }

        private async Task<bool> Authenticate()
        {
            // authenticate
            ServiceResponse response = await _gameApiHelper.Authenticate(
                userNameOrEmail: GameLoginPage_UserNameBox.Text.Trim(),
                password: GameLoginPage_PasswordBox.Text.Trim());

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                     progressBar: _progressBar,
                     messageBlock: _errorContainer,
                     message: error,
                     actionButtons: _actionButtons);

                return false;
            }

            // store auth token
            var authToken = _gameApiHelper.ParseResult<AuthToken>(response.Result);
            App.AuthToken = authToken;

            _cacheHelper.SetCachedPlayerCredentials(
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

        private async Task<bool> SubmitScore()
        {
            ServiceResponse response = await _gameApiHelper.SubmitGameScore(App.PlayerScore.Score);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                     progressBar: _progressBar,
                     messageBlock: _errorContainer,
                     message: error,
                     actionButtons: _actionButtons);

                return false;
            }

            return true;
        }

        private async Task<bool> GenerateSession()
        {
            ServiceResponse response = await _gameApiHelper.GenerateSession(
                gameId: Constants.GAME_ID,
                userId: App.GameProfile.User.UserId);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                    progressBar: _progressBar,
                    messageBlock: _errorContainer,
                    message: error,
                    actionButtons: _actionButtons);

                return false;
            }

            // store session
            var session = _gameApiHelper.ParseResult<Session>(response.Result);
            _cacheHelper.SetCachedSession(session);

            return true;
        }

        private void EnableLoginButton()
        {
            GameLoginPage_LoginButton.IsEnabled = !GameLoginPage_UserNameBox.Text.IsNullOrBlank() && !GameLoginPage_PasswordBox.Text.IsNullOrBlank();
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
            _localizationHelper.SetLocalizedResource(ApplicationName_Header);
            _localizationHelper.SetLocalizedResource(GameLoginPage_UserNameBox);
            _localizationHelper.SetLocalizedResource(GameLoginPage_PasswordBox);
            _localizationHelper.SetLocalizedResource(GameLoginPage_RegisterButton);
            _localizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
        }

        #endregion
    }
}
