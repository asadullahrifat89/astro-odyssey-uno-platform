using System;

namespace SpaceShooterGame
{
    public static class CelestialObjectFactory
    {
        #region Fields

        private static GameEnvironment _starView;
        private static GameEnvironment _planetView;

        private static readonly Random _random = new();

        private static double _starSpeed = 0.2d;

        private static int _spaceWarpDurationCounter;
        private static int _spaceWarpDurationAfter = 100;

        private static int _planetSpawnCounter;
        private static int _planetSpawnAfter = 2500;

        private static readonly double SPACE_WARP_STAR_SPEED_INCREASE = 60;
        private static readonly int SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE = 1998;

        private static double _lastStarSpeed;

        #endregion       

        #region Methods

        #region Public

        public static void Reset()
        {
            _starSpeed = 0.2d;
            _spaceWarpDurationAfter = 100;
            _planetSpawnAfter = 2500;
        }

        public static void SetGameEnvironments(
            GameEnvironment starView,
            GameEnvironment planetView)
        {
            _starView = starView;
            _planetView = planetView;
        }

        /// <summary>
        /// Spawns random stars, planets.
        /// </summary>
        public static void SpawnCelestialObject()
        {
            if (_starView.IsWarpingThroughSpace)
            {
                _spaceWarpDurationCounter--;

                if (_spaceWarpDurationCounter <= 0)                
                    StopSpaceWarp();                

                // slowing down effect
                if (_spaceWarpDurationCounter <= 50)
                {
                    if (_starSpeed > _lastStarSpeed + 4)
                        _starSpeed--;
                }
            }

            // star view can not have more than 20 stars 
            if (_starView.Children.Count < 20)            
                GenerateStar();            

            _planetSpawnCounter--;

            if (_planetSpawnCounter <= 0)
            {
                GeneratePlanet();

                _planetSpawnAfter = _random.Next(2500, 3500);
                _planetSpawnCounter = _planetSpawnAfter;
            }
        }

        /// <summary>
        /// Generates a random star.
        /// </summary>
        public static void GenerateStar()
        {
            var star = new CelestialObject();

            star.SetAttributes(
                scale: _starView.GameObjectScale,
                celestialObjectType: CelestialObjectType.Star);

            var top = _random.Next(100, (int)_starView.Height) * -1;
            var left = _random.Next(10, (int)_starView.Width - 10);

            star.AddToGameEnvironment(top: top, left: left, gameEnvironment: _starView);
        }

        /// <summary>
        /// Generates a random planet.
        /// </summary>
        public static void GeneratePlanet()
        {
            var planet = new CelestialObject();

            planet.SetAttributes(
                scale: _planetView.GameObjectScale,
                celestialObjectType: CelestialObjectType.Planet);

            var top = 0 - planet.Height;
            var left = _random.Next(10, (int)_planetView.Width - 100);

            planet.AddToGameEnvironment(top: top, left: left, gameEnvironment: _planetView);
        }

        /// <summary>
        /// Updates the celestial objects. Moves the stars, planets.
        /// </summary>
        /// <param name="celestialObject"></param>
        public static bool UpdateCelestialObject(CelestialObject celestialObject)
        {
            bool destroyed = false;

            switch (celestialObject.CelestialObjectType)
            {
                case CelestialObjectType.Star:
                    {
                        // move star down
                        celestialObject.MoveY(_starSpeed);

                        if (celestialObject.GetY() > _starView.Height)
                        {
                            // send the start on top again
                            RecycleStar(celestialObject);
                            destroyed = true;
                        }
                    }
                    break;
                case CelestialObjectType.Planet:
                    {
                        // move planet down
                        celestialObject.MoveY(_starSpeed * 2);

                        if (celestialObject.GetY() > _planetView.Height)
                        {
                            _planetView.AddDestroyableGameObject(celestialObject);
                            destroyed = true;
                        }
                    }
                    break;
                default:
                    break;
            }

            return destroyed;
        }

        /// <summary>
        /// Starts the space warp effect.
        /// </summary>
        public static void StartSpaceWarp()
        {
            _lastStarSpeed = _starSpeed;
            _starSpeed += SPACE_WARP_STAR_SPEED_INCREASE;

            _planetSpawnAfter -= SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE;
            _planetSpawnCounter = _planetSpawnAfter;

            _spaceWarpDurationCounter = _spaceWarpDurationAfter;

            _starView.IsWarpingThroughSpace = true;
            _planetView.IsWarpingThroughSpace = true;
        }

        /// <summary>
        /// Stops the space warp effect.
        /// </summary>
        public static void StopSpaceWarp()
        {
            _starSpeed = _lastStarSpeed;
            _planetSpawnAfter += SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE;
            _planetSpawnCounter = _planetSpawnAfter;

            _starView.IsWarpingThroughSpace = false;
            _planetView.IsWarpingThroughSpace = false;

            // generate a planet after each warp
            GeneratePlanet();
        }

        /// <summary>
        /// Levels up stars.
        /// </summary>
        public static void LevelUp()
        {
            var scale = _starView.GameObjectScale;
            _starSpeed += (0.1d * scale);
        }

        #endregion

        #region Private

        private static void RecycleStar(CelestialObject celestialObject)
        {
            var top = _random.Next(100, (int)_starView.Height) * -1;
            var left = _random.Next(10, (int)_starView.Width - 10);

            celestialObject.SetPosition(top, left);
        }

        #endregion

        #endregion
    }
}
