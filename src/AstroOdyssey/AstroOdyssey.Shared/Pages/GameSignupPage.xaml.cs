using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public sealed partial class GameSignupPage : Page
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

        public GameSignupPage()
        {
            this.InitializeComponent();
            Loaded += GameSignupPage_Loaded;

            _gameApiHelper = App.Container.GetService<IGameApiHelper>();
            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
            _cacheHelper = App.Container.GetService<ICacheHelper>();

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

            await this.PlayLoadedTransition();
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
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(GameStartPage));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameLoginPage));
        }

        #endregion

        #region Methods

        private async Task PerformSignup()
        {
            RunProgressBar();

            if (await Signup() && await Authenticate())
            {
                StopProgressBar();

                _audioHelper.PlaySound(SoundType.MENU_SELECT);
                await this.PlayUnLoadedTransition();
                App.NavigateToPage(typeof(GameLoginPage));
            }
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
                     messageBlock: _errorContainer,
                     message: error,
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
                    messageBlock: _errorContainer,
                    message: error,
                    actionButtons: _actionButtons);

                return false;
            }

            // store auth token
            var authToken = _gameApiHelper.ParseResult<AuthToken>(response.Result);
            App.AuthToken = authToken;

            _cacheHelper.SetCachedPlayerCredentials(
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
            _localizationHelper.SetLocalizedResource(GameSignupPage_UserEmailBox);
            _localizationHelper.SetLocalizedResource(GameSignupPage_UserNameBox);
            _localizationHelper.SetLocalizedResource(GameSignupPage_PasswordBox);
            _localizationHelper.SetLocalizedResource(GameSignupPage_SignupButton);
            _localizationHelper.SetLocalizedResource(GameSignupPage_LoginButton);
        }

        #endregion
    }
}
