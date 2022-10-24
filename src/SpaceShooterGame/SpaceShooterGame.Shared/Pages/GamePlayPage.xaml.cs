using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Windows.Foundation;
using static SpaceShooterGame.Constants;

namespace SpaceShooterGame
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
        private readonly List<double> _framesCount = new();

        private int _frameStatUpdateSpawnCounter;
        private readonly int _frameStatUpdateAfter = 5;

        private long _frameStartTime;
        private long _frameEndTime;
        private double _frameDuration;
        private double _maxFrameDuration = 0;
#endif
        private double _scoreMultiplierCoolDownCounter;
        private readonly double _scoreMultiplierCoolDownAfter = 1000;

        private double _frameTime;
        private PeriodicTimer _frameTimer;

        private double _windowWidth, _windowHeight;

#if DEBUG
        private Stopwatch _stopwatch;
#endif
        private Player _player;
        private SpaceShooterGameScore _playerScore = new SpaceShooterGameScore();
        private int _scoreCap;

        private int _gameLevel;

        private PowerUpType _powerUpType;

        private bool _isGameRunning;
        private bool _isGamePaused;
        private bool _isGameQuitting;

        private List<Enemy> _bosses;

        private double _bossTotalHealth;

        private bool _isPointerActivated;
        private Point _pointerPosition;

        private double _pointerX;
        private double _pointerY;

        private bool _moveLeft;
        private bool _moveRight;

        private bool _isScoreMultiplierActivated;
        private int _scoreMultiplierCount;

        #endregion

        #region Ctor

        public GamePlayPage()
        {
            InitializeComponent();

            Loaded += GamePage_Loaded;
            Unloaded += GamePage_Unloaded;

            _windowWidth = Window.Current.Bounds.Width - 10;
            _windowHeight = Window.Current.Bounds.Height - 10;

            _pointerX = _windowWidth / 2;

            SetViewSize(); // at constructor

            CelestialObjectFactory.SetGameEnvironments(StarView, PlanetView);
            MeteorFactory.SetGameEnvironment(GameView);
            EnemyFactory.SetGameEnvironment(GameView);
            HealthFactory.SetGameEnvironment(GameView);
            PowerUpFactory.SetGameEnvironment(GameView);
            CollectibleFactory.SetGameEnvironment(GameView);
            PlayerFactory.SetGameEnvironment(GameView);
            PlayerProjectileFactory.SetGameEnvironment(GameView);
            EnemyProjectileFactory.SetGameEnvironment(GameView);
        }

        #endregion        

        #region Events

        #region Input

        private void InputView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                    {
                        _moveLeft = true;
                        _moveRight = false;
                    }
                    break;
                case Windows.System.VirtualKey.Right:
                    {
                        _moveRight = true;
                        _moveLeft = false;
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
                        _moveLeft = false;
                    }
                    break;
                case Windows.System.VirtualKey.Right:
                    {
                        _moveRight = false;
                    }
                    break;
                case Windows.System.VirtualKey.Escape:
                    {
                        if (_isGamePaused)
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
            if (_isGameRunning)
            {
                _isPointerActivated = true;
                PointerPoint point = e.GetCurrentPoint(GameView);
                _pointerPosition = point.Position;
            }
        }

        private void InputView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_isGameRunning && _isPointerActivated)
            {
                PointerPoint point = e.GetCurrentPoint(GameView);
                _pointerPosition = point.Position;
            }
        }

        private void InputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_isGameQuitting)
            {
                QuitGame();
                return;
            }

            if (_isGameRunning)
            {
                if (_isGamePaused)
                    ResumeGame();

                _isPointerActivated = false;
                _pointerPosition = null;
            }
            else
            {
                InputView.Focus(FocusState.Programmatic);
                StartGame();
            }
        }

        #endregion        

        #region Buttons

        private void PauseGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }

        private void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isGameQuitting)
            {
                _isGameQuitting = false;
                PauseGame();
            }
            else
            {
                HideInGameContent();
                AudioHelper.PlaySound(SoundType.MENU_SELECT);
                _isGameQuitting = true;
                ShowInGameText($"🛸\n{LocalizationHelper.GetLocalizedResource("QUIT_GAME")}\n{LocalizationHelper.GetLocalizedResource("TAP_TO_QUIT")}");

                InputView.Focus(FocusState.Programmatic);
            }
        }

        #endregion        

        #region Page

        /// <summary>
        /// Invoked when the page is leaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += GamePage_SizeChanged;
            SetDefaultGameState();
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

            SetViewSize(); // at view size change
