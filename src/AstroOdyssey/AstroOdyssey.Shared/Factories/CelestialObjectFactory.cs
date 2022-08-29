using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace AstroOdyssey
{
    public class CelestialObjectFactory
    {
        #region Fields

        private readonly GameEnvironment starView;
        private readonly GameEnvironment planetView;

        private readonly Random random = new Random();

        private int starSpawnCounter;
        private int starSpawnDelay = 250;
        private double starSpeed = 0.2d;

        private int spaceWarpDurationCounter;
        private int spaceWarpDurationDelay = 100;

        private int planetSpawnCounter;
        private int planetSpawnDelay = 2000;

        private readonly double SPACE_WARP_STAR_SPEED_INCREASE = 60;
        private readonly int SPACE_WARP_STAR_SPAWN_DELAY_DECREASE = 249;
        private readonly int SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE = 1998;

        private double lastStarSpeed;

        #endregion

        #region Ctor

        public CelestialObjectFactory(GameEnvironment starView, GameEnvironment planetView)
        {
            this.starView = starView;
            this.planetView = planetView;
        }

        #endregion       

        #region Methods

        /// <summary>
        /// Starts the space warp effect.
        /// </summary>
        public void StartSpaceWarp()
        {
            lastStarSpeed = starSpeed;

            starSpawnDelay -= SPACE_WARP_STAR_SPAWN_DELAY_DECREASE;

            starSpeed += SPACE_WARP_STAR_SPEED_INCREASE;

            starSpawnCounter = starSpawnDelay;

            planetSpawnDelay -= SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE;
            planetSpawnCounter = planetSpawnDelay;

            spaceWarpDurationCounter = spaceWarpDurationDelay;

            starView.IsWarpingThroughSpace = true;
            planetView.IsWarpingThroughSpace = true;

            //starView.Background = new SolidColorBrush(Colors.DeepPink);
        }

        /// <summary>
        /// Stops the space warp effect.
        /// </summary>
        public void StopSpaceWarp()
        {
            starSpeed = lastStarSpeed;
            starSpawnDelay += SPACE_WARP_STAR_SPAWN_DELAY_DECREASE;
            starSpawnCounter = starSpawnDelay;

            planetSpawnDelay += SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE;
            planetSpawnCounter = planetSpawnDelay;

            starView.IsWarpingThroughSpace = false;
            planetView.IsWarpingThroughSpace = false;

            // generate a planet after each warp
            GeneratePlanet();

            //starView.Background = new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        /// Spawns random stars, planets.
        /// </summary>
        public void SpawnCelestialObject()
        {
            if (starView.IsWarpingThroughSpace)
            {
                spaceWarpDurationCounter--;

                if (spaceWarpDurationCounter <= 0)
                {
                    StopSpaceWarp();
                    //return;
                }

                // slowing down effect
                if (spaceWarpDurationCounter <= 50)
                {
                    if (starSpeed > lastStarSpeed + 4)
                        starSpeed--;
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

                planetSpawnDelay = random.Next(2500, 3500);
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
                speed: starSpeed * 2,
                scale: planetView.GetGameObjectScale(),
                celestialObjectType: CelestialObjectType.Planet);

            var top = 0 - planet.Height;
            var left = random.Next(10, (int)planetView.Width - 100);

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
                        celestialObject.MoveY(starSpeed * 2);

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
