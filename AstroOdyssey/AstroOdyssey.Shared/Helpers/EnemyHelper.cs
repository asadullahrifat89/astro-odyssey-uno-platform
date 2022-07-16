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
        /// Generates a random Enemy.
        /// </summary>
        public void GenerateEnemy(double enemySpeed, GameLevel gameLevel)
        {
            var newEnemy = new Enemy();

            newEnemy.SetAttributes(speed: enemySpeed + random.Next(0, 4), scale: gameEnvironment.GetGameObjectScale());

            var left = 0;
            var top = 0;

            // when not noob anymore enemy moves sideways
            if ((int)gameLevel > 0 && rotatedEnemySpawnCounter <= 0)
            {
                newEnemy.XDirection = (XDirection)random.Next(1, 3);
                rotatedEnemySpawnCounter = rotatedEnemySpawnLimit;

                switch (newEnemy.XDirection)
                {
                    case XDirection.LEFT:
                        newEnemy.Rotation = 50;
                        left = (int)gameEnvironment.Width;
                        break;
                    case XDirection.RIGHT:
                        left = 0 - (int)newEnemy.Width + 10;
                        newEnemy.Rotation = -50;
                        break;
                    default:
                        break;
                }
#if DEBUG
                Console.WriteLine("Enemy XDirection: " + newEnemy.XDirection + ", " + "X: " + left + " " + "Y: " + top);
#endif
                top = random.Next(0, (int)gameEnvironment.Height / 3);
                newEnemy.Rotate();

                rotatedEnemySpawnLimit = random.Next(5, 15);
            }
            else
            {
                left = random.Next(10, (int)gameEnvironment.Width - 70);
                top = 0 - (int)newEnemy.Height;
            }

            newEnemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            rotatedEnemySpawnCounter -= 1;
        }

        /// <summary>
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="Enemy"></param>
        public void DestroyEnemy(Enemy Enemy)
        {
            Enemy.MarkedForFadedRemoval = true;
            App.PlaySound(baseUrl, SoundType.ENEMY_DESTRUCTION);
        }

        #endregion
    }
}
