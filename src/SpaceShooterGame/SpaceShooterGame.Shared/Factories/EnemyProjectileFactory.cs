using System;

namespace SpaceShooterGame
{
    public static class EnemyProjectileFactory
    {
        #region Fields

        private static GameEnvironment _gameView;
        private static Random _random = new();

        #endregion        

        #region Methods

        #region Public

        /// <summary>
        /// Sets the game environment.
        /// </summary>
        /// <param name="gameView"></param>
        public static void SetGameEnvironment(GameEnvironment gameView)
        {
            _gameView = gameView;
        }

        /// <summary>
        /// Spawns a projectile.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="gameLevel"></param>
        public static void SpawnProjectile(Enemy enemy, GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            enemy.ProjectileSpawnCounter -= 1;

            if (enemy.ProjectileSpawnCounter <= 0)
            {
                GenerateProjectile(enemy);
                enemy.ProjectileSpawnCounter = enemy.ProjectileSpawnAfter;
            }
        }

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public static bool UpdateProjectile(EnemyProjectile projectile)
        {
            bool destroyed = false;

            if (projectile.IsDestroyedByCollision)
            {
                projectile.Explode();

                if (projectile.HasExploded)
                {
                    _gameView.AddDestroyableGameObject(projectile);
                    destroyed = true;
                }
            }
            else
            {
                // move projectile down                
                projectile.MoveY();
                projectile.MoveX();

                if (projectile.IsOverPowered)
                    projectile.Lengthen();

                // remove projectile if outside game canvas
                if (projectile.GetY() > _gameView.Height)
                {
                    _gameView.AddDestroyableGameObject(projectile);
                    destroyed = true;
                }
            }

            return destroyed;
        }

        #endregion

        #region Private

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="enemy"></param>
        /// 
        private static void GenerateProjectile(Enemy enemy)
        {
            var scale = _gameView.GameObjectScale;

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
                               gameEnvironment: _gameView);

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
                                    gameEnvironment: _gameView);


                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnAfter * scale;
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
                               gameEnvironment: _gameView);

                            var projectile2 = new EnemyProjectile();

                            projectile2.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            projectile2.AddToGameEnvironment(
                                top: enemy.GetY() + enemy.Height - (15 * scale) + projectile2.Height / 2,
                                left: enemy.GetX() + enemy.Width - (70 * scale) - projectile2.HalfWidth,
                                gameEnvironment: _gameView);

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
                                   gameEnvironment: _gameView);

                                var projectile4 = new EnemyProjectile();

                                projectile4.SetAttributes(
                                    enemy: enemy,
                                    scale: scale,
                                    isOverPowered: enemy.IsOverPowered);

                                projectile4.AddToGameEnvironment(
                                    top: enemy.GetY() + enemy.Height - (15 * scale) + projectile4.Height / 2,
                                    left: enemy.GetX() + enemy.Width - (35 * scale) - projectile4.HalfWidth,
                                    gameEnvironment: _gameView);

                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnAfter * scale;
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
                                gameEnvironment: _gameView);

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
                                    gameEnvironment: _gameView);


                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnAfter * scale;
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
                    gameEnvironment: _gameView);
            }

            AudioHelper.PlaySound(SoundType.ENEMY_ROUNDS_FIRE);
        }

        private static void OverPowerProjectile(Enemy enemy, EnemyProjectile projectile)
        {
            if (enemy.IsOverPowered)
            {
                enemy.OverPoweredProjectileSpawnCounter--;

                if (enemy.OverPoweredProjectileSpawnCounter <= 0)
                {
                    projectile.OverPower();

                    enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnAfter;
                }
            }
        }

        #endregion

        #endregion
    }
}
