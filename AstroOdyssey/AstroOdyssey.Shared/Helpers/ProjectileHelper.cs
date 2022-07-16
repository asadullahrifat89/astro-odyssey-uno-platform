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
        private readonly double projectileSpeed = 18;

        private readonly string baseUrl;

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
        public void PowerUp()
        {
            projectileSpawnLimit -= 1;
        }

        /// <summary>
        /// Triggers the powered up state down.
        /// </summary>
        public void PowerDown()
        {
            projectileSpawnLimit += 1;
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
        public void SpawnProjectile(bool isPoweredUp, bool firingProjectiles, Player player, GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            projectileCounter -= 1;

            if (projectileCounter <= 0)
            {
                if (firingProjectiles)
                // any object falls within player range
                //if (GameView.GetGameObjects<GameObject>().Where(x => x.IsDestructible).Any(x => Player.AnyObjectsOnTheRightProximity(gameObject: x) || Player.AnyObjectsOnTheLeftProximity(gameObject: x)))
                {
                    GenerateProjectile(isPoweredUp: isPoweredUp, player, gameLevel);
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
        public void GenerateProjectile(bool isPoweredUp, Player player, GameLevel gameLevel)
        {
            var newProjectile = new Projectile();

            newProjectile.SetAttributes(speed: projectileSpeed, gameLevel: gameLevel, isPoweredUp: isPoweredUp, scale: gameEnvironment.GetGameObjectScale());

            newProjectile.AddToGameEnvironment(top: player.GetY() + 5, left: player.GetX() + player.Width / 2 - newProjectile.Width / 2, gameEnvironment: gameEnvironment);

            if (newProjectile.IsPoweredUp)
                App.PlaySound(baseUrl, SoundType.LASER_FIRE_POWERED_UP);
            else
                App.PlaySound(baseUrl, SoundType.LASER_FIRE);
        }

        #endregion
    }
}
