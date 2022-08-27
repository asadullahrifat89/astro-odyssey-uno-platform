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

            ScoreText.Text = "You Scored " + score + (score == 0 ? "\nNo luck!" : score <= 400 ? "\nGood game!" : "\nGreat game!");
        }

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(typeof(ShipSelectionPage));
            AudioHelper.PlaySound(SoundType.GAME_INTRO);
        }
    }
}
