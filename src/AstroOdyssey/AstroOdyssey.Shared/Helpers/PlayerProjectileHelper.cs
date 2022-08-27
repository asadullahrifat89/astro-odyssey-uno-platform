using System;
using System.Linq;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class PlayerProjectileHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private int projectileSpawnCounter;
        private int projectileSpawnDelay = 14;
        private double projectileSpeed = 18;

        private readonly int RAPID_SHOT_ROUNDS_DELAY_DECREASE = 2;
        private readonly int RAPID_SHOT_ROUNDS_SPEED_INCREASE = 1;

        private readonly int DEAD_SHOT_ROUNDS_DELAY_INCREASE = 20;
        private readonly int DEAD_SHOT_ROUNDS_SPEED_DECREASE = 5;

        private readonly int DOOM_SHOT_ROUNDS_DELAY_INCREASE = 25;
        private readonly int DOOM_SHOT_ROUNDS_SPEED_INCREASE = 25;

        private readonly int SONIC_BLAST_ROUNDS_DELAY_INCREASE = 10;
        private readonly int SONIC_BLAST_ROUNDS_SPEED_INCREASE = 5;

        private int xSide = 15;

        #endregion

        #region Ctor

        public PlayerProjectileHelper(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods       

        /// <summary>
        /// Spawns a projectile.
        /// </summary>
        /// <param name="isPoweredUp"></param>
        /// <param name="firingProjectiles"></param>
        /// <param name="player"></param>
        /// <param name="gameLevel"></param>
        /// <param name="powerUpType"></param>
        public void SpawnProjectile(
            bool isPoweredUp,
            bool firingProjectiles,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType)
        {
            // each frame progress decreases this counter
            projectileSpawnCounter -= 1;

            //player.CoolDownRecoilEffect();

            if (projectileSpawnCounter <= 0)
            {
                //if (firingProjectiles)
                //// any object falls within player range
                //if (gameEnvironment.GetGameObjects<GameObject>().Where(x => x.IsDestructible).Any(x => player.AnyObjectsOnTheRightProximity(gameObject: x) || player.AnyObjectsOnTheLeftProximity(gameObject: x)))
                //{
                GenerateProjectile(isPoweredUp: isPoweredUp, player: player, gameLevel: gameLevel, powerUpType: powerUpType);
                //}

                projectileSpawnCounter = projectileSpawnDelay;

                //player.SetRecoilEffect();
            }
        }

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="projectileHeight"></param>
        /// <param name="projectileWidth"></param>
        public void GenerateProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType)
        {
            var projectile = new PlayerProjectile();

            var scale = gameEnvironment.GetGameObjectScale();

            projectile.SetAttributes(
                speed: projectileSpeed,
                gameLevel: gameLevel,
                shipClass: player.ShipClass,
                isPoweredUp: isPoweredUp,
                powerUpType: powerUpType,
                scale: scale);

            projectile.AddToGameEnvironment(
                top: player.GetY() - projectile.Height,
                left: player.GetX() + player.HalfWidth - projectile.HalfWidth + (projectile.IsPoweredUp && powerUpType == PowerUpType.RAPID_SHOT_ROUNDS ? xSide * scale : 0),
                gameEnvironment: gameEnvironment);

            if (projectile.IsPoweredUp)
            {
                switch (powerUpType)
                {
                    case PowerUpType.NONE:
                        AudioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
                        break;
                    case PowerUpType.RAPID_SHOT_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_RAPID_SHOT_ROUNDS_FIRE);
                        xSide = xSide * -1;
                        break;
                    case PowerUpType.DEAD_SHOT_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_DEAD_SHOT_ROUNDS_FIRE);
                        break;
                    case PowerUpType.DOOM_SHOT_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_DOOM_SHOT_ROUNDS_FIRE);
                        break;
                    case PowerUpType.SONIC_BLAST_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                AudioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
                //xSide = xSide * -1;
            }
        }

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public void UpdateProjectile(PlayerProjectile projectile, out bool destroyed)
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
        /// Collides a projectile with destructible game objects.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="score"></param>
        /// <param name="destroyedObject"></param>
        public void CollidePlayerProjectile(PlayerProjectile projectile, out double score, out GameObject destroyedObject)
        {
            score = 0;
            destroyedObject = null;

            // get the destructible objects which intersect with the current projectile
            var destructibles = gameEnvironment.GetDestructibles(projectile.GetRect());

            foreach (var destructible in destructibles)
            {
                // if projectile is powered up then execute over kill
                if (projectile.IsPoweredUp)
                {
                    switch (projectile.PowerUpType)
                    {
                        case PowerUpType.RAPID_SHOT_ROUNDS:
                            {
                                // upon hit with a destructible object remove the projectile
                                //gameEnvironment.AddDestroyableGameObject(projectile);
                                projectile.IsMarkedForFadedDestruction = true;

                                destructible.LooseHealth(destructible.HitPoint);
                            }
                            break;
                        case PowerUpType.DEAD_SHOT_ROUNDS:
                            {
                                // upon hit with a destructible object remove the projectile
                                //gameEnvironment.AddDestroyableGameObject(projectile);
                                projectile.IsMarkedForFadedDestruction = true;

                                // loose 5 times hit point
                                destructible.LooseHealth(destructible.HitPoint * 5);
                            }
                            break;
                        case PowerUpType.SONIC_BLAST_ROUNDS:
                        case PowerUpType.DOOM_SHOT_ROUNDS:
                            {
                                // loose health point but projectile is armor penetrating
                                destructible.LooseHealth(destructible.HitPoint);
                            }
                            break;
                        default:
                            {
                                // upon hit with a destructible object remove the projectile
                                //gameEnvironment.AddDestroyableGameObject(projectile);
                                projectile.IsMarkedForFadedDestruction = true;

                                destructible.LooseHealth();
                            }
                            break;
                    }
                }
                else
                {
                    // upon hit with a destructible object remove the projectile
                    //gameEnvironment.AddDestroyableGameObject(projectile);
                    projectile.IsMarkedForFadedDestruction = true;
                    destructible.LooseHealth();
                }

                AudioHelper.PlaySound(SoundType.ROUNDS_HIT);

                switch (destructible.Tag)
                {
                    case ENEMY:
                        {
                            var enemy = destructible as Enemy;

                            //if (enemy.IsBoss)
                            //{
                            //    if (enemy.Health <= 3)
                            //        enemy.Fade();
                            //}
                            //else
                            //{
                            //    // fade the a bit on projectile hit
                            //    enemy.Fade();
                            //}

                            enemy.SetProjectileImpactEffect();

                            // bosses cause a score penalty as long as not destroyed
                            if (destructible.HasNoHealth)
                            {
                                if (enemy.IsPlayerTargeting)
                                    score += gameEnvironment.IsBossEngaged ? 1 : 3;
                                else if (enemy.IsOverPowered)
                                    score += gameEnvironment.IsBossEngaged ? 2 : 4;
                                else if (enemy.IsBoss)
                                    score += 6;
                                else
                                    score += gameEnvironment.IsBossEngaged ? 1 : 2;

                                destroyedObject = enemy;

                                return;
                            }

                            if (destructible.HasHealth)
                            {
                                if (enemy.IsEvading && !enemy.IsEvadingTriggered)
                                    enemy.Evade();
                            }
                        }
                        break;
                    case METEOR:
                        {
                            var meteor = destructible as Meteor;

                            //// fade the a bit on projectile hit
                            //meteor.Fade();

                            meteor.SetProjectileImpactEffect();

                            if (destructible.HasNoHealth)
                            {
                                if (meteor.IsOverPowered)
                                    score += gameEnvironment.IsBossEngaged ? 1 : 2;
                                else
                                    score++;

                                destroyedObject = meteor;

                                return;
                            }

                            if (destructible.HasHealth)
                            {
                                // meteors float away on impact
                                if (!meteor.IsFloating)
                                    meteor.Float();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Triggers the powered up state.
        /// </summary>
        public void PowerUp(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.RAPID_SHOT_ROUNDS:
                    {
                        projectileSpawnDelay -= RAPID_SHOT_ROUNDS_DELAY_DECREASE; // fast firing rate
                        projectileSpeed += RAPID_SHOT_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case PowerUpType.DEAD_SHOT_ROUNDS:
                    {
                        projectileSpawnDelay += DEAD_SHOT_ROUNDS_DELAY_INCREASE; // slow firing rate
                        projectileSpeed -= DEAD_SHOT_ROUNDS_SPEED_DECREASE; // slow projectile
                    }
                    break;
                case PowerUpType.DOOM_SHOT_ROUNDS:
                    {
                        projectileSpawnDelay += DOOM_SHOT_ROUNDS_DELAY_INCREASE; // slow firing rate
                        projectileSpeed += DOOM_SHOT_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case PowerUpType.SONIC_BLAST_ROUNDS:
                    {
                        projectileSpawnDelay += SONIC_BLAST_ROUNDS_DELAY_INCREASE; // slow firing rate
                        projectileSpeed += SONIC_BLAST_ROUNDS_SPEED_INCREASE; // slow projectile
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
                case PowerUpType.RAPID_SHOT_ROUNDS:
                    {
                        projectileSpawnDelay += RAPID_SHOT_ROUNDS_DELAY_DECREASE;
                        projectileSpeed -= RAPID_SHOT_ROUNDS_SPEED_INCREASE;
                    }
                    break;
                case PowerUpType.DEAD_SHOT_ROUNDS:
                    {
                        projectileSpawnDelay -= DEAD_SHOT_ROUNDS_DELAY_INCREASE;
                        projectileSpeed += DEAD_SHOT_ROUNDS_SPEED_DECREASE;
                    }
                    break;
                case PowerUpType.DOOM_SHOT_ROUNDS:
                    {
                        projectileSpawnDelay -= DOOM_SHOT_ROUNDS_DELAY_INCREASE;
                        projectileSpeed -= DOOM_SHOT_ROUNDS_SPEED_INCREASE;
                    }
                    break;
                case PowerUpType.SONIC_BLAST_ROUNDS:
                    {
                        projectileSpawnDelay -= SONIC_BLAST_ROUNDS_DELAY_INCREASE;
                        projectileSpeed -= SONIC_BLAST_ROUNDS_SPEED_INCREASE;
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
            projectileSpawnDelay -= 1;
        }

        #endregion
    }
}
