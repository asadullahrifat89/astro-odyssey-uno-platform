using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public sealed partial class GamePlayPage : Page
    {
        #region Fields

        private Image _rageImage;
        private Image _powerUpImage;
        private Image _healthImage;
        private Image _bossAppearedImage;
        private Image _bossClearedImage;
        private Image _scoreMultiplierImage;
#if DEBUG
        private int _fpsSpawnCounter = 0;
        private int _fpsCount = 0;
        private float _lastFpsTime = 0;

        private int _frameStatUpdateSpawnCounter;
        private int _frameStatUpdateAfter = 5;

        private long _frameStartTime;
        private long _frameEndTime;
        private double _frameDuration;
#endif
        private int _showInGameTextSpawnCounter = 110;
        private int _showInGameTextAfter = 110;

        private int _showInGameImagePanelSpawnCounter = 110;
        private int _showInGameImagePanelAfter = 110;

        private double _scoreMultiplierCoolDownCounter;
        private readonly double _scoreMultiplierCoolDownAfter = 1000;

        private double _frameTime;

        private double _windowWidth, _windowHeight;

        private readonly ICelestialObjectFactory _celestialObjectFactory;
        private readonly IMeteorFactory _meteorFactory;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IHealthFactory _healthFactory;
        private readonly IPowerUpFactory _powerUpFactory;
        private readonly ICollectibleFactory _collectibleFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerProjectileFactory _playerProjectileFactory;
        private readonly IEnemyProjectileFactory _enemyProjectileFactory;

        private readonly IAudioHelper _audioHelper;
        private readonly ILocalizationHelper _localizationHelper;

        #endregion

        #region Ctor

        public GamePlayPage()
        {
            InitializeComponent();

            Loaded += GamePage_Loaded;
            Unloaded += GamePage_Unloaded;

            _windowWidth = Window.Current.Bounds.Width - 10;
            _windowHeight = Window.Current.Bounds.Height - 10;

            PointerX = _windowWidth / 2;

            AdjustView(); // at constructor

            _celestialObjectFactory = App.Container.GetService<ICelestialObjectFactory>();
            _celestialObjectFactory.SetGameEnvironments(StarView, PlanetView);

            _meteorFactory = App.Container.GetService<IMeteorFactory>();
            _meteorFactory.SetGameEnvironment(GameView);

            _enemyFactory = App.Container.GetService<IEnemyFactory>();
            _enemyFactory.SetGameEnvironment(GameView);

            _healthFactory = App.Container.GetService<IHealthFactory>();
            _healthFactory.SetGameEnvironment(GameView);

            _powerUpFactory = App.Container.GetService<IPowerUpFactory>();
            _powerUpFactory.SetGameEnvironment(GameView);

            _collectibleFactory = App.Container.GetService<ICollectibleFactory>();
            _collectibleFactory.SetGameEnvironment(GameView);

            _playerFactory = App.Container.GetService<IPlayerFactory>();
            _playerFactory.SetGameEnvironment(GameView);

            _playerProjectileFactory = App.Container.GetService<IPlayerProjectileFactory>();
            _playerProjectileFactory.SetGameEnvironment(GameView);

            _enemyProjectileFactory = App.Container.GetService<IEnemyProjectileFactory>();
            _enemyProjectileFactory.SetGameEnvironment(GameView);

            _audioHelper = App.Container.GetService<IAudioHelper>();
            _localizationHelper = App.Container.GetService<ILocalizationHelper>();
        }

        #endregion

        #region Properties
#if DEBUG
        public Stopwatch Stopwatch { get; set; }
#endif
        public PeriodicTimer GameFrameTimer { get; set; }

        public Player Player { get; set; }

        public PlayerScore PlayerScore { get; set; } = new PlayerScore();

        public GameLevel GameLevel { get; set; }

        public PowerUpType PowerUpType { get; set; }

        public bool IsGameRunning { get; set; }

        public bool IsGamePaused { get; set; }

        public bool IsGameQuitting { get; set; }

        public List<Enemy> Bosses { get; set; }

        private double BossTotalHealth { get; set; }

        public bool IsPointerPressed { get; set; }

        public double PointerPressedX { get; set; }

        public double PointerX { get; set; }

        public double PointerY { get; set; }

        public bool MoveLeft { get; set; }

        public bool MoveRight { get; set; }

        public bool IsScoreMultiplierActivated { get; set; }

        public int ScoreMultiplierCount { get; set; }

        public double ScoreMultiplierThreashold { get; set; } = 8;

        #endregion

        #region Events

        #region Pointer & Keyboard

        private void InputView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                    {
                        MoveLeft = true;
                        MoveRight = false;
                    }
                    break;
                case Windows.System.VirtualKey.Right:
                    {
                        MoveRight = true;
                        MoveLeft = false;
                    }
                    break;
                default:
                    break;
            }
        }

        private void InputView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                    {
                        MoveLeft = false;
                    }
                    break;
                case Windows.System.VirtualKey.Right:
                    {
                        MoveRight = false;
                    }
                    break;
                case Windows.System.VirtualKey.Escape:
                    {
                        if (IsGamePaused)
                            ResumeGame();
                        else
                            PauseGame();
                    }
                    break;
                default:
                    break;
            }
        }

        private void InputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsGameRunning)
            {
                IsPointerPressed = true;

                StartPlayerMovement(e);
            }
        }

        private void InputView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsGameRunning && IsPointerPressed)
            {
                StartPlayerMovement(e);
            }
        }

        private void InputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (IsGameQuitting)
            {
                QuitGame();
                return;
            }

            if (IsGameRunning)
            {
                if (IsGamePaused)
                    ResumeGame();

                if (IsPointerPressed)
                    IsPointerPressed = false;

                if (MoveLeft)
                    MoveLeft = false;

                if (MoveRight)
                    MoveRight = false;
            }
            else
            {
                InputView.Focus(FocusState.Programmatic);
                StartGame();
            }
        }

        #endregion        

        #region Pause & Quit

        private void PauseGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsGamePaused)
                ResumeGame();
            else
                PauseGame();
        }

        private void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsGameQuitting)
            {
                IsGameQuitting = false;
                PauseGame();
            }
            else
            {
                _audioHelper.PlaySound(SoundType.MENU_SELECT);
                IsGameQuitting = true;
                ShowInGameText($"🛸\n{_localizationHelper.GetLocalizedResource("QUIT_GAME")}\n{_localizationHelper.GetLocalizedResource("TAP_TO_QUIT")}");

                InputView.Focus(FocusState.Programmatic);
            }
        }

        #endregion        

        #region Page Load, Unload, Size Change

        /// <summary>
        /// Invoked when the page is leaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += GamePage_SizeChanged;

            GameView.Children.Clear();
            StarView.Children.Clear();
            PlanetView.Children.Clear();

            ScoreBarPanel.Visibility = Visibility.Collapsed;

            FPSText.Text = "";
            ObjectsCountText.Text = "";

            Bosses = new List<Enemy>();
            BossHealthBarPanel.Visibility = Visibility.Collapsed;

            Player = null;
            PlayerHealthBarPanel.Visibility = Visibility.Collapsed;

            GameLevel = GameLevel.Level_1;
            SetGameLevelText();

            PowerUpType = PowerUpType.NONE;
            PlayerPowerBar.Maximum = POWER_UP_METER;
            PlayerPowerBar.Value = POWER_UP_METER;

            PlayerScore = new PlayerScore();
            SetScoreBarCountText(25);

            PointerX = _windowWidth / 2;

            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Collapsed;

            ScoreMultiplierCount = 0;
            IsScoreMultiplierActivated = false;
            SetScoreMultiplierCountText();
            ScoreMultiplierPanel.Visibility = Visibility.Collapsed;

            ShowInGameText("👆\n" + _localizationHelper.GetLocalizedResource("TAP_ON_SCREEN_TO_BEGIN"));
            InputView.Focus(FocusState.Programmatic);

            _powerUpImage = new Image
            {
                Stretch = Stretch.Uniform,
            };
            _powerUpImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/powerup.png", UriKind.RelativeOrAbsolute));

            _healthImage = new Image
            {
                Stretch = Stretch.Uniform,
            };
            _healthImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/health.png", UriKind.RelativeOrAbsolute));

            _bossAppearedImage = new Image
            {
                Stretch = Stretch.Uniform,
            };
            _bossAppearedImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/boss_appeared.png", UriKind.RelativeOrAbsolute));

            _bossClearedImage = new Image
            {
                Stretch = Stretch.Uniform,
            };
            _bossClearedImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/boss_cleared.png", UriKind.RelativeOrAbsolute));

            _scoreMultiplierImage = new Image
            {
                Stretch = Stretch.Uniform,
            };
            _scoreMultiplierImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/score_multiplier.gif", UriKind.RelativeOrAbsolute));

            await this.PlayLoadedTransition();
        }

        /// <summary>
        /// Invoked when the page is unloaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= GamePage_SizeChanged;
            StopGame();
        }

        /// <summary>
        /// Invoked when the size of the page changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GamePage_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            _windowWidth = args.NewSize.Width - 10; //Window.Current.Bounds.Width;
            _windowHeight = args.NewSize.Height - 10; //Window.Current.Bounds.Height;

            AdjustView(); // at view size change

