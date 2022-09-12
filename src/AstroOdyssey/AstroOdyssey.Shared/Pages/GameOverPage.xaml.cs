using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;
        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;
        private readonly ICacheHelper _cacheHelper;

        private readonly ProgressBar _progressBar;
        private readonly TextBlock _errorContainer;
        private readonly Button[] _actionButtons;

        #endregion

        #region Ctor

        public GameOverPage()
        {
            InitializeComponent();
            Loaded += GameOverPage_Loaded;

            _gameApiHelper = App.Container.GetService<IGameApiHelper>();
            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
            _cacheHelper = App.Container.GetService<ICacheHelper>();

            _progressBar = GameOverPage_ProgressBar;
            _errorContainer = GameOverPage_ErrorText;
            _actionButtons = new[] { GameOverPage_PlayAgainButton, GameLoginPage_LoginButton, GameOverPage_LeaderboardButton };
        }

        #endregion

        #region Events

        private async void GameOverPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
            SetGameResults();

            // if user has not logged in or session has expired
            if (App.AuthToken is null || App.AuthToken.AccessToken.IsNullOrBlank() || _cacheHelper.HasSessionExpired())
            {                
                MakeLoginControlsVisible();
            }
            else
            {
                this.RunProgressBar(
                    progressBar: _progressBar,
                    errorContainer: _errorContainer,
                    actionButtons: _actionButtons);
                                
                if (await SubmitScore())
                {
                    MakeLeaderboardControlsVisible(); // if score submission was successful make leaderboard button visible
                }
                else
                {
                    MakeLoginControlsVisible();
                }

                this.StopProgressBar(
                    progressBar: _progressBar,
                    actionButtons: _actionButtons);
            }

            await this.PlayPageLoadedTransition();
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
        }

        private async void GameLoginPage_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameLoginPage));
        }

        private async void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameLeaderboardPage));
        }

        #endregion

        #region Methods

        private void MakeLeaderboardControlsVisible()
        {
            GameLoginPage_LoginButton.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
        }

        private void MakeLoginControlsVisible()
        {
            // submit score on user login, or signup then login
            App.GameScoreSubmissionPending = true;

            GameLoginPage_LoginButton.Visibility = Visibility.Visible;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
        }

        private void SetGameResults()
        {
            ScoreText.Text = $"{_localizationHelper.GetLocalizedResource("YOUR_SCORE")} " + App.GameScore.Score;

            EnemiesDestroyedText.Text = $"{_localizationHelper.GetLocalizedResource("ENEMIES_DESTROYED")} x " + App.GameScore.EnemiesDestroyed;
            MeteorsDestroyedText.Text = $"{_localizationHelper.GetLocalizedResource("METEORS_DESTROYED")} x " + App.GameScore.MeteorsDestroyed;
            BossesDestroyedText.Text = $"{_localizationHelper.GetLocalizedResource("BOSSES_DESTROYED")} x " + App.GameScore.BossesDestroyed;
            CollectiblesCollectedText.Text = $"{_localizationHelper.GetLocalizedResource("COLLECTIBLES_COLLECTED")} x " + App.GameScore.CollectiblesCollected;

            CongratulationsText.Text = (App.GameScore.Score == 0
                ? _localizationHelper.GetLocalizedResource("NO_LUCK") : App.GameScore.Score <= 400
                ? _localizationHelper.GetLocalizedResource("GOOD_GAME") : App.GameScore.Score <= 800
                ? _localizationHelper.GetLocalizedResource("GREAT_GAME") : App.GameScore.Score <= 1400
                ? _localizationHelper.GetLocalizedResource("FANTASTIC_GAME") : _localizationHelper.GetLocalizedResource("SUPREME_GAME")) + "!";
        }

        private async Task<bool> SubmitScore()
        {
            ServiceResponse response = await _gameApiHelper.SubmitGameScore(App.GameScore.Score);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                    progressBar: _progressBar,
                    errorContainer: _errorContainer,
                    error: error,
                    actionButtons: _actionButtons);

                return false;
            }

            return true;
        }

        private void SetLocalization()
        {
            _localizationHelper.SetLocalizedResource(GameOverPage_Tagline);
            _localizationHelper.SetLocalizedResource(GameOverPage_PlayAgainButton);
            _localizationHelper.SetLocalizedResource(GameLoginPage_LoginButton);
            _localizationHelper.SetLocalizedResource(GameOverPage_LeaderboardButton);
        }

        #endregion      
    }
}
