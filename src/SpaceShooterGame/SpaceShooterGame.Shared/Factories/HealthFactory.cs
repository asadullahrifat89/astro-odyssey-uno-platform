using System;

namespace SpaceShooterGame
{
    public class HealthFactory
    {
        #region Fields

        private static GameEnvironment _gameView;
        private static readonly Random _random = new();

        private static int _healthSpawnCounter;
        private static int _healthSpawnAfter = 1000;
        private static double _healthSpeed = 2;

        #endregion

        #region Methods      

        public static void Reset()
        {
            _healthSpawnAfter = 1000;
            _healthSpeed = 2;
        }

        /// <summary>
        /// Sets the game environment.
        /// </summary>
        /// <param name="gameView"></param>
        public static void SetGameEnvironment(GameEnvironment gameView)
        {
            _gameView = gameView;
        }

        /// <summary>
        /// Spawns a Health.
        /// </summary>
        public static void SpawnHealth(Player player)
        {
            if (player.Health <= 80)
            {
                // each frame progress decreases this counter
                _healthSpawnCounter -= 1;

                // when counter reaches zero, create a Health
                if (_healthSpawnCounter < 0)
                {
                    GenerateHealth();
                    _healthSpawnCounter = _healthSpawnAfter;
                }
            }
        }

        /// <summary>
        /// Generates a random Health.
        /// </summary>
        public static void GenerateHealth()
        {
            var health = new Health();

            health.SetAttributes(speed: _healthSpeed + _random.NextDouble(), scale: _gameView.GameObjectScale);
            health.AddToGameEnvironment(top: 0 - health.Height, left: _random.Next(10, (int)_gameView.Width - 100), gameEnvironment: _gameView);
        }

        /// <summary>
        /// Updates an health. Moves the health inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="destroyed"></param>
        public static bool UpdateHealth(Health health)
        {
            bool destroyed = false;

            // move Health down
            health.MoveY();

            // if health or meteor object has gone beyond game view
            destroyed = _gameView.CheckAndAddDestroyableGameObject(health);

            return destroyed;
        }

        /// <summary>
        /// Levels up healths.
        /// </summary>
        public static void LevelUp()
        {
            if (_healthSpawnAfter > 1000 / 3)
            {
                var scale = _gameView.GameObjectScale;
                _healthSpawnAfter -= 50;
                _healthSpeed += (1 * scale);
            }
        }

        #endregion
    }
}
