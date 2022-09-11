using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

        private int _pageIndex = 0;
        private int _pageSize = 15;
        private long _totalPageCount = 0;

        public ObservableCollection<GameProfile> GameProfiles { get; set; } = new ObservableCollection<GameProfile>();

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

            LeaderboardList.ItemsSource = GameProfiles;

            _progressBar = GameLeaderboardPage_ProgressBar;
            _errorContainer = GameLeaderboardPage_ErrorText;
            _actionButtons = new[] { GameLeaderboardPage_PlayNowButton };
        }

        #endregion

        #region Events

        private async void GameLeaderboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();

            await this.PlayPageLoadedTransition();

            this.RunProgressBar(
                progressBar: _progressBar,
                errorContainer: _errorContainer,
                actionButtons: _actionButtons);

            // get game profile
            if (!await GetGameProfile())
                return;

            // get game profiles
            if (!await GetGameProfiles())
                return;

            this.StopProgressBar(
                progressBar: _progressBar,
                actionButtons: _actionButtons);
        }

        private async void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            await this.PlayPageUnLoadedTransition();
            App.NavigateToPage(typeof(ShipSelectionPage));
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            await this.PlayPageUnLoadedTransition();

            App.NavigateToPage(typeof(GameStartPage));
        }

        #endregion

        #region Methods

        private async Task<bool> GetGameProfile()
        {
            var recordResponse = await _gameApiHelper.GetGameProfile();

            if (!recordResponse.IsSuccess)
            {
                var error = recordResponse.Errors.Errors;
                this.ShowError(
                    progressBar: GameLeaderboardPage_ProgressBar,
                    errorContainer: GameLeaderboardPage_ErrorText,
                    error: string.Join("\n", error),
                    actionButtons: GameLeaderboardPage_PlayNowButton);

                return false;
            }

            // store game profile
            var gameProfile = recordResponse.Result;
            App.GameProfile = gameProfile;

            PersonalBestScoreText.Text = LocalizationHelper.GetLocalizedResource("PERSONAL_BEST_SCORE") + ": " + App.GameProfile.PersonalBestScore;
            ScoreText.Text = LocalizationHelper.GetLocalizedResource("LAST_GAME_SCORE") + ": " + App.GameProfile.LastGameScore;

            return true;
        }

        private async Task<bool> GetGameProfiles()
        {
            var recordsResponse = await _gameApiHelper.GetGameProfiles(pageIndex: _pageIndex, pageSize: _pageSize);

            if (!recordsResponse.IsSuccess)
            {
                var error = recordsResponse.Errors.Errors;
                this.ShowError(
                    progressBar: GameLeaderboardPage_ProgressBar,
                    errorContainer: GameLeaderboardPage_ErrorText,
                    error: string.Join("\n", error),
                    actionButtons: GameLeaderboardPage_PlayNowButton);

                return false;
            }

            var result = recordsResponse.Result;
            var count = recordsResponse.Result.Count;

            if (count > 0)
            {
                _totalPageCount = PaginationHelper.GetTotalPageCount(pageSize: _pageSize, dataCount: count);

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

            return true;
        }

        private void SetLocalization()
        {
            LocalizationHelper.SetLocalizedResource(GameLeaderboardPage_Tagline);
            LocalizationHelper.SetLocalizedResource(GameLeaderboardPage_PlayNowButton);
        }

        #endregion
    }
}
