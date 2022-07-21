using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class EnemyProjectileHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;
        private readonly string baseUrl;

        private int projectileCounter;
        private int projectileSpawnLimit = 60;

        private int sideWaysProjectileCounter;
        private int sideWaysProjectileSpawnLimit = 4;

        private Random random = new Random();

        #endregion

        #region Ctor

        public EnemyProjectileHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns a projectile.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="gameLevel"></param>
        public void SpawnProjectile(Enemy enemy, GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            projectileCounter -= 1;

            if (projectileCounter <= 0)
            {
                GenerateProjectile(enemy, gameLevel);

                projectileCounter = projectileSpawnLimit;
                projectileSpawnLimit = random.Next(30, 60);
            }
        }

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="gameLevel"></param>
        private void GenerateProjectile(Enemy enemy, GameLevel gameLevel)
        {
            var projectile = new EnemyProjectile();

            var scale = gameEnvironment.GetGameObjectScale();

            projectile.SetAttributes(speed: enemy.IsBoss ? enemy.Speed * 2 / 1.15 : enemy.Speed * 2 / 1.50, gameLevel: gameLevel, scale: scale, isOverPowered: enemy.IsOverPowered);

            projectile.AddToGameEnvironment(top: enemy.GetY() + enemy.Height - (5 * scale) + projectile.Height / 2, left: enemy.GetX() + enemy.Width / 2 - projectile.Width / 2, gameEnvironment: gameEnvironment);

            if (enemy.IsBoss)
            {
                sideWaysProjectileCounter--;

                if (sideWaysProjectileCounter <= 0)
                    projectile.XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);

                sideWaysProjectileCounter = sideWaysProjectileSpawnLimit;
                sideWaysProjectileSpawnLimit = random.Next(2, 5);
            }

            App.PlaySound(baseUrl, SoundType.ENEMY_ROUNDS_FIRE);
        }

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public void UpdateProjectile(EnemyProjectile projectile, out bool destroyed)
        {
            destroyed = false;

            // move projectile down                
            projectile.MoveY();
            projectile.MoveX();

            // remove projectile if outside game canvas
            if (projectile.GetY() > gameEnvironment.Height)
            {
                gameEnvironment.AddDestroyableGameObject(projectile);
                destroyed = true;
            }
        }

        /// <summary>
        /// Levels up projectiles.
        /// </summary>
        public void LevelUp()
        {
            projectileSpawnLimit -= 1;
        }

        #endregion
    }
}
