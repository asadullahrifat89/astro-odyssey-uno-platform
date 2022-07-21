﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using static AstroOdyssey.Constants;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AstroOdyssey
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePlayPage : Page
    {
        #region Fields

        private string baseUrl;

        const float FRAME_CAP_MS = 1000.0f / 50.0f;
        private int fpsCounter = 0;
        private int fpsCount = 0;
        private float lastFPSTime = 0;
        private long frameStartTime;
        private long frameEndTime;
        private int frameStatUpdateCounter;

        private int frameStatUpdateFrequency = 5;
        private int showInGameTextFrequency = 100;

        private double windowWidth, windowHeight;

        private int showInGameTextCounter;

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

            SetWindowSize();
            GetBaseUrl();

            _starHelper = new StarHelper(GameView);
            _meteorHelper = new MeteorHelper(GameView, baseUrl);
            _enemyHelper = new EnemyHelper(GameView, baseUrl);
            _healthHelper = new HealthHelper(GameView, baseUrl);
            _powerUpHelper = new PowerUpHelper(GameView, baseUrl);
            _playerHelper = new PlayerHelper(GameView, baseUrl);
            _playerProjectileHelper = new PlayerProjectileHelper(GameView, baseUrl);
            _enemyProjectileHelper = new EnemyProjectileHelper(GameView, baseUrl);
        }

        #endregion

        #region Properties        

        private bool IsPoweredUp { get; set; }

        private PowerUpType PowerUpType { get; set; }

        private double Score { get; set; } = 0;

        private double PointerX { get; set; }

        private int FrameTime { get; set; } = 19;

        private long FrameDuration { get; set; }

        private bool IsGameRunning { get; set; }

        private GameLevel GameLevel { get; set; }

        private PeriodicTimer GameFrameTimer { get; set; }

        private Player Player { get; set; }

        private bool MoveLeft { get; set; } = false;

        private bool MoveRight { get; set; } = false;

        private bool FiringProjectiles { get; set; } = false;

        #endregion

        #region Methods

        #region Game Methods

        /// <summary>
        /// Starts the game. Spawns the player and starts game and projectile loops.
        /// </summary>
        private async void StartGame()
        {
            App.PlaySound(baseUrl, SoundType.GAME_START);

            SpawnPlayer();

            SetPlayerY(); // set y position at game start            

            UpdateScore();

            InGameText.Text = "";

            IsGameRunning = true;

            await Task.Delay(TimeSpan.FromSeconds(1));

            RunGame();

            App.PlaySound(baseUrl, SoundType.BACKGROUND_MUSIC);
        }

        /// <summary>
        /// Runs game. Updates stats, gets player bounds, spawns enemies and meteors, moves the player, updates the frame, scales difficulty, checks player health, calculates fps and frame time.
        /// </summary>
        private async void RunGame()
        {
            var watch = Stopwatch.StartNew();

            GameFrameTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(FrameTime));

            while (IsGameRunning && await GameFrameTimer.WaitForNextTickAsync())
            {
                frameStartTime = watch.ElapsedMilliseconds;

                UpdateFrame();

                CalculateFPS();

                frameEndTime = watch.ElapsedMilliseconds;

                SetFrameTime();
#if DEBUG
                SetAnalytics();
#endif
                //await Task.Delay(FrameTime);
            }
        }

        /// <summary>
        /// Sets the frame time.
        /// </summary>
        private void SetFrameTime()
        {
            FrameDuration = frameEndTime - frameStartTime;
            //FrameTime = Math.Max((int)(FRAME_CAP_MS - FrameDuration), 10);
        }

        /// <summary>
        /// Calculates the frames per second.
        /// </summary>
        private void CalculateFPS()
        {
            // calculate FPS
            if (lastFPSTime + 1000 < frameStartTime)
            {
                fpsCount = fpsCounter;
                fpsCounter = 0;
                lastFPSTime = frameStartTime;
            }

            fpsCounter++;
        }

        /// <summary>
        /// Updates a frame in the game.
        /// </summary>
        private void UpdateFrame()
        {
            GameOver();

            UpdateGameObjects();

            SetGameLevel();

            SpawnGameObjects();

            UpdateScore();

            HandleInGameText();

            _playerHelper.HandleDamageRecovery(Player);
        }

        /// <summary>
        /// Check if game if over.
        /// </summary>
        private void GameOver()
        {
            if (Player.HasNoHealth)
            {
                HealthText.Text = "Game Over";

                StopGame();

                App.PlaySound(baseUrl, SoundType.GAME_OVER);

                App.SetScore(Score);

                App.NavigateToPage(typeof(GameOverPage));
            }
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
                if (gameObject.IsMarkedForFadedDestruction)
                {
                    gameObject.Fade();

                    if (gameObject.HasFadedAway)
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
                        if (MoveLeft || MoveRight)
                            PointerX = _playerHelper.UpdatePlayer(player: Player, pointerX: PointerX, moveLeft: MoveLeft, moveRight: MoveRight);

                        if (IsPoweredUp)
                        {
                            if (_playerHelper.PowerDown(Player))
                            {
                                _playerProjectileHelper.PowerDown(PowerUpType);
                                IsPoweredUp = false;
                                PowerUpType = PowerUpType.NONE;
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

                        _playerProjectileHelper.CollidePlayerProjectile(projectile: projectile, score: out double score, destroyedObject: out GameObject destroyedObject);

                        if (score > 0)
                            Score += score;

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
                                            ShowInGameText("BOSS CLEARED");
                                            _enemyHelper.DisengageBossEnemy();
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

                        // check if enemy projectile collides with player
                        _playerHelper.PlayerCollision(player: Player, gameObject: projectile);
                    }
                    break;
                case ENEMY:
                    {
                        var enemy = gameObject as Enemy;

                        _enemyHelper.UpdateEnemy(enemy: enemy, pointerX: PointerX, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        // check if enemy collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: enemy))
                            return;

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

                        // check if meteor collides with player
                        _playerHelper.PlayerCollision(player: Player, gameObject: meteor);
                    }
                    break;
                case HEALTH:
                    {
                        var health = gameObject as Health;

                        _healthHelper.UpdateHealth(health: health, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        // check if health collides with player
                        _playerHelper.PlayerCollision(player: Player, gameObject: health);
                    }
                    break;
                case POWERUP:
                    {
                        var powerUp = gameObject as PowerUp;

                        _powerUpHelper.UpdatePowerUp(powerUp: powerUp, destroyed: out bool destroyed);

                        if (destroyed)
                            return;

                        // check if power up collides with player
                        if (_playerHelper.PlayerCollision(player: Player, gameObject: powerUp))
                        {
                            IsPoweredUp = true;
                            PowerUpType = powerUp.PowerUpType;
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

            _meteorHelper.SpawnMeteor(GameLevel);

            _enemyHelper.SpawnEnemy(GameLevel);

            _healthHelper.SpawnHealth(Player);

            _powerUpHelper.SpawnPowerUp();

            _playerProjectileHelper.SpawnProjectile(isPoweredUp: IsPoweredUp, firingProjectiles: FiringProjectiles, player: Player, gameLevel: GameLevel, powerUpType: PowerUpType);
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        private void StopGame()
        {
            IsGameRunning = false;

            GameFrameTimer?.Dispose();

            App.StopSound();
        }

        /// <summary>
        /// Updates the game score, player health.
        /// </summary>
        private void UpdateScore()
        {
            var timeSpan = TimeSpan.FromMilliseconds(frameStartTime);

            ScoreText.Text = $"{Score} - {GameLevel.ToString().Replace("_", " ")} - {(timeSpan.Hours > 0 ? $"{timeSpan.Hours}h " : "")}{(timeSpan.Minutes > 0 ? $"{timeSpan.Minutes}m " : "")}{timeSpan.Seconds}s";
            HealthText.Text = Player.GetHealthPoints();
        }

        /// <summary>
        /// Shows the level up text in game view.
        /// </summary>
        private void ShowLevelUpInGame()
        {
            ShowInGameText(GameLevel.ToString().ToUpper().Replace("_", " "));
            App.PlaySound(baseUrl, SoundType.LEVEL_UP);
        }

        /// <summary>
        /// Get base url for the app.
        /// </summary>
        private void GetBaseUrl()
        {
            var indexUrl = Uno.Foundation.WebAssemblyRuntime.InvokeJS("window.location.href;");
            var appPackage = Environment.GetEnvironmentVariable("UNO_BOOTSTRAP_APP_BASE");
            baseUrl = $"{indexUrl}{appPackage}";

#if DEBUG
            Console.WriteLine(baseUrl);
#endif
        }

        /// <summary>
        /// Sets analytics of fps, frame time and objects currently in view.
        /// </summary>
        private void SetAnalytics()
        {
            frameStatUpdateCounter -= 1;

            if (frameStatUpdateCounter < 0)
            {
                var enemies = GameView.Children.OfType<Enemy>().Count();
                var meteors = GameView.Children.OfType<Meteor>().Count();
                var powerUps = GameView.Children.OfType<PowerUp>().Count();
                var healths = GameView.Children.OfType<Health>().Count();
                var playerProjectiles = GameView.Children.OfType<PlayerProjectile>().Count();
                var enemyProjectiles = GameView.Children.OfType<EnemyProjectile>().Count();

                var stars = GameView.Children.OfType<Star>().Count();

                var total = GameView.Children.Count;

                FPSText.Text = "{ FPS: " + fpsCount + ", Frame: { Time: " + FrameTime + ", Duration: " + (int)FrameDuration + ",  Start Time: " + frameStartTime + ",  End Time: " + frameEndTime + " }}";
                ObjectsCountText.Text = "{ Enemies: " + enemies + ",  Meteors: " + meteors + ",  Power Ups: " + powerUps + ",  Healths: " + healths + ",  Projectiles: { Player: " + playerProjectiles + ",  Enemy: " + enemyProjectiles + "},  Stars: " + stars + " }\n{ Total: " + total + " }";

                frameStatUpdateCounter = frameStatUpdateFrequency;
            }
        }

        /// <summary>
        /// Shows the in game text in game view.
        /// </summary>
        private void ShowInGameText(string text)
        {
            InGameText.Text = text;
            showInGameTextCounter = showInGameTextFrequency;
        }

        /// <summary>
        /// Hides the in game text after keeping it visible for a few frames.
        /// </summary>
        private void HandleInGameText()
        {
            if (!InGameText.Text.IsNullOrBlank())
            {
                showInGameTextCounter -= 1;

                if (showInGameTextCounter <= 0)
                {
                    InGameText.Text = "";
                }
            }
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
                GameLevel = GameLevel.Level_1;
            if (Score > 50)
                GameLevel = GameLevel.Level_2;
            if (Score > 100)
                GameLevel = GameLevel.Level_3;
            if (Score > 200)
                GameLevel = GameLevel.Level_4;
            if (Score > 400)
                GameLevel = GameLevel.Level_5;
            if (Score > 600)
                GameLevel = GameLevel.Level_6;
            if (Score > 800)
                GameLevel = GameLevel.Level_7;
            if (Score > 1000)
                GameLevel = GameLevel.Level_8;
            if (Score > 1200)
                GameLevel = GameLevel.Level_9;
            if (Score > 1400)
                GameLevel = GameLevel.Level_10;

            // when difficulty changes show level up
            if (lastGameLevel != GameLevel)
            {
                ShowLevelUpInGame();
                LevelUpObjects();
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

                        if (GameLevel > GameLevel.Level_2)
                        {
                            ShowInGameText("BOSS INCOMING");
                            _enemyHelper.EngageBossEnemy(GameLevel);
                        }

                        _meteorHelper.LevelUp();

                        _healthHelper.LevelUp();

                        _powerUpHelper.LevelUp();

                        _starHelper.LevelUp();

                        _playerProjectileHelper.LevelUp();

                        //_enemyProjectileHelper.LevelUp();
                    }
                    break;
            }
        }

        #endregion

        #region Player Methods

        /// <summary>
        /// Spawns the player.
        /// </summary>
        private void SpawnPlayer()
        {
            var scale = GameView.GetGameObjectScale();

#if DEBUG
            Console.WriteLine($"Render Scale: {scale}");
#endif

            Player = _playerHelper.SpawnPlayer(pointerX: PointerX);
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        private void SetPlayerY()
        {
            Player.SetY(windowHeight - Player.Height - 20);
        }

        #endregion      

        #region Window Events

        /// <summary>
        /// When the window is loaded, we add the event Current_SizeChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += GamePage_SizeChanged;
            ShowInGameText("TAP TO START");
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

            PointerX = windowWidth / 2;

#if DEBUG
            Console.WriteLine($"View Size: {windowWidth} x {windowHeight}");
#endif
            SetViewSizes();

            if (IsGameRunning)
            {
                SetPlayerY(); // windows size changed so reset y position
            }
        }

        /// <summary>
        /// Sets the window and canvas size on startup.
        /// </summary>
        private void SetWindowSize()
        {
            windowWidth = Window.Current.Bounds.Width - 10;
            windowHeight = Window.Current.Bounds.Height - 10;

            PointerX = windowWidth / 2;

            SetViewSizes();
        }

        /// <summary>
        /// Sets the game and star view sizes according to current window size.
        /// </summary>
        private void SetViewSizes()
        {
            GameView.SetSize(windowHeight, windowWidth);

            // resize player size
            if (IsGameRunning)
            {
                MoveLeft = false; MoveRight = false;

                var scale = GameView.GetGameObjectScale();

                Player.ReAdjustScale(scale: scale);

#if DEBUG
                Console.WriteLine($"Render Scale: {scale}");
#endif
            }
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

        #endregion   

        #region Input Events       

        private void InputView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left: { MoveLeft = true; MoveRight = false; } break;
                case Windows.System.VirtualKey.Right: { MoveRight = true; MoveLeft = false; } break;
                case Windows.System.VirtualKey.Up: { FiringProjectiles = true; } break;
                case Windows.System.VirtualKey.Space: { FiringProjectiles = true; } break;
                default:
                    break;
            }
        }

        private void InputView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left: { MoveLeft = false; } break;
                case Windows.System.VirtualKey.Right: { MoveRight = false; } break;
                case Windows.System.VirtualKey.Up: { FiringProjectiles = false; } break;
                case Windows.System.VirtualKey.Space: { FiringProjectiles = false; } break;
                default:
                    break;
            }
        }

        private void LeftInputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FiringProjectiles = true;
            MoveLeft = true;
            MoveRight = false;
        }

        private void LeftInputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FiringProjectiles = true;
            MoveLeft = false;

            if (!IsGameRunning)
                StartGame();
        }

        private void RightInputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FiringProjectiles = true;
            MoveRight = true;
            MoveLeft = false;
        }

        private void RightInputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FiringProjectiles = true;
            MoveRight = false;

            if (!IsGameRunning)
                StartGame();
        }

        #endregion

        #endregion
    }
}
