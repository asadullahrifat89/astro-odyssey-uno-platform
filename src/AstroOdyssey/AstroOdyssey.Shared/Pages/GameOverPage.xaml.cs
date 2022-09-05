using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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

            CongratulationsText.Text = (App.GameScore.Score == 0
                ? LocalizationHelper.GetLocalizedResource("NO_LUCK") : App.GameScore.Score <= 400
                ? LocalizationHelper.GetLocalizedResource("GOOD_GAME") : App.GameScore.Score <= 800
                ? LocalizationHelper.GetLocalizedResource("GREAT_GAME") : App.GameScore.Score <= 1400
                ? LocalizationHelper.GetLocalizedResource("FANTASTIC_GAME") : LocalizationHelper.GetLocalizedResource("SUPREME_GAME")) + "!";

            

            await this.PlayPageLoadedTransition();
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
            AudioHelper.PlaySound(SoundType.GAME_INTRO);
        }

        #endregion

        #region Methods

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(GameOverPage_Tagline);
            LocalizationHelper.SetLocalizedResource(GameOverPage_PlayAgainButton);
        }

        #endregion
    }
}
