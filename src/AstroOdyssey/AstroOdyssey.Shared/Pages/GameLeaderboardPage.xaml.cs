using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
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

        private int _pageIndex = 0;
        private int _pageSize = 10;
        private long _totalPageCount = 0;

        public ObservableCollection<GameProfile> GameProfiles { get; set; } = new ObservableCollection<GameProfile>();

        #endregion

        #region Ctor

        public GameLeaderboardPage()
        {
            this.InitializeComponent();
            Loaded += GameLeaderboardPage_Loaded;

            // Get a local instance of the container
            var container = ((App)App.Current).Container;
            _gameApiHelper = (IGameApiHelper)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(GameApiHelper));

            LeaderboardList.ItemsSource = GameProfiles;
        }

        #endregion

        #region Events

        private async void GameLeaderboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLocalization();
            ScoreText.Text = $"{LocalizationHelper.GetLocalizedResource("SCORE")} " + (App.GameScore is null ? 0 : App.GameScore.Score);

            await this.PlayPageLoadedTransition();

            this.RunProgressBar(
                progressBar: GameLeaderboardPage_ProgressBar,
                errorContainer: GameLeaderboardPage_ErrorText,
                actionButtons: GameLeaderboardPage_PlayNowButton);

            // get game profile
            if (!await GetGameProfile())
                return;

            // get game profiles
            if (!await GetGameProfiles())
                return;

            this.StopProgressBar(
                progressBar: GameLeaderboardPage_ProgressBar,
                actionButtons: GameLeaderboardPage_PlayNowButton);
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

            //TODO: show personal best in UI           

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

                //TODO: show game profiles in UI
                foreach (var record in records)
                {
                    GameProfiles.Add(record);
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
