using System;

namespace AstroOdyssey
{
    public class HealthHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private readonly string baseUrl;

        private int healthCounter;
        private int healthSpawnLimit = 1000;
        private double healthSpeed = 2;

        #endregion

        #region Ctor

        public HealthHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Levels up healths.
        /// </summary>
        public void LevelUp()
        {
            healthSpeed += 1;
        }

        /// <summary>
        /// Spawns a Health.
        /// </summary>
        public void SpawnHealth(Player player)
        {
            if (player.Health <= 60)
            {
                // each frame progress decreases this counter
                healthCounter -= 1;

                // when counter reaches zero, create a Health
                if (healthCounter < 0)
                {
                    GenerateHealth();
                    healthCounter = healthSpawnLimit;
                }
            }
        }

        /// <summary>
        /// Generates a random Health.
        /// </summary>
        public void GenerateHealth()
        {
            var NewHealth = new Health();

            NewHealth.SetAttributes(speed: healthSpeed + random.NextDouble(), scale: gameEnvironment.GetGameObjectScale());
            NewHealth.AddToGameEnvironment(top: 0 - NewHealth.Height, left: random.Next(10, (int)gameEnvironment.Width - 100), gameEnvironment: gameEnvironment);

            // change the next health spawn time
            healthSpawnLimit = random.Next(1000, 1500);
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
            destroyed = gameEnvironment.CheckAndAddDestroyableGameObject(health);
        }

        #endregion
    }
}