#if DEBUG
            Console.WriteLine($"View Size: {_windowWidth} x {_windowHeight}");
            Console.WriteLine($"View Scale: {GameView.GameObjectScale}");
#endif
        }

        #endregion

        #endregion

        #region Methods

        #region Game

        private void SetDefaultGameState()
        {
            GameView.Children.Clear();
            StarView.Children.Clear();
            PlanetView.Children.Clear();

            ScoreBarPanel.Visibility = Visibility.Collapsed;

            FPSText.Text = "";
            ObjectsCountText.Text = "";

            _bosses = new List<Enemy>();
            BossHealthBarPanel.Visibility = Visibility.Collapsed;

            _player = null;
            PlayerHealthBarPanel.Visibility = Visibility.Collapsed;

            _gameLevel = 1;
            SetGameLevelText();

            _powerUpType = PowerUpType.NONE;
            PlayerPowerBar.Maximum = POWER_UP_METER;
            PlayerPowerBar.Value = POWER_UP_METER;

            _playerScore = new SpaceShooterGameScore();
            _scoreCap = 15;
            SetScoreBarCountText(_scoreCap);

            _pointerX = _windowWidth / 2;
            _isPointerActivated = false;
            _moveLeft = false;
            _moveRight = false;

            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Collapsed;

            _scoreMultiplierCount = 0;
            _isScoreMultiplierActivated = false;
            SetScoreMultiplierCountText();
            ScoreMultiplierPanel.Visibility = Visibility.Collapsed;
            ScoreMultiplierBar.Maximum = SCORE_MULTIPLIER_THREASHOLD;
            ScoreMultiplierBar.Value = 0;

            RageGradientBorder.Visibility = Visibility.Collapsed;

            ShowInGameText("👆\n" + LocalizationHelper.GetLocalizedResource("TAP_ON_SCREEN_TO_BEGIN"));
            InputView.Focus(FocusState.Programmatic);

            _powerUpImage = new Image
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.POWERUP).AssetUri)
            };

            _healthImage = new Image
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.HEALTH).AssetUri)
            };

            _bossAppearedImage = new Image
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.BOSS_APPEARED).AssetUri)
            };

            _bossClearedImage = new Image
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.BOSS_CLEARED).AssetUri)
            };

            _scoreMultiplierImage = new Image
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.SCORE_MULTIPLIER).AssetUri)
            };
#if DEBUG
            _framesCount.Clear();
