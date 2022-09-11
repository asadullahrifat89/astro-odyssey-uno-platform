using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public sealed partial class GameSignupPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;
        private readonly IAudioHelper _audioHelper;

        private readonly ProgressBar _progressBar;
        private readonly TextBlock _errorContainer;
        private readonly Button[] _actionButtons;

        #endregion

        #region Ctor

        public GameSignupPage()
        {
            this.InitializeComponent();
            Loaded += GameSignupPage_Loaded;

            _gameApiHelper = App.Container.GetService<IGameApiHelper>();
            _audioHelper = App.Container.GetService<IAudioHelper>();

            _progressBar = GameSignupPage_ProgressBar;
            _errorContainer = GameSignupPage_ErrorText;
            _actionButtons = new[] { GameSignupPage_SignupButton, GameSignupPage_LoginButton };
        }

        #endregion

        #region Events

        private async void GameSignupPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            GameSignupPage_UserEmailBox.Text = " ";
            GameSignupPage_UserNameBox.Text = " ";
            GameSignupPage_PasswordBox.Text = "";

            await this.PlayPageLoadedTransition();
        }

        private void UserEmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && GameSignupPage_SignupButton.IsEnabled)
                await PerformSignup();
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameSignupPage_SignupButton.IsEnabled)
                await PerformSignup();
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameStartPage));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameLoginPage));
        }

        #endregion

        #region Methods

        private async Task PerformSignup()
        {
            this.RunProgressBar(
                progressBar: _progressBar,
                errorContainer: _errorContainer,
                actionButtons: _actionButtons);

            if (!await Signup())
                return;

            if (!await Authenticate())
                return;

            this.StopProgressBar(
                progressBar: _progressBar,
                actionButtons: _actionButtons);

            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            // redirect to login page
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameLoginPage));
        }

        private async Task<bool> Signup()
        {
            // sign up
            ServiceResponse response = await _gameApiHelper.Signup(
                userName: GameSignupPage_UserNameBox.Text.Trim(),
                email: GameSignupPage_UserEmailBox.Text.Trim(),
                password: GameSignupPage_PasswordBox.Text.Trim());

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                     progressBar: _progressBar,
                     errorContainer: _errorContainer,
                     error: error,
                     actionButtons: _actionButtons);

                return false;
            }

            // store game profile
            var gameProfile = _gameApiHelper.ParseResult<GameProfile>(response.Result);
            App.GameProfile = gameProfile;

            return true;
        }

        private async Task<bool> Authenticate()
        {
            // authenticate
            ServiceResponse response = await _gameApiHelper.Authenticate(
                userNameOrEmail: GameSignupPage_UserNameBox.Text.Trim(),
                password: GameSignupPage_PasswordBox.Text.Trim());

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                    progressBar: _progressBar,
                    errorContainer: _errorContainer,
                    error: error,
                    actionButtons: _actionButtons);

                return false;
            }

            // store auth token
            var authToken = _gameApiHelper.ParseResult<AuthToken>(response.Result);
            App.AuthToken = authToken;

            CacheHelper.SetCachedPlayerCredentials(
                userName: GameSignupPage_UserNameBox.Text.Trim(),
                password: GameSignupPage_PasswordBox.Text.Trim());

            return true;
        }

        private void EnableSignupButton()
        {
            GameSignupPage_SignupButton.IsEnabled = !GameSignupPage_UserNameBox.Text.IsNullOrBlank()
                && !GameSignupPage_PasswordBox.Text.IsNullOrBlank()
                && !GameSignupPage_UserEmailBox.Text.IsNullOrBlank()
                && StringExtensions.IsValidEmail(GameSignupPage_UserEmailBox.Text);
        }

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(ApplicationName_Header);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_UserEmailBox);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_UserNameBox);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_PasswordBox);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_SignupButton);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_LoginButton);
        }

        #endregion
    }
}
