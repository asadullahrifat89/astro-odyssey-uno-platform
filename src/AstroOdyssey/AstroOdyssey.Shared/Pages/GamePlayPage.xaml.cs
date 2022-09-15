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
using static AstroOdyssey.Constants;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

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

        private int _fpsSpawnCounter = 0;
        private int _fpsCount = 0;
        private float _lastFpsTime = 0;
        private long _frameStartTime;
        private long _frameEndTime;

#if DEBUG
        private int _frameStatUpdateSpawnCounter;
        private int _frameStatUpdateAfter = 5;
        private double _frameDuration;
#endif

        private int _showInGameTextSpawnCounter = 110;
        private int _showInGameTextAfter = 110;

        private int _showInGameImagePanelSpawnCounter = 110;
        private int _showInGameImagePanelAfter = 110;

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

            _celestialObjectFactory = new CelestialObjectFactory();
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

        public PeriodicTimer GameFrameTimer { get; set; }

        public Stopwatch Stopwatch { get; set; }

        public bool IsRageUp { get; set; }

        public double Rage { get; set; } = 0;

        public PlayerScore GameScore { get; set; } = new PlayerScore();

        public double PointerX { get; set; }

        public double PointerY { get; set; }

        public Player Player { get; set; }

        public GameLevel GameLevel { get; set; }

        public PowerUpType PowerUpType { get; set; }

        public bool IsGameRunning { get; set; }

        public bool IsGamePaused { get; set; }

        public bool IsGameQuitting { get; set; }

        private bool FiringProjectiles { get; set; } = false;

        private bool IsPoweredUp { get; set; }

        public bool MoveLeft { get; set; }

        public bool MoveRight { get; set; }

        public List<Enemy> Bosses { get; set; }

        private double BossTotalHealth { get; set; }

        #endregion

        #region Events

        #region Window

        async void GamePage_Loaded(object sender, RoutedEventArgs e)
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

            IsPoweredUp = false;
            PowerUpType = PowerUpType.NONE;
            PlayerPowerBar.Maximum = POWER_UP_METER;
            PlayerPowerBar.Value = POWER_UP_METER;

            IsRageUp = false;
            Rage = 0;
            PlayerRageBar.Maximum = RAGE_THRESHOLD;

            GameScore = new PlayerScore();
            SetScoreBarCountText(25);

            PointerX = _windowWidth / 2;

            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Collapsed;

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

            await this.PlayLoadedTransition();
        }

        void GamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= GamePage_SizeChanged;
            StopGame();
        }

        private void GamePage_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            _windowWidth = args.NewSize.Width - 10; //Window.Current.Bounds.Width;
            _windowHeight = args.NewSize.Height - 10; //Window.Current.Bounds.Height;

            AdjustView(); // at view size change

#if DEBUG
            Console.WriteLine($"View Size: {_windowWidth} x {_windowHeight}");
