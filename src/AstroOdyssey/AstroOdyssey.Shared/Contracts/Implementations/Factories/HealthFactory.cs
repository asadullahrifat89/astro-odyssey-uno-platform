using System;

namespace AstroOdyssey
{
    public class HealthFactory : IHealthFactory
    {
        #region Fields

        private GameEnvironment _gameEnvironment;
        private readonly Random _random = new Random();

        private int _healthSpawnCounter;
        private int _healthSpawnAfter = 1000;
        private double _healthSpeed = 2;

        #endregion

        #region Ctor

        public HealthFactory()
        {

        }

        #endregion

        #region Methods

        #region Public

        public void SetGameEnvironment(GameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }

        /// <summary>
        /// Spawns a Health.
        /// </summary>
        public void SpawnHealth(Player player)
        {
            if (player.Health <= 70)
            {
                // each frame progress decreases this counter
                _healthSpawnCounter -= 1;

                // when counter reaches zero, create a Health
                if (_healthSpawnCounter < 0)
                {
                    GenerateHealth();
                    _healthSpawnCounter = _healthSpawnAfter;
                    _healthSpawnAfter = _random.Next(900, 1001);
                }
            }
        }

        /// <summary>
        /// Generates a random Health.
        /// </summary>
        public void GenerateHealth()
        {
            var health = new Health();

            health.SetAttributes(speed: _healthSpeed + _random.NextDouble(), scale: _gameEnvironment.GetGameObjectScale());
            health.AddToGameEnvironment(top: 0 - health.Height, left: _random.Next(10, (int)_gameEnvironment.Width - 100), gameEnvironment: _gameEnvironment);
        }

        /// <summary>
        /// Updates an health. Moves the health inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="destroyed"></param>
        public void UpdateHealth(Health health, out bool destroyed)
        {
            destroyed = false;

            // move Health down
            health.MoveY();

            // if health or meteor object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(health);
        }

        /// <summary>
        /// Levels up healths.
        /// </summary>
        public void LevelUp()
        {
            var scale = _gameEnvironment.GetGameObjectScale();
            _healthSpeed += (1 * scale);
        }

        #endregion

        #endregion
    }
}
