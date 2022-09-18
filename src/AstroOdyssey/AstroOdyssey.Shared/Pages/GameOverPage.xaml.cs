using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
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
            ShowUserName();

            await this.PlayLoadedTransition();

            ScoreDetailsStack.AnimateChildElements();

            // if user has not logged in or session has expired
            if (!App.HasUserLoggedIn || _cacheHelper.HasSessionExpired())
            {
                MakeLoginControlsVisible();
            }
            else
            {
                RunProgressBar();

                if (await SubmitScore())
                {
                    MakeLeaderboardControlsVisible(); // if score submission was successful make leaderboard button visible
                }
                else
                {
                    MakeLoginControlsVisible();
                }

                StopProgressBar();
            }
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(ShipSelectionPage));
        }

        private async void GameLoginPage_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameLoginPage));
        }

        private async void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            await this.PlayUnLoadedTransition();

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
            ScoreText.Text = $"{_localizationHelper.GetLocalizedResource("YOUR_SCORE")} " + App.PlayerScore.Score;

            EnemiesDestroyedText.Text = $"{_localizationHelper.GetLocalizedResource("ENEMIES_DESTROYED")} x " + App.PlayerScore.EnemiesDestroyed;
            MeteorsDestroyedText.Text = $"{_localizationHelper.GetLocalizedResource("METEORS_DESTROYED")} x " + App.PlayerScore.MeteorsDestroyed;
            BossesDestroyedText.Text = $"{_localizationHelper.GetLocalizedResource("BOSSES_DESTROYED")} x " + App.PlayerScore.BossesDestroyed;
            CollectiblesCollectedText.Text = $"{_localizationHelper.GetLocalizedResource("COLLECTIBLES_COLLECTED")} x " + App.PlayerScore.CollectiblesCollected;

            CongratulationsText.Text = (App.PlayerScore.Score == 0
                ? _localizationHelper.GetLocalizedResource("NO_LUCK") : App.PlayerScore.Score <= 400
                ? _localizationHelper.GetLocalizedResource("GOOD_GAME") : App.PlayerScore.Score <= 800
                ? _localizationHelper.GetLocalizedResource("GREAT_GAME") : App.PlayerScore.Score <= 1400
                ? _localizationHelper.GetLocalizedResource("FANTASTIC_GAME") : _localizationHelper.GetLocalizedResource("SUPREME_GAME")) + "!";
        }

        private async Task<bool> SubmitScore()
        {
            ServiceResponse response = await _gameApiHelper.SubmitGameScore(App.PlayerScore.Score);

            if (response is null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response?.ExternalError;
                this.ShowError(
                    progressBar: _progressBar,
                    messageBlock: _errorContainer,
                    message: error,
                    actionButtons: _actionButtons);

                return false;
            }

            return true;
        }

        private void ShowUserName()
        {
            if (App.HasUserLoggedIn)
            {
                Page_UserName.Text = App.GameProfile.User.UserName;
                Page_UserPicture.Initials = App.GameProfile.Initials;
                PlayerNameHolder.Visibility = Visibility.Visible;
            }
            else
            {
                PlayerNameHolder.Visibility = Visibility.Collapsed;
            }
        }

        private void RunProgressBar()
        {
            this.RunProgressBar(
                progressBar: _progressBar,
                messageBlock: _errorContainer,
                actionButtons: _actionButtons);
        }

        private void StopProgressBar()
        {
            this.StopProgressBar(
                progressBar: _progressBar,
                actionButtons: _actionButtons);
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
