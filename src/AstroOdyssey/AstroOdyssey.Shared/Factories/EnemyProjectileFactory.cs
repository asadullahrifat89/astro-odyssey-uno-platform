using Microsoft.UI.Xaml.Media;
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
            var scale = _gameEnvironment.GetGameObjectScale();

            if (enemy.IsBoss)
            {
                switch (enemy.BossClass)
                {
                    case BossClass.JUGGERNAUT:
                        {
                            var projectile = new EnemyProjectile();

                            // boss fires a little faster than usual enemies
                            projectile.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            OverPowerProjectile(enemy, projectile);

                            projectile.AddToGameEnvironment(
                               top: enemy.GetY() + enemy.Height - (40 * scale) + projectile.Height / 2,
                               left: enemy.GetX() + enemy.HalfWidth - projectile.HalfWidth,
                               gameEnvironment: _gameEnvironment);
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
                               top: enemy.GetY() + enemy.Height - (10 * scale) + projectile1.Height / 2,
                               left: enemy.GetX() + (70 * scale) - projectile1.HalfWidth,
                               gameEnvironment: _gameEnvironment);

                            var projectile2 = new EnemyProjectile();

                            projectile2.SetAttributes(
                                enemy: enemy,
                                scale: scale,
                                isOverPowered: enemy.IsOverPowered);

                            projectile2.AddToGameEnvironment(
                                top: enemy.GetY() + enemy.Height - (10 * scale) + projectile1.Height / 2,
                                left: enemy.GetX() + enemy.Width - (70 * scale) - projectile1.HalfWidth,
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
                                   top: enemy.GetY() + enemy.Height - (20 * scale) + projectile3.Height / 2,
                                   left: enemy.GetX() + (35 * scale) - projectile3.HalfWidth,
                                   gameEnvironment: _gameEnvironment);

                                var projectile4 = new EnemyProjectile();

                                projectile4.SetAttributes(
                                    enemy: enemy,
                                    scale: scale,
                                    isOverPowered: enemy.IsOverPowered);

                                projectile4.AddToGameEnvironment(
                                    top: enemy.GetY() + enemy.Height - (20 * scale) + projectile1.Height / 2,
                                    left: enemy.GetX() + enemy.Width - (35 * scale) - projectile1.HalfWidth,
                                    gameEnvironment: _gameEnvironment);

                                enemy.OverPoweredProjectileSpawnCounter = enemy.OverPoweredProjectileSpawnDelay;
                                enemy.OverPoweredProjectileSpawnDelay = _random.Next(4, 7);
                            }
                        }
                        break;
                    case BossClass.VULTURE:
                        {
                            var projectile = new EnemyProjectile();

                            // boss fires a little faster than usual enemies
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

            AudioHelper.PlaySound(SoundType.ENEMY_ROUNDS_FIRE);
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
