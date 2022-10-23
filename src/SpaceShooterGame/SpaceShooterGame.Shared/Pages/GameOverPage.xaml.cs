using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceShooterGame
{
    public sealed partial class GameOverPage : Page
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

        #region Ctor

        public GameOverPage()
        {
            InitializeComponent();
            _backendService = (Application.Current as App).Host.Services.GetRequiredService<IBackendService>();

            _windowHeight = Window.Current.Bounds.Height;
            _windowWidth = Window.Current.Bounds.Width;

            PopulateGameViews();

            Loaded += GameOverPage_Loaded;
            Unloaded += GamePage_Unloaded;
        }

        #endregion

        #region Events

        #region Page

        private async void GameOverPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetLocalization();

            SetGameResults();
            ShowUserName();

            // if user has not logged in or session has expired
            if (!GameProfileHelper.HasUserLoggedIn() || SessionHelper.HasSessionExpired())
            {
                MakeLoginControlsVisible();
            }
            else
            {
                this.RunProgressBar();

                if (await SubmitScore())
                {
                    MakeLeaderboardControlsVisible(); // if score submission was successful make leaderboard button visible
                }
                else
                {
                    MakeLoginControlsVisible();
                }

                this.StopProgressBar();
            }

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

        private void GameLoginPage_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLoginPage));
        }

        private void GameOverPage_LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GameLeaderboardPage));
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

        #endregion

        #region Logic

        private void MakeLeaderboardControlsVisible()
        {
            GameOverPage_SignupPromptPanel.Visibility = Visibility.Collapsed;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Visible;
        }

        private void MakeLoginControlsVisible()
        {
            // submit score on user login, or signup then login
            PlayerScoreHelper.GameScoreSubmissionPending = true;

            GameOverPage_SignupPromptPanel.Visibility = Visibility.Visible;
            GameOverPage_LeaderboardButton.Visibility = Visibility.Collapsed;
        }

        private void SetGameResults()
        {
            ScoreText.Text = LocalizationHelper.GetLocalizedResource("YOUR_SCORE");
            ScoreNumberText.Text = PlayerScoreHelper.PlayerScore.Score.ToString();

            EnemiesDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("ENEMIES_DESTROYED")} x " + PlayerScoreHelper.PlayerScore.EnemiesDestroyed;
            MeteorsDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("METEORS_DESTROYED")} x " + PlayerScoreHelper.PlayerScore.MeteorsDestroyed;
            BossesDestroyedText.Text = $"{LocalizationHelper.GetLocalizedResource("BOSSES_DESTROYED")} x " + PlayerScoreHelper.PlayerScore.BossesDestroyed;
            CollectiblesCollectedText.Text = $"{LocalizationHelper.GetLocalizedResource("COLLECTIBLES_COLLECTED")} x " + PlayerScoreHelper.PlayerScore.CollectiblesCollected;
        }

        private async Task<bool> SubmitScore()
        {
            //TODO: HIGH SCORE GOAL-> check if player has reached high score goal after this game and show a congratulations dialog

            (bool IsSuccess, string Message) = await _backendService.SubmitUserGameScore(PlayerScoreHelper.PlayerScore.Score);

            if (!IsSuccess)
            {
                var error = Message;
                this.ShowError(error);
                return false;
            }

            return true;
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
