using System;
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
        private int xFlyingEnemySpawnLimit = 10;

        private int overPoweredEnemySpawnCounter = 15;
        private int overPoweredEnemySpawnLimit = 15;

        private int evadingEnemySpawnCounter = 3;
        private int evadingEnemySpawnLimit = 3;

        private int firingEnemySpawnCounter = 7;
        private int firingEnemySpawnLimit = 7;

        private int enemyCounter;
        private int enemySpawnLimit = 48;
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
        /// Spawns an enemy.
        /// </summary>
        public void SpawnEnemy(GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            enemyCounter -= 1;

            // when counter reaches zero, create an enemy
            if (enemyCounter < 0)
            {
                GenerateEnemy(gameLevel);
                enemyCounter = enemySpawnLimit;
            }
        }

        /// <summary>
        /// Generates a random Enemy.
        /// </summary>
        public void GenerateEnemy(GameLevel gameLevel)
        {
            var newEnemy = new Enemy();

            newEnemy.SetAttributes(speed: enemySpeed + random.Next(0, 4), scale: gameEnvironment.GetGameObjectScale());

            double left = 0;
            double top = 0;

            // spawn blaster shooting enemies after level 2
            if (gameLevel > GameLevel.Level_2 && firingEnemySpawnCounter <= 0)
            {
                firingEnemySpawnCounter = firingEnemySpawnLimit;
                newEnemy.FiresProjectiles = true;
                firingEnemySpawnLimit = random.Next(1, 8);
            }

            // spawn evading enemies after level 3
            if (gameLevel > GameLevel.Level_3 && evadingEnemySpawnCounter <= 0)
            {
                evadingEnemySpawnCounter = evadingEnemySpawnLimit;
                newEnemy.EvadesOnHit = true;
                evadingEnemySpawnLimit = random.Next(1, 4);
            }

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > GameLevel.Level_3 && overPoweredEnemySpawnCounter <= 0)
            {
                overPoweredEnemySpawnCounter = overPoweredEnemySpawnLimit;
                newEnemy.OverPower();
                overPoweredEnemySpawnLimit = random.Next(10, 20);
            }

            // generate side ways flying enemies after level 2
            if (gameLevel > GameLevel.Level_2 && xFlyingEnemySpawnCounter <= 0)
            {
                newEnemy.XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                xFlyingEnemySpawnCounter = xFlyingEnemySpawnLimit;

                newEnemy.YDirection = (YDirection)random.Next(0, Enum.GetNames<YDirection>().Length);

                // enemies can not go up
                if (newEnemy.YDirection == YDirection.UP)
                    newEnemy.YDirection = YDirection.DOWN;

                switch (newEnemy.XDirection)
                {
                    case XDirection.LEFT:
                        newEnemy.Rotation = newEnemy.YDirection == YDirection.NONE ? 0 : 50;
                        left = gameEnvironment.Width;
                        break;
                    case XDirection.RIGHT:
                        newEnemy.Rotation = newEnemy.YDirection == YDirection.NONE ? 0 : -50;
                        left = 0 - newEnemy.Width + 10;
                        break;
                    default:
                        break;
                }
#if DEBUG
                Console.WriteLine("Enemy XDirection: " + newEnemy.XDirection + ", " + "X: " + left + " " + "Y: " + top);
#endif
                top = random.Next(0, (int)gameEnvironment.Height / 3);
                newEnemy.Rotate();

                // randomize next x flying enemy pop up
                xFlyingEnemySpawnLimit = random.Next(5, 15);
            }
            else
            {
                left = random.Next(10, (int)gameEnvironment.Width - 70);
                top = 0 - newEnemy.Height;
            }

            newEnemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            xFlyingEnemySpawnCounter--;
            overPoweredEnemySpawnCounter--;
            evadingEnemySpawnCounter--;
            firingEnemySpawnCounter--;
        }

        /// <summary>
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="enemy"></param>
        public void DestroyEnemy(Enemy enemy)
        {
            enemy.IsMarkedForFadedRemoval = true;
            App.PlaySound(baseUrl, SoundType.ENEMY_DESTRUCTION);
        }

        /// <summary>
        /// Updates an enemy. Moves the enemy inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="destroyed"></param>
        public void UpdateEnemy(Enemy enemy, out bool destroyed)
        {
            //TODO: take player coordinates and follow the the player position

            destroyed = false;

            // move enemy down
            enemy.MoveY();
            enemy.MoveX();

            // if the object is marked for lazy destruction then no need to perform collisions
            if (enemy.IsMarkedForFadedRemoval)
                return;

            // if object has gone beyond game view
            destroyed = gameEnvironment.CheckAndAddDestroyableGameObject(enemy);
        }

        /// <summary>
        /// Levels up enemies.
        /// </summary>
        public void LevelUp()
        {
            enemySpawnLimit -= 3;
            enemySpeed += 1;
        }

        #endregion
    }
}
