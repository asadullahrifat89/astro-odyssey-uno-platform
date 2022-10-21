using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Uno.Extensions;

namespace SpaceShooterGame
{
    public sealed partial class GameLeaderboardPage : Page
    {
        #region Fields

        private readonly IBackendService _backendService;

        #endregion

        #region Properties

        public ObservableCollection<GameProfile> GameProfilesCollection { get; set; } = new ObservableCollection<GameProfile>();

        public ObservableCollection<GameScore> GameScoresCollection { get; set; } = new ObservableCollection<GameScore>();

        #endregion

        #region Ctor

        public GameLeaderboardPage()
        {
            this.InitializeComponent();
            Loaded += GameLeaderboardPage_Loaded;

            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();

            GameLeaderboardPage_GameProfiles.ItemsSource = GameProfilesCollection;
            GameLeaderboardPage_GameScores.ItemsSource = GameScoresCollection;
        }

        #endregion

        #region Events

        #region Page

        private async void GameLeaderboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();
            this.RunProgressBar();

            if (await GetGameProfile())
                ShowUserName();

            GameLeaderboardPage_DailyScoreboardToggle.IsChecked = true;

            this.StopProgressBar();
        }

        #endregion

        #region Button

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ShipSelectionPage));
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameStartPage));
        }

        private async void GameLeaderboardPage_AllTimeScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            this.RunProgressBar();

            GameLeaderboardPage_DailyScoreboardToggle.IsChecked = false;
            await GetGameProfiles();

            this.StopProgressBar();
        }

        private async void GameLeaderboardPage_DailyScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            this.RunProgressBar();

            GameLeaderboardPage_AllTimeScoreboardToggle.IsChecked = false;
            await GetGameScores();

            this.StopProgressBar();
        }

        #endregion

        #endregion

        #region Methods

        #region Page

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        #endregion

        #region Logic

        private async Task<bool> GetGameProfile()
        {
            (bool IsSuccess, string Message, _) = await _backendService.GetUserGameProfile();

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            SetGameScores(
                personalBestScore: GameProfileHelper.GameProfile.PersonalBestScore,
                lastGameScore: GameProfileHelper.GameProfile.LastGameScore);

            return true;
        }

        private async Task<bool> GetGameProfiles()
        {
            GameProfilesCollection.Clear();
            SetListViewMessage(LocalizationHelper.GetLocalizedResource("LOADING_DATA"));

            (bool IsSuccess, string Message, GameProfile[] GameProfiles) = await _backendService.GetUserGameProfiles(pageIndex: 0, pageSize: 10);

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            if (GameProfiles is not null && GameProfiles.Length > 0)
            {
                SetListViewMessage();
                GameProfilesCollection.AddRange(GameProfiles);
                SetLeaderboardPlacements(GameProfilesCollection);
                IndicateCurrentPlayer(GameProfilesCollection.Cast<LeaderboardPlacement>().ToObservableCollection());
            }
            else
            {
                SetListViewMessage(LocalizationHelper.GetLocalizedResource("NO_DATA_AVAILABLE"));
            }

            return true;
        }

        private async Task<bool> GetGameScores()
        {
            GameScoresCollection.Clear();
            SetListViewMessage(LocalizationHelper.GetLocalizedResource("LOADING_DATA"));

            (bool IsSuccess, string Message, GameScore[] GameScores) = await _backendService.GetUserGameScores(pageIndex: 0, pageSize: 10);

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            if (GameScores is not null && GameScores.Length > 0)
            {
                SetListViewMessage();
                GameScoresCollection.AddRange(GameScores);
                SetLeaderboardPlacements(GameScoresCollection);
                IndicateCurrentPlayer(GameScoresCollection.Cast<LeaderboardPlacement>().ToObservableCollection());
            }
            else
            {
                SetListViewMessage(LocalizationHelper.GetLocalizedResource("NO_DATA_AVAILABLE"));
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

        private void IndicateCurrentPlayer(ObservableCollection<LeaderboardPlacement> leaderboardPlacements)
        {
            if (leaderboardPlacements is not null)
            {
                if (leaderboardPlacements.FirstOrDefault(x => x.User.UserName == GameProfileHelper.GameProfile.User.UserName || x.User.UserEmail == GameProfileHelper.GameProfile.User.UserEmail) is LeaderboardPlacement placement)
                {
                    placement.Emoji = "👨‍🚀";
                }
            }
        }

        private void ShowUserName()
        {
            if (GameProfileHelper.HasUserLoggedIn())
            {
                Page_UserName.Text = GameProfileHelper.GameProfile.User.UserName;
                Page_UserPicture.Initials = GameProfileHelper.GameProfile.Initials;
                PlayerNameHolder.Visibility = Visibility.Visible;
            }
            else
            {
                PlayerNameHolder.Visibility = Visibility.Collapsed;
            }
        }

        private void SetGameScores(double personalBestScore, double lastGameScore)
        {
            PersonalBestScoreText.Text = LocalizationHelper.GetLocalizedResource("PERSONAL_BEST_SCORE") + ": " + personalBestScore;
            ScoreText.Text = LocalizationHelper.GetLocalizedResource("LAST_GAME_SCORE") + ": " + lastGameScore;
        }

        private void SetListViewMessage(string message = null)
        {
            GameLeaderboardPage_ListViewMessage.Text = message;
            GameLeaderboardPage_ListViewMessage.Visibility = message.IsNullOrBlank() ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion

        #endregion
    }
}
