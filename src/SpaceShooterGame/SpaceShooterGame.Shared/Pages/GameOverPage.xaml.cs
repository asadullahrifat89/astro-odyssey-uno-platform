﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameOverPage : Page
    {
        #region Fields

        private readonly IBackendService _backendService;

        #endregion

        #region Ctor

        public GameOverPage()
        {
            InitializeComponent();
            Loaded += GameOverPage_Loaded;

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();
        }

        #endregion

        #region Events

        private async void GameOverPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();

            SetGameResults();
            ShowUserName();

            // if user has not logged in or session has expired
            if (!GameProfileHelper.HasUserLoggedIn() || SessionHelper.HasSessionExpired())
            {
                MakeLoginControlsVisible();
            }
            else
            {
                this.RunProgressBar();

                if (await SubmitScore())
                {
                    MakeLeaderboardControlsVisible(); // if score submission was successful make leaderboard button visible
                }
                else
                {
                    MakeLoginControlsVisible();
                }

                this.StopProgressBar();
            }
        }

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ShipSelectionPage));
        }

        private void GameLoginPage_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLoginPage));
        }

        private void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLeaderboardPage));
        }

        #endregion

        #region Methods

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        private void MakeLeaderboardControlsVisible()
        {
            GameOverPage_SignupPromptPanel.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
        }

        private void MakeLoginControlsVisible()
        {
            // submit score on user login, or signup then login
            PlayerScoreHelper.GameScoreSubmissionPending = true;

            GameOverPage_SignupPromptPanel.Visibility = Visibility.Visible;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
        }

        private void SetGameResults()
        {
            ScoreText.Text = LocalizationHelper.GetLocalizedResource("YOUR_SCORE");
            ScoreNumberText.Text = PlayerScoreHelper.PlayerScore.Score.ToString();

            EnemiesDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("ENEMIES_DESTROYED")} x " + PlayerScoreHelper.PlayerScore.EnemiesDestroyed;
            MeteorsDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("METEORS_DESTROYED")} x " + PlayerScoreHelper.PlayerScore.MeteorsDestroyed;
            BossesDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("BOSSES_DESTROYED")} x " + PlayerScoreHelper.PlayerScore.BossesDestroyed;
            CollectiblesCollectedText.Text = $"{LocalizationHelper.GetLocalizedResource("COLLECTIBLES_COLLECTED")} x " + PlayerScoreHelper.PlayerScore.CollectiblesCollected;
        }

        private async Task<bool> SubmitScore()
        {
            //TODO: HIGH SCORE GOAL-> check if player has reached high score goal after this game and show a congratulations dialog

            (bool IsSuccess, string Message) = await _backendService.SubmitUserGameScore(PlayerScoreHelper.PlayerScore.Score);

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
        }

        private void ShowUserName()
        {
            if (GameProfileHelper.HasUserLoggedIn())
            {
                Page_UserName.Text = GameProfileHelper.GameProfile.User.UserName;
                Page_UserPicture.Initials = GameProfileHelper.GameProfile.Initials;
                PlayerNameHolder.Visibility = Visibility.Visible;
            }
            else
            {
                PlayerNameHolder.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}