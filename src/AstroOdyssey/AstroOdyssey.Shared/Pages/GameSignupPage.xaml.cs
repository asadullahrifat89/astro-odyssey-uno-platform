using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using AstroOdysseyCore;
using System.Threading.Tasks;

namespace AstroOdyssey
{
    public sealed partial class GameSignupPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;

        #endregion

        #region Ctor

        public GameSignupPage()
        {
            this.InitializeComponent();
            Loaded += GameSignupPage_Loaded;

            // Get a local instance of the container
            var container = ((App)App.Current).Container;
            _gameApiHelper = (IGameApiHelper)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(GameApiHelper));
        }

        #endregion

        #region Events

        private async void GameSignupPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
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

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && GameSignupPage_SignupButton.IsEnabled)
                PerformSignup();
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameSignupPage_SignupButton.IsEnabled)
                PerformSignup();
        }


        #endregion

        #region Methods

        private async Task<bool> Signup()
        {
            // sign up
            ServiceResponse response = await _gameApiHelper.Signup(
                userName: GameSignupPage_UserNameBox.Text,
                email: GameSignupPage_UserEmailBox.Text,
                password: GameSignupPage_PasswordBox.Text);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response.ExternalError;
                this.ShowErrorMessage(errorContainer: GameSignupPage_ErrorText, error: error);
                this.ShowErrorProgressBar(GameSignupPage_ProgressBar);

                return false;
            }

            // store game profile
            var gameProfile = _gameApiHelper.ParseResult<GameProfile>(response.Result);
            App.GameProfile = gameProfile;

            return true;
        }

        private async void PerformSignup()
        {
            this.RunProgressBar(GameSignupPage_ProgressBar);

            if (!await Signup())
                return;            

            this.StopProgressBar(GameSignupPage_ProgressBar);

            // redirect to login page
            await this.PlayPageUnLoadedTransition();            
            App.NavigateToPage(typeof(GameLoginPage));
        }

        private void EnableSignupButton()
        {
            GameSignupPage_SignupButton.IsEnabled = !GameSignupPage_UserNameBox.Text.IsNullOrBlank()
                && !GameSignupPage_PasswordBox.Text.IsNullOrBlank()
                && !GameSignupPage_UserEmailBox.Text.IsNullOrBlank();
        }

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(ApplicationName_Header);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_UserEmailBox);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_UserNameBox);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_PasswordBox);
            LocalizationHelper.SetLocalizedResource(GameSignupPage_SignupButton);
        }

        #endregion
    }
}
