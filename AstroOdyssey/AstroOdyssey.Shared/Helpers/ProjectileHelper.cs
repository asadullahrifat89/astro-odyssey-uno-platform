using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class ProjectileHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private int projectileCounter;
        private int projectileSpawnLimit = 16;
        private double projectileSpeed = 18;

        private readonly string baseUrl;

        private readonly int DEADSHOT_ROUNDS_LIMIT = 30;
        private readonly int DEADSHOT_ROUNDS_SPEED = 10;

        private readonly int RAPIDSHOT_ROUNDS_LIMIT = 1;
        private readonly int RAPIDSHOT_ROUNDS_SPEED = 1;

        #endregion

        #region Ctor

        public ProjectileHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Triggers the powered up state.
        /// </summary>
        public void PowerUp(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.RAPIDSHOT_ROUNDS:
                    {
                        projectileSpawnLimit -= RAPIDSHOT_ROUNDS_LIMIT; // fast firing rate
                        projectileSpeed += RAPIDSHOT_ROUNDS_SPEED; // fast projectile
                    }
                    break;
                case PowerUpType.DEADSHOT_ROUNDS:
                    {
                        projectileSpawnLimit += DEADSHOT_ROUNDS_LIMIT; // slow firing rate
                        projectileSpeed -= DEADSHOT_ROUNDS_SPEED; // slow projectile
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Triggers the powered up state down.
        /// </summary>
        public void PowerDown(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.RAPIDSHOT_ROUNDS:
                    {
                        projectileSpawnLimit += RAPIDSHOT_ROUNDS_LIMIT;
                        projectileSpeed -= RAPIDSHOT_ROUNDS_SPEED;
                    }
                    break;
                case PowerUpType.DEADSHOT_ROUNDS:
                    {
                        projectileSpawnLimit -= DEADSHOT_ROUNDS_LIMIT;
                        projectileSpeed += DEADSHOT_ROUNDS_SPEED;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Levels up projectiles.
        /// </summary>
        public void LevelUp()
        {
            projectileSpawnLimit -= 1;
        }

        /// <summary>
        /// Spawns a projectile.
        /// </summary>
        public void SpawnProjectile(bool isPoweredUp, bool firingProjectiles, Player player, GameLevel gameLevel, PowerUpType powerUpType)
        {
            // each frame progress decreases this counter
            projectileCounter -= 1;

            if (projectileCounter <= 0)
            {
                if (firingProjectiles)
                // any object falls within player range
                //if (GameView.GetGameObjects<GameObject>().Where(x => x.IsDestructible).Any(x => Player.AnyObjectsOnTheRightProximity(gameObject: x) || Player.AnyObjectsOnTheLeftProximity(gameObject: x)))
                {
                    GenerateProjectile(isPoweredUp: isPoweredUp, player, gameLevel, powerUpType);
                }

                projectileCounter = projectileSpawnLimit;
            }
        }

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public void UpdateProjectile(Projectile projectile, out bool destroyed)
        {
            destroyed = false;

            // move projectile up                
            projectile.MoveY();

            // remove projectile if outside game canvas
            if (projectile.GetY() < 10)
            {
                gameEnvironment.AddDestroyableGameObject(projectile);
                destroyed = true;
            }
        }

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="projectileHeight"></param>
        /// <param name="projectileWidth"></param>
        public void GenerateProjectile(bool isPoweredUp, Player player, GameLevel gameLevel, PowerUpType powerUpType)
        {
            var newProjectile = new Projectile();

            var scale = gameEnvironment.GetGameObjectScale();

            newProjectile.SetAttributes(speed: projectileSpeed, gameLevel: gameLevel, isPoweredUp: isPoweredUp, powerUpType: powerUpType, scale: scale);

            newProjectile.AddToGameEnvironment(top: player.GetY() + 5 * scale - newProjectile.Height / 2, left: player.GetX() + player.Width / 2 - newProjectile.Width / 2, gameEnvironment: gameEnvironment);

            if (newProjectile.IsPoweredUp)
            {
                switch (powerUpType)
                {
                    case PowerUpType.NONE:
                        App.PlaySound(baseUrl, SoundType.ROUNDS_FIRE);
                        break;
                    case PowerUpType.RAPIDSHOT_ROUNDS:
                        App.PlaySound(baseUrl, SoundType.RAPIDSHOT_ROUNDS_FIRE);
                        break;
                    case PowerUpType.DEADSHOT_ROUNDS:
                        App.PlaySound(baseUrl, SoundType.DEADSHOT_ROUNDS_FIRE);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                App.PlaySound(baseUrl, SoundType.ROUNDS_FIRE);
            }
        }

        #endregion
    }
}
