using System;

namespace AstroOdyssey
{
    public class EnemyProjectileFactory : IEnemyProjectileFactory
    {
        #region Fields

        private GameEnvironment _gameEnvironment;
        private Random _random = new Random();
        private readonly IAudioHelper _audioHelper;
        #endregion

        #region Ctor

        public EnemyProjectileFactory(IAudioHelper audioHelper)
        {
            _audioHelper = audioHelper;
        }

        #endregion

        #region Methods

        #region Public

        public void SetGameEnvironment(GameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }

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
                GenerateProjectile(enemy);
                enemy.ProjectileSpawnCounter = enemy.ProjectileSpawnDelay;
            }
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

        #region Private

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="enemy"></param>
        /// 
        private void GenerateProjectile(Enemy enemy)
        {
            var scale = _gameEnvironment.GetGameObjectScale();

            if (enemy.IsBoss)
            {
                switch (enemy.BossClass)
                {
                    case BossClass.GREEN:
                        {
                            var projectile = new EnemyProjectile();

                            // boss fires a little faster than usual enemies
                            projectile.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            projectile.AddToGameEnvironment(
                               top: enemy.GetY() + enemy.Height - (15 * scale) + projectile.Height / 2,
                               left: enemy.GetX() + enemy.HalfWidth - projectile.HalfWidth,
                               gameEnvironment: _gameEnvironment);

                            enemy.OverPoweredProjectileSpawnCounter--;

                            if (enemy.OverPoweredProjectileSpawnCounter <= 0)
                            {
                                var projectile2 = new EnemyProjectile();

                                // boss fires a little faster than usual enemies
                                projectile2.SetAttributes(
                                    enemy: enemy,
                                    scale: scale,
                                    isOverPowered: enemy.IsOverPowered);

                                projectile2.Width += 35;
                                projectile2.Height += 35;
                                projectile2.Speed--;

                                projectile2.AddToGameEnvironment(
                                    top: enemy.GetY() + enemy.Height - (15 * scale) + projectile2.Height / 2,
                                    left: enemy.GetX() + enemy.HalfWidth - projectile2.HalfWidth,
                                    gameEnvironment: _gameEnvironment);


                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnDelay;
                                enemy.OverPoweredProjectileSpawnDelay = _random.Next(4, 7);
                            }
                        }
                        break;
                    case BossClass.CRIMSON:
                        {
                            var projectile1 = new EnemyProjectile();

                            // boss fires a little faster than usual enemies
                            projectile1.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            projectile1.AddToGameEnvironment(
                               top: enemy.GetY() + enemy.Height - (15 * scale) + projectile1.Height / 2,
                               left: enemy.GetX() + (70 * scale) - projectile1.HalfWidth,
                               gameEnvironment: _gameEnvironment);

                            var projectile2 = new EnemyProjectile();

                            projectile2.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            projectile2.AddToGameEnvironment(
                                top: enemy.GetY() + enemy.Height - (15 * scale) + projectile2.Height / 2,
                                left: enemy.GetX() + enemy.Width - (70 * scale) - projectile2.HalfWidth,
                                gameEnvironment: _gameEnvironment);

                            enemy.OverPoweredProjectileSpawnCounter--;

                            if (enemy.OverPoweredProjectileSpawnCounter <= 0)
                            {
                                var projectile3 = new EnemyProjectile();

                                // boss fires a little faster than usual enemies
                                projectile3.SetAttributes(
                                    enemy: enemy,
                                    scale: scale,
                                    isOverPowered: enemy.IsOverPowered);

                                projectile3.AddToGameEnvironment(
                                   top: enemy.GetY() + enemy.Height - (15 * scale) + projectile3.Height / 2,
                                   left: enemy.GetX() + (35 * scale) - projectile3.HalfWidth,
                                   gameEnvironment: _gameEnvironment);

                                var projectile4 = new EnemyProjectile();

                                projectile4.SetAttributes(
                                    enemy: enemy,
                                    scale: scale,
                                    isOverPowered: enemy.IsOverPowered);

                                projectile4.AddToGameEnvironment(
                                    top: enemy.GetY() + enemy.Height - (15 * scale) + projectile4.Height / 2,
                                    left: enemy.GetX() + enemy.Width - (35 * scale) - projectile4.HalfWidth,
                                    gameEnvironment: _gameEnvironment);

                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnDelay;
                                enemy.OverPoweredProjectileSpawnDelay = _random.Next(4, 7);
                            }
                        }
                        break;
                    case BossClass.BLUE:
                        {
                            var projectile = new EnemyProjectile();

                            // boss fires a little faster than usual enemies
                            projectile.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            //OverPowerProjectile(enemy, projectile);

                            projectile.AddToGameEnvironment(
                                top: enemy.GetY() + enemy.Height - (10 * scale) + projectile.Height / 2,
                                left: enemy.GetX() + enemy.HalfWidth - projectile.HalfWidth,
                                gameEnvironment: _gameEnvironment);

                            enemy.OverPoweredProjectileSpawnCounter--;

                            if (enemy.OverPoweredProjectileSpawnCounter <= 0)
                            {
                                var projectile2 = new EnemyProjectile();

                                // boss fires a little faster than usual enemies
                                projectile2.SetAttributes(
                                    enemy: enemy,
                                    scale: scale,
                                    isOverPowered: enemy.IsOverPowered);

                                projectile2.Height += 45;
                                projectile2.Speed *= 1.5;

                                projectile2.AddToGameEnvironment(
                                    top: enemy.GetY() + enemy.Height - (10 * scale) + projectile2.Height / 2,
                                    left: enemy.GetX() + enemy.HalfWidth - projectile2.HalfWidth,
                                    gameEnvironment: _gameEnvironment);


                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnDelay;
                                enemy.OverPoweredProjectileSpawnDelay = _random.Next(4, 7);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var projectile = new EnemyProjectile();

                projectile.SetAttributes(
                    enemy: enemy,
                    scale: scale,
                    isOverPowered: enemy.IsOverPowered);

                OverPowerProjectile(enemy, projectile);

                projectile.AddToGameEnvironment(
                    top: enemy.GetY() + enemy.Height - (10 * scale) + projectile.Height / 2,
                    left: enemy.GetX() + enemy.HalfWidth - projectile.HalfWidth,
                    gameEnvironment: _gameEnvironment);
            }

            _audioHelper.PlaySound(SoundType.ENEMY_ROUNDS_FIRE);
        }

        private void OverPowerProjectile(Enemy enemy, EnemyProjectile projectile)
        {
            if (enemy.IsOverPowered)
            {
                enemy.OverPoweredProjectileSpawnCounter--;

                if (enemy.OverPoweredProjectileSpawnCounter <= 0)
                {
                    projectile.OverPower();

                    enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnDelay;
                    enemy.OverPoweredProjectileSpawnDelay = _random.Next(4, 7);
                }
            }
        }

        #endregion

        #endregion
    }
}
