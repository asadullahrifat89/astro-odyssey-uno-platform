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

        private int powerUpCounter = 1500;
        private int powerUpTriggerCounter;
        private int laserCounter;
        private int enemyCounter;
        private int meteorCounter;
        private int rotatedEnemySpawnCounter = 10;
        private int healthCounter;
        private int starCounter;
        private int playerDamagedOpacityCounter;
        private int showInGameTextCounter;

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public GamePlayPage()
        {
            InitializeComponent();

            Loaded += GamePage_Loaded;
            Unloaded += GamePage_Unloaded;

            SetWindowSize();
        }

        #endregion

        #region Properties

        private int FpsCount { get; set; } = 0;

        private float LastFrameTime { get; set; } = 0;

        private long FrameStartTime { get; set; }

        private bool PowerUpTriggered { get; set; }

        private double LaserSpeed { get; set; } = 18;

        private double EnemySpeed { get; set; } = 2;

        private double MeteorSpeed { get; set; } = 1.5;

        private double HealthSpeed { get; set; } = 2;

        private double PowerUpSpeed { get; set; } = 2;

        private double StarSpeed { get; set; } = 0.1d;

        private double PlayerSpeed { get; set; } = 12;

        private int FrameStatUpdateLimit { get; set; } = 5;

        private int LaserSpawnLimit { get; set; } = 16;

        private int PowerUpTriggerLimit { get; set; } = 1000;

        private int EnemySpawnLimit { get; set; } = 50;

        private int MeteorSpawnLimit { get; set; } = 55;

        private int HealthSpawnLimit { get; set; } = 1000;

        private int PowerUpSpawnLimit { get; set; } = 1500;

        private int StarSpawnLimit { get; set; } = 100;

        private int PlayerDamagedOpacityLimit { get; set; } = 100;

        private int ShowInGameTextLimit { get; set; } = 100;

        private int RotatedEnemySpawnLimit { get; set; } = 10;

        private double Score { get; set; } = 0;

        private double PointerX { get; set; }

        private double PlayerX { get; set; }

        private double PlayerWidthHalf { get; set; }

        private int FrameDuration { get; set; } = 17;

        private bool IsGameRunning { get; set; }

        //private bool IsPointerPressed { get; set; }

        //private bool IsKeyboardPressed { get; set; }

        private GameLevel GameLevel { get; set; }
        private PeriodicTimer GameViewTimer { get; set; }
        private PeriodicTimer StarViewTimer { get; set; }

        private Player Player { get; set; }

        private Enemy NewEnemy { get; set; }
        private Meteor NewMeteor { get; set; }
        private Health NewHealth { get; set; }
        private PowerUp NewPowerUp { get; set; }
        private Star NewStar { get; set; }

        private bool MoveLeft { get; set; } = false;
        private bool MoveRight { get; set; } = false;
        private bool FireLasers { get; set; } = false;

        #endregion

        #region Methods

        #region Game Methods

        /// <summary>
        /// Starts the game. Spawns the player and starts game and laser loops.
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

            bool frameRenderable = true;

            while (IsGameRunning && await GameViewTimer.WaitForNextTickAsync() && frameRenderable)
            {
                frameRenderable = false;

                PlayerX = Player.GetX();

                PlayerWidthHalf = Player.Width / 2;

                UpdateScore();

                SpawnEnemy();

                SpawnMeteor();

                SpawnHealth();

                SpawnPowerUp();

                SpawnLaser(PowerUpTriggered);

                MovePlayer();

                UpdateGameView();

                UpdateStarView();

                ShiftGameLevel();

                HideInGameText();

                TriggerPowerDown();

                PlayerOpacity();

                GameOver();

                CalculateFps();

                SetFrameAnalytics();

                frameRenderable = true;
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

                SpawnStar();
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

        /// <summary>
        /// Updates a frame of game view. Advances game objects in the frame.
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
                case LASER:
                    {
                        var laser = gameObject as Laser;

                        // move laser up                
                        laser.MoveY();

                        // remove laser if outside game canvas
                        if (laser.GetY() < 10)
                            GameView.AddDestroyableGameObject(laser);

                        var laserBounds = laser.GetRect();

                        // get the destructible objects which intersect with the current laser
                        var destructibles = GameView.GetGameObjects<GameObject>().Where(destructible => destructible.IsDestructible && destructible.GetRect().Intersects(laserBounds));

                        foreach (var destructible in destructibles)
                        {
                            GameView.AddDestroyableGameObject(laser);

                            // if laser is powered up then execute over kill
                            if (laser.IsPoweredUp)
                                destructible.LooseHealth(destructible.HealthSlot * 2);
                            else
                                destructible.LooseHealth();

                            // fade the a bit on laser hit
                            destructible.Fade();

                            //App.PlaySound(SoundType.LASER_HIT);

                            if (destructible.HasNoHealth)
                            {
                                switch (destructible.Tag)
                                {
                                    case ENEMY:
                                        {
                                            DestroyEnemy(destructible as Enemy);
                                        }
                                        break;
                                    case METEOR:
                                        {
                                            DestroyMeteor(destructible as Meteor);
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

                        // move enemy down
                        enemy.MoveY();
                        enemy.MoveX();

                        // if the object is marked for lazy destruction then no need to perform collisions
                        if (enemy.MarkedForFadedRemoval)
                            return;

                        // if enemy or meteor object has gone beyond game view
                        if (AddDestroyableGameObject(enemy))
                            return;

                        // check if enemy collides with player
                        if (PlayerCollision(enemy))
                            return;
                    }
                    break;
                case METEOR:
                    {
                        var meteor = gameObject as Meteor;

                        // move meteor down
                        meteor.Rotate();
                        meteor.MoveY();

                        // if the object is marked for lazy destruction then no need to perform collisions
                        if (meteor.MarkedForFadedRemoval)
                            return;

                        // if enemy or meteor object has gone beyond game view
                        if (AddDestroyableGameObject(meteor))
                            return;

                        // check if meteor collides with player
                        if (PlayerCollision(meteor))
                            return;
                    }
                    break;
                case HEALTH:
                    {
                        var health = gameObject as Health;

                        // move Health down
                        health.MoveY();

                        // if health object has gone below game view
                        if (health.GetY() > GameView.Height)
                        {
                            GameView.AddDestroyableGameObject(health);
                            return;
                        }

                        if (Player.GetRect().Intersects(health.GetRect()))
                        {
                            GameView.AddDestroyableGameObject(health);
                            PlayerHealthGain(health);
                        }
                    }
                    break;
                case POWERUP:
                    {
                        var powerUp = gameObject as PowerUp;

                        // move PowerUp down
                        powerUp.MoveY();

                        // if PowerUp object has gone below game view
                        if (powerUp.GetY() > GameView.Height)
                        {
                            GameView.AddDestroyableGameObject(powerUp);
                            return;
                        }

                        if (Player.GetRect().Intersects(powerUp.GetRect()))
                        {
                            GameView.AddDestroyableGameObject(powerUp);
                            TriggerPowerUp();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Removes a game object from game view. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private bool AddDestroyableGameObject(GameObject gameObject)
        {
            // if game object is out of bounds of game view
            if (gameObject.GetY() > GameView.Height || gameObject.GetX() > GameView.Width || gameObject.GetX() + gameObject.Width < 0)
            {
                GameView.AddDestroyableGameObject(gameObject);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets scaling factor for a game object according to game view width.
        /// </summary>
        /// <returns></returns>
        private double GetGameObjectScale()
        {
            return GameView.Width <= 500 ? 0.6 : (GameView.Width <= 700 ? 0.8 : (GameView.Width <= 800 ? 0.9 : 1));
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

        #region Score Methods

        /// <summary>
        /// Increase player score if an enemy was destroyed.
        /// </summary>
        private void PlayerScoreByEnemyDestruction()
        {
            Score += 2;
        }

        /// <summary>
        /// Increase player score if a meteor was destroyed.
        /// </summary>
        private void PlayerScoreByMeteorDestruction()
        {
            Score++;
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
                        EnemySpawnLimit -= 2;
                        EnemySpeed += 1;

                        MeteorSpawnLimit -= 2;
                        MeteorSpeed += 1;

                        LaserSpawnLimit -= 1;

                        HealthSpeed += 1;
                        PowerUpSpeed += 1;

                        StarSpeed += 0.1d;
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
            Player = new Player();

            var scale = GetGameObjectScale();

            Player.SetAttributes(speed: PlayerSpeed * scale, scale: scale);
            Player.AddToGameEnvironment(top: windowHeight - Player.Height - 20, left: PointerX - 35, gameEnvironment: GameView);

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
            // game over
            if (Player.HasNoHealth)
            {
                HealthText.Text = "Game Over";
                StopGame();
                App.PlaySound(baseUrl, SoundType.GAME_OVER);

                //TODO: Set Score
                App.SetScore(Score);

                App.NavigateToPage(typeof(GameOverPage));
            }
        }

        /// <summary>
        /// Makes the player loose health.
        /// </summary>
        private void PlayerHealthLoss()
        {
            Player.LooseHealth();

            App.PlaySound(baseUrl, SoundType.HEALTH_LOSS);

            Player.Opacity = 0.4d;

            playerDamagedOpacityCounter = PlayerDamagedOpacityLimit;
        }

        /// <summary>
        /// Sets the player opacity.
        /// </summary>
        private void PlayerOpacity()
        {
            playerDamagedOpacityCounter -= 1;

            if (playerDamagedOpacityCounter <= 0)
                Player.Opacity = 1;
        }

        /// <summary>
        /// Makes the player gain health.
        /// </summary>
        /// <param name="health"></param>
        private void PlayerHealthGain(Health health)
        {
            Player.GainHealth(health.Health);
            App.PlaySound(baseUrl, SoundType.HEALTH_GAIN);
        }

        /// <summary>
        /// Checks and performs player collision.
        /// </summary>
        /// <param name="gameObject"></param>
        /// 
        /// <returns></returns>
        private bool PlayerCollision(GameObject gameObject)
        {
            if (Player.GetRect().Intersects(gameObject.GetRect()))
            {
                GameView.AddDestroyableGameObject(gameObject);
                PlayerHealthLoss();

                return true;
            }

            return false;
        }

        #endregion

        #region Laser Methods        

        /// <summary>
        /// Spawns a laser.
        /// </summary>
        private void SpawnLaser(bool isPoweredUp)
        {
            // each frame progress decreases this counter
            laserCounter -= 1;

            if (laserCounter <= 0)
            {
                // any object falls within player range
                if (FireLasers)
                //if (GameView.GetGameObjects<GameObject>().Where(x => x.IsDestructible).Any(x => Player.AnyNearbyObjectsOnTheRight(gameObject: x) || Player.AnyNearbyObjectsOnTheLeft(gameObject: x)))
                {
                    GenerateLaser(isPoweredUp: isPoweredUp);
                }

                laserCounter = LaserSpawnLimit;
            }
        }

        /// <summary>
        /// Generates a laser.
        /// </summary>
        /// <param name="laserHeight"></param>
        /// <param name="laserWidth"></param>
        private void GenerateLaser(bool isPoweredUp)
        {
            var newLaser = new Laser();

            newLaser.SetAttributes(speed: LaserSpeed, gameLevel: GameLevel, isPoweredUp: isPoweredUp, scale: GetGameObjectScale());

            newLaser.AddToGameEnvironment(top: Player.GetY() + 5, left: Player.GetX() + Player.Width / 2 - newLaser.Width / 2, gameEnvironment: GameView);

            if (newLaser.IsPoweredUp)
                App.PlaySound(baseUrl, SoundType.LASER_FIRE_POWERED_UP);
            else
                App.PlaySound(baseUrl, SoundType.LASER_FIRE);
        }

        #endregion

        #region Enemy Methods

        /// <summary>
        /// Spawns an enemy.
        /// </summary>
        private void SpawnEnemy()
        {
            // each frame progress decreases this counter
            enemyCounter -= 1;

            // when counter reaches zero, create an enemy
            if (enemyCounter < 0)
            {
                GenerateEnemy();

                rotatedEnemySpawnCounter -= 1;

                enemyCounter = EnemySpawnLimit;
            }
        }

        /// <summary>
        /// Generates a random enemy.
        /// </summary>
        private void GenerateEnemy()
        {
            NewEnemy = new Enemy();

            NewEnemy.SetAttributes(speed: EnemySpeed + random.Next(0, 4), scale: GetGameObjectScale());

            var left = 0;
            var top = 0;

            // when not noob anymore enemy moves sideways
            if ((int)GameLevel > 0 && rotatedEnemySpawnCounter <= 0)
            {
                NewEnemy.XDirection = (XDirection)random.Next(1, 3);
                rotatedEnemySpawnCounter = RotatedEnemySpawnLimit;

                switch (NewEnemy.XDirection)
                {
                    case XDirection.LEFT:
                        NewEnemy.Rotation = 50;
                        left = (int)windowWidth;
                        break;
                    case XDirection.RIGHT:
                        left = 0 - (int)NewEnemy.Width + 10;
                        NewEnemy.Rotation = -50;
                        break;
                    default:
                        break;
                }

#if DEBUG
                Console.WriteLine("Enemy XDirection: " + NewEnemy.XDirection + ", " + "X: " + left + " " + "Y: " + top);
#endif
                top = random.Next(0, (int)GameView.Height / 3);
                NewEnemy.Rotate();

                RotatedEnemySpawnLimit = random.Next(5, 15);
            }
            else
            {
                left = random.Next(10, (int)windowWidth - 70);
                top = 0 - (int)NewEnemy.Height;
            }

            NewEnemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: GameView);
        }

        /// <summary>
        /// Destroys an enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="meteor"></param>
        private void DestroyEnemy(Enemy enemy)
        {
            enemy.MarkedForFadedRemoval = true;

            PlayerScoreByEnemyDestruction();

            App.PlaySound(baseUrl, SoundType.ENEMY_DESTRUCTION);
        }

        #endregion

        #region Meteor Methods

        /// <summary>
        /// Spawns a meteor.
        /// </summary>
        private void SpawnMeteor()
        {
            if ((int)GameLevel > 0)
            {
                // each frame progress decreases this counter
                meteorCounter -= 1;

                // when counter reaches zero, create a meteor
                if (meteorCounter < 0)
                {
                    GenerateMeteor();
                    meteorCounter = MeteorSpawnLimit;
                }
            }
        }

        /// <summary>
        /// Generates a random meteor.
        /// </summary>
        private void GenerateMeteor()
        {
            NewMeteor = new Meteor();

            NewMeteor.SetAttributes(speed: MeteorSpeed + random.NextDouble(), scale: GetGameObjectScale());
            NewMeteor.AddToGameEnvironment(top: 0 - NewMeteor.Height, left: random.Next(10, (int)windowWidth - 100), gameEnvironment: GameView);
        }

        /// <summary>
        /// Destroys a meteor. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="meteor"></param>
        private void DestroyMeteor(Meteor meteor)
        {
            meteor.MarkedForFadedRemoval = true;

            PlayerScoreByMeteorDestruction();

            App.PlaySound(baseUrl, SoundType.METEOR_DESTRUCTION);
        }

        #endregion

        #region Health Methods

        /// <summary>
        /// Spawns a Health.
        /// </summary>
        private void SpawnHealth()
        {
            if (Player.Health <= 60)
            {
                // each frame progress decreases this counter
                healthCounter -= 1;

                // when counter reaches zero, create a Health
                if (healthCounter < 0)
                {
                    GenerateHealth();
                    healthCounter = HealthSpawnLimit;
                }
            }
        }

        /// <summary>
        /// Generates a random Health.
        /// </summary>
        private void GenerateHealth()
        {
            NewHealth = new Health();

            NewHealth.SetAttributes(speed: HealthSpeed + random.NextDouble(), scale: GetGameObjectScale());
            NewHealth.AddToGameEnvironment(top: 0 - NewHealth.Height, left: random.Next(10, (int)windowWidth - 100), gameEnvironment: GameView);

            // change the next health spawn time
            HealthSpawnLimit = random.Next(1000, 1500);
        }

        #endregion

        #region PowerUp Methods

        /// <summary>
        /// Spawns a PowerUp.
        /// </summary>
        private void SpawnPowerUp()
        {
            // each frame progress decreases this counter
            powerUpCounter -= 1;

            // when counter reaches zero, create a PowerUp
            if (powerUpCounter < 0)
            {
                GeneratePowerUp();
                powerUpCounter = PowerUpSpawnLimit;
            }

        }

        /// <summary>
        /// Generates a random PowerUp.
        /// </summary>
        private void GeneratePowerUp()
        {
            NewPowerUp = new PowerUp();

            NewPowerUp.SetAttributes(speed: PowerUpSpeed + random.NextDouble(), scale: GetGameObjectScale());
            NewPowerUp.AddToGameEnvironment(top: 0 - NewPowerUp.Height, left: random.Next(10, (int)windowWidth - 100), gameEnvironment: GameView);

            // change the next power up spawn time
            PowerUpSpawnLimit = random.Next(1500, 2000);
        }

        /// <summary>
        /// Triggers the powered up state.
        /// </summary>
        private void TriggerPowerUp()
        {
            PowerUpTriggered = true;
            powerUpTriggerCounter = PowerUpTriggerLimit;

            LaserSpawnLimit -= 1;
            ShowInGameText("POWER UP!");

            App.PlaySound(baseUrl, SoundType.POWER_UP);
            Player.TriggerPowerUp();
        }

        /// <summary>
        /// Triggers the powered up state down.
        /// </summary>
        private void TriggerPowerDown()
        {
            if (PowerUpTriggered)
            {
                powerUpTriggerCounter -= 1;

                var powerGauge = ((powerUpTriggerCounter / 100) + 1) * GetGameObjectScale();

                Player.SetPowerGauge(powerGauge);

                if (powerUpTriggerCounter <= 0)
                {
                    PowerUpTriggered = false;
                    LaserSpawnLimit += 1;

                    App.PlaySound(baseUrl, SoundType.POWER_DOWN);
                    Player.TriggerPowerDown();
                }
            }
        }

        #endregion

        #region Star Methods

        /// <summary>
        /// Spawns random stars in the star view.
        /// </summary>
        private void SpawnStar()
        {
            // each frame progress decreases this counter
            starCounter -= 1;

            // when counter reaches zero, create an star
            if (starCounter < 0)
            {
                GenerateStar();
                starCounter = StarSpawnLimit;
            }
        }

        /// <summary>
        /// Generates a random star.
        /// </summary>
        private void GenerateStar()
        {
            NewStar = new Star();

            NewStar.SetAttributes(speed: StarSpeed, scale: GetGameObjectScale());

            var top = 0 - NewStar.Height;
            var left = random.Next(10, (int)windowWidth - 10);

            NewStar.AddToGameEnvironment(top: top, left: left, gameEnvironment: StarView);
        }

        /// <summary>
        /// Updates stars on the star canvas.
        /// </summary>
        private void UpdateStarView()
        {
            var starObjects = StarView.GetGameObjects<GameObject>();

            foreach (var star in starObjects)
            {
                UpdateStarViewObject(star);
            }

            StarView.RemoveDestroyableGameObjects();
        }

        /// <summary>
        /// Updates the star objects. Moves the stars.
        /// </summary>
        /// <param name="star"></param>
        private void UpdateStarViewObject(GameObject star)
        {
            // move star down
            star.MoveY(StarSpeed);

            if (star.GetY() > windowHeight)
                StarView.AddDestroyableGameObject(star);
        }

        #endregion

        #region Input Events

        private void InputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var currentPoint = e.GetCurrentPoint(GameView);

            PointerX = currentPoint.Position.X;

            FireLasers = true;
        }

        private void InputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FireLasers = false;
        }

        private void InputView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left: { MoveLeft = true; MoveRight = false; } break;
                case Windows.System.VirtualKey.Right: { MoveRight = true; MoveLeft = false; } break;
                case Windows.System.VirtualKey.Up: { FireLasers = true; } break;
                default:
                    break;
            }
        }

        private void InputView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left: { MoveLeft = false; MoveRight = false; } break;
                case Windows.System.VirtualKey.Right: { MoveRight = false; MoveLeft = false; } break;
                case Windows.System.VirtualKey.Up: { FireLasers = false; } break;
                default:
                    break;
            }
        }

        private void LeftInputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FireLasers = true;
            MoveLeft = true;
            MoveRight = false;
        }

        private void LeftInputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FireLasers = true;
            MoveLeft = false;
            MoveRight = false;
        }

        private void RightInputView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FireLasers = true;
            MoveRight = true;
            MoveLeft = false;
        }

        private void RightInputView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FireLasers = true;
            MoveRight = false;
            MoveLeft = false;
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
            var indexUrl = Uno.Foundation.WebAssemblyRuntime.InvokeJS("window.location.href;");
            var appPackage = Environment.GetEnvironmentVariable("UNO_BOOTSTRAP_APP_BASE");

            this.SizeChanged += GamePage_SizeChanged;
            baseUrl = $"{indexUrl}{appPackage}";

            Console.WriteLine(baseUrl);

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

                var scale = GetGameObjectScale();

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
            this.SizeChanged -= GamePage_SizeChanged;
            StopGame();
        }

        #endregion   

        #endregion
    }
}
