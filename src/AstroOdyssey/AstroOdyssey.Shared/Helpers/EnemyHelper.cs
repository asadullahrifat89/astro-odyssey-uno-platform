using System;
using System.Linq;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class EnemyHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;
        private readonly string baseUrl;

        private readonly Random random = new Random();

        private int xFlyingEnemySpawnCounter = 10;
        private int xFlyingEnemySpawnFrequency = 10;

        private int overPoweredEnemySpawnCounter = 15;
        private int overPoweredEnemySpawnFrequency = 15;

        private int evadingEnemySpawnCounter = 3;
        private int evadingEnemySpawnFrequency = 3;

        private int firingEnemySpawnCounter = 7;
        private int firingEnemySpawnFrequency = 7;

        private int targetingEnemySpawnFrequency = 10;
        private int targetingEnemySpawnCounter = 10;

        private int hoveringEnemySpawnFrequency = 12;
        private int hoveringEnemySpawnCounter = 12;

        private int enemySpawnCounter;
        private int enemySpawnFrequency = 48;
        private double enemySpeed = 2;

        #endregion

        #region Ctor

        public EnemyHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Engages a boss.
        /// </summary>
        /// <param name="gameLevel"></param>
        public Enemy EngageBossEnemy(GameLevel gameLevel)
        {
            //TODO: bosses must have health bars

            var enemy = new Enemy();

            enemy.SetAttributes(speed: enemySpeed + random.Next(0, 4), scale: gameEnvironment.GetGameObjectScale());

            enemy.IsBoss = true;
            enemy.IsOverPowered = true;

            enemy.Height = enemy.Height * 2 + (int)gameLevel / 3 + 0.25d;
            enemy.Width = enemy.Width * 2 + (int)gameLevel / 3 + 0.25d;
            enemy.Speed--;

            enemy.Health = 50 * (int)gameLevel;

            SetProjectileFiringEnemy(enemy);
            SetHoveringEnemy(enemy);

            // appear in the middle of the screen
            double left = gameEnvironment.HalfWidth + enemy.HalfWidth;
            double top = 0 - enemy.Height;

            if (enemy.IsHovering)
                enemy.XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);

            enemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            gameEnvironment.IsBossEngaged = true;

            App.StopSound();
            App.PlaySound(baseUrl, SoundType.BOSS_APPEARANCE);

            return enemy;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        public void DisengageBossEnemy()
        {
            gameEnvironment.IsBossEngaged = false;
            App.StopSound();
            App.PlaySound(baseUrl, SoundType.BOSS_DESTRUCTION);
            App.PlaySound(baseUrl, SoundType.BACKGROUND_MUSIC);
        }

        /// <summary>
        /// Spawns an enemy.
        /// </summary>
        public void SpawnEnemy(GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            enemySpawnCounter -= 1;

            // when counter reaches zero, create an enemy
            if (enemySpawnCounter < 0)
            {
                GenerateEnemy(gameLevel);
                enemySpawnCounter = enemySpawnFrequency;
            }
        }

        /// <summary>
        /// Generates a random Enemy.
        /// </summary>
        public void GenerateEnemy(GameLevel gameLevel)
        {
            var enemy = new Enemy();

            enemy.SetAttributes(speed: enemySpeed + random.Next(0, 4), scale: gameEnvironment.GetGameObjectScale());

            double left = 0;
            double top = 0;

            xFlyingEnemySpawnCounter--;
            overPoweredEnemySpawnCounter--;
            evadingEnemySpawnCounter--;
            firingEnemySpawnCounter--;
            targetingEnemySpawnCounter--;
            hoveringEnemySpawnCounter--;

            // spawn evading enemies after level 3
            if (gameLevel > GameLevel.Level_3 && evadingEnemySpawnCounter <= 0)
            {
                SetEvadingEnemy(enemy);
            }

            // spawn following enemies after level 3
            if (gameLevel > GameLevel.Level_3 && targetingEnemySpawnCounter <= 0)
            {
                SetPlayerTargetingEnemy(enemy);
            }

            // spawn following enemies after level 3
            if (gameLevel > GameLevel.Level_3 && hoveringEnemySpawnCounter <= 0)
            {
                SetHoveringEnemy(enemy);
            }

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > GameLevel.Level_3 && overPoweredEnemySpawnCounter <= 0)
            {
                SetOverPoweredEnemy(enemy);
            }

            // spawn blaster shooting enemies after level 2
            if (gameLevel > GameLevel.Level_2 && firingEnemySpawnCounter <= 0)
            {
                SetProjectileFiringEnemy(enemy);
            }

            // generate side ways flying enemies after level 2
            if (gameLevel > GameLevel.Level_2 && xFlyingEnemySpawnCounter <= 0)
            {
                SetSideWaysMovingEnemy(enemy, ref left, ref top);
            }
            else
            {
                if (enemy.IsHovering)
                {
                    // appear in the middle of the screen
                    left = gameEnvironment.HalfWidth + enemy.HalfWidth;
                    enemy.XDirection = XDirection.LEFT;
                }
                else
                {
                    left = random.Next(10, (int)gameEnvironment.Width - (int)enemy.Width);
                }

                top = 0 - enemy.Height;
            }

            enemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);
        }

        /// <summary>
        /// Generates an enemy that hovers across the screen.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetHoveringEnemy(Enemy enemy)
        {
            hoveringEnemySpawnCounter = hoveringEnemySpawnFrequency;
            enemy.IsHovering = true;
            hoveringEnemySpawnFrequency = random.Next(9, 13);

            // hovering enemies do not evade or target player
            enemy.IsEvading = false;
            enemy.IsPlayerTargeting = false;
        }

        /// <summary>
        /// Generates an enemy that evades player fire.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetEvadingEnemy(Enemy enemy)
        {
            evadingEnemySpawnCounter = evadingEnemySpawnFrequency;
            enemy.IsEvading = true;
            evadingEnemySpawnFrequency = random.Next(1, 4);
        }

        /// <summary>
        /// Generates an enemy that targets player.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetPlayerTargetingEnemy(Enemy enemy)
        {
            targetingEnemySpawnCounter = targetingEnemySpawnFrequency;
            enemy.IsPlayerTargeting = true;
            targetingEnemySpawnFrequency = random.Next(7, 12);

            // player targeting enemies do not evade on hit
            enemy.IsEvading = false;
        }

        /// <summary>
        /// Generates an enemy that fires projectiles.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetProjectileFiringEnemy(Enemy enemy)
        {
            firingEnemySpawnCounter = firingEnemySpawnFrequency;
            enemy.IsProjectileFiring = true;
            firingEnemySpawnFrequency = random.Next(1, 8);
        }

        /// <summary>
        /// Generates an enemy that is overpowered. Bigger and stronger but slower enemies.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetOverPoweredEnemy(Enemy enemy)
        {
            overPoweredEnemySpawnCounter = overPoweredEnemySpawnFrequency;
            enemy.OverPower();
            overPoweredEnemySpawnFrequency = random.Next(10, 20);
        }

        /// <summary>
        /// Generates an enemy that moves sideways from one direction.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetSideWaysMovingEnemy(Enemy enemy, ref double left, ref double top)
        {
            enemy.XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
            xFlyingEnemySpawnCounter = xFlyingEnemySpawnFrequency;

            enemy.YDirection = (YDirection)random.Next(0, Enum.GetNames<YDirection>().Length);

            // enemies can not go up
            if (enemy.YDirection == YDirection.UP)
                enemy.YDirection = YDirection.DOWN;

            switch (enemy.XDirection)
            {
                case XDirection.LEFT:
                    enemy.Rotation = enemy.YDirection == YDirection.NONE ? 0 : 50;
                    left = gameEnvironment.Width;
                    break;
                case XDirection.RIGHT:
                    enemy.Rotation = enemy.YDirection == YDirection.NONE ? 0 : -50;
                    left = 0 - enemy.Width + 10;
                    break;
                default:
                    break;
            }
//#if DEBUG
//            Console.WriteLine("Enemy XDirection: " + enemy.XDirection + ", " + "X: " + left + " " + "Y: " + top);
//#endif
            top = random.Next(0, (int)gameEnvironment.Height / 3);
            enemy.Rotate();

            // sideways flying enemies do not follow player or evade on hit
            enemy.IsPlayerTargeting = false;
            enemy.IsEvading = false;
            enemy.IsHovering = false;

            // randomize next x flying enemy pop up
            xFlyingEnemySpawnFrequency = random.Next(5, 15);
        }

        /// <summary>
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="enemy"></param>
        public void DestroyEnemy(Enemy enemy)
        {
            enemy.IsMarkedForFadedDestruction = true;
            App.PlaySound(baseUrl, SoundType.ENEMY_DESTRUCTION);
        }

        /// <summary>
        /// Updates an enemy. Moves the enemy inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="destroyed"></param>
        public void UpdateEnemy(Enemy enemy, double pointerX, out bool destroyed)
        {
            destroyed = false;

            if (enemy.IsBoss)
            {
                // move boss down upto a certain point
                if (enemy.GetY() + enemy.HalfWidth <= gameEnvironment.Height / 4)
                {
                    enemy.MoveY();
                }
            }
            else
            {
                // move enemy down
                enemy.MoveY();
            }

            if (enemy.IsPlayerTargeting)
            {
                var enemyX = enemy.GetX();

                var enemyWidthHalf = enemy.HalfWidth;

                // move right
                if (pointerX - enemyWidthHalf > enemyX + enemy.Speed)
                {
                    if (enemyX + enemyWidthHalf < gameEnvironment.Width)
                    {
                        SetEnemyX(enemy: enemy, left: enemyX + enemy.Speed);
                    }
                }

                // move left
                if (pointerX - enemyWidthHalf < enemyX - enemy.Speed)
                {
                    SetEnemyX(enemy: enemy, left: enemyX - enemy.Speed);
                }
            }
            else
            {
                enemy.MoveX();

                if (enemy.IsHovering)
                {
                    // change direction of x axis movement
                    if (enemy.GetX() + enemy.HalfWidth >= gameEnvironment.Width - 50 || enemy.GetX() + enemy.HalfWidth <= 50)
                        enemy.XDirection = enemy.XDirection == XDirection.LEFT ? XDirection.RIGHT : XDirection.LEFT;
                }
            }

            // if the object is marked for lazy destruction then do not destroy immidiately
            if (enemy.IsMarkedForFadedDestruction)
                return;

            // if object has gone beyond game view
            destroyed = gameEnvironment.CheckAndAddDestroyableGameObject(enemy);
        }

        /// <summary>
        /// Sets X axis position of an enemy.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="left"></param>
        private void SetEnemyX(Enemy enemy, double left)
        {
            enemy.SetX(left);
        }

        /// <summary>
        /// Levels up enemies.
        /// </summary>
        public void LevelUp()
        {
            //enemySpawnFrequency -= 1;
            enemySpeed += 1;
        }

        #endregion
    }
}
