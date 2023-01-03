using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;

namespace SpaceShooterGame
{
    public sealed partial class GameLeaderboardPage : Page
    {
        #region Fields

        private PeriodicTimer _gameViewTimer;
        private readonly TimeSpan _frameTime = TimeSpan.FromMilliseconds(Constants.DEFAULT_FRAME_TIME);

        private readonly Random _random = new();

        private double _windowHeight, _windowWidth;
        private double _scale;

        private readonly int _gameSpeed = 5;

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
            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();

            GameLeaderboardPage_GameProfiles.ItemsSource = GameProfilesCollection;
            GameLeaderboardPage_GameScores.ItemsSource = GameScoresCollection;

            _windowHeight = Window.Current.Bounds.Height;
            _windowWidth = Window.Current.Bounds.Width;

            PopulateGameViews();

            Loaded += GameLeaderboardPage_Loaded;
            Unloaded += GamePage_Unloaded;
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

            GameLeaderboardPage_SeasonToggle.IsChecked = true;

            this.StopProgressBar();

            SizeChanged += GamePage_SizeChanged;
            StartAnimation();
        }

        private void GamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= GamePage_SizeChanged;
            StopAnimation();
        }

        private void GamePage_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            _windowWidth = args.NewSize.Width;
            _windowHeight = args.NewSize.Height;

            SetViewSize();

#if DEBUG
            Console.WriteLine($"WINDOWS SIZE: {_windowWidth}x{_windowHeight}");
