using System;

namespace AstroOdyssey
{
    public class PowerUpHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;
        private readonly string baseUrl;

        private readonly Random random = new Random();

        private int powerUpSpawnCounter = 1500;
        private int powerUpSpawnDelay = 1500;
        private double powerUpSpeed = 2;

        #endregion

        #region Ctor

        public PowerUpHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns a PowerUp.
        /// </summary>
        public void SpawnPowerUp()
        {
            // each frame progress decreases this counter
            powerUpSpawnCounter -= 1;

            // when counter reaches zero, create a PowerUp
            if (powerUpSpawnCounter < 0)
            {
                GeneratePowerUp();
                powerUpSpawnCounter = powerUpSpawnDelay;
                powerUpSpawnDelay = random.Next(1400, 1501);
            }
        }

        /// <summary>
        /// Generates a random PowerUp.
        /// </summary>
        public void GeneratePowerUp()
        {
            var powerUp = new PowerUp();

            powerUp.SetAttributes(speed: powerUpSpeed + random.NextDouble(), scale: gameEnvironment.GetGameObjectScale());
            powerUp.AddToGameEnvironment(top: 0 - powerUp.Height, left: random.Next(10, (int)gameEnvironment.Width - 100), gameEnvironment: gameEnvironment);

            // change the next power up spawn time
            powerUpSpawnDelay = random.Next(1500, 2000);
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
            destroyed = gameEnvironment.CheckAndAddDestroyableGameObject(powerUp);
        }

        /// <summary>
        /// Levels up power ups.
        /// </summary>
        public void LevelUp()
        {
            powerUpSpeed += 1;
        }

        #endregion
    }
}
