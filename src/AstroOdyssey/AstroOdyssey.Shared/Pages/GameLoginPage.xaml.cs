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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameLoginPage_LoginButton.IsEnabled)
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
            if (e.Key == Windows.System.VirtualKey.Enter && GameLoginPage_LoginButton.IsEnabled)
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
            RunProgressBar();

            //TODO: call api to get token
            ServiceResponse response = await _gameApiHelper.Authenticate(GameLoginPage_UserNameBox.Text, GameLoginPage_PasswordBox.Text);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = response.Result as AuthToken;
                Constants.ACCESS_TOKEN = result;

                StopProgressBar();

                //TODO: Navigate to gameScores page.
                App.NavigateToPage(typeof(GameStartPage));
            }
            else
            {                
                var error = response.ExternalError;
                ShowErrorMessage(error);
                ShowErrorProgressBar();
            }
        }

        private void ShowErrorMessage(string error)
        {
            GameLoginPage_ErrorText.Text = error;
            GameLoginPage_ErrorText.Visibility = Visibility.Visible;
        }

        private void RunProgressBar()
        {
            GameLoginPage_ProgressBar.ShowError = false;
            GameLoginPage_ProgressBar.ShowPaused = false;
        }

        private void StopProgressBar()
        {
            GameLoginPage_ProgressBar.ShowError = false;
            GameLoginPage_ProgressBar.ShowPaused = true;
        }

        private void ShowErrorProgressBar()
        {
            GameLoginPage_ProgressBar.ShowPaused = true;
            GameLoginPage_ProgressBar.ShowError = true;
        }

        private void EnableLoginButton()
        {
            GameLoginPage_LoginButton.IsEnabled = !GameLoginPage_UserNameBox.Text.IsNullOrBlank() && !GameLoginPage_PasswordBox.Text.IsNullOrBlank();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

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
