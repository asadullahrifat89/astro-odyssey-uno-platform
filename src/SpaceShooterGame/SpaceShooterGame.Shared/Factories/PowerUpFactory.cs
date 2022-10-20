using System;

namespace SpaceShooterGame
{
    public static class PowerUpFactory
    {
        #region Fields

        private static GameEnvironment _gameView;
        private static readonly Random _random = new();

        private static int _powerUpSpawnCounter = 1500;
        private static int _powerUpSpawnAfter = 1500;
        private static double _powerUpSpeed = 2;

        #endregion

        #region Methods

        #region Public

        public static void Reset()
        {
            _powerUpSpawnCounter = 1500;
            _powerUpSpawnAfter = 1500;
            _powerUpSpeed = 2;
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
        /// Spawns a PowerUp.
        /// </summary>
        public static void SpawnPowerUp()
        {
            // each frame progress decreases this counter
            _powerUpSpawnCounter -= 1;

            // when counter reaches zero, create a PowerUp
            if (_powerUpSpawnCounter < 0)
            {
                GeneratePowerUp();
                _powerUpSpawnCounter = _powerUpSpawnAfter;
            }
        }

        /// <summary>
        /// Generates a random PowerUp.
        /// </summary>
        public static void GeneratePowerUp()
        {
            var powerUp = new PowerUp();

            powerUp.SetAttributes(speed: _powerUpSpeed + _random.NextDouble(), scale: _gameView.GameObjectScale);
            powerUp.AddToGameEnvironment(top: 0 - powerUp.Height, left: _random.Next(10, (int)_gameView.Width - 100), gameEnvironment: _gameView);
        }

        /// <summary>
        /// Updates an powerUp. Moves the powerUp inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="powerUp"></param>
        /// <param name="destroyed"></param>
        public static bool UpdatePowerUp(PowerUp powerUp)
        {
            bool destroyed = false;

            // move PowerUp down
            powerUp.MoveY();

            // if powerUp or meteor object has gone beyond game view
            destroyed = _gameView.CheckAndAddDestroyableGameObject(powerUp);

            return destroyed;
        }

        /// <summary>
        /// Levels up power ups.
        /// </summary>
        public static void LevelUp()
        {
            if (_powerUpSpawnAfter > 1500 / 3)
            {
                var scale = _gameView.GameObjectScale;
                _powerUpSpawnAfter -= 50;                
                _powerUpSpeed += (1 * scale);
            }   
        }

        #endregion

        #endregion
    }
}
