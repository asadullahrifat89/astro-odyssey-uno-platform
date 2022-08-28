using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class PlayerProjectileFactory
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private int projectileSpawnCounter;
        private int projectileSpawnDelay = 14;
        private double projectileSpeed = 18;

        private readonly int BLAZE_CHAIN_ROUNDS_DELAY_DECREASE = 2;
        private readonly int BLAZE_CHAIN_ROUNDS_SPEED_INCREASE = 1;

        private readonly int PLASMA_BOMB_ROUNDS_DELAY_INCREASE = 20;
        private readonly int PLASMA_BOMB_ROUNDS_SPEED_DECREASE = 5;

        private readonly int BEAM_CANON_ROUNDS_DELAY_INCREASE = 25;
        private readonly int BEAM_CANON_ROUNDS_SPEED_INCREASE = 25;

        private readonly int SONIC_BLAST_ROUNDS_DELAY_INCREASE = 15;
        private readonly int SONIC_BLAST_ROUNDS_SPEED_INCREASE = 3;

        private readonly int RAGE_UP_ROUNDS_DELAY_DECREASE = 5;
        private readonly int RAGE_UP_ROUNDS_SPEED_INCREASE = 3;

        private int xSide = 15;

        #endregion

        #region Ctor

        public PlayerProjectileFactory(GameEnvironment gameEnvironment)
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
                GenerateProjectile(
                    isPoweredUp: isPoweredUp,
                    player: player,
                    gameLevel: gameLevel,
                    powerUpType: powerUpType);
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
                left: player.GetX() + player.HalfWidth - projectile.HalfWidth + (projectile.IsPoweredUp && powerUpType == PowerUpType.BLAZE_CHAIN_ROUNDS ? xSide * scale : 0),
                gameEnvironment: gameEnvironment);

            if (projectile.IsPoweredUp)
            {
                switch (powerUpType)
                {
                    case PowerUpType.NONE:
                        AudioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
                        break;
                    case PowerUpType.BLAZE_CHAIN_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_BLAZE_CHAIN_ROUNDS_FIRE);
                        xSide = xSide * -1;
                        break;
                    case PowerUpType.PLASMA_BOMB_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE);
                        break;
                    case PowerUpType.BEAM_CANON_ROUNDS:
                        AudioHelper.PlaySound(SoundType.PLAYER_BEAM_CANON_ROUNDS_FIRE);
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

            if (projectile.IsPoweredUp)
            {
                switch (projectile.PowerUpType)
                {
                    case PowerUpType.NONE:
                        break;
                    case PowerUpType.BLAZE_CHAIN_ROUNDS:
                        break;
                    case PowerUpType.PLASMA_BOMB_ROUNDS:
                        projectile.Lengthen();
                        break;
                    case PowerUpType.BEAM_CANON_ROUNDS:
                        break;
                    case PowerUpType.SONIC_BLAST_ROUNDS:
                        projectile.Widen();
                        break;
                    default:
                        break;
                }
            }

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
                        case PowerUpType.BLAZE_CHAIN_ROUNDS:
                            {
                                // upon hit with a destructible object remove the projectile
                                projectile.IsMarkedForFadedDestruction = true;
                                destructible.LooseHealth(destructible.HitPoint);
                            }
                            break;
                        case PowerUpType.PLASMA_BOMB_ROUNDS:
                            {
                                // upon hit with a destructible object remove the projectile
                                projectile.IsMarkedForFadedDestruction = true;

                                // loose 5 times hit point
                                destructible.LooseHealth(destructible.HitPoint * 5);
                            }
                            break;
                        case PowerUpType.BEAM_CANON_ROUNDS:
                            {
                                // loose health point but projectile is armor penetrating
                                destructible.LooseHealth(destructible.HitPoint);
                            }
                            break;
                        case PowerUpType.SONIC_BLAST_ROUNDS:
                            {
                                // loose 1/2 health point but projectile is armor penetrating
                                destructible.LooseHealth(destructible.HitPoint / 2);
                            }
                            break;
                        default:
                            {
                                // upon hit with a destructible object remove the projectile                             
                                projectile.IsMarkedForFadedDestruction = true;
                                destructible.LooseHealth();
                            }
                            break;
                    }
                }
                else
                {
                    // upon hit with a destructible object remove the projectile                    
                    projectile.IsMarkedForFadedDestruction = true;
                    destructible.LooseHealth();
                }

                AudioHelper.PlaySound(SoundType.ROUNDS_HIT);

                switch (destructible.Tag)
                {
                    case ENEMY:
                        {
                            var enemy = destructible as Enemy;

                            enemy.SetProjectileImpactEffect();

                            // bosses cause a score penalty
                            if (destructible.HasNoHealth)
                            {
                                if (enemy.IsPlayerColliding)
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
                case PowerUpType.BLAZE_CHAIN_ROUNDS:
                    {
                        projectileSpawnDelay -= BLAZE_CHAIN_ROUNDS_DELAY_DECREASE; // fast firing rate
                        projectileSpeed += BLAZE_CHAIN_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case PowerUpType.PLASMA_BOMB_ROUNDS:
                    {
                        projectileSpawnDelay += PLASMA_BOMB_ROUNDS_DELAY_INCREASE; // slow firing rate
                        projectileSpeed -= PLASMA_BOMB_ROUNDS_SPEED_DECREASE; // slow projectile
                    }
                    break;
                case PowerUpType.BEAM_CANON_ROUNDS:
                    {
                        projectileSpawnDelay += BEAM_CANON_ROUNDS_DELAY_INCREASE; // slow firing rate
                        projectileSpeed += BEAM_CANON_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case PowerUpType.SONIC_BLAST_ROUNDS:
                    {
                        projectileSpawnDelay += SONIC_BLAST_ROUNDS_DELAY_INCREASE; // medium firing rate
                        projectileSpeed += SONIC_BLAST_ROUNDS_SPEED_INCREASE; // medium projectile
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
                case PowerUpType.BLAZE_CHAIN_ROUNDS:
                    {
                        projectileSpawnDelay += BLAZE_CHAIN_ROUNDS_DELAY_DECREASE;
                        projectileSpeed -= BLAZE_CHAIN_ROUNDS_SPEED_INCREASE;
                    }
                    break;
                case PowerUpType.PLASMA_BOMB_ROUNDS:
                    {
                        projectileSpawnDelay -= PLASMA_BOMB_ROUNDS_DELAY_INCREASE;
                        projectileSpeed += PLASMA_BOMB_ROUNDS_SPEED_DECREASE;
                    }
                    break;
                case PowerUpType.BEAM_CANON_ROUNDS:
                    {
                        projectileSpawnDelay -= BEAM_CANON_ROUNDS_DELAY_INCREASE;
                        projectileSpeed -= BEAM_CANON_ROUNDS_SPEED_INCREASE;
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

        public void RageUp(Player player)
        {
            switch (player.ShipClass)
            {
                case ShipClass.Antimony:
                    break;
                case ShipClass.Bismuth:
                    {
                        projectileSpawnDelay -= RAGE_UP_ROUNDS_DELAY_DECREASE; // fast firing rate
                        projectileSpeed += RAGE_UP_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case ShipClass.Curium:
                    break;
                default:
                    break;
            }
        }

        public void RageDown(Player player)
        {
            switch (player.ShipClass)
            {
                case ShipClass.Antimony:
                    break;
                case ShipClass.Bismuth:
                    {
                        projectileSpawnDelay += RAGE_UP_ROUNDS_DELAY_DECREASE; // fast firing rate
                        projectileSpeed -= RAGE_UP_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case ShipClass.Curium:
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
