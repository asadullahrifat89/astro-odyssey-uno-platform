using AstroOdysseyCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;

        #endregion

        #region Ctor

        public LoginPage()
        {
            InitializeComponent();
            Loaded += LoginPage_Loaded;

            // Get a local instance of the container
            var container = ((App)App.Current).Container;

            _gameApiHelper = (IGameApiHelper)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(GameApiHelper));
        }

        #endregion

        #region Events

        private async void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
            await this.PlayPageLoadedTransition();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginPage_LoginButton.IsEnabled)
            {
                Login();
            }
        }

        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        private void PasswordBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && LoginPage_LoginButton.IsEnabled)
                Login();
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableLoginButton();
        }

        #endregion

        #region Methods

        private async void Login()
        {
            LoginPage_ProgressBar.ShowError = false;
            LoginPage_ProgressBar.ShowPaused = false;

            //TODO: call api to get token
            ServiceResponse response = await _gameApiHelper.Authenticate(LoginPage_UserNameBox.Text, LoginPage_PasswordBox.Text);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = response.Result as AuthToken;
                Constants.ACCESS_TOKEN = result;

                App.NavigateToPage(typeof(GameStartPage));
            }
            else
            {
                LoginPage_ProgressBar.ShowPaused = true;
                LoginPage_ProgressBar.ShowError = true;

                var error  = response.ExternalError;
                LoginPage_ErrorText.Text = error;
                LoginPage_ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void EnableLoginButton()
        {
            LoginPage_LoginButton.IsEnabled = !LoginPage_UserNameBox.Text.IsNullOrBlank() && !LoginPage_PasswordBox.Text.IsNullOrBlank();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(ApplicationName_Header);
            LocalizationHelper.SetLocalizedResource(LoginPage_UserNameBox);
            LocalizationHelper.SetLocalizedResource(LoginPage_PasswordBox);
            LocalizationHelper.SetLocalizedResource(LoginPage_RegisterButton);
            LocalizationHelper.SetLocalizedResource(LoginPage_LoginButton);
        }

        #endregion
    }
}