#endif
        }

        #endregion

        #region Game

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

        #region Input       

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
                var point = e.GetCurrentPoint(GameView);

                if (point.Position.X < _windowWidth / 2)  // move left
                {
                    MoveLeft = true;
                    MoveRight = false;
                }
                else if (point.Position.X > _windowWidth / 2) // move right
                {
                    MoveRight = true;
                    MoveLeft = false;
                }
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

                if (MoveLeft)
                    MoveLeft = false;

                if (MoveRight)
                    MoveRight = false;
            }
            else
            {
                InputView.Focus(FocusState.Programmatic);
                StartGame();
                FiringProjectiles = true;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Game

        /// <summary>
        /// Starts the game. Spawns the player and starts game and projectile loops.
        /// </summary>
        private async void StartGame()
        {
#if !DEBUG
            FPSText.Visibility = Visibility.Collapsed;
            ObjectsCountText.Visibility = Visibility.Collapsed;
#endif
            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.MENU_SELECT);
            _audioHelper.PlaySound(SoundType.GAME_START);

            SpawnPlayer();

            SetPlayerY(); // set y position at game start

            SetPlayerHealthBar(); // set player health bar at game start

            HideInGameText();

            IsGameRunning = true;

            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            PlayerHealthBarPanel.Visibility = Visibility.Visible;
            ScoreBarPanel.Visibility = Visibility.Visible;

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
            Stopwatch = Stopwatch.StartNew();

            GameFrameTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(_frameTime));

            while (await GameFrameTimer.WaitForNextTickAsync())
            {
                _frameStartTime = Stopwatch.ElapsedMilliseconds;

                RenderGameFrame();

                CalculateFPS();

                _frameEndTime = Stopwatch.ElapsedMilliseconds;

                GetFrameDuration();

                SetAnalytics();
            }
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
        /// Sets the game and star view sizes according to current window size.
        /// </summary>
        private void AdjustView()
        {
            GameView.SetSize(_windowHeight, _windowWidth);
            StarView.SetSize(_windowHeight, _windowWidth);
            PlanetView.SetSize(_windowHeight, _windowWidth);

            _frameTime = DEFAULT_FRAME_TIME;

            // resize player size
            if (IsGameRunning)
            {
                PointerX = _windowWidth / 2;

                Player.SetX(PointerX - Player.HalfWidth);

                SetPlayerY(); // windows size changed so reset y position               

                var scale = GameView.GetGameObjectScale();
                Player.ReAdjustScale(scale: scale);
#if DEBUG
                Console.WriteLine($"View Scale: {scale}");
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

            FiringProjectiles = false;
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

            FiringProjectiles = true;
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
        /// Renders a frame in the game.
        /// </summary>
        private void RenderGameFrame()
        {
            GameOver();

            UpdateGameObjects();

            SpawnGameObjects();

            HandleInGameText();

            HandleInGameImagePanel();

            DamageRecoveryCoolDown();
        }

        /// <summary>
        /// Updates meteors, enemies, projectiles in the game view. Advances game objects in the frame.
        /// </summary>
        private void UpdateGameObjects()
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

                UpdateGameObject(gameObject);

            }).IsCompleted)
            {
                // clean removable objects from game view
                GameView.RemoveDestroyableGameObjects();
            }

            var starObjects = StarView.GetGameObjects<GameObject>();

            // update game view objects
            if (Parallel.ForEach(starObjects, gameObject =>
            {
                UpdateGameObject(gameObject);

            }).IsCompleted)
            {
                // clean removable objects from game view
                StarView.RemoveDestroyableGameObjects();
            }

            var planetObjects = PlanetView.GetGameObjects<GameObject>();

            // update game view objects
            if (Parallel.ForEach(planetObjects, gameObject =>
            {
                UpdateGameObject(gameObject);

            }).IsCompleted)
            {
                // clean removable objects from game view
                PlanetView.RemoveDestroyableGameObjects();
            }
        }

        /// <summary>
        /// Updates a game object.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateGameObject(GameObject gameObject)
        {
            var tag = gameObject.Tag;

            switch (tag)
            {
                case PLAYER_TAG:
                    {
                        if (MoveLeft || MoveRight)
                        {
                            var pointerX = _playerFactory.UpdatePlayer(
                                player: Player,
                                pointerX: PointerX,
                                moveLeft: MoveLeft,
                                moveRight: MoveRight);

                            PointerX = pointerX;
                        }
                        else
                        {
                            var pointerX = _playerFactory.UpdateAcceleration(player: Player, pointerX: PointerX);

                            PointerX = pointerX;
                        }

                        if (IsPoweredUp)
                        {
                            var coolDown = _playerFactory.PowerUpCoolDown(Player);

                            PlayerPowerBar.Value = coolDown.PowerRemaining;

                            if (coolDown.PowerDown)
                            {
                                _playerProjectileFactory.PowerDown(PowerUpType);
                                PlayerPowerBar.Visibility = Visibility.Collapsed;
                                IsPoweredUp = false;
                                PowerUpType = PowerUpType.NONE;
                                ShowInGameContent(_powerUpImage, $"{_localizationHelper.GetLocalizedResource("POWER_DOWN")}");
                            }
                        }

                        if (IsRageUp)
                        {
                            var coolDown = _playerFactory.RageUpCoolDown(Player);

                            PlayerRageBar.Value = coolDown.RageRemaining;

                            if (coolDown.RageDown)
                            {
                                _playerProjectileFactory.RageDown(Player);
                                IsRageUp = false;
                                Rage = 0;
                                PlayerRageBar.Value = Rage;

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
                    }
                    break;
                case PLAYER_PROJECTILE_TAG:
                    {
                        var projectile = gameObject as PlayerProjectile;

                        // move the projectile up and check if projectile has gone beyond the game view
                        _playerProjectileFactory.UpdateProjectile(projectile: projectile, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (StarView.IsWarpingThroughSpace)
                            return;

                        if (projectile.IsMarkedForFadedDestruction)
                            return;

                        _playerProjectileFactory.CollidePlayerProjectile(projectile: projectile, score: out double score, destroyedObject: out GameObject destroyedObject);

                        if (GameView.IsBossEngaged)
                        {
                            SetBossHealthBar(); // set boss health bar on projectile hit
                        }

                        if (score > 0)
                        {
                            if (!IsRageUp)
                            {
                                Rage++;
                                PlayerRageBar.Value = Rage;
                            }

                            // trigger rage after each 25 kills
                            if (!IsRageUp && Rage >= RAGE_THRESHOLD)
                            {
                                IsRageUp = true;
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

                            GameScore.Score += score;
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
                                        GameScore.EnemiesDestroyed++;

                                        if (enemy.IsBoss)
                                        {
                                            DisengageBoss(enemy);
                                            GameScore.BossesDestroyed++;
                                        }
                                    }
                                    break;
                                case METEOR_TAG:
                                    {
                                        _meteorFactory.DestroyMeteor(destroyedObject as Meteor);
                                        GameScore.MeteorsDestroyed++;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                case ENEMY_PROJECTILE_TAG:
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
                            _playerProjectileFactory.DecreaseProjectilePower();
                            SetPlayerHealthBar();
                        }
                    }
                    break;
                case ENEMY_TAG:
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
                            _playerProjectileFactory.DecreaseProjectilePower();
                            SetPlayerHealthBar();
                            return;
                        }

                        // fire projectiles if at a legitimate distance from player
                        if (enemy.IsProjectileFiring && Player.GetY() - enemy.GetY() > 100)
                            _enemyProjectileFactory.SpawnProjectile(enemy: enemy, gameLevel: GameLevel);
                    }
                    break;
                case METEOR_TAG:
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
                            _playerProjectileFactory.DecreaseProjectilePower();
                            SetPlayerHealthBar();
                        }
                    }
                    break;
                case HEALTH_TAG:
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
                    break;
                case COLLECTIBLE_TAG:
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
                            _playerProjectileFactory.IncreaseProjectilePower();

                            GameScore.Score++;
                            GameScore.CollectiblesCollected++;

                            SetGameLevel(); // check game level on score change
                            //ShowInGameText($"‍💫 {_localizationHelper.GetLocalizedResource("COLLECTIBLE_COLLECTED")}");
                        }
                    }
                    break;
                case POWERUP_TAG:
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
                            PlayerPowerBar.Visibility = Visibility.Visible;

                            IsPoweredUp = true;
                            PowerUpType = powerUp.PowerUpType;

                            ShowInGameContent(_powerUpImage, $"‍{_localizationHelper.GetLocalizedResource(PowerUpType.ToString())}"); // show power up text

                            _playerProjectileFactory.PowerUp(PowerUpType);
                        }
                    }
                    break;
                case STAR_TAG:
                    {
                        var star = gameObject as CelestialObject;

                        _celestialObjectFactory.UpdateCelestialObject(celestialObject: star, destroyed: out bool destroyed);
                    }
                    break;
                default:
                    break;
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
                    isPoweredUp: IsPoweredUp,
                    firingProjectiles: FiringProjectiles,
                    player: Player,
                    gameLevel: GameLevel,
                    powerUpType: PowerUpType);
            }
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
        /// Shows the in game text in game view.
        /// </summary>
        private void ShowInGameText(string text)
        {
            InGameText.Visibility = Visibility.Visible;
            InGameText.Text = text;
            _showInGameTextSpawnCounter = _showInGameTextAfter;
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
        /// Hides the in game text after keeping it visible for a few frames.
        /// </summary>
        private void HandleInGameText()
        {
            if (!InGameText.Text.IsNullOrBlank())
            {
                _showInGameTextSpawnCounter -= 1;

                if (_showInGameTextSpawnCounter <= 0)
                {
                    HideInGameText();
                }
            }
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
        /// Hides the in game image after keeping it visible for a few frames.
        /// </summary>
        private void HandleInGameImagePanel()
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
        /// Hides in game image.
        /// </summary>
        private void HideInGameImagePanel()
        {
            InGameImagePanel.Visibility = Visibility.Collapsed;
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

            App.GameScore = GameScore;

            await this.PlayUnLoadedTransition();

            App.NavigateToPage(typeof(GameOverPage));
        }

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
        /// Shows in game content.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        private void ShowInGameContent(Image image, string text)
        {
            ShowInGameImagePanel(image);
            ShowInGameText(text);
        }

        #endregion

        #region Frame

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
                        PlayerHealthBarPanel.BorderBrush = new SolidColorBrush(Colors.DarkGoldenrod);
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

#if DEBUG
            Console.WriteLine($"Render Scale: {scale}");
#endif
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

        #region Difficulty

        /// <summary>
        /// Sets the game level according to score; 
        /// </summary>
        private void SetGameLevel()
        {
            var lastGameLevel = GameLevel;

            if (GameScore.Score >= 0)
            {
                GameLevel = GameLevel.Level_1;
                ScoreBar.Value = GameScore.Score / 25 * 100;
                SetScoreBarCountText(25);
            }
            if (GameScore.Score > 25)
            {
                GameLevel = GameLevel.Level_2;
                ScoreBar.Value = GameScore.Score / 100 * 100;
                SetScoreBarCountText(100);
            }
            if (GameScore.Score > 100)
            {
                GameLevel = GameLevel.Level_3;
                ScoreBar.Value = GameScore.Score / 200 * 100;
                SetScoreBarCountText(200);
            }
            if (GameScore.Score > 200)
            {
                GameLevel = GameLevel.Level_4;
                ScoreBar.Value = GameScore.Score / 400 * 100;
                SetScoreBarCountText(400);
            }
            if (GameScore.Score > 400)
            {
                GameLevel = GameLevel.Level_5;
                ScoreBar.Value = GameScore.Score / 600 * 100;
                SetScoreBarCountText(600);
            }
            if (GameScore.Score > 600)
            {
                GameLevel = GameLevel.Level_6;
                ScoreBar.Value = GameScore.Score / 800 * 100;
                SetScoreBarCountText(800);
            }
            if (GameScore.Score > 800)
            {
                GameLevel = GameLevel.Level_7;
                ScoreBar.Value = GameScore.Score / 1000 * 100;
                SetScoreBarCountText(1000);
            }
            if (GameScore.Score > 1000)
            {
                GameLevel = GameLevel.Level_8;
                ScoreBar.Value = GameScore.Score / 1200 * 100;
                SetScoreBarCountText(1200);
            }
            if (GameScore.Score > 1200)
            {
                GameLevel = GameLevel.Level_9;
                ScoreBar.Value = GameScore.Score / 1400 * 100;
                SetScoreBarCountText(1400);
            }
            if (GameScore.Score > 1400)
            {
                GameLevel = GameLevel.Level_10;
                ScoreBar.Value = GameScore.Score / 1600 * 100;
                SetScoreBarCountText(1600);
            }
            if (GameScore.Score > 1600)
            {
                GameLevel = GameLevel.Level_11;
                ScoreBar.Value = GameScore.Score / 1800 * 100;
                SetScoreBarCountText(1800);
            }
            if (GameScore.Score > 1800)
            {
                GameLevel = GameLevel.Level_12;
                ScoreBar.Value = GameScore.Score / 2000 * 100;
                SetScoreBarCountText(2000);
            }
            if (GameScore.Score > 2000)
            {
                GameLevel = GameLevel.Level_13;
                ScoreBarCount.Text = $"{_localizationHelper.GetLocalizedResource("SCORE")} {GameScore.Score}/MAX";
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
            ScoreBarCount.Text = $"{_localizationHelper.GetLocalizedResource("SCORE")} {GameScore.Score}/{capacity}";
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
                        _playerProjectileFactory.LevelUp();
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}
