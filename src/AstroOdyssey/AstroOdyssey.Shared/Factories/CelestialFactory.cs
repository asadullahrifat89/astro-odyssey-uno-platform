using System;

namespace AstroOdyssey
{
    public class CelestialFactory
    {
        #region Fields

        private readonly GameEnvironment starView;
        private readonly GameEnvironment planetView;

        private readonly Random random = new Random();

        private int starSpawnCounter;
        private int starSpawnDelay = 250;
        private double starSpeed = 0.1d;

        private int spaceWarpCounter;
        private int spaceWarpDelay = 100;

        private int planetSpawnCounter = 3000;
        private int planetSpawnDelay = 3000;

        #endregion

        #region Ctor

        public CelestialFactory(GameEnvironment starView, GameEnvironment planetView)
        {
            this.starView = starView;
            this.planetView = planetView;
        }

        #endregion       

        #region Methods

        public void StartSpaceWarp()
        {
            starSpawnDelay -= 249;
            starSpeed += 35d;
            starSpawnCounter = starSpawnDelay;

            planetSpawnDelay -= 1000;
            planetSpawnCounter = planetSpawnDelay;

            spaceWarpCounter = spaceWarpDelay;

            starView.IsWarpingThroughSpace = true;
            planetView.IsWarpingThroughSpace = true;
        }

        public void StopSpaceWarp()
        {
            starSpawnDelay += 249;
            starSpeed -= 35d;
            starSpawnCounter = starSpawnDelay;

            planetSpawnDelay = random.Next(3000, 3500);
            planetSpawnCounter = planetSpawnDelay;

            starView.IsWarpingThroughSpace = false;
            planetView.IsWarpingThroughSpace = false;
        }

        /// <summary>
        /// Spawns random stars, planets.
        /// </summary>
        public void SpawnCelestialObject()
        {
            if (starView.IsWarpingThroughSpace)
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
            }

            planetSpawnCounter--;

            if (planetSpawnCounter <= 0)
            {
                GeneratePlanet();
               
                planetSpawnDelay = random.Next(3000, 3500);
                planetSpawnCounter = planetSpawnDelay;
            }
        }

        /// <summary>
        /// Generates a random star.
        /// </summary>
        public void GenerateStar()
        {
            var star = new CelestialObject();

            star.SetAttributes(
                speed: starSpeed,
                scale: starView.GetGameObjectScale(),
                celestialObjectType: CelestialObjectType.Star);

            var top = 0 - star.Height;
            var left = random.Next(10, (int)starView.Width - 10);

            star.AddToGameEnvironment(top: top, left: left, gameEnvironment: starView);
        }

        /// <summary>
        /// Generates a random planet.
        /// </summary>
        public void GeneratePlanet()
        {
            var planet = new CelestialObject();

            planet.SetAttributes(
                speed: starSpeed,
                scale: planetView.GetGameObjectScale(),
                celestialObjectType: CelestialObjectType.Planet);

            var top = 0 - planet.Height;
            var left = random.Next(10, (int)planetView.Width - 50);

            planet.AddToGameEnvironment(top: top, left: left, gameEnvironment: planetView);
        }

        /// <summary>
        /// Updates the celestial objects. Moves the stars, planets.
        /// </summary>
        /// <param name="celestialObject"></param>
        public void UpdateCelestialObject(CelestialObject celestialObject, out bool destroyed)
        {
            destroyed = false;

            switch (celestialObject.CelestialObjectType)
            {
                case CelestialObjectType.Star:
                    {
                        // move star down
                        celestialObject.MoveY(starSpeed);

                        if (celestialObject.GetY() > starView.Height)
                        {
                            starView.AddDestroyableGameObject(celestialObject);
                            destroyed = true;
                        }
                    }
                    break;
                case CelestialObjectType.Planet:
                    {
                        // move star down
                        celestialObject.MoveY(starSpeed);

                        if (celestialObject.GetY() > planetView.Height)
                        {
                            planetView.AddDestroyableGameObject(celestialObject);
                            destroyed = true;
                        }
                    }
                    break;
                default:
                    break;
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
