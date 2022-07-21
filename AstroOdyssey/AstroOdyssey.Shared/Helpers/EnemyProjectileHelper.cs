using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class EnemyProjectileHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;
        private readonly string baseUrl;

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
            enemy.ProjectileSpawnCounter -= 1;

            if (enemy.ProjectileSpawnCounter <= 0)
            {
                GenerateProjectile(enemy, gameLevel);

                enemy.ProjectileSpawnCounter = enemy.ProjectileSpawnFrequency;
                enemy.ProjectileSpawnFrequency = random.Next(30, 60);
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

            // boss fires a little faster than usual enemies
            projectile.SetAttributes(speed: enemy.IsBoss ? enemy.Speed * 2 / 1.15 : enemy.Speed * 2 / 1.50, gameLevel: gameLevel, scale: scale, isOverPowered: enemy.IsOverPowered);

            if (enemy.IsBoss)
            {
                enemy.SideWaysProjectileSpawnCounter--;

                if (enemy.SideWaysProjectileSpawnCounter <= 0)
                    projectile.XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);

                enemy.SideWaysProjectileSpawnCounter = enemy.SideWaysProjectileSpawnFrequency;
                enemy.SideWaysProjectileSpawnFrequency = random.Next(5, 7);
            }

            projectile.AddToGameEnvironment(top: enemy.GetY() + enemy.Height - (5 * scale) + projectile.Height / 2, left: enemy.GetX() + enemy.Width / 2 - projectile.Width / 2, gameEnvironment: gameEnvironment);

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

        #endregion
    }
}