#if DEBUG
            Console.WriteLine($"View Size: {_windowWidth} x {_windowHeight}");

            var scale = GameView.GetGameObjectScale();
            Console.WriteLine($"View Scale: {scale}");
#endif
        }

        #endregion

        #endregion

        #region Methods

        #region Game Start, Run, Stop, Pause, Resume, Over, & Quit

        /// <summary>
        /// Starts the game.
        /// </summary>
        private async void StartGame()
        {
#if DEBUG
            GameAnalyticsPanel.Visibility = Visibility.Visible;
#endif
            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            _audioHelper.PlaySound(SoundType.GAME_START);

            SpawnPlayer();

            SetPlayerY(); // set y position at game start

            SetPlayerHealthBar(); // set player health bar at game start

            HideInGameText();

            ResetFactories();

            IsGameRunning = true;

            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            PlayerHealthBarPanel.Visibility = Visibility.Visible;
            ScoreBarPanel.Visibility = Visibility.Visible;
            ScoreMultiplierPanel.Visibility = Visibility.Visible;

            SetStars();

            WarpThroughSpace();
            _audioHelper.PlaySound(SoundType.BACKGROUND_MUSIC);

            await RunGame();
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        /// <returns></returns>
        private async Task RunGame()
        {
#if DEBUG
            Stopwatch = Stopwatch.StartNew();
#endif
            GameFrameTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(_frameTime));

            while (await GameFrameTimer.WaitForNextTickAsync())
            {
#if DEBUG
                _frameStartTime = Stopwatch.ElapsedMilliseconds;
#endif
                UpdateFrame();
#if DEBUG
                CalculateFPS();

                _frameEndTime = Stopwatch.ElapsedMilliseconds;
#endif
                GetFrameDuration();
#if DEBUG
                SetAnalytics();
#endif
            }
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        private void PauseGame()
        {
            InputView.Focus(FocusState.Programmatic);

            GameFrameTimer.Dispose();

            ShowInGameText($"👨‍🚀\n{_localizationHelper.GetLocalizedResource("GAME_PAUSED")}\n{_localizationHelper.GetLocalizedResource("TAP_TO_RESUME")}");

            IsGamePaused = true;
            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Visible;

            _audioHelper.PlaySound(SoundType.MENU_SELECT);

            _audioHelper.PauseSound(SoundType.BACKGROUND_MUSIC);

            if (GameView.IsBossEngaged)
                _audioHelper.PauseSound(SoundType.BOSS_APPEARANCE);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        private async void ResumeGame()
        {
            InputView.Focus(FocusState.Programmatic);

            HideInGameText();

            IsGamePaused = false;
            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            _audioHelper.ResumeSound(SoundType.BACKGROUND_MUSIC);

            if (GameView.IsBossEngaged)
                _audioHelper.ResumeSound(SoundType.BOSS_APPEARANCE);

            await RunGame();
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        private void StopGame()
        {
            IsGameRunning = false;

            if (StarView.IsWarpingThroughSpace)
                _celestialObjectFactory.StopSpaceWarp();

            GameFrameTimer.Dispose();

            _audioHelper.StopSound();
        }

        /// <summary>
        /// Check if game if over.
        /// </summary>
        private void GameOver()
        {
            if (Player.HasNoHealth)
            {
                PlayerHealthBar.Width = 0;

                QuitGame();
            }
        }

        /// <summary>
        /// Quits the current game.
        /// </summary>
        private async void QuitGame()
        {
            StopGame();

            _audioHelper.PlaySound(SoundType.GAME_OVER);

            App.PlayerScore = PlayerScore;

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameOverPage));
        }

        #endregion

        #region Game Objects Update

        /// <summary>
        /// Updates a frame in the game.
        /// </summary>
        private void UpdateFrame()
        {
            GameOver();

            UpdateGameViewObjects();

            UpdateStarViewObjects();

            UpdatePlanetViewObjects();

            SpawnGameObjects();

            InGameTextCoolDown();

            InGameImagePanelCoolDown();

            DamageRecoveryCoolDown();

            ScoreMultiplierCoolDown();
        }

        /// <summary>
        /// Updates objects in the planet view.
        /// </summary>
        private void UpdatePlanetViewObjects()
        {
            var planetObjects = PlanetView.GetGameObjects<GameObject>();

            // update game view objects
            if (Parallel.ForEach(planetObjects, gameObject =>
            {
                UpdateCelestialObject(gameObject);

            }).IsCompleted)
            {
                // clean removable objects from game view
                PlanetView.RemoveDestroyableGameObjects();
            }
        }

        /// <summary>
        /// Updates objects in the star view.
        /// </summary>
        private void UpdateStarViewObjects()
        {
            var starObjects = StarView.GetGameObjects<GameObject>();

            // update game view objects
            if (Parallel.ForEach(starObjects, gameObject =>
            {
                UpdateCelestialObject(gameObject);

            }).IsCompleted)
            {
                // clean removable objects from game view
                StarView.RemoveDestroyableGameObjects();
            }
        }

        /// <summary>
        /// Updates objects in the game view.
        /// </summary>
        private void UpdateGameViewObjects()
        {
            var gameObjects = GameView.GetGameObjects<GameObject>();

            // update game view objects
            if (Parallel.ForEach(gameObjects, gameObject =>
            {
                // fade away objects marked to be destroyed
                if (gameObject.IsMarkedForFadedDestruction)
                {
                    gameObject.Explode();

                    if (gameObject.HasExploded)
                    {
                        GameView.AddDestroyableGameObject(gameObject);
                        return;
                    }
                }

                switch (gameObject.Tag)
                {
                    case PLAYER_TAG:
                        {
                            UpdatePlayer();
                        }
                        break;
                    case PLAYER_PROJECTILE_TAG:
                        {
                            UpdatePlayerProjectile(gameObject);
                        }
                        break;
                    case ENEMY_PROJECTILE_TAG:
                        {
                            UpdateEnemyProjectile(gameObject);
                        }
                        break;
                    case ENEMY_TAG:
                        {
                            UpdateEnemy(gameObject);
                        }
                        break;
                    case METEOR_TAG:
                        {
                            UpdateMeteor(gameObject);
                        }
                        break;
                    case HEALTH_TAG:
                        {
                            UpdateHealth(gameObject);
                        }
                        break;
                    case COLLECTIBLE_TAG:
                        {
                            UpdateCollectible(gameObject);
                        }
                        break;
                    case POWERUP_TAG:
                        {
                            UpdatePowerUp(gameObject);
                        }
                        break;
                    default:
                        break;
                }

            }).IsCompleted)
            {
                // clean removable objects from game view
                GameView.RemoveDestroyableGameObjects();
            }
        }

        /// <summary>
        /// Spawns game objects.
        /// </summary>
        private void SpawnGameObjects()
        {
            _celestialObjectFactory.SpawnCelestialObject();

            // only generate game objects if not warping thorugh space
            if (!StarView.IsWarpingThroughSpace)
            {
                _meteorFactory.SpawnMeteor(GameLevel);

                _enemyFactory.SpawnEnemy(GameLevel);

                _healthFactory.SpawnHealth(Player);

                _powerUpFactory.SpawnPowerUp();

                _collectibleFactory.SpawnCollectible(GameLevel);

                _playerProjectileFactory.SpawnProjectile(
                    isPoweredUp: Player.IsPoweredUp,
                    player: Player,
                    gameLevel: GameLevel,
                    powerUpType: PowerUpType);
            }
        }

        #endregion

        #region In Game Text & Image

        /// <summary>
        /// Shows in game content.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        private void ShowInGameContent(Image image, string text)
        {
            ShowInGameImagePanel(image);
            ShowInGameText(text);
        }

        /// <summary>
        /// Shows the in game text in game view.
        /// </summary>
        private void ShowInGameText(string text)
        {
            InGameText.Visibility = Visibility.Visible;
            InGameText.Text = text;
            _showInGameTextSpawnCounter = _showInGameTextAfter;
        }

        /// <summary>
        /// Shows in game image.
        /// </summary>
        /// <param name="image"></param>
        private void ShowInGameImagePanel(Image image)
        {
            var scale = GameView.GetGameObjectScale();

            image.Height = 100 * scale;
            image.Width = 100 * scale;

            InGameImagePanel.Children.Clear();
            InGameImagePanel.Children.Add(image);
            InGameImagePanel.Visibility = Visibility.Visible;
            _showInGameImagePanelSpawnCounter = _showInGameImagePanelAfter;
        }

        /// <summary>
        /// Hides the in game text after keeping it visible for a few frames.
        /// </summary>
        private void InGameTextCoolDown()
        {
            if (!InGameText.Text.IsNullOrBlank())
            {
                _showInGameTextSpawnCounter--;

                if (_showInGameTextSpawnCounter <= 0)
                    HideInGameText();
            }
        }

        /// <summary>
        /// Hides the in game image after keeping it visible for a few frames.
        /// </summary>
        private void InGameImagePanelCoolDown()
        {
            if (InGameImagePanel.Visibility == Visibility.Visible)
            {
                _showInGameImagePanelSpawnCounter -= 1;

                if (_showInGameImagePanelSpawnCounter <= 0)
                {
                    HideInGameImagePanel();
                }
            }
        }

        /// <summary>
        /// Hides the in game text.
        /// </summary>
        private void HideInGameText()
        {
            InGameText.Visibility = Visibility.Collapsed;
            InGameText.Text = null;
        }

        /// <summary>
        /// Hides in game image.
        /// </summary>
        private void HideInGameImagePanel()
        {
            InGameImagePanel.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Misc View Functionality

        /// <summary>
        /// Warps the player through space.
        /// </summary>
        private void WarpThroughSpace()
        {
            var destructibles = GameView.GetGameObjects<GameObject>().Where(x => x.IsDestructible);

            if (destructibles is not null)
            {
                Parallel.ForEach(destructibles, destructible =>
                {
                    destructible.IsMarkedForFadedDestruction = true;
                });
            }

            var projectiles = GameView.GetGameObjects<GameObject>().Where(x => x.IsProjectile);

            if (projectiles is not null)
            {
                Parallel.ForEach(projectiles, projectile =>
                {
                    GameView.AddDestroyableGameObject(projectile);
                });
            }

            var pickups = GameView.GetGameObjects<GameObject>().Where(x => x.IsPickup);

            if (pickups is not null)
            {
                Parallel.ForEach(pickups, pickup =>
                {
                    GameView.AddDestroyableGameObject(pickup);
                });
            }

            var collectibles = GameView.GetGameObjects<GameObject>().Where(x => x.IsCollectible);

            if (collectibles is not null)
            {
                Parallel.ForEach(collectibles, collectible =>
                {
                    GameView.AddDestroyableGameObject(collectible);
                });
            }

            _celestialObjectFactory.StartSpaceWarp();
        }

        /// <summary>
        /// Add stars to game environemnt randomly.
        /// </summary>
        private void SetStars()
        {
            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var star = new CelestialObject();

                star.SetAttributes(speed: 0.1d, scale: StarView.GetGameObjectScale());

                var top = random.Next(10, (int)StarView.Height - 10);
                var left = random.Next(10, (int)StarView.Width - 10);

                star.AddToGameEnvironment(top: top, left: left, gameEnvironment: StarView);
            }
        }

        /// <summary>
        /// Sets the game and star view sizes according to current window size.
        /// </summary>
        private void AdjustView()
        {
            GameView.SetSize(_windowHeight, _windowWidth);
            StarView.SetSize(_windowHeight, _windowWidth);
            PlanetView.SetSize(_windowHeight, _windowWidth);

            _frameTime = DEFAULT_FRAME_TIME + GameView.GetFrameTimeBuffer();

#if DEBUG
            Console.WriteLine($"Frame Time : {_frameTime}");
#endif
            if (IsGameRunning)
            {
                PauseGame();

                PointerX = _windowWidth / 2;

                Player.SetX(PointerX - Player.HalfWidth);

                SetPlayerY(); // windows size changed so reset y position

                // resize player size
                var scale = GameView.GetGameObjectScale();
                Player.ReAdjustScale(scale: scale);
#if DEBUG
                Console.WriteLine($"View Scale: {scale}");
#endif
                return;
            }
        }

        #endregion

        #region Misc Game Functionality

        /// <summary>
        /// Reset all factories to default value.
        /// </summary>
        private void ResetFactories()
        {
            _celestialObjectFactory.Reset();
            _meteorFactory.Reset();
            _enemyFactory.Reset();
            _healthFactory.Reset();
            _powerUpFactory.Reset();
            _collectibleFactory.Reset();
        }

        /// <summary>
        /// Sets analytics of fps, frame time and objects currently in view.
        /// </summary>
        private void SetAnalytics()
        {

#if DEBUG
            _frameStatUpdateSpawnCounter -= 1;

            if (_frameStatUpdateSpawnCounter < 0)
            {
                var gameObjects = GameView.Children.OfType<GameObject>();
                var starObjects = StarView.Children.OfType<GameObject>();
                var planetObjects = PlanetView.Children.OfType<GameObject>();

                var fpsText = $"FPS: {_fpsCount} | FRAME_TIME: {_frameTime} | FRAME_DURATION: {(int)_frameDuration}";
                FPSText.Text = fpsText;

                var objectsCountText =
                    $"ENEMIES: {gameObjects.Count(x => (string)x.Tag == Constants.ENEMY_TAG)} " +
                    $"| METEORS : {gameObjects.Count(x => (string)x.Tag == Constants.METEOR_TAG)} " +
                    $"| POWERUPS : {gameObjects.Count(x => (string)x.Tag == Constants.POWERUP_TAG)} " +
                    $"| ENEMY_PROJECTILES : {gameObjects.Count(x => (string)x.Tag == Constants.ENEMY_PROJECTILE_TAG)} " +
                    $"| PLAYER_PROJECTILES : {gameObjects.Count(x => (string)x.Tag == Constants.PLAYER_PROJECTILE_TAG)} " +
                    $"| STARS : {starObjects.Count(x => (string)x.Tag == Constants.STAR_TAG)} " +
                    $"| PLANETS : {planetObjects.Count(x => (string)x.Tag == Constants.STAR_TAG)} ";

                var total = gameObjects.Count() + starObjects.Count() + planetObjects.Count();
                var totalText = $"TOTAL: {total}";

                ObjectsCountText.Text = objectsCountText + "\n" + totalText;
                _frameStatUpdateSpawnCounter = _frameStatUpdateAfter;
            }
#endif
        }

        /// <summary>
        /// Sets the frame time.
        /// </summary>
        private void GetFrameDuration()
        {
#if DEBUG
            _frameDuration = _frameEndTime - _frameStartTime;
#endif
        }

        /// <summary>
        /// Calculates the frames per second.
        /// </summary>
        private void CalculateFPS()
        {
#if DEBUG
            // calculate FPS
            if (_lastFpsTime + 1000 < _frameStartTime)
            {
                _fpsCount = _fpsSpawnCounter;
                _fpsSpawnCounter = 0;
                _lastFpsTime = _frameStartTime;
            }

            _fpsSpawnCounter++;
#endif
        }

        #endregion

        #region Player

        /// <summary>
        /// Spawns the player.
        /// </summary>
        private void SpawnPlayer()
        {
            var scale = GameView.GetGameObjectScale();
            Player = _playerFactory.SpawnPlayer(pointerX: PointerX, ship: App.Ship);

            _rageImage = new Image()
            {
                Stretch = Stretch.Uniform,
            };

            _rageImage.Source = new BitmapImage(GameObjectTemplates.PLAYER_RAGE_TEMPLATES.FirstOrDefault(x => x.ShipClass == Player.ShipClass).AssetUri);

            switch (Player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    {
                        PlayerHealthBarPanel.Background = new SolidColorBrush(Colors.Goldenrod);
                        PlayerHealthBarPanel.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                    }
                    break;
                case ShipClass.BERSERKER:
                    {
                        PlayerHealthBarPanel.Background = new SolidColorBrush(Colors.Silver);
                        PlayerHealthBarPanel.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                    break;
                case ShipClass.SPECTRE:
                    {
                        PlayerHealthBarPanel.Background = new SolidColorBrush(Colors.MediumPurple);
                        PlayerHealthBarPanel.BorderBrush = new SolidColorBrush(Colors.Purple);
                    }
                    break;
                default:
                    break;
            }

            PlayerRageBar.Maximum = Player.RageThreashold;
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        private void SetPlayerY()
        {
            PointerY = _playerFactory.GetOptimalPlayerY(Player);
            Player.SetY(PointerY);
        }

        /// <summary>
        /// Sets player health bar.
        /// </summary>
        private void SetPlayerHealthBar()
        {
            PlayerHealthBar.Value = Player.Health;
        }

        /// <summary>
        /// Handles damage recovery of the player after getting hit.
        /// </summary>
        private void DamageRecoveryCoolDown()
        {
            _playerFactory.DamageRecoveryCoolDown(Player);
        }

        /// <summary>
        /// Start player movement on pointer press.
        /// </summary>
        /// <param name="e"></param>
        private void StartPlayerMovement(PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(GameView);

            PointerPressedX = point.Position.X;

            /* Left corner
             * ->x|  w  |
             *    |  w  |
             */

            /* Right corner
             * |  w  |x<-
             * |  w  |
             */

            if (PointerPressedX < Player.GetX())  // move left
            {
                MoveLeft = true;
                MoveRight = false;
            }
            else if (PointerPressedX > Player.GetX() + Player.Width) // move right
            {
                MoveRight = true;
                MoveLeft = false;
            }
        }

        /// <summary>
        /// Stops player movement on reaching pointer.
        /// </summary>
        private void StopPlayerMovement()
        {
            if (IsPointerPressed)
            {
                /* Left corner with half width
                * ->x...|  w  |
                *       |  w  |
                */

                /* Right corner with half width
                * |  w  |...x<-
                * |  w  |
                */

                if (MoveLeft)
                {
                    if (Player.GetX() - Player.HalfWidth <= PointerPressedX)
                        MoveLeft = false;
                }
                else if (MoveRight)
                {
                    if (Player.GetX() + Player.Width + Player.HalfWidth >= PointerPressedX)
                        MoveRight = false;
                }
            }
        }

        /// <summary>
        /// Update player in the game view.
        /// </summary>
        private void UpdatePlayer()
        {
            if (MoveLeft || MoveRight)
            {
                var pointerX = _playerFactory.UpdatePlayer(
                    player: Player,
                    pointerX: PointerX,
                    moveLeft: MoveLeft,
                    moveRight: MoveRight);

                PointerX = pointerX;

                StopPlayerMovement();
            }
            else
            {
                var pointerX = _playerFactory.UpdateAcceleration(player: Player, pointerX: PointerX);
                PointerX = pointerX;
            }

            if (Player.IsPoweredUp && !StarView.IsWarpingThroughSpace)
            {
                PowerUpCoolDown();
            }

            if (Player.IsRageUp && !StarView.IsWarpingThroughSpace)
            {
                RageCoolDown();
            }
        }

        /// <summary>
        /// Update a player projectile in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdatePlayerProjectile(GameObject gameObject)
        {
            var projectile = gameObject as PlayerProjectile;

            // move the projectile up and check if projectile has gone beyond the game view
            _playerProjectileFactory.UpdateProjectile(
                projectile: projectile,
                destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (projectile.IsMarkedForFadedDestruction)
                return;

            _playerProjectileFactory.CollidePlayerProjectile(
                projectile: projectile,
                score: out double score,
                destroyedObject: out GameObject destroyedObject);

            if (GameView.IsBossEngaged)
            {
                SetBossHealthBar(); // set boss health bar on projectile hit
            }

            if (score > 0)
            {
                if (!Player.IsRageUp)
                {
                    AddRage();
                }

                // trigger rage after rage threashold kills
                if (!Player.IsRageUp && Player.Rage >= Player.RageThreashold)
                {
                    ActivateRage();
                }

                AddScore(score);
                SetGameLevel(); // check game level on score change
            }

            if (destroyedObject is not null)
            {
                switch (destroyedObject.Tag)
                {
                    case ENEMY_TAG:
                        {
                            var enemy = destroyedObject as Enemy;

                            _enemyFactory.DestroyEnemy(enemy);
                            PlayerScore.EnemiesDestroyed++;

                            if (enemy.IsBoss)
                            {
                                DisengageBoss(enemy);
                                PlayerScore.BossesDestroyed++;
                            }
                        }
                        break;
                    case METEOR_TAG:
                        {
                            _meteorFactory.DestroyMeteor(destroyedObject as Meteor);
                            PlayerScore.MeteorsDestroyed++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion        

        #region Enemy

        /// <summary>
        /// Update an enemy in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateEnemy(GameObject gameObject)
        {
            var enemy = gameObject as Enemy;

            _enemyFactory.UpdateEnemy(enemy: enemy, pointerX: PointerX, destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (enemy.IsMarkedForFadedDestruction)
                return;

            // check if enemy collides with player
            if (_playerFactory.PlayerCollision(player: Player, gameObject: enemy))
            {
                _playerProjectileFactory.DecreaseProjectilePower(player: Player);
                SetPlayerHealthBar();
                return;
            }

            // fire projectiles if at a legitimate distance from player
            if (enemy.IsProjectileFiring && Player.GetY() - enemy.GetY() > 100)
                _enemyProjectileFactory.SpawnProjectile(enemy: enemy, gameLevel: GameLevel);
        }

        /// <summary>
        /// Update an enemy projectile in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateEnemyProjectile(GameObject gameObject)
        {
            var projectile = gameObject as EnemyProjectile;

            _enemyProjectileFactory.UpdateProjectile(projectile, destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (projectile.IsMarkedForFadedDestruction)
                return;

            // check if enemy projectile collides with player
            if (_playerFactory.PlayerCollision(player: Player, gameObject: projectile))
            {
                _playerProjectileFactory.DecreaseProjectilePower(player: Player);
                SetPlayerHealthBar();
            }
        }

        #endregion

        #region Boss

        /// <summary>
        /// Engages a boss.
        /// </summary>
        private void EngageBoss()
        {
            ShowInGameContent(image: _bossAppearedImage, text: $"{_localizationHelper.GetLocalizedResource("LEVEL")} {(int)GameLevel} {_localizationHelper.GetLocalizedResource("BOSS")}");

            var boss = _enemyFactory.EngageBoss(GameLevel);
            Bosses.Add(boss);

            switch (boss.BossClass)
            {
                case BossClass.GREEN:
                    BossHealthBar.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case BossClass.CRIMSON:
                    BossHealthBar.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case BossClass.BLUE:
                    BossHealthBar.Foreground = new SolidColorBrush(Colors.SkyBlue);
                    break;
                default:
                    break;
            }

            BossHealthBarPanel.Visibility = Visibility.Visible;
            BossTotalHealth = Bosses.Sum(x => x.Health);

            SetBossHealthBar(); // set boss health on boss appearance            
        }

        /// <summary>
        /// Sets the boss health bar.
        /// </summary>
        private void SetBossHealthBar()
        {
            BossHealthBar.Value = Bosses.Sum(x => x.Health) / BossTotalHealth * 100;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        /// <param name="boss"></param>
        private void DisengageBoss(Enemy boss)
        {
            WarpThroughSpace();
            ShowInGameContent(_bossClearedImage, $"{_localizationHelper.GetLocalizedResource("LEVEL")} {(int)GameLevel} {_localizationHelper.GetLocalizedResource("COMPLETE")}");

            _enemyFactory.DisengageBoss();

            Bosses.Remove(boss);

            if (Bosses.Count == 0)
            {
                BossHealthBarPanel.Visibility = Visibility.Collapsed;
                BossTotalHealth = 0;
            }

            SetGameLevelText();
        }

        #endregion

        #region Meteor

        /// <summary>
        /// Update a meteor in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateMeteor(GameObject gameObject)
        {
            var meteor = gameObject as Meteor;

            _meteorFactory.UpdateMeteor(meteor: meteor, destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (meteor.IsMarkedForFadedDestruction)
                return;

            // check if meteor collides with player
            if (_playerFactory.PlayerCollision(player: Player, gameObject: meteor))
            {
                _playerProjectileFactory.DecreaseProjectilePower(player: Player);
                SetPlayerHealthBar();
            }
        }


        #endregion

        #region Health

        /// <summary>
        /// Update a health in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateHealth(GameObject gameObject)
        {
            var health = gameObject as Health;

            _healthFactory.UpdateHealth(health: health, destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            // check if health collides with player
            if (_playerFactory.PlayerCollision(player: Player, gameObject: health))
            {
                SetPlayerHealthBar();
                ShowInGameContent(_healthImage, $"‍{_localizationHelper.GetLocalizedResource("SHIP_REPAIRED")}");
            }
        }

        #endregion

        #region Collectible

        /// <summary>
        /// Update a collectible in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateCollectible(GameObject gameObject)
        {
            var collectible = gameObject as Collectible;

            _collectibleFactory.UpdateCollectible(collectible: collectible, destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            // check if collectible collides with player
            if (_playerFactory.PlayerCollision(player: Player, gameObject: collectible))
            {
                _playerProjectileFactory.IncreaseProjectilePower(player: Player);
                PlayerScore.CollectiblesCollected++;

                AddScore(1);
                AddScoreMultiplier();

                if (ScoreMultiplierCount >= ScoreMultiplierThreashold)
                    ActivateScoreMultiplier();

                SetGameLevel(); // check game level on score change                                
            }
        }

        #endregion

        #region Rage

        /// <summary>
        /// Adds rage.
        /// </summary>
        private void AddRage()
        {
            Player.Rage++;
            PlayerRageBar.Value = Player.Rage;
        }

        /// <summary>
        /// Activates rage.
        /// </summary>
        private void ActivateRage()
        {
            PlayerRageIcon.Text = "🤬";
            _playerFactory.RageUp(Player);
            _playerProjectileFactory.RageUp(Player);

            switch (Player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    ShowInGameContent(_rageImage, $"{_localizationHelper.GetLocalizedResource("SHIELD_UP")}");
                    break;
                case ShipClass.BERSERKER:
                    ShowInGameContent(_rageImage, $"{_localizationHelper.GetLocalizedResource("FIRING_RATE_INCREASED")}");
                    break;
                case ShipClass.SPECTRE:
                    ShowInGameContent(_rageImage, $"{_localizationHelper.GetLocalizedResource("CLOAK_UP")}");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Cools down rage effect.
        /// </summary>
        private void RageCoolDown()
        {
            var coolDown = _playerFactory.RageUpCoolDown(Player);

            PlayerRageBar.Value = coolDown.RageRemaining;

            if (coolDown.RageDown)
            {
                _playerProjectileFactory.RageDown(Player);

                PlayerRageBar.Value = Player.Rage;
                PlayerRageIcon.Text = "😡";

                switch (Player.ShipClass)
                {
                    case ShipClass.DEFENDER:
                        ShowInGameContent(_rageImage, $"{_localizationHelper.GetLocalizedResource("SHIELD_DOWN")}");
                        break;
                    case ShipClass.BERSERKER:
                        ShowInGameContent(_rageImage, $"{_localizationHelper.GetLocalizedResource("FIRING_RATE_DECREASED")}");
                        break;
                    case ShipClass.SPECTRE:
                        ShowInGameContent(_rageImage, $"{_localizationHelper.GetLocalizedResource("CLOAK_DOWN")}");
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region PowerUp

        /// <summary>
        /// Update a powerup in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdatePowerUp(GameObject gameObject)
        {

            var powerUp = gameObject as PowerUp;

            _powerUpFactory.UpdatePowerUp(powerUp: powerUp, destroyed: out bool destroyed);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            // check if power up collides with player
            if (_playerFactory.PlayerCollision(player: Player, gameObject: powerUp))
            {
                ActivatePowerUp(powerUp);
            }
        }

        /// <summary>
        /// Activates power up.
        /// </summary>
        /// <param name="powerUp"></param>

        private void ActivatePowerUp(PowerUp powerUp)
        {
            PlayerPowerBar.Visibility = Visibility.Visible;

            PowerUpType = powerUp.PowerUpType;

            ShowInGameContent(_powerUpImage, $"‍{_localizationHelper.GetLocalizedResource(PowerUpType.ToString())}"); // show power up text

            _playerProjectileFactory.PowerUp(powerUpType: PowerUpType, player: Player);
        }

        /// <summary>
        /// Cools down power up effect.
        /// </summary>
        private void PowerUpCoolDown()
        {
            var coolDown = _playerFactory.PowerUpCoolDown(Player);

            PlayerPowerBar.Value = coolDown.PowerRemaining;

            if (coolDown.PowerDown)
            {
                _playerProjectileFactory.PowerDown(PowerUpType, player: Player);
                PlayerPowerBar.Visibility = Visibility.Collapsed;

                PowerUpType = PowerUpType.NONE;
                ShowInGameContent(_powerUpImage, $"{_localizationHelper.GetLocalizedResource("POWER_DOWN")}");
            }
        }

        #endregion

        #region Star & Planet

        /// <summary>
        /// Update a star in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateCelestialObject(GameObject gameObject)
        {
            var star = gameObject as CelestialObject;

            _celestialObjectFactory.UpdateCelestialObject(celestialObject: star, destroyed: out bool destroyed);
        }

        #endregion

        #region Difficulty

        /// <summary>
        /// Sets the game level according to score; 
        /// </summary>
        private void SetGameLevel()
        {
            var lastGameLevel = GameLevel;

            if (PlayerScore.Score >= 0)
            {
                GameLevel = GameLevel.Level_1;
                ScoreBar.Value = PlayerScore.Score / 25 * 100;
                SetScoreBarCountText(25);
            }
            if (PlayerScore.Score > 25)
            {
                GameLevel = GameLevel.Level_2;
                ScoreBar.Value = PlayerScore.Score / 100 * 100;
                SetScoreBarCountText(100);
            }
            if (PlayerScore.Score > 100)
            {
                GameLevel = GameLevel.Level_3;
                ScoreBar.Value = PlayerScore.Score / 200 * 100;
                SetScoreBarCountText(200);
            }
            if (PlayerScore.Score > 200)
            {
                GameLevel = GameLevel.Level_4;
                ScoreBar.Value = PlayerScore.Score / 400 * 100;
                SetScoreBarCountText(400);
            }
            if (PlayerScore.Score > 400)
            {
                GameLevel = GameLevel.Level_5;
                ScoreBar.Value = PlayerScore.Score / 600 * 100;
                SetScoreBarCountText(600);
            }
            if (PlayerScore.Score > 600)
            {
                GameLevel = GameLevel.Level_6;
                ScoreBar.Value = PlayerScore.Score / 800 * 100;
                SetScoreBarCountText(800);
            }
            if (PlayerScore.Score > 800)
            {
                GameLevel = GameLevel.Level_7;
                ScoreBar.Value = PlayerScore.Score / 1000 * 100;
                SetScoreBarCountText(1000);
            }
            if (PlayerScore.Score > 1000)
            {
                GameLevel = GameLevel.Level_8;
                ScoreBar.Value = PlayerScore.Score / 1200 * 100;
                SetScoreBarCountText(1200);
            }
            if (PlayerScore.Score > 1200)
            {
                GameLevel = GameLevel.Level_9;
                ScoreBar.Value = PlayerScore.Score / 1400 * 100;
                SetScoreBarCountText(1400);
            }
            if (PlayerScore.Score > 1400)
            {
                GameLevel = GameLevel.Level_10;
                ScoreBar.Value = PlayerScore.Score / 1600 * 100;
                SetScoreBarCountText(1600);
            }
            if (PlayerScore.Score > 1600)
            {
                GameLevel = GameLevel.Level_11;
                ScoreBar.Value = PlayerScore.Score / 1800 * 100;
                SetScoreBarCountText(1800);
            }
            if (PlayerScore.Score > 1800)
            {
                GameLevel = GameLevel.Level_12;
                ScoreBar.Value = PlayerScore.Score / 2000 * 100;
                SetScoreBarCountText(2000);
            }
            if (PlayerScore.Score > 2000)
            {
                GameLevel = GameLevel.Level_13;
                ScoreBarCount.Text = $"{_localizationHelper.GetLocalizedResource("SCORE")} {PlayerScore.Score}/MAX";
            }

            // when difficulty changes show level up
            if (lastGameLevel != GameLevel)
            {
                LevelUpObjects();

                // bosses apprear after level 2
                if (GameLevel > GameLevel.Level_2) //TODO: SET TO LEVEL 2
                {
                    EngageBoss();
                }
                else
                {
                    WarpThroughSpace();
                    ShowInGameText($"👊 {_localizationHelper.GetLocalizedResource("ENEMY_APPROACHES")}");
                    _audioHelper.PlaySound(SoundType.ENEMY_INCOMING);
                    _audioHelper.PlaySound(SoundType.BACKGROUND_MUSIC);
                    SetGameLevelText();
                }
            }
        }

        /// <summary>
        /// Sets the score bar text in ui.
        /// </summary>
        /// <param name="capacity"></param>
        private void SetScoreBarCountText(int capacity)
        {
            ScoreBarCount.Text = $"{_localizationHelper.GetLocalizedResource("SCORE")} {PlayerScore.Score}/{capacity}";
        }

        /// <summary>
        /// Sets the game level text in ui.
        /// </summary>
        private void SetGameLevelText()
        {
            GameLevelText.Text = $"{_localizationHelper.GetLocalizedResource("LEVEL")} {(int)GameLevel + 1}";
        }

        /// <summary>
        /// Performs level up of all game view objects.
        /// </summary>
        private void LevelUpObjects()
        {
            switch (GameLevel)
            {
                case GameLevel.Level_1:
                    break;
                default:
                    {
                        _enemyFactory.LevelUp();
                        _meteorFactory.LevelUp();
                        _healthFactory.LevelUp();
                        _powerUpFactory.LevelUp();
                        _collectibleFactory.LevelUp();
                        _celestialObjectFactory.LevelUp();
                        _playerProjectileFactory.LevelUp(player: Player);
                    }
                    break;
            }
        }

        #endregion

        #region Score

        private void AddScoreMultiplier()
        {
            ScoreMultiplierCount++;
            SetScoreMultiplierCountText();

            // TODO: increase multiplier progress bar
        }


        private void ActivateScoreMultiplier()
        {
            ScoreMultiplierCount = 0;
            SetScoreMultiplierCountText();

            _scoreMultiplierCoolDownCounter = _scoreMultiplierCoolDownAfter;
            IsScoreMultiplierActivated = true;
            _audioHelper.PlaySound(SoundType.SCORE_MULTIPLIER_ON);
            ShowInGameContent(_scoreMultiplierImage, _localizationHelper.GetLocalizedResource("SCORE_MULTIPLIER_ON"));
        }

        private void ScoreMultiplierCoolDown()
        {
            if (IsScoreMultiplierActivated)
            {
                _scoreMultiplierCoolDownCounter--;

                // TODO: decrease multiplier progress bar
                ScoreMultiplierBar.Value = _scoreMultiplierCoolDownCounter / _scoreMultiplierCoolDownAfter * ScoreMultiplierThreashold;

                if (_scoreMultiplierCoolDownCounter <= 0)
                {
                    IsScoreMultiplierActivated = false;
                    _audioHelper.PlaySound(SoundType.SCORE_MULTIPLIER_OFF);
                    ShowInGameContent(_scoreMultiplierImage, _localizationHelper.GetLocalizedResource("SCORE_MULTIPLIER_OFF"));
                }
            }
        }

        private void SetScoreMultiplierCountText()
        {
            ScoreMultiplierCountText.Text = $"x{ScoreMultiplierCount}";
            ScoreMultiplierBar.Value = ScoreMultiplierCount;
        }

        /// <summary>
        /// Adds the score.
        /// </summary>
        /// <param name="score"></param>
        private void AddScore(double score)
        {
            PlayerScore.Score += IsScoreMultiplierActivated ? score * 2 : score;
        }

        #endregion

        #endregion
    }
}
