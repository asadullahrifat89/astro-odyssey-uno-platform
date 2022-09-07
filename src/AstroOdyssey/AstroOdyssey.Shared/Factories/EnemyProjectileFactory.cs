using System;

namespace AstroOdyssey
{
    public class EnemyProjectileFactory
    {
        #region Fields

        private readonly GameEnvironment _gameEnvironment;
        private Random _random = new Random();

        #endregion

        #region Ctor

        public EnemyProjectileFactory(GameEnvironment gameEnvironment)
        {
            this._gameEnvironment = gameEnvironment;
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

            //enemy.CoolDownRecoilEffect();

            if (enemy.ProjectileSpawnCounter <= 0)
            {
                //TODO: Boss: star blast shot across screen

                GenerateProjectile(enemy);
                enemy.ProjectileSpawnCounter = enemy.ProjectileSpawnDelay;

                //enemy.SetRecoilEffect();
            }
        }

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="enemy"></param>
        /// 
        private void GenerateProjectile(Enemy enemy)
        {
            var projectile = new EnemyProjectile();

            var scale = _gameEnvironment.GetGameObjectScale();

            // boss fires a little faster than usual enemies
            projectile.SetAttributes(
                enemy: enemy,
                scale: scale,
                isOverPowered: enemy.IsOverPowered);

            if (enemy.IsBoss)
            {
                enemy.OverPoweredProjectileSpawnCounter--;

                if (enemy.OverPoweredProjectileSpawnCounter <= 0)
                {
                    projectile.OverPower();

                    enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnDelay;
                    enemy.OverPoweredProjectileSpawnDelay = _random.Next(4, 7);
                }
            }

            projectile.AddToGameEnvironment(
                top: enemy.GetY() + enemy.Height - (10 * scale) + projectile.Height / 2,
                left: enemy.GetX() + enemy.HalfWidth - projectile.HalfWidth,
                gameEnvironment: _gameEnvironment);

            AudioHelper.PlaySound(SoundType.ENEMY_ROUNDS_FIRE);
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

            if (projectile.IsOverPowered)
                projectile.Lengthen();

            // remove projectile if outside game canvas
            if (projectile.GetY() > _gameEnvironment.Height)
            {
                _gameEnvironment.AddDestroyableGameObject(projectile);
                destroyed = true;
            }
        }

        #endregion
    }
}
