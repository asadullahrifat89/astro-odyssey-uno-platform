using System;

namespace AstroOdyssey
{
    public class StarHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private int starCounter;
        private int starSpawnLimit = 100;

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
        public void SpawnStar(double starSpeed)
        {
            // each frame progress decreases this counter
            starCounter -= 1;

            // when counter reaches zero, create an star
            if (starCounter < 0)
            {
                GenerateStar(starSpeed);
                starCounter = starSpawnLimit;
            }
        }

        /// <summary>
        /// Generates a random star.
        /// </summary>
        public void GenerateStar(double starSpeed)
        {
            var newStar = new Star();

            newStar.SetAttributes(speed: starSpeed, scale: gameEnvironment.GetGameObjectScale());

            var top = 0 - newStar.Height;
            var left = random.Next(10, (int)gameEnvironment.Width - 10);

            newStar.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);
        }

        /// <summary>
        /// Updates the star objects. Moves the stars.
        /// </summary>
        /// <param name="star"></param>
        public void UpdateStar(double starSpeed, Star star)
        {
            // move star down
            star.MoveY(starSpeed);

            if (star.GetY() > gameEnvironment.Height)
                gameEnvironment.AddDestroyableGameObject(star);
        }

        #endregion
    }
}
