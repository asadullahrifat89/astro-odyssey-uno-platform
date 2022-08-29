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
        public GameOverPage()
        {
            InitializeComponent();
            Loaded += GameOverPage_Loaded;
        }

        private void GameOverPage_Loaded(object sender, RoutedEventArgs e)
        {
            var score = App.GetScore();

            ScoreText.Text = $"{LocalizationHelper.GetLocalizedResource("SCORE")} " + score + "\n" + (score == 0 ? LocalizationHelper.GetLocalizedResource("NO_LUCK") : score <= 400 ? LocalizationHelper.GetLocalizedResource("GOOD_GAME") : score <= 800 ? LocalizationHelper.GetLocalizedResource("GREAT_GAME") : score <= 1400 ? LocalizationHelper.GetLocalizedResource("FANTASTIC_GAME") : LocalizationHelper.GetLocalizedResource("SUPREME_GAME")) + "!";
        }

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(typeof(ShipSelectionPage));
            AudioHelper.PlaySound(SoundType.GAME_INTRO);
        }
    }
}