#endif
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        private void StartGame()
        {
#if DEBUG
            GameAnalyticsPanel.Visibility = Visibility.Visible;
#endif
            AudioHelper.StopSound();
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            AudioHelper.PlaySound(SoundType.GAME_START);

            SpawnPlayer();
            SetPlayerY(); // set y position at game start
            SetPlayerHealthBar(); // set player health bar at game start
            HideInGameContent();
            ResetFactories();

            _isGameRunning = true;

            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            PlayerHealthBarPanel.Visibility = Visibility.Visible;

            ScoreBarPanel.Visibility = Visibility.Visible;
            ScoreMultiplierPanel.Visibility = Visibility.Visible;

            AudioHelper.PlaySound(SoundType.BACKGROUND);

            WarpThroughSpace(); // at the starting of the game
            RunGame();
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        /// <returns></returns>
        private async void RunGame()
        {
#if DEBUG
            _stopwatch = Stopwatch.StartNew();
#endif          

            var interval = TimeSpan.FromMilliseconds(_frameTime);
            _frameTimer = new PeriodicTimer(interval);

            while (await _frameTimer.WaitForNextTickAsync())
            {
                GameViewFrameAction();
                PlanetViewFrameAction();
                StarViewFrameAction();
            }
        }

        private void PlanetViewFrameAction()
        {
            UpdatePlanetViewObjects();
            PlanetView.RemoveDestroyableGameObjects();
        }

        private void StarViewFrameAction()
        {
            UpdateStarViewObjects();
            CelestialObjectFactory.SpawnCelestialObject();
            StarView.RemoveDestroyableGameObjects();
        }

        private void GameViewFrameAction()
        {
#if DEBUG
            _frameStartTime = _stopwatch.ElapsedMilliseconds;
#endif
            CheckGameOver();
            UpdateGameViewObjects();
            SpawnGameViewObjects();
            InGameContentCoolDown();
            DamageRecoveryCoolDown();
            ScoreMultiplierCoolDown();
            GameView.RemoveDestroyableGameObjects();

#if DEBUG
            _frameEndTime = _stopwatch.ElapsedMilliseconds;
            CalculateFPS();
            GetFrameDuration();
            SetAnalytics();
#endif
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        private void PauseGame()
        {
            HideInGameContent();
            InputView.Focus(FocusState.Programmatic);

            _frameTimer?.Dispose();

            ShowInGameText($"👨‍🚀\n{LocalizationHelper.GetLocalizedResource("GAME_PAUSED")}\n{LocalizationHelper.GetLocalizedResource("TAP_TO_RESUME")}");

            _isGamePaused = true;
            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Visible;

            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            AudioHelper.PauseSound(SoundType.BACKGROUND);

            if (GameView.IsBossEngaged)
                AudioHelper.PauseSound(SoundType.BOSS_APPEARANCE);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        private void ResumeGame()
        {
            InputView.Focus(FocusState.Programmatic);

            HideInGameContent();

            _isGamePaused = false;
            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            if (GameView.IsBossEngaged)
                AudioHelper.ResumeSound(SoundType.BOSS_APPEARANCE);
            else
                AudioHelper.ResumeSound(SoundType.BACKGROUND);

            RunGame();
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        private void StopGame()
        {
            _isGameRunning = false;

            if (StarView.IsWarpingThroughSpace)
                CelestialObjectFactory.StopSpaceWarp();

            _frameTimer.Dispose();
            AudioHelper.StopSound();
        }

        /// <summary>
        /// Check if game if over.
        /// </summary>
        private void CheckGameOver()
        {
            if (_player.HasNoHealth)
            {
                PlayerHealthBar.Width = 0;
                QuitGame();
            }
        }

        /// <summary>
        /// Quits the current game.
        /// </summary>
        private void QuitGame()
        {
            StopGame();
            PlayerScoreHelper.PlayerScore = _playerScore;
            AudioHelper.PlaySound(SoundType.GAME_OVER);
            NavigateToPage(typeof(GameOverPage));
        }

        /// <summary>
        /// Warps the player through space.
        /// </summary>
        private void WarpThroughSpace()
        {
            CelestialObjectFactory.StartSpaceWarp();

            //TODO: increase everythings speed and not remove them

            if (GameView.GetGameObjects<GameObject>().Where(x => !x.IsPlayer) is IEnumerable<GameObject> gameObjects)
            {
                foreach (var gameObject in gameObjects)
                {
                    GameView.AddDestroyableGameObject(gameObject);
                }
            }
        }

        /// <summary>
        /// Reset all factories to default value.
        /// </summary>
        private void ResetFactories()
        {
            CelestialObjectFactory.Reset();
            MeteorFactory.Reset();
            EnemyFactory.Reset();
            HealthFactory.Reset();
            PowerUpFactory.Reset();
            CollectibleFactory.Reset();
        }

        /// <summary>
        /// Sets analytics of fps, frame time and objects currently in view.
        /// </summary>
        private void SetAnalytics()
        {
            _frameStatUpdateSpawnCounter -= 1;

            if (_frameStatUpdateSpawnCounter < 0)
            {
                var gameObjects = GameView.Children.OfType<GameObject>();
                var starObjects = StarView.Children.OfType<GameObject>();
                var planetObjects = PlanetView.Children.OfType<GameObject>();

                var fpsText = $"FPS: {_fpsCount}" +
                    "\n----" +
                    $"\nframe time: {_frameTime:0.00}" +
                    $"\nframe dur: {_frameDuration:0.00}" +
                    $"\navg. frame dur: {_framesCount.Sum() / _framesCount.Count:0.00}" +
                    $"\nmax frame dur: {_maxFrameDuration:0.00}" +
                    "\n----";

                FPSText.Text = fpsText;

                var total = gameObjects.Count() + starObjects.Count() + planetObjects.Count();

                var objectsCountText =
                    $"enemy: {gameObjects.Count(x => (ElementType)x.Tag == ElementType.ENEMY)} " +
                    $"\nmeteor: {gameObjects.Count(x => (ElementType)x.Tag == ElementType.METEOR)} " +
                    $"\npowerup: {gameObjects.Count(x => (ElementType)x.Tag == ElementType.POWERUP)} " +
                    $"\nhealth: {gameObjects.Count(x => (ElementType)x.Tag == ElementType.HEALTH)} " +
                    $"\nenemy projectile: {gameObjects.Count(x => (ElementType)x.Tag == ElementType.ENEMY_PROJECTILE)} " +
                    $"\nplayer projectile: {gameObjects.Count(x => (ElementType)x.Tag == ElementType.PLAYER_PROJECTILE)} " +
                    $"\nstar: {starObjects.Count(x => (ElementType)x.Tag == ElementType.CELESTIAL_OBJECT)} " +
                    $"\nplanet: {planetObjects.Count(x => (ElementType)x.Tag == ElementType.CELESTIAL_OBJECT)} " +
                    "\n----" +
                    $"\nTOTAL: {total}";

                ObjectsCountText.Text = objectsCountText;

                _frameStatUpdateSpawnCounter = _frameStatUpdateAfter;

                if (_framesCount.Count > 5000)
                {
                    _framesCount.Clear();
                    Console.WriteLine("AVG. FRAME DUR COUNTER RESET.");
                }
            }
        }

        /// <summary>
        /// Sets the frame time.
        /// </summary>
        private void GetFrameDuration()
        {
            _framesCount.Add(_frameDuration);
            _frameDuration = _frameEndTime - _frameStartTime;

            if (_frameDuration > _maxFrameDuration)
                _maxFrameDuration = _frameDuration;
        }

        /// <summary>
        /// Calculates the frames per second.
        /// </summary>
        private void CalculateFPS()
        {
            // calculate FPS
            if (_lastFpsTime + 1000 < _frameStartTime)
            {
                _fpsCount = _fpsSpawnCounter;
                _fpsSpawnCounter = 0;
                _lastFpsTime = _frameStartTime;
            }

            _fpsSpawnCounter++;
        }

        #endregion

        #region GameObjects

        /// <summary>
        /// Updates objects in the planet view.
        /// </summary>
        private void UpdatePlanetViewObjects()
        {
            var planetObjects = PlanetView.GetGameObjects<GameObject>();

            // update game view objects
            foreach (var gameObject in planetObjects)
            {
                UpdateCelestialObject(gameObject);
            }
        }

        /// <summary>
        /// Updates objects in the star view.
        /// </summary>
        private void UpdateStarViewObjects()
        {
            var starObjects = StarView.GetGameObjects<GameObject>();

            // update game view objects           
            foreach (var gameObject in starObjects)
            {
                UpdateCelestialObject(gameObject);
            }
        }

        /// <summary>
        /// Updates objects in the game view.
        /// </summary>
        private void UpdateGameViewObjects()
        {
            var gameObjects = GameView.GetGameObjects<GameObject>();

            // update game view objects           
            foreach (var gameObject in gameObjects)
            {
                switch (gameObject.Tag)
                {
                    case ElementType.PLAYER:
                        {
                            UpdatePlayer();
                        }
                        break;
                    case ElementType.PLAYER_PROJECTILE:
                        {
                            UpdatePlayerProjectile(gameObject);
                        }
                        break;
                    case ElementType.ENEMY_PROJECTILE:
                        {
                            UpdateEnemyProjectile(gameObject);
                        }
                        break;
                    case ElementType.ENEMY:
                        {
                            UpdateEnemy(gameObject);
                        }
                        break;
                    case ElementType.METEOR:
                        {
                            UpdateMeteor(gameObject);
                        }
                        break;
                    case ElementType.HEALTH:
                        {
                            UpdateHealth(gameObject);
                        }
                        break;
                    case ElementType.COLLECTIBLE:
                        {
                            UpdateCollectible(gameObject);
                        }
                        break;
                    case ElementType.POWERUP:
                        {
                            UpdatePowerUp(gameObject);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Spawns game objects.
        /// </summary>
        private void SpawnGameViewObjects()
        {
            // only generate game objects if not warping thorugh space
            if (!StarView.IsWarpingThroughSpace)
            {
                if (_gameLevel > 1)
                    EnemyFactory.SpawnEnemy(gameLevel: _gameLevel);

                MeteorFactory.SpawnMeteor(gameLevel: _gameLevel);
                HealthFactory.SpawnHealth(player: _player);
                PowerUpFactory.SpawnPowerUp();
                CollectibleFactory.SpawnCollectible(gameLevel: _gameLevel);

                PlayerProjectileFactory.SpawnProjectile(
                    isPoweredUp: _player.IsPoweredUp,
                    player: _player,
                    gameLevel: _gameLevel,
                    powerUpType: _powerUpType);
            }
        }

        #endregion

        #region InGameMessage

        /// <summary>
        /// Shows in game content.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        private void ShowInGameContent(Image image, string text)
        {
            var scale = GameView.GameObjectScale;
            InGameContentPanel.Children.Add(new InGameMessage(text, scale, image));
        }

        /// <summary>
        /// Shows the in game text in game view.
        /// </summary>
        private void ShowInGameText(string text)
        {
            var scale = GameView.GameObjectScale;
            InGameContentPanel.Children.Add(new InGameMessage(text, scale));
        }

        /// <summary>
        /// Hides the in game text after keeping it visible for a few frames.
        /// </summary>
        private void InGameContentCoolDown()
        {
            var removables = new List<InGameMessage>();

            foreach (InGameMessage content in InGameContentPanel.Children.Cast<InGameMessage>())
            {
                if (content.CoolDown())
                    removables.Add(content);
            }

            foreach (var removable in removables)
            {
                InGameContentPanel.Children.Remove(removable);
            }
        }

        /// <summary>
        /// Hides the in game text.
        /// </summary>
        private void HideInGameContent()
        {
            InGameContentPanel.Children.Clear();
        }

        #endregion

        #region Page

        private void NavigateToPage(Type pageType)
        {
            App.NavigateToPage(pageType);
        }

        /// <summary>
        /// Sets the game and star view sizes according to current window size.
        /// </summary>
        private void SetViewSize()
        {
            GameView.SetSize(_windowHeight, _windowWidth);
            StarView.SetSize(_windowHeight, _windowWidth);
            PlanetView.SetSize(_windowHeight, _windowWidth);

            _frameTime = DEFAULT_FRAME_TIME + GameView.GetFrameTimeBuffer();
#if DEBUG
            Console.WriteLine($"Frame Time : {_frameTime}");
#endif
            if (_isGameRunning)
            {
                PauseGame();

                _pointerX = _windowWidth / 2;
                _player.SetX(_pointerX - _player.HalfWidth);

                SetPlayerY(); // windows size changed so reset y position

                // resize player size
                var scale = GameView.GameObjectScale;
                _player.ReAdjustScale(scale: scale);
#if DEBUG
                Console.WriteLine($"View Scale: {scale}");
#endif
                return;
            }
        }

        #endregion

        #region Player

        /// <summary>
        /// Spawns the player.
        /// </summary>
        private void SpawnPlayer()
        {
            var scale = GameView.GameObjectScale;
            _player = PlayerFactory.SpawnPlayer(pointerX: _pointerX, ship: App.Ship);

            var playerRages = AssetHelper.PLAYER_RAGE_TEMPLATES;

            _rageImage = new Image()
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(playerRages.FirstOrDefault(x => (int)x.Size == (int)_player.ShipClass).AssetUri)
            };

            PlayerRageBar.Maximum = _player.RageThreashold;
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        private void SetPlayerY()
        {
            _pointerY = PlayerFactory.GetOptimalPlayerY(_player);
            _player.SetY(_pointerY);
        }

        /// <summary>
        /// Sets player health bar.
        /// </summary>
        private void SetPlayerHealthBar()
        {
            PlayerHealthBar.Value = _player.Health;
        }

        /// <summary>
        /// Handles damage recovery of the player after getting hit.
        /// </summary>
        private void DamageRecoveryCoolDown()
        {
            PlayerFactory.DamageRecoveryCoolDown(_player);
        }

        /// <summary>
        /// Update player in the game view.
        /// </summary>
        private void UpdatePlayer()
        {
            if (_moveLeft || _moveRight || _isPointerActivated)
            {
                PlayerFactory.UpdatePlayer(
                    player: _player,
                    pointerPosition: _pointerPosition,
                    moveLeft: _moveLeft,
                    moveRight: _moveRight,
                    isPointerActivated: _isPointerActivated);
            }
            else
            {
                PlayerFactory.UpdateAcceleration();
            }

            PowerUpCoolDown();
            RageCoolDown();
        }

        /// <summary>
        /// Update a player projectile in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdatePlayerProjectile(GameObject gameObject)
        {
            var projectile = gameObject as PlayerProjectile;

            // move the projectile up and check if projectile has gone beyond the game view
            bool destroyed = PlayerProjectileFactory.UpdateProjectile(projectile: projectile);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (projectile.IsDestroyedByCollision)
                return;

            var (Score, DestroyedObject) = PlayerProjectileFactory.CheckCollision(projectile: projectile);

            if (DestroyedObject is not null)
            {
                switch (DestroyedObject.Tag)
                {
                    case ElementType.ENEMY:
                        {
                            var enemy = DestroyedObject as Enemy;
                            _playerScore.EnemiesDestroyed++;

                            if (enemy.IsBoss)
                            {
                                DisengageBoss(enemy);
                                _playerScore.BossesDestroyed++;
                                EnemyFactory.DestroyByPlayerProjectle(enemy);
                            }
                            else
                            {
                                EnemyFactory.DestroyByPlayerProjectle(enemy);
                            }
                        }
                        break;
                    case ElementType.METEOR:
                        {
                            _playerScore.MeteorsDestroyed++;
                            MeteorFactory.DestroyByPlayerProjectle(DestroyedObject as Meteor);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (GameView.IsBossEngaged)
                SetBossHealthBar(); // set boss health bar on projectile hit

            if (Score > 0)
            {
                if (!_player.IsRageUp)
                    AddRage();

                // trigger rage after rage threashold kills
                if (!_player.IsRageUp && _player.Rage >= _player.RageThreashold)
                    ActivateRage();

                AddScore(Score);
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

            bool destroyed = EnemyFactory.UpdateEnemy(
                enemy: enemy,
                gameLevel: _gameLevel,
                pointerX: _pointerX);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (enemy.IsDestroyedByCollision)
                return;

            // check if enemy collides with player
            if (PlayerFactory.CheckCollision(player: _player, gameObject: enemy))
            {
                PlayerProjectileFactory.DecreaseProjectilePower(player: _player);
                SetPlayerHealthBar();
                return;
            }

            // fire projectiles if at a legitimate distance from player and in canvas view
            if (enemy.IsProjectileFiring && enemy.GetY() > 0 && Math.Abs(_player.GetY() - enemy.GetY()) > 100)
                EnemyProjectileFactory.SpawnProjectile(
                    enemy: enemy);
        }

        /// <summary>
        /// Update an enemy projectile in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateEnemyProjectile(GameObject gameObject)
        {
            var projectile = gameObject as EnemyProjectile;

            bool destroyed = EnemyProjectileFactory.UpdateProjectile(projectile);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (projectile.IsDestroyedByCollision)
                return;

            // check if enemy projectile collides with player
            if (PlayerFactory.CheckCollision(player: _player, gameObject: projectile))
            {
                PlayerProjectileFactory.DecreaseProjectilePower(player: _player);
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
            ShowInGameContent(image: _bossAppearedImage, text: $"{LocalizationHelper.GetLocalizedResource("LEVEL")} {(int)_gameLevel - 1} {LocalizationHelper.GetLocalizedResource("BOSS")}");

            var boss = EnemyFactory.EngageBoss(_gameLevel);
            _bosses.Add(boss);

            switch (boss.BossClass)
            {
                case BossClass.GREEN:
                    BossHealthBar.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case BossClass.CRIMSON:
                    BossHealthBar.Foreground = new SolidColorBrush(Colors.DarkRed);
                    break;
                case BossClass.BLUE:
                    BossHealthBar.Foreground = new SolidColorBrush(Colors.SkyBlue);
                    break;
                default:
                    break;
            }

            BossHealthBarPanel.Visibility = Visibility.Visible;
            _bossTotalHealth = _bosses.Sum(x => x.Health);

            SetBossHealthBar(); // set boss health on boss appearance            
        }

        /// <summary>
        /// Sets the boss health bar.
        /// </summary>
        private void SetBossHealthBar()
        {
            BossHealthBar.Value = _bosses.Sum(x => x.Health) / _bossTotalHealth * 100;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        /// <param name="boss"></param>
        private void DisengageBoss(Enemy boss)
        {
            _bosses.Remove(boss);

            if (_bosses.Count == 0)
            {
                EnemyFactory.DisengageBoss();

                ShowInGameContent(_bossClearedImage, $"{LocalizationHelper.GetLocalizedResource("LEVEL")} {(int)_gameLevel - 1} {LocalizationHelper.GetLocalizedResource("COMPLETE")}");

                BossHealthBarPanel.Visibility = Visibility.Collapsed;
                _bossTotalHealth = 0;

                SetGameLevelText();
                WarpThroughSpace(); // after defeating a boss
            }
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

            bool destroyed = MeteorFactory.UpdateMeteor(meteor: meteor);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            if (meteor.IsDestroyedByCollision)
                return;

            // check if meteor collides with player
            if (PlayerFactory.CheckCollision(player: _player, gameObject: meteor))
            {
                PlayerProjectileFactory.DecreaseProjectilePower(player: _player);
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

            bool destroyed = HealthFactory.UpdateHealth(health: health);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            // check if health collides with player
            if (PlayerFactory.CheckCollision(player: _player, gameObject: health))
            {
                SetPlayerHealthBar();
                ShowInGameContent(_healthImage, $"‍{LocalizationHelper.GetLocalizedResource("SHIP_REPAIRED")}");
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

            bool destroyed = CollectibleFactory.UpdateCollectible(collectible: collectible);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            // check if collectible collides with player
            if (PlayerFactory.CheckCollision(player: _player, gameObject: collectible))
            {
                PlayerProjectileFactory.IncreaseProjectilePower(player: _player);
                _playerScore.CollectiblesCollected++;

                AddScore(1);
                AddScoreMultiplier();

                if (_scoreMultiplierCount >= SCORE_MULTIPLIER_THREASHOLD)
                    ActivateScoreMultiplier();
            }
        }

        #endregion

        #region Rage

        /// <summary>
        /// Adds rage.
        /// </summary>
        private void AddRage()
        {
            _player.Rage++;
            PlayerRageBar.Value = _player.Rage;
        }

        /// <summary>
        /// Activates rage.
        /// </summary>
        private void ActivateRage()
        {
            PlayerRageIcon.Text = "😤";
            PlayerFactory.RageUp(_player);
            PlayerProjectileFactory.RageUp(_player);

            switch (_player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    ShowInGameContent(_rageImage, $"{LocalizationHelper.GetLocalizedResource("SHIELD_UP")}");
                    break;
                case ShipClass.BERSERKER:
                    ShowInGameContent(_rageImage, $"{LocalizationHelper.GetLocalizedResource("FIRING_RATE_INCREASED")}");
                    break;
                case ShipClass.SPECTRE:
                    ShowInGameContent(_rageImage, $"{LocalizationHelper.GetLocalizedResource("CLOAK_UP")}");
                    break;
                default:
                    break;
            }

            RageGradientBorder.Opacity = 1;
            RageGradientBorder.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Cools down rage effect.
        /// </summary>
        private void RageCoolDown()
        {
            if (_player.IsRageUp && !StarView.IsWarpingThroughSpace)
            {
                var (RageDown, RageRemaining) = PlayerFactory.RageUpCoolDown(_player);

                PlayerRageBar.Value = RageRemaining;

                // slowly fade away rage
                RageGradientBorder.Opacity = PlayerRageBar.Value / PlayerRageBar.Maximum;

                if (RageDown)
                {
                    PlayerProjectileFactory.RageDown(_player);

                    RageGradientBorder.Opacity = 0;
                    PlayerRageBar.Value = _player.Rage;
                    PlayerRageIcon.Text = "😡";

                    switch (_player.ShipClass)
                    {
                        case ShipClass.DEFENDER:
                            ShowInGameContent(_rageImage, $"{LocalizationHelper.GetLocalizedResource("SHIELD_DOWN")}");
                            break;
                        case ShipClass.BERSERKER:
                            ShowInGameContent(_rageImage, $"{LocalizationHelper.GetLocalizedResource("FIRING_RATE_DECREASED")}");
                            break;
                        case ShipClass.SPECTRE:
                            ShowInGameContent(_rageImage, $"{LocalizationHelper.GetLocalizedResource("CLOAK_DOWN")}");
                            break;
                        default:
                            break;
                    }

                    RageGradientBorder.Visibility = Visibility.Collapsed;
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

            bool destroyed = PowerUpFactory.UpdatePowerUp(powerUp: powerUp);

            if (destroyed)
                return;

            if (StarView.IsWarpingThroughSpace)
                return;

            // check if power up collides with player
            if (PlayerFactory.CheckCollision(player: _player, gameObject: powerUp))
                ActivatePowerUp(powerUp);
        }

        /// <summary>
        /// Activates power up.
        /// </summary>
        /// <param name="powerUp"></param>

        private void ActivatePowerUp(PowerUp powerUp)
        {
            PlayerPowerBar.Visibility = Visibility.Visible;

            // do not trigger same power up twice
            if (powerUp.PowerUpType != _powerUpType)
            {
                _powerUpType = powerUp.PowerUpType;
                PlayerProjectileFactory.PowerUp(powerUpType: _powerUpType, player: _player);
            }

            ShowInGameContent(_powerUpImage, $"‍{LocalizationHelper.GetLocalizedResource(_powerUpType.ToString())}"); // show power up text
        }

        /// <summary>
        /// Cools down power up effect.
        /// </summary>
        private void PowerUpCoolDown()
        {
            if (_player.IsPoweredUp && !StarView.IsWarpingThroughSpace)
            {
                var (PowerDown, PowerRemaining) = PlayerFactory.PowerUpCoolDown(_player);

                PlayerPowerBar.Value = PowerRemaining;

                if (PowerDown)
                {
                    PlayerProjectileFactory.PowerDown(_powerUpType, player: _player);
                    PlayerPowerBar.Visibility = Visibility.Collapsed;

                    _powerUpType = PowerUpType.NONE;
                    ShowInGameContent(_powerUpImage, $"{LocalizationHelper.GetLocalizedResource("POWER_DOWN")}");
                }
            }
        }

        #endregion

        #region CelestialObject

        /// <summary>
        /// Update a star in the game view.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateCelestialObject(GameObject gameObject)
        {
            var star = gameObject as CelestialObject;
            CelestialObjectFactory.UpdateCelestialObject(celestialObject: star);
        }

        #endregion

        #region Difficulty

        /// <summary>
        /// Sets the game level according to score; 
        /// </summary>
        private void ScaleDifficulty()
        {
            var lastGameLevel = _gameLevel;

            if (_playerScore.Score > _scoreCap)
            {
                _gameLevel++;
                _scoreCap += 50 * _gameLevel / 2;
                SetScoreBarCountText(_scoreCap);
            }

            // when difficulty changes show level up
            if (lastGameLevel != _gameLevel)
            {
                LevelUpObjects();

                // bosses apprear after level 2
                if (_gameLevel > 2) //TODO: SET TO LEVEL 2
                {
                    EngageBoss();
                }
                else
                {
                    AudioHelper.PlaySound(SoundType.ENEMY_INCOMING);
                    AudioHelper.PlaySound(SoundType.BACKGROUND);

                    SetGameLevelText();

                    ShowInGameText($"👊 {LocalizationHelper.GetLocalizedResource("ENEMY_APPROACHES")}");
                    WarpThroughSpace(); // after first level clearing
                }
            }
        }

        /// <summary>
        /// Sets the score bar text in ui.
        /// </summary>
        /// <param name="capacity"></param>
        private void SetScoreBarCountText(int capacity)
        {
            ScoreBarCount.Text = $"🏆{_playerScore.Score}/{capacity}";
        }

        /// <summary>
        /// Sets the game level text in ui.
        /// </summary>
        private void SetGameLevelText()
        {
            GameLevelText.Text = $"🔥 {(int)_gameLevel}";
        }

        /// <summary>
        /// Performs level up of all game view objects.
        /// </summary>
        private void LevelUpObjects()
        {
            if (_gameLevel > 1)
            {
                EnemyFactory.LevelUp();
                MeteorFactory.LevelUp();
                HealthFactory.LevelUp();
                PowerUpFactory.LevelUp();
                CollectibleFactory.LevelUp();
                CelestialObjectFactory.LevelUp();
                PlayerProjectileFactory.LevelUp(player: _player);
            }
        }

        #endregion

        #region Score

        private void AddScoreMultiplier()
        {
            _scoreMultiplierCount++;
            SetScoreMultiplierCountText();
        }

        private void ActivateScoreMultiplier()
        {
            _scoreMultiplierCount = 0;
            SetScoreMultiplierCountText();

            _scoreMultiplierCoolDownCounter = _scoreMultiplierCoolDownAfter;
            _isScoreMultiplierActivated = true;
            AudioHelper.PlaySound(SoundType.SCORE_MULTIPLIER_ON);
            ShowInGameContent(_scoreMultiplierImage, LocalizationHelper.GetLocalizedResource("SCORE_MULTIPLIER_ON"));
        }

        private void ScoreMultiplierCoolDown()
        {
            if (_isScoreMultiplierActivated && !StarView.IsWarpingThroughSpace)
            {
                _scoreMultiplierCoolDownCounter--;

                // decrease multiplier progress bar
                ScoreMultiplierBar.Value = _scoreMultiplierCoolDownCounter / _scoreMultiplierCoolDownAfter * SCORE_MULTIPLIER_THREASHOLD;

                if (_scoreMultiplierCoolDownCounter <= 0)
                {
                    _isScoreMultiplierActivated = false;
                    AudioHelper.PlaySound(SoundType.SCORE_MULTIPLIER_OFF);
                    ShowInGameContent(_scoreMultiplierImage, LocalizationHelper.GetLocalizedResource("SCORE_MULTIPLIER_OFF"));
                }
            }
        }

        private void SetScoreMultiplierCountText()
        {
            ScoreMultiplierCountText.Text = $"x{_scoreMultiplierCount}";
            ScoreMultiplierBar.Value = _scoreMultiplierCount;
        }

        /// <summary>
        /// Adds the score.
        /// </summary>
        /// <param name="score"></param>
        private void AddScore(double score)
        {
            _playerScore.Score += _isScoreMultiplierActivated ? score * 2 : score;
            SetScoreBarCountText(_scoreCap);

            ScaleDifficulty(); // check game level on score change
        }

        #endregion

        #endregion
    }
}
