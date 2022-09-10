using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverPage : Page
    {
        #region Ctor

        public GameOverPage()
        {
            InitializeComponent();
            Loaded += GameOverPage_Loaded;
        }

        #endregion

        #region Events

        private async void GameOverPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            ScoreText.Text = $"{LocalizationHelper.GetLocalizedResource("SCORE")} " + App.GameScore.Score;

            EnemiesDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("ENEMIES_DESTROYED")} x " + App.GameScore.EnemiesDestroyed;
            MeteorsDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("METEORS_DESTROYED")} x " + App.GameScore.MeteorsDestroyed;
            BossesDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("BOSSES_DESTROYED")} x " + App.GameScore.BossesDestroyed;
            CollectiblesCollectedText.Text = $"{LocalizationHelper.GetLocalizedResource("COLLECTIBLES_COLLECTED")} x " + App.GameScore.CollectiblesCollected;

            CongratulationsText.Text = (App.GameScore.Score == 0
                ? LocalizationHelper.GetLocalizedResource("NO_LUCK") : App.GameScore.Score <= 400
                ? LocalizationHelper.GetLocalizedResource("GOOD_GAME") : App.GameScore.Score <= 800
                ? LocalizationHelper.GetLocalizedResource("GREAT_GAME") : App.GameScore.Score <= 1400
                ? LocalizationHelper.GetLocalizedResource("FANTASTIC_GAME") : LocalizationHelper.GetLocalizedResource("SUPREME_GAME")) + "!";

#if DEBUG
            Console.WriteLine("AuthToken:" + App.AuthToken?.Token);
#endif

            // if user has not logged in
            if (App.AuthToken is null || App.AuthToken.Token.IsNullOrBlank())
            {
                GameLoginPage_LoginButton.Visibility = Visibility.Visible;
                GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                GameLoginPage_LoginButton.Visibility = Visibility.Collapsed;
                GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
            }

            await this.PlayPageLoadedTransition();
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));

            AudioHelper.PlaySound(SoundType.GAME_INTRO);
        }

        private async void GameLoginPage_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameLoginPage));
        }

        private async void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            //TODO: go to leaderboard page
            App.NavigateToPage(typeof(GameStartPage));
        }

        #endregion

        #region Methods

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(GameOverPage_Tagline);
            LocalizationHelper.SetLocalizedResource(GameOverPage_PlayAgainButton);
            LocalizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
            LocalizationHelper.SetLocalizedResource(GameOverPage_LeaderboardButton);
        }

        #endregion      
    }
}
