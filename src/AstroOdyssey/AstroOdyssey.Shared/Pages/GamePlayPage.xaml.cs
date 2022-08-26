using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using static AstroOdyssey.Constants;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePlayPage : Page
    {
        #region Fields

        private int fpsSpawnCounter = 0;
        private int fpsCount = 0;
        private float lastFPSTime = 0;
        private long frameStartTime;
        private long frameEndTime;
        private int frameStatUpdateSpawnCounter;

        private int frameStatUpdateDelay = 5;

        private int showInGameTextSpawnCounter = 110;
        private int showInGameTextDelay = 110;

        private double frameTime = 19;
        private double frameDuration;

        private double windowWidth, windowHeight;

        private readonly StarHelper _starHelper;
        private readonly MeteorHelper _meteorHelper;
        private readonly EnemyHelper _enemyHelper;
        private readonly HealthHelper _healthHelper;
        private readonly PowerUpHelper _powerUpHelper;
        private readonly PlayerHelper _playerHelper;
        private readonly PlayerProjectileHelper _playerProjectileHelper;
        private readonly EnemyProjectileHelper _enemyProjectileHelper;

        #endregion

        #region Ctor

        public GamePlayPage()
        {
            InitializeComponent();

            Loaded += GamePage_Loaded;
            Unloaded += GamePage_Unloaded;

            windowWidth = Window.Current.Bounds.Width - 10;
            windowHeight = Window.Current.Bounds.Height - 10;

            PointerX = windowWidth / 2;

            AdjustView(); // at constructor

            _starHelper = new StarHelper(GameView);
            _meteorHelper = new MeteorHelper(GameView);
            _enemyHelper = new EnemyHelper(GameView);
            _healthHelper = new HealthHelper(GameView);
            _powerUpHelper = new PowerUpHelper(GameView);
            _playerHelper = new PlayerHelper(GameView);
            _playerProjectileHelper = new PlayerProjectileHelper(GameView);
            _enemyProjectileHelper = new EnemyProjectileHelper(GameView);
        }

        #endregion

        #region Properties

        public DispatcherTimer GameFrameTimer { get; set; }

        public Stopwatch Stopwatch { get; set; }

        public double Score { get; set; } = 0;

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
        void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += GamePage_SizeChanged;

            GameView.Children.Clear();
            ScoreBarPanel.Visibility = Visibility.Collapsed;

            FPSText.Text = "";
            ObjectsCountText.Text = "";

            Boss = null;
            BossHealthBarPanel.Visibility = Visibility.Collapsed;

            Player = null;
            PlayerHealthBarPanel.Visibility = Visibility.Collapsed;

            GameLevel = GameLevel.Level_1;
            GameLevelText.Text = GameLevel.ToString().Replace("_", " ").ToUpper();

            PowerUpType = PowerUpType.NONE;

            Score = 0;
            ScoreBarCount.Text = $"{Score}/50";

            PointerX = windowWidth / 2;

            PauseGameButton.Visibility = Visibility.Collapsed;
            QuitGameButton.Visibility = Visibility.Collapsed;

            ShowInGameText("👆\nTAP ON SCREEN TO BEGIN");
            InputView.Focus(FocusState.Keyboard);
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
            windowWidth = args.NewSize.Width - 10; //Window.Current.Bounds.Width;
            windowHeight = args.NewSize.Height - 10; //Window.Current.Bounds.Height;

            AdjustView(); // at view size change

#if DEBUG
            Console.WriteLine($"View Size: {windowWidth} x {windowHeight}");
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
                ShowInGameText("🛸\nQUIT GAME?\nTAP TO QUIT");
            }
        }

        private async Task<ContentDialogResult> ShowContentDialog(
          string title,
          object content,
          string okButtonText = "Ok",
          string cancelButtonText = "Cancel")
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = title,
                PrimaryButtonText = okButtonText,
                CloseButtonText = cancelButtonText,
                DefaultButton = ContentDialogButton.Primary,
                Content = content,
            };

            var result = await dialog.ShowAsync();

            return result;
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

                if (point.Position.X < windowWidth / 2)  // move left
                {
                    MoveLeft = true;
                    MoveRight = false;
                }
                else if (point.Position.X > windowWidth / 2) // move right
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
                InputView.Focus(FocusState.Keyboard);
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
            AudioHelper.PlaySound(SoundType.MENU_SELECT);
            AudioHelper.PlaySound(SoundType.GAME_START);

            SpawnPlayer();

            SetPlayerY(); // set y position at game start

            SetPlayerHealthBar(); // set player health bar at game start

            InGameText.Text = "";

            IsGameRunning = true;

            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            PlayerHealthBarPanel.Visibility = Visibility.Visible;
            ScoreBarPanel.Visibility = Visibility.Visible;

            SetStars();

            Stopwatch = Stopwatch.StartNew();
            GameFrameTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(frameTime) };

            GameFrameTimer.Tick += (s, e) =>
            {
                frameStartTime = Stopwatch.ElapsedMilliseconds;

                RenderGameFrame();

                CalculateFPS();

                frameEndTime = Stopwatch.ElapsedMilliseconds;

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
                var star = new Star();

                star.SetAttributes(speed: 0.1d, scale: GameView.GetGameObjectScale());

                var top = random.Next(10, (int)GameView.Height - 10);
                var left = random.Next(10, (int)GameView.Width - 10);

                star.AddToGameEnvironment(top: top, left: left, gameEnvironment: GameView);
            }
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        private void StopGame()
        {
            IsGameRunning = false;

            GameFrameTimer.Stop();

            AudioHelper.StopSound();
        }

        /// <summary>
        /// Sets the game and star view sizes according to current window size.
        /// </summary>
        private void AdjustView()
        {
            GameView.SetSize(windowHeight, windowWidth);

            frameTime = 19 + (windowWidth <= 500 ? 3 : 0); // run a little slower on phones as phones have a faster timer

            // resize player size
            if (IsGameRunning)
            {
                PointerX = windowWidth / 2;

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
            GameFrameTimer?.Stop();
            ShowInGameText("👨‍🚀\nGAME PAUSED\nTAP TO RESUME");
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
            InGameText.Text = "";
            GameFrameTimer?.Start();
            FiringProjectiles = true;
            IsGamePaused = false;
            PauseGameButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Collapsed;

            AudioHelper.PlaySound(SoundType.MENU_SELECT);

            AudioHelper.ResumeSound(SoundType.BACKGROUND_MUSIC);
            if (GameView.IsBossEngaged)
                AudioHelper.ResumeSound(SoundType.BOSS_APPEARANCE);

            InputView.Focus(FocusState.Keyboard);
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
                            var pointerX = _playerHelper.UpdatePlayer(
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
                            var pointerX = _playerHelper.UpdateAcceleration(player: Player, pointerX: PointerX);

                            PointerX = pointerX;
                        }

                        if (IsPoweredUp)
                        {
                            if (_playerHelper.PowerUpCoolDown(Player))
                            {
                                _playerProjectileHelper.PowerDown(PowerUpType);
                                IsPoweredUp = false;
                                PowerUpType = PowerUpType.NONE;
                                ShowInGameText("POWER DOWN");
                            }
                        }
                    }
                    break;
                case PLAYER_PROJECTILE:
                    {
                        var projectile = gameObject as PlayerProjectile;

                        // move the projectile up and check if projectile has gone beyond the game view
                        _playerProjectileHelper.UpdateProjectile(projectile: projectile, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (GameView.IsWarpingThroughSpace)
                            return;

                        _playerProjectileHelper.CollidePlayerProjectile(projectile: projectile, score: out double score, destroyedObject: out GameObject destroyedObject);

                        if (GameView.IsBossEngaged)
                        {
                            SetBossHealthBar(); // set boss health bar on projectile hit
                        }

                        if (score > 0)
                        {
                            Score += score;
                            SetGameLevel(); // check game level on score change
                        }

                        if (destroyedObject is not null)
                        {
                            switch (destroyedObject.Tag)
                            {
                                case ENEMY:
                                    {
                                        var enemy = destroyedObject as Enemy;

                                        _enemyHelper.DestroyEnemy(enemy);

                                        if (enemy.IsBoss)
                                        {
                                            DisengageBoss();
                                        }
                                    }
                                    break;
                                case METEOR:
                                    {
                                        _meteorHelper.DestroyMeteor(destroyedObject as Meteor);
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

                        _enemyProjectileHelper.UpdateProjectile(projectile, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (GameView.IsWarpingThroughSpace)
                            return;

                        // check if enemy projectile collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: projectile))
                        {
                            SetPlayerHealthBar();
                        }
                    }
                    break;
                case ENEMY:
                    {
                        var enemy = gameObject as Enemy;

                        _enemyHelper.UpdateEnemy(enemy: enemy, pointerX: PointerX, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (GameView.IsWarpingThroughSpace)
                            return;

                        // check if enemy collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: enemy))
                        {
                            SetPlayerHealthBar();
                            return;
                        }

                        // fire projectiles if at a legitimate distance from player
                        if (enemy.IsProjectileFiring && Player.GetY() - enemy.GetY() > 100)
                            _enemyProjectileHelper.SpawnProjectile(enemy: enemy, gameLevel: GameLevel);
                    }
                    break;
                case METEOR:
                    {
                        var meteor = gameObject as Meteor;

                        _meteorHelper.UpdateMeteor(meteor: meteor, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (GameView.IsWarpingThroughSpace)
                            return;

                        // check if meteor collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: meteor))
                        {
                            SetPlayerHealthBar();
                        }
                    }
                    break;
                case HEALTH:
                    {
                        var health = gameObject as Health;

                        _healthHelper.UpdateHealth(health: health, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (GameView.IsWarpingThroughSpace)
                            return;

                        // check if health collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: health))
                        {
                            SetPlayerHealthBar();
                            ShowInGameText("‍❤️\nHEALTH PICKUP");
                        }
                    }
                    break;
                case POWERUP:
                    {
                        var powerUp = gameObject as PowerUp;

                        _powerUpHelper.UpdatePowerUp(powerUp: powerUp, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        if (GameView.IsWarpingThroughSpace)
                            return;

                        // check if power up collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: powerUp))
                        {
                            IsPoweredUp = true;
                            PowerUpType = powerUp.PowerUpType;
                            ShowInGameText("‍🔥\nPOWER UP\n" + PowerUpType.ToString().Replace("_", " ").Replace("ROUNDS", ""));
                            _playerProjectileHelper.PowerUp(PowerUpType);
                        }
                    }
                    break;
                case STAR:
                    {
                        var star = gameObject as Star;

                        _starHelper.UpdateStar(star: star, destroyed: out bool destroyed);
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
            _starHelper.SpawnStar();

            // only generate game objects if not warping thorugh space
            if (!GameView.IsWarpingThroughSpace)
            {
                _meteorHelper.SpawnMeteor(GameLevel);

                _enemyHelper.SpawnEnemy(GameLevel);

                _healthHelper.SpawnHealth(Player);

                _powerUpHelper.SpawnPowerUp();

                _playerProjectileHelper.SpawnProjectile(
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
            frameStatUpdateSpawnCounter -= 1;

            if (frameStatUpdateSpawnCounter < 0)
            {
                var enemies = GameView.Children.OfType<Enemy>().Count();
                var meteors = GameView.Children.OfType<Meteor>().Count();
                var powerUps = GameView.Children.OfType<PowerUp>().Count();
                var healths = GameView.Children.OfType<Health>().Count();

                var playerProjectiles = GameView.Children.OfType<PlayerProjectile>().Count();
                var enemyProjectiles = GameView.Children.OfType<EnemyProjectile>().Count();

                var stars = GameView.Children.OfType<Star>().Count();

                var total = GameView.Children.Count;

                FPSText.Text = "{ FPS: " + fpsCount + ", Frame: { Time: " + frameTime + ", Duration: " + (int)frameDuration + " }}";
                ObjectsCountText.Text = "{ Enemies: " + enemies + ",  Meteors: " + meteors + ",  Power Ups: " + powerUps + ",  Healths: " + healths + ",  Projectiles: { Player: " + playerProjectiles + ",  Enemy: " + enemyProjectiles + "},  Stars: " + stars + " }\n{ Total: " + total + " }";

                frameStatUpdateSpawnCounter = frameStatUpdateDelay;
            }
#endif
        }

        /// <summary>
        /// Shows the in game text in game view.
        /// </summary>
        private void ShowInGameText(string text)
        {
            InGameText.Text = text;
        }

        /// <summary>
        /// Hides the in game text after keeping it visible for a few frames.
        /// </summary>
        private void HandleInGameText()
        {
            if (!InGameText.Text.IsNullOrBlank())
            {
                showInGameTextSpawnCounter -= 1;

                if (showInGameTextSpawnCounter <= 0)
                {
                    InGameText.Text = null;
                    showInGameTextSpawnCounter = showInGameTextDelay;
                }
            }
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
        private void QuitGame()
        {
            StopGame();

            AudioHelper.PlaySound(SoundType.GAME_OVER);

            App.SetScore(Score);

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

            _starHelper.WarpThroughSpace();
        }

        #endregion

        #region Frame Methods

        /// <summary>
        /// Sets the frame time.
        /// </summary>
        private void GetFrameDuration()
        {
            frameDuration = frameEndTime - frameStartTime;
        }

        /// <summary>
        /// Calculates the frames per second.
        /// </summary>
        private void CalculateFPS()
        {
            // calculate FPS
            if (lastFPSTime + 1000 < frameStartTime)
            {
                fpsCount = fpsSpawnCounter;
                fpsSpawnCounter = 0;
                lastFPSTime = frameStartTime;
            }

            fpsSpawnCounter++;
        }

        #endregion

        #region Player Methods

        /// <summary>
        /// Spawns the player.
        /// </summary>
        private void SpawnPlayer()
        {
            var scale = GameView.GetGameObjectScale();
            Player = _playerHelper.SpawnPlayer(pointerX: PointerX, ship: App.Ship);

#if DEBUG
            Console.WriteLine($"Render Scale: {scale}");
#endif
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        private void SetPlayerY()
        {
            PointerY = windowHeight - Player.Height - 25;

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
            _playerHelper.DamageRecoveryCoolDown(Player);
        }

        #endregion

        #region Boss Methods

        /// <summary>
        /// Engages a boss.
        /// </summary>
        private void EngageBoss()
        {
            ShowInGameText("💀\nDEFEAT THE BOSS");
            Boss = _enemyHelper.EngageBossEnemy(GameLevel);

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

            ShowInGameText($"💥\nLEVEL {(int)GameLevel} BOSS DEFEATED");
            _enemyHelper.DisengageBossEnemy();
            Boss = null;

            //TODO: clear all objects except stars
        }

        #endregion

        #region Difficulty Methods

        /// <summary>
        /// Sets the game level according to score; 
        /// </summary>
        private void SetGameLevel()
        {
            var lastGameLevel = GameLevel;

            if (Score > 0)
            {
                GameLevel = GameLevel.Level_1;
                ScoreBar.Value = Score / 50 * 100;
                ScoreBarCount.Text = $"{Score}/50";
            }
            if (Score > 50)
            {
                GameLevel = GameLevel.Level_2;
                ScoreBar.Value = Score / 100 * 100;
                ScoreBarCount.Text = $"{Score}/100";
            }
            if (Score > 100)
            {
                GameLevel = GameLevel.Level_3;
                ScoreBar.Value = Score / 200 * 100;
                ScoreBarCount.Text = $"{Score}/200";
            }
            if (Score > 200)
            {
                GameLevel = GameLevel.Level_4;
                ScoreBar.Value = Score / 400 * 100;
                ScoreBarCount.Text = $"{Score}/400";
            }
            if (Score > 400)
            {
                GameLevel = GameLevel.Level_5;
                ScoreBar.Value = Score / 600 * 100;
                ScoreBarCount.Text = $"{Score}/600";
            }
            if (Score > 600)
            {
                GameLevel = GameLevel.Level_6;
                ScoreBar.Value = Score / 800 * 100;
                ScoreBarCount.Text = Score.ToString();
            }
            if (Score > 800)
            {
                GameLevel = GameLevel.Level_7;
                ScoreBar.Value = Score / 1000 * 100;
                ScoreBarCount.Text = $"{Score}/1000";
            }
            if (Score > 1000)
            {
                GameLevel = GameLevel.Level_8;
                ScoreBar.Value = Score / 1200 * 100;
                ScoreBarCount.Text = $"{Score}/1200";
            }
            if (Score > 1200)
            {
                GameLevel = GameLevel.Level_9;
                ScoreBar.Value = Score / 1400 * 100;
                ScoreBarCount.Text = $"{Score}/1400";
            }
            if (Score > 1400)
            {
                GameLevel = GameLevel.Level_10;
                ScoreBarCount.Text = $"{Score}/MAX";
            }

            // when difficulty changes show level up
            if (lastGameLevel != GameLevel)
            {
                LevelUpObjects();

                // bosses apprear after level 2
                if (GameLevel > GameLevel.Level_2)
                {
                    EngageBoss();
                }
                else
                {
                    WarpThroughSpace();
                    ShowInGameText("☄️\nMETEORS INCOMING");
                    AudioHelper.PlaySound(SoundType.METEOR_INCOMING);
                }

                GameLevelText.Text = GameLevel.ToString().Replace("_", " ").ToUpper();
            }
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
                        _enemyHelper.LevelUp();
                        _meteorHelper.LevelUp();
                        _healthHelper.LevelUp();
                        _powerUpHelper.LevelUp();
                        _starHelper.LevelUp();
                        _playerProjectileHelper.LevelUp();
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}
