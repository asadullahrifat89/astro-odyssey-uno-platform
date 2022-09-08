using System;

namespace AstroOdyssey
{
    public class PowerUpFactory
    {
        #region Fields

        private readonly GameEnvironment _gameEnvironment;
        private readonly Random _random = new Random();

        private int _powerUpSpawnCounter = 1500;
        private int _powerUpSpawnDelay = 1500;
        private double _powerUpSpeed = 2;

        #endregion

        #region Ctor

        public PowerUpFactory(GameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns a PowerUp.
        /// </summary>
        public void SpawnPowerUp()
        {
            // each frame progress decreases this counter
            _powerUpSpawnCounter -= 1;

            // when counter reaches zero, create a PowerUp
            if (_powerUpSpawnCounter < 0)
            {
                GeneratePowerUp();
                _powerUpSpawnCounter = _powerUpSpawnDelay;
                _powerUpSpawnDelay = _random.Next(1400, 1501);
            }
        }

        /// <summary>
        /// Generates a random PowerUp.
        /// </summary>
        public void GeneratePowerUp()
        {
            var powerUp = new PowerUp();

            powerUp.SetAttributes(speed: _powerUpSpeed + _random.NextDouble(), scale: _gameEnvironment.GetGameObjectScale());
            powerUp.AddToGameEnvironment(top: 0 - powerUp.Height, left: _random.Next(10, (int)_gameEnvironment.Width - 100), gameEnvironment: _gameEnvironment);
        }

        /// <summary>
        /// Updates an powerUp. Moves the powerUp inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="powerUp"></param>
        /// <param name="destroyed"></param>
        public void UpdatePowerUp(PowerUp powerUp, out bool destroyed)
        {
            destroyed = false;

            // move PowerUp down
            powerUp.MoveY();

            // if powerUp or meteor object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(powerUp);
        }

        /// <summary>
        /// Levels up power ups.
        /// </summary>
        public void LevelUp()
        {
            var scale = _gameEnvironment.GetGameObjectScale();
            _powerUpSpeed += (1 * scale);
        }

        #endregion
    }
}
