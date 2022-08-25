using System;
using System.Linq;

namespace AstroOdyssey
{
    public class StarHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private int starSpawnCounter;
        private int starSpawnDelay = 250;
        private double starSpeed = 0.1d;

        private int spaceWarpCounter;
        private int spaceWarpDelay = 50;        

        #endregion

        #region Ctor

        public StarHelper(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
        }

        #endregion       

        #region Methods

        public void WarpThroughSpace()
        {
            starSpawnDelay -= 249;
            starSpeed += 35d;
            starSpawnCounter = starSpawnDelay;

            spaceWarpCounter = spaceWarpDelay;
            gameEnvironment.IsWarpingThroughSpace = true;
        }

        public void StopSpaceWarp()
        {
            starSpawnDelay += 249;
            starSpeed -= 35d;
            starSpawnCounter = starSpawnDelay;

            gameEnvironment.IsWarpingThroughSpace = false;
        }

        /// <summary>
        /// Spawns random stars in the star view.
        /// </summary>
        public void SpawnStar()
        {
            if (gameEnvironment.IsWarpingThroughSpace)
            {
                spaceWarpCounter--;

                if (spaceWarpCounter <= 0)
                {
                    StopSpaceWarp();
                }
            }

            // each frame progress decreases this counter
            starSpawnCounter -= 1;

            // when counter reaches zero, create an star
            if (starSpawnCounter < 0)
            {
                GenerateStar();
                starSpawnCounter = starSpawnDelay;
            }
        }

        /// <summary>
        /// Generates a random star.
        /// </summary>
        public void GenerateStar()
        {
            var star = new Star();

            star.SetAttributes(speed: starSpeed, scale: gameEnvironment.GetGameObjectScale());

            var top = 0 - star.Height;
            var left = random.Next(10, (int)gameEnvironment.Width - 10);

            star.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);
        }

        /// <summary>
        /// Updates the star objects. Moves the stars.
        /// </summary>
        /// <param name="star"></param>
        public void UpdateStar(Star star, out bool destroyed)
        {
            destroyed = false;

            // move star down
            star.MoveY(starSpeed);

            if (star.GetY() > gameEnvironment.Height)
            {
                gameEnvironment.AddDestroyableGameObject(star);
                destroyed = true;
            }
        }

        /// <summary>
        /// Levels up stars.
        /// </summary>
        public void LevelUp()
        {
            starSpawnDelay -= 10;
            starSpeed += 0.1d;
        }

        #endregion
    }
}
