using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameLeaderboardPage : Page
    {
        #region Fields

        private readonly IBackendService _backendService;
        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;

        private readonly ProgressBar _progressBar;
        private readonly TextBlock _errorContainer;
        private readonly Button[] _actionButtons;

        #endregion

        #region Ctor

        public GameLeaderboardPage()
        {
            this.InitializeComponent();
            Loaded += GameLeaderboardPage_Loaded;

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();
            _audioHelper = (Application.Current as App).Host.Services.GetRequiredService<IAudioHelper>();
            _localizationHelper = (Application.Current as App).Host.Services.GetRequiredService<ILocalizationHelper>();

            GameLeaderboardPage_GameProfiles.ItemsSource = GameProfiles;
            GameLeaderboardPage_GameScores.ItemsSource = GameScores;

            _progressBar = GameLeaderboardPage_ProgressBar;
            _errorContainer = GameLeaderboardPage_ErrorText;
            _actionButtons = new[] { GameLeaderboardPage_PlayNowButton };
        }

        #endregion

        #region Properties

        public ObservableCollection<GameProfile> GameProfiles { get; set; } = new ObservableCollection<GameProfile>();

        public ObservableCollection<GameScore> GameScores { get; set; } = new ObservableCollection<GameScore>();

        #endregion

        #region Events

        private async void GameLeaderboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            // by default all scoreboards are invisible
            GameLeaderboardPage_GameScores.Visibility = Visibility.Collapsed;
            GameLeaderboardPage_GameProfiles.Visibility = Visibility.Visible;

            await this.PlayLoadedTransition();

            RunProgressBar();

            // get game profile
            if (!await GetGameProfile())
                return;

            ShowUserName();

            GameLeaderboardPage_AllTimeScoreboardToggle.IsChecked = true;

            StopProgressBar();
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayUnLoadedTransition();
            App.NavigateToPage(typeof(ShipSelectionPage));
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameStartPage));
        }

        private async void GameLeaderboardPage_AllTimeScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            RunProgressBar();

            GameLeaderboardPage_DailyScoreboardToggle.IsChecked = false;
            await GetGameProfiles();

            StopProgressBar();
        }

        private async void GameLeaderboardPage_DailyScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            RunProgressBar();

            GameLeaderboardPage_AllTimeScoreboardToggle.IsChecked = false;
            await GetGameScores();

            StopProgressBar();
        }

        #endregion

        #region Methods      

        private async Task<bool> GetGameProfile()
        {
            var recordResponse = await _backendService.GetGameProfile();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                this.ShowError(
                    progressBar: _progressBar,
                    messageBlock: _errorContainer,
                    message: string.Join("\n", error),
                    actionButtons: _actionButtons);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            SetGameScores(
                personalBestScore: App.GameProfile.PersonalBestScore,
                lastGameScore: App.GameProfile.LastGameScore);

            return true;
        }

        private async Task<bool> GetGameProfiles()
        {
            GameProfiles.Clear();
            SetListViewMessage(_localizationHelper.GetLocalizedResource("LOADING_DATA"));

            var recordsResponse = await _backendService.GetGameProfiles(pageIndex: 0, pageSize: 10);

            if (!recordsResponse.IsSuccess)
            {
                SetListViewMessage();

                var error = recordsResponse.Errors.Errors;
                this.ShowError(
                    progressBar: _progressBar,
                    messageBlock: _errorContainer,
                    message: string.Join("\n", error),
                    actionButtons: _actionButtons);

                return false;
            }

            var result = recordsResponse.Result;
            var count = recordsResponse.Result.Count;

            if (count > 0)
            {
                SetListViewMessage();

                var records = result.Records;

                foreach (var record in records)
                {
                    GameProfiles.Add(record);
                }

                SetLeaderboardPlacements(GameProfiles);

                // indicate current player
                if (GameProfiles.FirstOrDefault(x => x.User.UserName == App.GameProfile.User.UserName || x.User.UserEmail == App.GameProfile.User.UserEmail) is LeaderboardPlacement placement)
                {
                    placement.Emoji = "👨‍🚀";
                }
            }
            else
            {
                SetListViewMessage(_localizationHelper.GetLocalizedResource("NO_DATA_AVAILABLE"));
            }

            return true;
        }

        private async Task<bool> GetGameScores()
        {
            GameScores.Clear();
            SetListViewMessage(_localizationHelper.GetLocalizedResource("LOADING_DATA"));

            var recordsResponse = await _backendService.GetGameScores(pageIndex: 0, pageSize: 10);

            if (!recordsResponse.IsSuccess)
            {
                SetListViewMessage();

                var error = recordsResponse.Errors.Errors;
                this.ShowError(
                    progressBar: _progressBar,
                    messageBlock: _errorContainer,
                    message: string.Join("\n", error),
                    actionButtons: _actionButtons);

                return false;
            }

            var result = recordsResponse.Result;
            var count = recordsResponse.Result.Count;

            if (count > 0)
            {
                SetListViewMessage();

                var records = result.Records;

                foreach (var record in records)
                {
                    GameScores.Add(record);
                }

                SetLeaderboardPlacements(GameScores);

                // indicate current player
                if (GameScores.FirstOrDefault(x => x.User.UserName == App.GameProfile.User.UserName || x.User.UserEmail == App.GameProfile.User.UserEmail) is LeaderboardPlacement placement)
                {
                    placement.Emoji = "👨‍🚀";
                }
            }
            else
            {
                SetListViewMessage(_localizationHelper.GetLocalizedResource("NO_DATA_AVAILABLE"));
            }

            return true;
        }

        private void SetLeaderboardPlacements(dynamic leaderboardPlacements)
        {
            if (leaderboardPlacements.Count > 0)
            {
                // king of the ring
                if (leaderboardPlacements[0] is LeaderboardPlacement firstPlacement)
                {
                    firstPlacement.MedalEmoji = "🥇";
                    firstPlacement.Emoji = "🏆";
                }

                if (leaderboardPlacements.Count > 1)
                {
                    if (leaderboardPlacements[1] is LeaderboardPlacement secondPlacement)
                    {
                        secondPlacement.MedalEmoji = "🥈";
                    }
                }

                if (leaderboardPlacements.Count > 2)
                {
                    if (leaderboardPlacements[2] is LeaderboardPlacement thirdPlacement)
                    {
                        thirdPlacement.MedalEmoji = "🥉";
                    }
                }
            }
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

        private void SetGameScores(double personalBestScore, double lastGameScore)
        {
            PersonalBestScoreText.Text = _localizationHelper.GetLocalizedResource("PERSONAL_BEST_SCORE") + ": " + personalBestScore;
            ScoreText.Text = _localizationHelper.GetLocalizedResource("LAST_GAME_SCORE") + ": " + lastGameScore;
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

        private void SetListViewMessage(string message = null)
        {
            GameLeaderboardPage_ListViewMessage.Text = message;
            GameLeaderboardPage_ListViewMessage.Visibility = message.IsNullOrBlank() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SetLocalization()
        {
            _localizationHelper.SetLocalizedResource(GameLeaderboardPage_Tagline);
            _localizationHelper.SetLocalizedResource(GameLeaderboardPage_PlayNowButton);
            _localizationHelper.SetLocalizedResource(GameLeaderboardPage_DailyScoreboardToggle);
            _localizationHelper.SetLocalizedResource(GameLeaderboardPage_AllTimeScoreboardToggle);
        }

        #endregion      
    }
}
