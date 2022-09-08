using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using System.Threading.Tasks;
using static AstroOdyssey.Constants;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
        private int _frameStatUpdateDelay = 5;
        private double _frameDuration;
#endif

        private int _showInGameTextSpawnCounter = 110;
        private int _showInGameTextDelay = 110;

        private int _showInGameImagePanelSpawnCounter = 110;
        private int _showInGameImagePanelDelay = 110;

        private double _defaultFrameTime = 17;
        private double _frameTime;

        private double _windowWidth, _windowHeight;

        private readonly CelestialObjectFactory _celestialObjectFactory;
        private readonly MeteorFactory _meteorFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly HealthFactory _healthFactory;
        private readonly PowerUpFactory _powerUpFactory;
        private readonly CollectibleFactory _collectibleFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerProjectileFactory _playerProjectileFactory;
        private readonly EnemyProjectileFactory _enemyProjectileFactory;

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

            _celestialObjectFactory = new CelestialObjectFactory(StarView, PlanetView);
            _meteorFactory = new MeteorFactory(GameView);
            _enemyFactory = new EnemyFactory(GameView);
            _healthFactory = new HealthFactory(GameView);
            _powerUpFactory = new PowerUpFactory(GameView);
            _collectibleFactory = new CollectibleFactory(GameView);
            _playerFactory = new PlayerFactory(GameView);
            _playerProjectileFactory = new PlayerProjectileFactory(GameView);
            _enemyProjectileFactory = new EnemyProjectileFactory(GameView);
        }

        #endregion

        #region Properties

        public DispatcherTimer GameFrameTimer { get; set; }

        public Stopwatch Stopwatch { get; set; }

        public bool IsRageUp { get; set; }

        public double Rage { get; set; } = 0;

        public GameScore GameScore { get; set; } = new GameScore();

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

        private bool _moveLeft;
        public bool MoveLeft
        {
            get { return _moveLeft; }
            set
            {
                _moveLeft = value;
                MoveLeftFeed.Visibility = _moveLeft ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private bool _moveRight;
        public bool MoveRight
        {
            get { return _moveRight; }
            set
            {
                _moveRight = value;
                MoveRightFeed.Visibility = _moveRight ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Enemy _boss;
        public Enemy Boss
        {
            get { return _boss; }
            set
            {
                _boss = value;

                if (_boss is null)
                {
                    BossHealthBarPanel.Visibility = Visibility.Collapsed;
                    BossTotalHealth = 0;
                }
                else
                {
                    BossHealthBarPanel.Visibility = Visibility.Visible;
                    BossTotalHealth = _boss.Health;
                }
            }
        }

        private double BossTotalHealth { get; set; }

        #endregion

        #region Methods

        #region Window Events

        /// <summary>
        /// When the window is loaded, we add the event Current_SizeChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += GamePage_SizeChanged;

            GameView.Children.Clear();
            StarView.Children.Clear();
            PlanetView.Children.Clear();

            ScoreBarPanel.Visibility = Visibility.Collapsed;

            FPSText.Text = "";
            ObjectsCountText.Text = "";

            Boss = null;
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

            GameScore = new GameScore();
            SetScoreBarCountText(25);

            PointerX = _windowWidth / 2;

            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Collapsed;

            ShowInGameText("👆\n" + LocalizationHelper.GetLocalizedResource("TAP_ON_SCREEN_TO_BEGIN"));
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

            await this.PlayPageLoadedTransition();
        }

        /// <summary>
        /// When the window is unloaded, we remove the event Current_SizeChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= GamePage_SizeChanged;
            StopGame();
        }

        /// <summary>
        /// When the window size is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region UI Events

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
                AudioHelper.PlaySound(SoundType.MENU_SELECT);
                IsGameQuitting = true;
                ShowInGameText($"🛸\n{LocalizationHelper.GetLocalizedResource("QUIT_GAME")}\n{LocalizationHelper.GetLocalizedResource("TAP_TO_QUIT")}");

                InputView.Focus(FocusState.Programmatic);
            }
        }

        #endregion

        #region Input Events       

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

                //// move up
                //if (point.Position.Y < windowHeight / 2)
                //{
                //    FiringProjectiles = true;
                //    MoveUp = true;
                //    MoveDown = false;
                //} // move down
                //else if (point.Position.Y > windowHeight / 2)
                //{
                //    FiringProjectiles = true;
                //    MoveDown = true;
                //    MoveUp = false;
                //}
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

        #region Game Methods

        /// <summary>
        /// Starts the game. Spawns the player and starts game and projectile loops.
        /// </summary>
        private void StartGame()
        {
#if !DEBUG
            FPSText.Visibility = Visibility.Collapsed;
            ObjectsCountText.Visibility = Visibility.Collapsed;
#endif
            AudioHelper.StopSound();
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            AudioHelper.PlaySound(SoundType.GAME_START);

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

            Stopwatch = Stopwatch.StartNew();
            GameFrameTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_frameTime) };

            GameFrameTimer.Tick += (s, e) =>
            {
                _frameStartTime = Stopwatch.ElapsedMilliseconds;

                RenderGameFrame();

                CalculateFPS();

                _frameEndTime = Stopwatch.ElapsedMilliseconds;

                GetFrameDuration();

                SetAnalytics();
            };

            GameFrameTimer.Start();
            WarpThroughSpace();
            AudioHelper.PlaySound(SoundType.BACKGROUND_MUSIC);
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

            GameFrameTimer.Stop();

            AudioHelper.StopSound();
        }

        /// <summary>
        /// Sets the game and star view sizes according to current window size.
        /// </summary>
        private void AdjustView()
        {
            GameView.SetSize(_windowHeight, _windowWidth);
            StarView.SetSize(_windowHeight, _windowWidth);
            PlanetView.SetSize(_windowHeight, _windowWidth);

            _frameTime = _defaultFrameTime + (_windowWidth <= 500 ? 4 : 0); // run a little slower on phones as phones have a faster timer

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

            GameFrameTimer?.Stop();
            ShowInGameText($"👨‍🚀\n{LocalizationHelper.GetLocalizedResource("GAME_PAUSED")}\n{LocalizationHelper.GetLocalizedResource("TAP_TO_RESUME")}");
            FiringProjectiles = false;
            IsGamePaused = true;
            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Visible;

            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            AudioHelper.PauseSound(SoundType.BACKGROUND_MUSIC);
            if (GameView.IsBossEngaged)
                AudioHelper.PauseSound(SoundType.BOSS_APPEARANCE);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        private void ResumeGame()
        {
            InputView.Focus(FocusState.Programmatic);

            HideInGameText();
            GameFrameTimer?.Start();
            FiringProjectiles = true;
            IsGamePaused = false;
            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            AudioHelper.ResumeSound(SoundType.BACKGROUND_MUSIC);
            if (GameView.IsBossEngaged)
                AudioHelper.ResumeSound(SoundType.BOSS_APPEARANCE);
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
                case PLAYER:
                    {
                        if (MoveLeft || MoveRight /*|| MoveUp || MoveDown*/)
                        {
                            var pointerX = _playerFactory.UpdatePlayer(
                                player: Player,
                                pointerX: PointerX,
                                //pointerY: PointerY,
                                moveLeft: MoveLeft,
                                moveRight: MoveRight/*,*/
                                //moveUp: MoveUp,
                                /*moveDown: MoveDown*/);

                            //PointerX = pointer.PointerX;
                            //PointerY = pointer.PointerY;

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
                                ShowInGameImagePanel(_powerUpImage);
                                ShowInGameText($"{LocalizationHelper.GetLocalizedResource("POWER_DOWN")}");
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

                                ShowInGameImagePanel(_rageImage);

                                switch (Player.ShipClass)
                                {
                                    case ShipClass.DEFENDER:
                                        ShowInGameText($"{LocalizationHelper.GetLocalizedResource("SHIELD_DOWN")}");
                                        break;
                                    case ShipClass.BERSERKER:
                                        ShowInGameText($"{LocalizationHelper.GetLocalizedResource("FIREPOWER_DOWN")}");
                                        break;
                                    case ShipClass.SPECTRE:
                                        ShowInGameText($"{LocalizationHelper.GetLocalizedResource("CLOAK_DOWN")}");
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case PLAYER_PROJECTILE:
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

                                ShowInGameImagePanel(_rageImage);

                                switch (Player.ShipClass)
                                {
                                    case ShipClass.DEFENDER:
                                        ShowInGameText($"{LocalizationHelper.GetLocalizedResource("SHIELD_UP")}");
                                        break;
                                    case ShipClass.BERSERKER:
                                        ShowInGameText($"{LocalizationHelper.GetLocalizedResource("FIREPOWER_UP")}");
                                        break;
                                    case ShipClass.SPECTRE:
                                        ShowInGameText($"{LocalizationHelper.GetLocalizedResource("CLOAK_UP")}");
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
                                case ENEMY:
                                    {
                                        var enemy = destroyedObject as Enemy;

                                        _enemyFactory.DestroyEnemy(enemy);
                                        GameScore.EnemiesDestroyed++;

                                        if (enemy.IsBoss)
                                        {
                                            DisengageBoss();
                                            GameScore.BossesDestroyed++;
                                        }
                                    }
                                    break;
                                case METEOR:
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
                case ENEMY_PROJECTILE:
                    {
                        var projectile = gameObject as EnemyProjectile;

                        _enemyProjectileFactory.UpdateProjectile(projectile, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (StarView.IsWarpingThroughSpace)
                            return;

                        // check if enemy projectile collides with player
                        if (_playerFactory.PlayerCollision(player: Player, gameObject: projectile))
                        {
                            SetPlayerHealthBar();
                        }
                    }
                    break;
                case ENEMY:
                    {
                        var enemy = gameObject as Enemy;

                        _enemyFactory.UpdateEnemy(enemy: enemy, pointerX: PointerX, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (StarView.IsWarpingThroughSpace)
                            return;

                        // check if enemy collides with player
                        if (_playerFactory.PlayerCollision(player: Player, gameObject: enemy))
                        {
                            SetPlayerHealthBar();
                            return;
                        }

                        // fire projectiles if at a legitimate distance from player
                        if (enemy.IsProjectileFiring && Player.GetY() - enemy.GetY() > 100)
                            _enemyProjectileFactory.SpawnProjectile(enemy: enemy, gameLevel: GameLevel);
                    }
                    break;
                case METEOR:
                    {
                        var meteor = gameObject as Meteor;

                        _meteorFactory.UpdateMeteor(meteor: meteor, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (StarView.IsWarpingThroughSpace)
                            return;

                        // check if meteor collides with player
                        if (_playerFactory.PlayerCollision(player: Player, gameObject: meteor))
                        {
                            SetPlayerHealthBar();
                        }
                    }
                    break;
                case HEALTH:
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

                            ShowInGameImagePanel(_healthImage);
                            ShowInGameText($"‍{LocalizationHelper.GetLocalizedResource("SHIP_REPAIRED")}");
                        }
                    }
                    break;
                case COLLECTIBLE:
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
                            GameScore.Score++;
                            GameScore.CollectiblesCollected++;
                            SetGameLevel(); // check game level on score change
                            //ShowInGameText($"‍💫 {LocalizationHelper.GetLocalizedResource("COLLECTIBLE_COLLECTED")}");
                        }
                    }
                    break;
                case POWERUP:
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
                            ShowInGameImagePanel(_powerUpImage);
                            ShowInGameText($"‍{LocalizationHelper.GetLocalizedResource(PowerUpType.ToString())}"); // show power up text
                            _playerProjectileFactory.PowerUp(PowerUpType);
                        }
                    }
                    break;
                case STAR:
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

        ///// <summary>
        ///// Updates the game score, player health.
        ///// </summary>
        //private void UpdateScore()
        //{   
        //    //GameLevelText.Text = GameLevel.ToString().Replace("_", " ").ToUpper();
        //    //ScoreBarCount.Text = Score.ToString();

        //    //var timeSpan = TimeSpan.FromMilliseconds(frameStartTime);
        //    //TimeText.Text = $"{(timeSpan.Hours > 0 ? $"{timeSpan.Hours}h" : "")}{(timeSpan.Minutes > 0 ? $"{timeSpan.Minutes}m" : "")}{timeSpan.Seconds}s";
        //    //ScoreText.Text = $"SCORE: {Score} - {GameLevel.ToString().Replace("_", " ").ToUpper()} - TIME: {(timeSpan.Hours > 0 ? $"{timeSpan.Hours}h" : "")}{(timeSpan.Minutes > 0 ? $"{timeSpan.Minutes}m" : "")}{timeSpan.Seconds}s";
        //}

        /// <summary>
        /// Sets analytics of fps, frame time and objects currently in view.
        /// </summary>
        private void SetAnalytics()
        {
#if DEBUG
            _frameStatUpdateSpawnCounter -= 1;

            if (_frameStatUpdateSpawnCounter < 0)
            {
                var enemies = GameView.Children.OfType<Enemy>().Count();
                var meteors = GameView.Children.OfType<Meteor>().Count();
                var powerUps = GameView.Children.OfType<PowerUp>().Count();
                var healths = GameView.Children.OfType<Health>().Count();

                var playerProjectiles = GameView.Children.OfType<PlayerProjectile>().Count();
                var enemyProjectiles = GameView.Children.OfType<EnemyProjectile>().Count();

                var stars = StarView.Children.OfType<CelestialObject>().Count();
                var planets = PlanetView.Children.OfType<CelestialObject>().Count();

                var total = GameView.Children.Count + StarView.Children.Count + PlanetView.Children.Count;

                FPSText.Text = "{ FPS: " + _fpsCount + ", Frame: { Time: " + _frameTime + ", Duration: " + (int)_frameDuration + " }}";
                ObjectsCountText.Text = "{ Enemies: " + enemies + ",  Meteors: " + meteors + ",  Power Ups: " + powerUps + ",  Healths: " + healths + ",  Projectiles: { Player: " + playerProjectiles + ",  Enemy: " + enemyProjectiles + "},  Stars: " + stars + ", Planets: " + planets + " }\n{ Total: " + total + " }";

                _frameStatUpdateSpawnCounter = _frameStatUpdateDelay;
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
            _showInGameTextSpawnCounter = _showInGameTextDelay;
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

        private void ShowInGameImagePanel(Image image)
        {
            var scale = GameView.GetGameObjectScale();

            image.Height = 150 * scale;
            image.Width = 150 * scale;

            RageImagePanel.Children.Clear();
            RageImagePanel.Children.Add(image);
            RageImagePanel.Visibility = Visibility.Visible;
            _showInGameImagePanelSpawnCounter = _showInGameImagePanelDelay;
        }

        private void HandleInGameImagePanel()
        {
            if (RageImagePanel.Visibility == Visibility.Visible)
            {
                _showInGameImagePanelSpawnCounter -= 1;

                if (_showInGameImagePanelSpawnCounter <= 0)
                {
                    HideInGameImagePanel();
                }
            }
        }

        private void HideInGameImagePanel()
        {
            RageImagePanel.Visibility = Visibility.Collapsed;
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

            AudioHelper.PlaySound(SoundType.GAME_OVER);

            App.SetScore(GameScore);

            await this.PlayPageUnLoadedTransition();

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

        #endregion

        #region Frame Methods

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

        #region Player Methods

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

        #region Boss Methods

        /// <summary>
        /// Engages a boss.
        /// </summary>
        private void EngageBoss()
        {
            ShowInGameImagePanel(_bossAppearedImage);
            ShowInGameText($"{LocalizationHelper.GetLocalizedResource("LEVEL")} {(int)GameLevel} {LocalizationHelper.GetLocalizedResource("BOSS")}");

            Boss = _enemyFactory.EngageBossEnemy(GameLevel);

            SetBossHealthBar(); // set boss health on boss appearance            
        }

        /// <summary>
        /// Sets the boss health bar.
        /// </summary>
        private void SetBossHealthBar()
        {
            BossHealthBar.Value = Boss.Health / BossTotalHealth * 100;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        private void DisengageBoss()
        {
            WarpThroughSpace();

            ShowInGameImagePanel(_bossClearedImage);
            ShowInGameText($"{LocalizationHelper.GetLocalizedResource("LEVEL")} {(int)GameLevel} {LocalizationHelper.GetLocalizedResource("COMPLETE")}");

            _enemyFactory.DisengageBossEnemy();

            Boss = null;
            SetGameLevelText();
        }

        #endregion

        #region Difficulty Methods

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
                ScoreBarCount.Text = $"{LocalizationHelper.GetLocalizedResource("SCORE")} {GameScore.Score}/MAX";
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
                    ShowInGameText($"👊 {LocalizationHelper.GetLocalizedResource("ENEMY_APPROACHES")}");
                    AudioHelper.PlaySound(SoundType.ENEMY_INCOMING);
                    AudioHelper.PlaySound(SoundType.BACKGROUND_MUSIC);
                    SetGameLevelText();
                }
            }
        }

        private void SetScoreBarCountText(int capacity)
        {
            ScoreBarCount.Text = $"{LocalizationHelper.GetLocalizedResource("SCORE")} {GameScore.Score}/{capacity}";
        }

        private void SetGameLevelText()
        {
            GameLevelText.Text = $"{LocalizationHelper.GetLocalizedResource("LEVEL")} {(int)GameLevel + 1}";
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
