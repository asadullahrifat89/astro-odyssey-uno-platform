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
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public void UpdateProjectile(EnemyProjectile projectile, out bool destroyed)
        {
            destroyed = false;

            // move projectile down                
            projectile.MoveY();

            // remove projectile if outside game canvas
            if (projectile.GetY() > gameEnvironment.Height)
            {
                gameEnvironment.AddDestroyableGameObject(projectile);
                destroyed = true;
            }
        }

        public void SpawnProjectile(Enemy enemy, GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            projectileCounter -= 1;

            if (projectileCounter <= 0)
            {
                GenerateProjectile(enemy, gameLevel);

                projectileCounter = projectileSpawnLimit;
            }
        }

        private void GenerateProjectile(Enemy enemy, GameLevel gameLevel)
        {
            var newProjectile = new EnemyProjectile();

            var scale = gameEnvironment.GetGameObjectScale();

            newProjectile.SetAttributes(speed: enemy.Speed * 2 / 1.5, gameLevel: gameLevel, scale: scale);

            newProjectile.AddToGameEnvironment(top: enemy.GetY() + enemy.Height - (5 * scale) + newProjectile.Height / 2, left: enemy.GetX() + enemy.Width / 2 - newProjectile.Width / 2, gameEnvironment: gameEnvironment);

            App.PlaySound(baseUrl, SoundType.ENEMY_ROUNDS_FIRE);
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
