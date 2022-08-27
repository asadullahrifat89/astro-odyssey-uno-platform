using System;

namespace AstroOdyssey
{
    public class HealthFactory
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private int healthSpawnCounter;
        private int healthSpawnDelay = 1000;
        private double healthSpeed = 2;

        #endregion

        #region Ctor

        public HealthFactory(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns a Health.
        /// </summary>
        public void SpawnHealth(Player player)
        {
            if (player.Health <= 70)
            {
                // each frame progress decreases this counter
                healthSpawnCounter -= 1;

                // when counter reaches zero, create a Health
                if (healthSpawnCounter < 0)
                {
                    GenerateHealth();
                    healthSpawnCounter = healthSpawnDelay;
                    healthSpawnDelay = random.Next(900, 1001);
                }
            }
        }

        /// <summary>
        /// Generates a random Health.
        /// </summary>
        public void GenerateHealth()
        {
            var health = new Health();

            health.SetAttributes(speed: healthSpeed + random.NextDouble(), scale: gameEnvironment.GetGameObjectScale());
            health.AddToGameEnvironment(top: 0 - health.Height, left: random.Next(10, (int)gameEnvironment.Width - 100), gameEnvironment: gameEnvironment);

            // change the next health spawn time
            healthSpawnDelay = random.Next(1000, 1500);
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

        /// <summary>
        /// Levels up healths.
        /// </summary>
        public void LevelUp()
        {
            healthSpeed += 1;
        }

        #endregion
    }
}
