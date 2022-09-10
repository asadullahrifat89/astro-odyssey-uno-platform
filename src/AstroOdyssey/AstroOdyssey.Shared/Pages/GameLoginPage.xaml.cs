using AstroOdysseyCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;

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

        #endregion

        #region Ctor

        public GameLoginPage()
        {
            InitializeComponent();
            Loaded += GameLoginPage_Loaded;

            // Get a local instance of the container
            var container = ((App)App.Current).Container;
            _gameApiHelper = (IGameApiHelper)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(GameApiHelper));
        }

        #endregion

        #region Events

        private async void GameLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
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

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && GameLoginPage_LoginButton.IsEnabled)
                Login();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameLoginPage_LoginButton.IsEnabled)
                Login();
        }

        #endregion

        #region Methods

        private async void Login()
        {
            this.RunProgressBar(GameLoginPage_ProgressBar);

            //TODO: call api to get token
            ServiceResponse response = await _gameApiHelper.Authenticate(
                userNameOrEmail: GameLoginPage_UserNameBox.Text,
                password: GameLoginPage_PasswordBox.Text);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var authToken = _gameApiHelper.ParseResult<AuthToken>(response.Result);
                App.AuthToken = authToken;

#if DEBUG
                Console.WriteLine("AuthToken:" + App.AuthToken?.Token);
#endif

                this.StopProgressBar(GameLoginPage_ProgressBar);

                await this.PlayPageUnLoadedTransition();
                //TODO: Navigate to gameScores page.
                App.NavigateToPage(typeof(GameStartPage));
            }
            else
            {
                var error = response.ExternalError;
                this.ShowErrorMessage(errorContainer: GameLoginPage_ErrorText, error: error);
                this.ShowErrorProgressBar(GameLoginPage_ProgressBar);
            }
        }

        private void EnableLoginButton()
        {
            GameLoginPage_LoginButton.IsEnabled = !GameLoginPage_UserNameBox.Text.IsNullOrBlank() && !GameLoginPage_PasswordBox.Text.IsNullOrBlank();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(GameSignupPage));
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
