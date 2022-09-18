using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameLeaderboardPage : Page
    {
        #region Fields

        private readonly IGameApiHelper _gameApiHelper;
        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;
        //private readonly IPaginationHelper _paginationHelper;

        private readonly ProgressBar _progressBar;
        private readonly TextBlock _errorContainer;
        private readonly Button[] _actionButtons;

        #endregion

        #region Ctor

        public GameLeaderboardPage()
        {
            this.InitializeComponent();
            Loaded += GameLeaderboardPage_Loaded;

            _gameApiHelper = App.Container.GetService<IGameApiHelper>();
            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
            //_paginationHelper = App.Container.GetService<IPaginationHelper>();

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

            // get game profile
            await GetGameProfile();

            ShowUserName();

            // get game scores
            await GetGameScores();

            // get game profiles
            await GetGameProfiles();
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

        private async void GameLeaderboardPage_DailyScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            GameLeaderboardPage_GameProfiles.Visibility = Visibility.Collapsed;
            GameLeaderboardPage_GameScores.Visibility = Visibility.Visible;
            await GetGameScores();
        }

        private async void GameLeaderboardPage_AllTimeScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            GameLeaderboardPage_GameScores.Visibility = Visibility.Collapsed;
            GameLeaderboardPage_GameProfiles.Visibility = Visibility.Visible;
            await GetGameProfiles();
        }

        #endregion

        #region Methods      

        private async Task<bool> GetGameProfile()
        {
            RunProgressBar();

            var recordResponse = await _gameApiHelper.GetGameProfile();

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

            StopProgressBar();

            return true;
        }

        private async Task<bool> GetGameProfiles()
        {
            RunProgressBar();

            GameProfiles.Clear();

            var recordsResponse = await _gameApiHelper.GetGameProfiles(pageIndex: 0, pageSize: 15);

            if (!recordsResponse.IsSuccess)
            {
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
                //_totalPageCount = _paginationHelper.GetTotalPageCount(pageSize: _pageSize, dataCount: count);

                var records = result.Records;

                foreach (var record in records)
                {
                    GameProfiles.Add(record);
                }

                // king of the ring
                GameProfiles[0].Emoji = "👑";

                // indicate current player
                if (GameProfiles.FirstOrDefault(x => x.User.UserName == App.GameProfile.User.UserName || x.User.UserEmail == App.GameProfile.User.UserEmail) is GameProfile gameProfile)
                {
                    gameProfile.Emoji = "👨‍🚀";
                }
            }

            StopProgressBar();

            return true;
        }

        private async Task<bool> GetGameScores()
        {
            RunProgressBar();

            GameScores.Clear();

            var recordsResponse = await _gameApiHelper.GetGameScores(pageIndex: 0, pageSize: 15);

            if (!recordsResponse.IsSuccess)
            {
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
                var records = result.Records;

                foreach (var record in records)
                {
                    GameScores.Add(record);
                }

                // king of the ring
                GameScores[0].Emoji = "👑";

                // indicate current player
                if (GameScores.FirstOrDefault(x => x.User.UserName == App.GameProfile.User.UserName || x.User.UserEmail == App.GameProfile.User.UserEmail) is GameScore gameScore)
                {
                    gameScore.Emoji = "👨‍🚀";
                }               
            }

            StopProgressBar();

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
