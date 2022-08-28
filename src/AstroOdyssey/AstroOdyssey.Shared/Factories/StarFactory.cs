using System;

namespace AstroOdyssey
{
    public class StarFactory
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private int starSpawnCounter;
        private int starSpawnDelay = 250;
        private double starSpeed = 0.1d;

        private int spaceWarpCounter;
        private int spaceWarpDelay = 100;

        private int celestialObjectSpawnCounter = 2500;
        private int celestialObjectSpawnDelay = 2500;

        #endregion

        #region Ctor

        public StarFactory(GameEnvironment gameEnvironment)
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

            celestialObjectSpawnDelay -= 1000;
            celestialObjectSpawnCounter = celestialObjectSpawnDelay;

            spaceWarpCounter = spaceWarpDelay;

            gameEnvironment.IsWarpingThroughSpace = true;
        }

        public void StopSpaceWarp()
        {
            starSpawnDelay += 249;
            starSpeed -= 35d;
            starSpawnCounter = starSpawnDelay;

            celestialObjectSpawnDelay = random.Next(1500, 2500);
            celestialObjectSpawnCounter = celestialObjectSpawnDelay;

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
            starSpawnCounter--;

            // when counter reaches zero, create an star
            if (starSpawnCounter < 0)
            {
                GenerateStar();
                starSpawnCounter = starSpawnDelay;

                return;
            }

            celestialObjectSpawnCounter--;

            if (celestialObjectSpawnCounter <= 0)
            {
                GenerateStar(isCelestialObject: true);

                celestialObjectSpawnCounter = celestialObjectSpawnDelay;
                celestialObjectSpawnDelay = random.Next(2500, 3500);
            }
        }

        /// <summary>
        /// Generates a random star.
        /// </summary>
        public void GenerateStar(bool isCelestialObject = false)
        {
            var star = new Star();

            star.SetAttributes(
                speed: starSpeed,
                scale: gameEnvironment.GetGameObjectScale(),
                isCelestialObject: isCelestialObject);

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