#endif
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

        private async void GameLeaderboardPage_SeasonToggle_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            this.RunProgressBar();

            UncheckScoreboardChoiceToggles(sender);

            await GetGameSeason();

            this.StopProgressBar();
        }

        private async void GameLeaderboardPage_AllTimeScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            this.RunProgressBar();

            UncheckScoreboardChoiceToggles(sender);

            await GetGameProfiles();

            this.StopProgressBar();
        }

        private async void GameLeaderboardPage_DailyScoreboardToggle_Click(object sender, RoutedEventArgs e)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            this.RunProgressBar();

            UncheckScoreboardChoiceToggles(sender);

            await GetGameScores();

            this.StopProgressBar();
        }

        #endregion

        #endregion

        #region Methods

        #region Page

        private void SetViewSize()
        {
            _scale = ScalingHelper.GetGameObjectScale(_windowWidth);

            UnderView.SetSize(_windowHeight, _windowWidth);
        }

        private void NavigateToPage(Type pageType)
        {
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            App.NavigateToPage(pageType);
        }

        private void UncheckScoreboardChoiceToggles(object sender)
        {
            foreach (var toggleButton in ScoreboardChoice.Children.OfType<ToggleButton>().Where(x => x.Name != ((ToggleButton)sender).Name))
            {
                toggleButton.IsChecked = false;
            }
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
                SetListViewMessage();
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
                SetListViewMessage();
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

        private async Task<bool> GetGameSeason()
        {
            SetListViewMessage(LocalizationHelper.GetLocalizedResource("LOADING_DATA"));

            (bool IsSuccess, string Message, Season Season) = await _backendService.GetGameSeason();

            if (!IsSuccess)
            {
                SetListViewMessage();
                var error = Message;
                this.ShowError(error);
                return false;
            }

            if (Season is not null && Season.PrizeDescriptions is not null && Season.PrizeDescriptions.Length > 0)
            {
                SetListViewMessage();
                SeasonPrizeDescriptionText.Text = Season.PrizeDescriptions.FirstOrDefault(x => x.Culture == LocalizationHelper.CurrentCulture).Value;
                await GetGamePrize();
            }
            else
            {
                SeasonPrizeContainer.Visibility = Visibility.Collapsed;
                SetListViewMessage(LocalizationHelper.GetLocalizedResource("NO_DATA_AVAILABLE"));
            }

            return true;
        }

        private async Task<bool> GetGamePrize()
        {
            (bool IsSuccess, string Message, GamePrizeOfTheDay GamePrize) = await _backendService.GetGameDailyPrize();

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            if (GamePrize is not null
                && GamePrize.WinningCriteria is not null
                && GamePrize.WinningCriteria.CriteriaDescriptions is not null
                && GamePrize.PrizeDescriptions is not null
                && GamePrize.WinningCriteria.CriteriaDescriptions.Length > 0
                && GamePrize.PrizeDescriptions.Length > 0)
            {
                SetListViewMessage();
                WinningCriteriaDescriptionText.Text = GamePrize.WinningCriteria.CriteriaDescriptions.FirstOrDefault(x => x.Culture == LocalizationHelper.CurrentCulture).Value;
                GamePrizeDescriptionText.Text = GamePrize.PrizeDescriptions.FirstOrDefault(x => x.Culture == LocalizationHelper.CurrentCulture).Value;
            }
            else
            {
                DailyPrizeContainer.Visibility = Visibility.Collapsed;
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
                if (leaderboardPlacements.FirstOrDefault(x => x.User.UserId == GameProfileHelper.GameProfile.User.UserId) is LeaderboardPlacement placement)
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

        #region Animation

        #region Game

        private void PopulateGameViews()
        {
#if DEBUG
            Console.WriteLine("INITIALIZING GAME");
#endif
            SetViewSize();
            PopulateUnderView();
        }

        private void PopulateUnderView()
        {
            // add some clouds underneath
            for (int i = 0; i < 15; i++)
            {
                SpawnStar();
            }

            for (int i = 0; i < 1; i++)
            {
                SpawnStar(CelestialObjectType.Planet);
            }
        }

        private void StartAnimation()
        {
#if DEBUG
            Console.WriteLine("GAME STARTED");
#endif      
            RecycleGameObjects();
            RunGame();
        }

        private void RecycleGameObjects()
        {
            foreach (CelestialObject x in UnderView.Children.OfType<CelestialObject>())
            {
                switch ((ElementType)x.Tag)
                {
                    case ElementType.CELESTIAL_OBJECT:
                        {
                            RecyleStar(x);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private async void RunGame()
        {
            _gameViewTimer = new PeriodicTimer(_frameTime);

            while (await _gameViewTimer.WaitForNextTickAsync())
            {
                GameViewLoop();
            }
        }

        private void GameViewLoop()
        {
            UpdateGameObjects();
        }

        private void UpdateGameObjects()
        {
            foreach (CelestialObject x in UnderView.Children.OfType<CelestialObject>())
            {
                switch ((ElementType)x.Tag)
                {
                    case ElementType.CELESTIAL_OBJECT:
                        {
                            UpdateStar(x);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void StopAnimation()
        {
            _gameViewTimer?.Dispose();
        }

        #endregion

        #region Star

        private void SpawnStar(CelestialObjectType celestialObjectType = CelestialObjectType.Star)
        {
            CelestialObject star = new();
            star.SetAttributes(scale: _scale, celestialObjectType: celestialObjectType);

            RandomizeStarPosition(star);

            UnderView.Children.Add(star);
        }

        private void UpdateStar(CelestialObject star)
        {
            star.SetY(star.GetY() + (star.CelestialObjectType == CelestialObjectType.Planet ? _gameSpeed / 1.5 : _gameSpeed));

            if (star.GetY() > UnderView.Height)
            {
                RecyleStar(star);
            }
        }

        private void RecyleStar(CelestialObject star)
        {
            if (star.CelestialObjectType == CelestialObjectType.Planet)
                star.SetAttributes(scale: _scale, celestialObjectType: star.CelestialObjectType);
            RandomizeStarPosition(star);
        }

        private void RandomizeStarPosition(CelestialObject star)
        {
            star.SetPosition(
                left: _random.Next(0, (int)UnderView.Width) - (100 * _scale),
                top: _random.Next(800, 1400) * -1);
        }

        #endregion

        #endregion

        #endregion
    }
}
