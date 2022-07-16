using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class EnemyHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private readonly string baseUrl;

        private int rotatedEnemySpawnCounter = 10;
        private int rotatedEnemySpawnLimit = 10;

        private int enemyCounter;
        private int enemySpawnLimit = 50;
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
        /// Levels up enemies.
        /// </summary>
        public void LevelUp()
        {
            enemySpawnLimit -= 2;
            enemySpeed += 1;
        }

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

            if (gameLevel > GameLevel.Level_1 && rotatedEnemySpawnCounter <= 0)
            {
                newEnemy.XDirection = (XDirection)random.Next(1, 3);
                rotatedEnemySpawnCounter = rotatedEnemySpawnLimit;

                newEnemy.YDirection = (YDirection)random.Next(0, 3);

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
                rotatedEnemySpawnLimit = random.Next(5, 15);
            }
            else
            {
                left = random.Next(10, (int)gameEnvironment.Width - 70);
                top = 0 - newEnemy.Height;
            }

            newEnemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            rotatedEnemySpawnCounter -= 1;
        }

        /// <summary>
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="enemy"></param>
        public void DestroyEnemy(Enemy enemy)
        {
            enemy.MarkedForFadedRemoval = true;
            App.PlaySound(baseUrl, SoundType.ENEMY_DESTRUCTION);
        }

        /// <summary>
        /// Updates an enemy. Moves the enemy inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="destroyed"></param>
        public void UpdateEnemy(Enemy enemy, out bool destroyed)
        {
            destroyed = false;

            // move enemy down
            enemy.MoveY();
            enemy.MoveX();

            // if the object is marked for lazy destruction then no need to perform collisions
            if (enemy.MarkedForFadedRemoval)
                return;

            // if enemy or meteor object has gone beyond game view
            destroyed = gameEnvironment.CheckAndAddDestroyableGameObject(enemy);
        }

        #endregion
    }
}
