﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameSignupPage : Page
    {
        #region Fields

        private readonly IBackendService _backendService;      

        #endregion

        #region Ctor

        public GameSignupPage()
        {
            this.InitializeComponent();
            Loaded += GameSignupPage_Loaded;

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();
        }

        #endregion

        #region Events

        private void GameSignupPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();
        }

        private void UserFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
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

        private void ConfirmPasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSignupButton();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && GameSignupPage_SignupButton.IsEnabled)
                await PerformSignup();
        }

        private void ConfirmCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EnableSignupButton();
        }

        private void ConfirmCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableSignupButton();
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameSignupPage_SignupButton.IsEnabled)
                await PerformSignup();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameStartPage));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLoginPage));
        }

        #endregion

        #region Methods

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        private async Task PerformSignup()
        {
            this.RunProgressBar();

            if (await Signup() && await Authenticate())
            {
                this.StopProgressBar();
                NavigateToPage(typeof(GameLoginPage));
            }
        }

        private async Task<bool> Signup()
        {
            (bool IsSuccess, string Message) = await _backendService.SignupUser(
               fullName: GameSignupPage_UserFullNameBox.Text.Trim(),
               userName: GameSignupPage_UserNameBox.Text.Trim(),
               email: GameSignupPage_UserEmailBox.Text.ToLower().Trim(),
               password: GameSignupPage_PasswordBox.Text.Trim(),
               subscribedNewsletters: GameSignupPage_ConfirmNewsLettersCheckBox.IsChecked.Value);

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private async Task<bool> Authenticate()
        {
            (bool IsSuccess, string Message) = await _backendService.AuthenticateUser(
              userNameOrEmail: GameSignupPage_UserNameBox.Text.Trim(),
              password: GameSignupPage_PasswordBox.Text.Trim());

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private void EnableSignupButton()
        {
            GameSignupPage_SignupButton.IsEnabled =
                !GameSignupPage_UserFullNameBox.Text.IsNullOrBlank()
                && IsValidFullName()
                && IsStrongPassword()
                && DoPasswordsMatch()
                && !GameSignupPage_UserNameBox.Text.IsNullOrBlank()
                && !GameSignupPage_UserEmailBox.Text.IsNullOrBlank()
                && IsValidEmail()
                && GameSignupPage_ConfirmCheckBox.IsChecked == true;
        }

        private bool IsValidFullName()
        {
            var result = StringExtensions.IsValidFullName(GameSignupPage_UserFullNameBox.Text);

            if (!result.IsValid)
                SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource(result.Message), isError: true);
            else
                ProgressBarMessageBlock.Visibility = Visibility.Collapsed;

            return result.IsValid;
        }

        private bool IsStrongPassword()
        {
            var result = StringExtensions.IsStrongPassword(GameSignupPage_PasswordBox.Text);
            SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource(result.Message), isError: !result.IsStrong);

            return result.IsStrong;
        }

        private bool DoPasswordsMatch()
        {
            if (GameSignupPage_PasswordBox.Text.IsNullOrBlank() || GameSignupPage_ConfirmPasswordBox.Text.IsNullOrBlank())
                return false;

            if (GameSignupPage_PasswordBox.Text != GameSignupPage_ConfirmPasswordBox.Text)
            {
                SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource("PASSWORDS_DIDNT_MATCH"), isError: true);

                return false;
            }
            else
            {
                SetProgressBarMessage(message: LocalizationHelper.GetLocalizedResource("PASSWORDS_MATCHED"), isError: false);
            }

            return true;
        }

        private bool IsValidEmail()
        {
            return StringExtensions.IsValidEmail(GameSignupPage_UserEmailBox.Text);
        }     

        private void SetProgressBarMessage(string message, bool isError)
        {
            this.SetProgressBarMessage(
                message: message,
                isError: isError);
        }      

        #endregion
    }
}