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

        private int fpsCounter = 0;
        private int frameStatUpdateCounter;

        private double windowWidth, windowHeight;

        private int showInGameTextCounter;

        private readonly StarHelper _starHelper;
        private readonly MeteorHelper _meteorHelper;
        private readonly EnemyHelper _enemyHelper;
        private readonly HealthHelper _healthHelper;
        private readonly PowerUpHelper _powerUpHelper;
        private readonly ProjectileHelper _projectileHelper;
        private readonly PlayerHelper _playerHelper;

        #endregion

        #region Ctor

        public GamePlayPage()
        {
            InitializeComponent();

            Loaded += GamePage_Loaded;
            Unloaded += GamePage_Unloaded;

            SetWindowSize();
            GetBaseUrl();

            _starHelper = new StarHelper(StarView);
            _meteorHelper = new MeteorHelper(GameView, baseUrl);
            _enemyHelper = new EnemyHelper(GameView, baseUrl);
            _healthHelper = new HealthHelper(GameView, baseUrl);
            _powerUpHelper = new PowerUpHelper(GameView, baseUrl);
            _projectileHelper = new ProjectileHelper(GameView, baseUrl);
            _playerHelper = new PlayerHelper(GameView, baseUrl);
        }

        #endregion

        #region Properties

        private int FpsCount { get; set; } = 0;

        private float LastFrameTime { get; set; } = 0;

        private long FrameStartTime { get; set; }

        private bool IsPoweredUp { get; set; }

        private int FrameStatUpdateLimit { get; set; } = 5;

        private int ShowInGameTextLimit { get; set; } = 100;

        private double Score { get; set; } = 0;

        private double PointerX { get; set; }

        private int FrameDuration { get; set; } = 17;

        private bool IsGameRunning { get; set; }

        private GameLevel GameLevel { get; set; }

        private PeriodicTimer GameViewTimer { get; set; }

        private PeriodicTimer StarViewTimer { get; set; }

        private Player Player { get; set; }

        private double PlayerSpeed { get; set; } = 12;

        private double PlayerX { get; set; }

        private double PlayerWidthHalf { get; set; }

        private bool MoveLeft { get; set; } = false;

        private bool MoveRight { get; set; } = false;

        private bool FiringProjectiles { get; set; } = false;

        #endregion

        #region Methods

        #region Views

        /// <summary>
        /// Updates meteors, enemies, projectiles in the game view. Advances game objects in the frame.
        /// </summary>
        private void UpdateGameView()
        {
            var gameObjects = GameView.GetGameObjects<GameObject>().Where(x => x is not AstroOdyssey.Player);

            foreach (var gameObject in gameObjects)
            {
                UpdateGameViewObjects(gameObject);
            }

            GameView.RemoveDestroyableGameObjects();
        }

        /// <summary>
        /// Updates game objects in game view. Moves the objects. Detects collision causes and applies effects.
        /// </summary>
        /// <param name="gameObject"></param>
        private void UpdateGameViewObjects(GameObject gameObject)
        {
            if (gameObject.MarkedForFadedRemoval)
            {
                gameObject.Fade();

                if (gameObject.HasFadedAway)
                {
                    GameView.AddDestroyableGameObject(gameObject);
                    return;
                }
            }

            var tag = gameObject.Tag;

            switch (tag)
            {
                case PROJECTILE:
                    {
                        var projectile = gameObject as Projectile;

                        _projectileHelper.UpdateProjectile(projectile, out bool destroyed);

                        if (destroyed)
                            return;

                        // get the destructible objects which intersect with the current projectile
                        var destructibles = GameView.GetDestructibles(projectile.GetRect());

                        foreach (var destructible in destructibles)
                        {
                            GameView.AddDestroyableGameObject(projectile);

                            // if projectile is powered up then execute over kill
                            if (projectile.IsPoweredUp)
                                destructible.LooseHealth(destructible.HealthSlot * 2);
                            else
                                destructible.LooseHealth();

                            // fade the a bit on projectile hit
                            destructible.Fade();

                            //App.PlaySound(SoundType.LASER_HIT);

                            if (destructible.HasNoHealth)
                            {
                                switch (destructible.Tag)
                                {
                                    case ENEMY:
                                        {
                                            var enemy = destructible as Enemy;

                                            if (enemy.IsOverPowered)
                                                Score += 4;
                                            else
                                                Score += 2;

                                            _enemyHelper.DestroyEnemy(enemy);
                                        }
                                        break;
                                    case METEOR:
                                        {
                                            var meteor = destructible as Meteor;

                                            if (meteor.IsOverPowered)
                                                Score += 2;
                                            else
                                                Score++;

                                            _meteorHelper.DestroyMeteor(meteor);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case ENEMY:
                    {
                        var enemy = gameObject as Enemy;

                        _enemyHelper.UpdateEnemy(enemy, out bool destroyed);

                        if (destroyed)
                            return;

                        // check if enemy collides with player
                        _playerHelper.PlayerCollision(Player, enemy);
                    }
                    break;
                case METEOR:
                    {
                        var meteor = gameObject as Meteor;

                        _meteorHelper.UpdateMeteor(meteor, out bool destroyed);

                        if (destroyed)
                            return;

                        // check if meteor collides with player
                        _playerHelper.PlayerCollision(Player, meteor);
                    }
                    break;
                case HEALTH:
                    {
                        var health = gameObject as Health;

                        _healthHelper.UpdateHealth(health, out bool destroyed);

                        if (destroyed)
                            return;

                        // check if health collides with player
                        _playerHelper.PlayerCollision(Player, health);
                    }
                    break;
                case POWERUP:
                    {
                        var powerUp = gameObject as PowerUp;

                        _powerUpHelper.UpdatePowerUp(powerUp, out bool destroyed);

                        if (destroyed)
                            return;

                        // check if power up collides with player
                        if (_playerHelper.PlayerCollision(Player, powerUp))
                        {
                            IsPoweredUp = true;
                            _projectileHelper.PowerUp();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///  Updates stars in the game view. Advances game objects in the frame.
        /// </summary>
        private void UpdateStarView()
        {
            var starObjects = StarView.GetGameObjects<GameObject>();

            foreach (var star in starObjects)
            {
                _starHelper.UpdateStar(star as Star);
            }

            StarView.RemoveDestroyableGameObjects();
        }

        #endregion

        #region Game Methods

        /// <summary>
        /// Starts the game. Spawns the player and starts game and projectile loops.
        /// </summary>
        private async void StartGame()
        {
            App.PlaySound(baseUrl, SoundType.GAME_START);

            SpawnPlayer();

            IsGameRunning = true;

            RunStarView();

            await Task.Delay(TimeSpan.FromSeconds(1));

            RunGameView();

            App.PlaySound(baseUrl, SoundType.BACKGROUND_MUSIC);
        }

        /// <summary>
        /// Runs game. Updates stats, gets player bounds, spawns enemies and meteors, moves the player, updates the frame, scales difficulty, checks player health, calculates fps and frame time.
        /// </summary>
        private async void RunGameView()
        {
#if DEBUG
            var watch = Stopwatch.StartNew();
#endif
            GameViewTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(FrameDuration));

            bool renderable = true;

            while (IsGameRunning && await GameViewTimer.WaitForNextTickAsync() && renderable)
            {
                renderable = false;

                //TODO: play player animation

                GameOver();

                PlayerX = Player.GetX();

                PlayerWidthHalf = Player.Width / 2;

                MovePlayer();

                UpdateGameView();

                ShiftGameLevel();

                _playerHelper.PlayerOpacity(Player);

                _enemyHelper.SpawnEnemy(GameLevel);

                _meteorHelper.SpawnMeteor(GameLevel);

                _healthHelper.SpawnHealth(Player);

                _powerUpHelper.SpawnPowerUp();

                _projectileHelper.SpawnProjectile(isPoweredUp: IsPoweredUp, firingProjectiles: FiringProjectiles, player: Player, gameLevel: GameLevel);

                TriggerPowerDown();

                UpdateScore();

                HideInGameText();

                CalculateFps();

                SetFrameAnalytics();

                renderable = true;
#if DEBUG
                FrameStartTime = watch.ElapsedMilliseconds;
#endif
            }
        }

        /// <summary>
        /// Runs stars. Moves the stars.
        /// </summary>
        private async void RunStarView()
        {
            StarViewTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(FrameDuration));

            while (IsGameRunning && await StarViewTimer.WaitForNextTickAsync())
            {
                UpdateStarView();

                _starHelper.SpawnStar();
            }
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        private void StopGame()
        {
            IsGameRunning = false;

            GameViewTimer?.Dispose();
            StarViewTimer?.Dispose();

            App.StopSound();
        }

        /// <summary>
        /// Updates the game score, player health.
        /// </summary>
        private void UpdateScore()
        {
            ScoreText.Text = $"{Score} - {GameLevel.ToString().Replace("_", " ")}";
            HealthText.Text = Player.GetHealthPoints();
        }

        /// <summary>
        /// Shows the level up text in game view.
        /// </summary>
        private void ShowLevelUp()
        {
            ShowInGameText(GameLevel.ToString().ToUpper().Replace("_", " "));
            App.PlaySound(baseUrl, SoundType.LEVEL_UP);
        }

        /// <summary>
        /// Shows the in game text in game view.
        /// </summary>
        private void ShowInGameText(string text)
        {
            InGameText.Text = text;
            InGameText.Visibility = Visibility.Visible;
            showInGameTextCounter = ShowInGameTextLimit;
        }

        /// <summary>
        /// Hides the in game text after keeping it visible.
        /// </summary>
        private void HideInGameText()
        {
            if (InGameText.Visibility == Visibility.Visible)
            {
                showInGameTextCounter -= 1;

                if (showInGameTextCounter <= 0)
                {
                    InGameText.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GetBaseUrl()
        {
            var indexUrl = Uno.Foundation.WebAssemblyRuntime.InvokeJS("window.location.href;");
            var appPackage = Environment.GetEnvironmentVariable("UNO_BOOTSTRAP_APP_BASE");
            baseUrl = $"{indexUrl}{appPackage}";
        }

        #endregion

        #region Frame Methods      

        /// <summary>
        /// Sets analytics of fps, frame time and objects currently in view.
        /// </summary>
        private void SetFrameAnalytics()
        {
#if DEBUG
            frameStatUpdateCounter -= 1;

            if (frameStatUpdateCounter < 0)
            {
                FPSText.Text = "FPS: " + FpsCount;
                FrameDurationText.Text = "Frame: " + FrameDuration + "ms";
                ObjectsCountText.Text = "Objects: " + GameView.Children.Count();

                frameStatUpdateCounter = FrameStatUpdateLimit;
            }
#endif
        }

        /// <summary>
        /// Calculates frames per second.
        /// </summary>
        /// <param name="frameStartTime"></param>
        private void CalculateFps()
        {
#if DEBUG
            // calculate FPS
            if (LastFrameTime + 1000 < FrameStartTime)
            {
                FpsCount = fpsCounter;
                fpsCounter = 0;
                LastFrameTime = FrameStartTime;
            }

            fpsCounter++;
#endif
        }

        #endregion

        #region Difficulty Methods

        /// <summary>
        /// Shifts the game level according to score; 
        /// </summary>
        private void ShiftGameLevel()
        {
            var lastGameLevel = GameLevel;

            if (Score > 0)
                GameLevel = GameLevel.Level_1;
            if (Score > 25)
                GameLevel = GameLevel.Level_2;
            if (Score > 50)
                GameLevel = GameLevel.Level_3;
            if (Score > 100)
                GameLevel = GameLevel.Level_4;
            if (Score > 200)
                GameLevel = GameLevel.Level_5;
            if (Score > 400)
                GameLevel = GameLevel.Level_6;
            if (Score > 800)
                GameLevel = GameLevel.Level_7;
            if (Score > 1600)
                GameLevel = GameLevel.Level_8;

            // when difficulty changes show level up
            if (lastGameLevel != GameLevel)
            {
                ShowLevelUp();
                AdjustGameEnvironment();
            }
        }

        /// <summary>
        /// Adjusts the game environment according to game level.
        /// </summary>
        private void AdjustGameEnvironment()
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

                        _projectileHelper.LevelUp();
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
            Player = _playerHelper.SpawnPlayer(PointerX, PlayerSpeed);
            PlayerWidthHalf = Player.Width / 2;
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        private void SetPlayerY()
        {
            Player.SetY(windowHeight - Player.Height - 20);
        }

        /// <summary>
        /// Sets the x axis position of the player on game canvas.
        /// </summary>
        /// <param name="x"></param>
        private void SetPlayerX(double x)
        {
            Player.SetX(x);
        }

        /// <summary>
        /// Moves the player to last pointer pressed position by x axis.
        /// </summary>
        private void MovePlayer()
        {
            if (MoveLeft && PlayerX > 0)
                PointerX -= Player.Speed;

            if (MoveRight && PlayerX + Player.Width < windowWidth)
                PointerX += Player.Speed;

            // move right
            if (PointerX - PlayerWidthHalf > PlayerX + Player.Speed)
            {
                if (PlayerX + PlayerWidthHalf < windowWidth)
                {
                    SetPlayerX(PlayerX + Player.Speed);
                }
            }

            // move left
            if (PointerX - PlayerWidthHalf < PlayerX - Player.Speed)
            {
                SetPlayerX(PlayerX - Player.Speed);
            }
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

        #endregion

        #region PowerUp Methods      

        /// <summary>
        /// Triggers the powered up state down.
        /// </summary>
        private void TriggerPowerDown()
        {
            if (IsPoweredUp)
            {
                if (_playerHelper.PowerDown(Player))
                {
                    _projectileHelper.PowerDown();
                    IsPoweredUp = false;
                }
            }
        }

        #endregion

        #region Input Events

        //private void InputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    var currentPoint = e.GetCurrentPoint(GameView);

        //    PointerX = currentPoint.Position.X;

        //    FiringProjectiles = true;
        //}

        //private void InputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    FiringProjectiles = false;
        //}

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
#if DEBUG
            Console.WriteLine(baseUrl);
#endif
            StartGame();
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
            Console.WriteLine($"{windowWidth} x {windowHeight}");
#endif

            SetViewSizes();
            SetPlayerY();
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
            StarView.SetSize(windowHeight, windowWidth);

            // resize player size
            if (IsGameRunning)
            {
                MoveLeft = false; MoveRight = false;

                var scale = GameView.GetGameObjectScale();

                Player.SetAttributes(speed: PlayerSpeed * scale, scale: scale);
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

        #endregion
    }
}
