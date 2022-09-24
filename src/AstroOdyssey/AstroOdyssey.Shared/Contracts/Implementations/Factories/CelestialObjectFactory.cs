using System;

namespace AstroOdyssey
{
    public class CelestialObjectFactory : ICelestialObjectFactory
    {
        #region Fields

        private GameEnvironment _starView;
        private GameEnvironment _planetView;

        private readonly Random _random = new Random();

        private double _starSpawnCounter;
        private double _starSpawnAfter = 250;
        private double _starSpeed = 0.2d;

        private int _spaceWarpDurationCounter;
        private int _spaceWarpDurationAfter = 100;

        private int _planetSpawnCounter;
        private int _planetSpawnAfter = 2500;

        private readonly double SPACE_WARP_STAR_SPEED_INCREASE = 60;
        private readonly int SPACE_WARP_STAR_SPAWN_DELAY_DECREASE = 249;
        private readonly int SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE = 1998;

        private double _lastStarSpeed;

        #endregion

        #region Ctor

        public CelestialObjectFactory()
        {

        }

        #endregion

        #region Methods

        #region Public

        public void Reset()
        {
            _starSpawnAfter = 250;
            _starSpeed = 0.2d;
            _spaceWarpDurationAfter = 100;
            _planetSpawnAfter = 2500;
        }

        public void SetGameEnvironments(GameEnvironment starView, GameEnvironment planetView)
        {
            _starView = starView;
            _planetView = planetView;
        }

        /// <summary>
        /// Starts the space warp effect.
        /// </summary>
        public void StartSpaceWarp()
        {
            _lastStarSpeed = _starSpeed;

            _starSpawnAfter -= SPACE_WARP_STAR_SPAWN_DELAY_DECREASE;

            _starSpeed += SPACE_WARP_STAR_SPEED_INCREASE;

            _starSpawnCounter = _starSpawnAfter;

            _planetSpawnAfter -= SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE;
            _planetSpawnCounter = _planetSpawnAfter;

            _spaceWarpDurationCounter = _spaceWarpDurationAfter;

            _starView.IsWarpingThroughSpace = true;
            _planetView.IsWarpingThroughSpace = true;
        }

        /// <summary>
        /// Stops the space warp effect.
        /// </summary>
        public void StopSpaceWarp()
        {
            _starSpeed = _lastStarSpeed;
            _starSpawnAfter += SPACE_WARP_STAR_SPAWN_DELAY_DECREASE;
            _starSpawnCounter = _starSpawnAfter;

            _planetSpawnAfter += SPACE_WARP_PLANET_SPAWN_DELAY_DECREASE;
            _planetSpawnCounter = _planetSpawnAfter;

            _starView.IsWarpingThroughSpace = false;
            _planetView.IsWarpingThroughSpace = false;

            // generate a planet after each warp
            GeneratePlanet();
        }

        /// <summary>
        /// Spawns random stars, planets.
        /// </summary>
        public void SpawnCelestialObject()
        {
            if (_starView.IsWarpingThroughSpace)
            {
                _spaceWarpDurationCounter--;

                if (_spaceWarpDurationCounter <= 0)
                {
                    StopSpaceWarp();
                    //return;
                }

                // slowing down effect
                if (_spaceWarpDurationCounter <= 50)
                {
                    if (_starSpeed > _lastStarSpeed + 4)
                        _starSpeed--;
                }
            }

            // each frame progress decreases this counter
            _starSpawnCounter--;

            // when counter reaches zero, create an star
            if (_starSpawnCounter < 0)
            {
                GenerateStar();
                _starSpawnCounter = _starSpawnAfter;
            }

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
        public void GenerateStar()
        {
            var star = new CelestialObject();

            star.SetAttributes(
                speed: _starSpeed,
                scale: _starView.GetGameObjectScale(),
                celestialObjectType: CelestialObjectType.Star);

            var top = 0 - star.Height;
            var left = _random.Next(10, (int)_starView.Width - 10);

            star.AddToGameEnvironment(top: top, left: left, gameEnvironment: _starView);
        }

        /// <summary>
        /// Generates a random planet.
        /// </summary>
        public void GeneratePlanet()
        {
            var planet = new CelestialObject();

            planet.SetAttributes(
                speed: _starSpeed * 2,
                scale: _planetView.GetGameObjectScale(),
                celestialObjectType: CelestialObjectType.Planet);

            var top = 0 - planet.Height;
            var left = _random.Next(10, (int)_planetView.Width - 100);

            planet.AddToGameEnvironment(top: top, left: left, gameEnvironment: _planetView);
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
                        celestialObject.MoveY(_starSpeed);

                        if (celestialObject.GetY() > _starView.Height)
                        {
                            _starView.AddDestroyableGameObject(celestialObject);
                            destroyed = true;
                        }
                    }
                    break;
                case CelestialObjectType.Planet:
                    {
                        // move star down
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
        }

        /// <summary>
        /// Levels up stars.
        /// </summary>
        public void LevelUp()
        {
            var scale = _starView.GetGameObjectScale();
            _starSpawnAfter -= (10 * scale);
            _starSpeed += (0.1d * scale);
        }

        #endregion

        #endregion
    }
}
