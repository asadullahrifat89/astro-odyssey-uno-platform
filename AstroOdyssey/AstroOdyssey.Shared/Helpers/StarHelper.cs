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
        private readonly int starSpawnFrequency = 220;
        private double starSpeed = 0.1d;

        #endregion

        #region Ctor

        public StarHelper(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns random stars in the star view.
        /// </summary>
        public void SpawnStar()
        {
            // each frame progress decreases this counter
            starSpawnCounter -= 1;

            // when counter reaches zero, create an star
            if (starSpawnCounter < 0)
            {
                GenerateStar();
                starSpawnCounter = starSpawnFrequency;
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
            starSpeed += 0.1d;
        }

        #endregion
    }
}
