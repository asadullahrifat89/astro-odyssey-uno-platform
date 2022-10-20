namespace SpaceShooterGame
{
    public static class PlayerProjectileFactory
    {
        #region Fields

        private static GameEnvironment _gameView;

        private static readonly int BLAZE_BLITZ_ROUNDS_DELAY = 2;
        private static readonly int BLAZE_BLITZ_ROUNDS_SPEED = 1;

        private static readonly int PLASMA_BOMB_ROUNDS_DELAY = 15;
        private static readonly int PLASMA_BOMB_ROUNDS_SPEED = 5;

        private static readonly int BEAM_CANNON_ROUNDS_DELAY = 25;
        private static readonly int BEAM_CANNON_ROUNDS_SPEED = 20;

        private static readonly int SONIC_BLAST_ROUNDS_DELAY = 15;
        private static readonly int SONIC_BLAST_ROUNDS_SPEED = 3;

        private static readonly int RAGE_UP_ROUNDS_DELAY_DECREASE = 2;
        private static readonly int RAGE_UP_ROUNDS_SPEED_INCREASE = 3;

        private static int _xSide = 15;

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
        /// Increases projectile power.
        /// </summary>
        public static void IncreaseProjectilePower(Player player)
        {
            if (player.ProjectilePower <= 8)
                player.ProjectilePower += 1;
        }

        /// <summary>
        /// Decreases projectile power.
        /// </summary>
        public static void DecreaseProjectilePower(Player player)
        {
            if (player.ProjectilePower > 1)
                player.ProjectilePower -= 1;
        }

        /// <summary>
        /// Spawns a projectile.
        /// </summary>
        /// <param name="isPoweredUp"></param>
        /// <param name="player"></param>
        /// <param name="gameLevel"></param>
        /// <param name="powerUpType"></param>
        /// 
        public static void SpawnProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType)
        {
            // each frame progress decreases this counter
            player.ProjectileSpawnCounter -= 1;

            if (player.ProjectileSpawnCounter <= 0)
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

                player.ProjectileSpawnCounter = player.ProjectileSpawnAfter;
            }
        }

        /// <summary>
        /// Generates a projectile.
        /// </summary>
        /// <param name="projectileHeight"></param>
        /// <param name="projectileWidth"></param>
        public static void GenerateProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType)
        {
            var projectile = new PlayerProjectile();

            var scale = _gameView.GameObjectScale;

            projectile.SetAttributes(
                speed: player.ProjectileSpeed,
                gameLevel: gameLevel,
                shipClass: player.ShipClass,
                projectilePower: player.ProjectilePower,
                isPoweredUp: isPoweredUp,
                powerUpType: powerUpType,
                scale: scale);

            projectile.AddToGameEnvironment(
                top: player.GetY() - projectile.Height,
                left: player.GetX() + player.HalfWidth - projectile.HalfWidth + (projectile.IsPoweredUp && powerUpType == PowerUpType.BLAZE_BLITZ ? _xSide * scale : 0),
                gameEnvironment: _gameView);

            if (projectile.IsPoweredUp)
            {
                switch (powerUpType)
                {
                    case PowerUpType.NONE:
                        AudioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
                        break;
                    case PowerUpType.BLAZE_BLITZ:
                        AudioHelper.PlaySound(SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE);
                        _xSide = _xSide * -1;
                        break;
                    case PowerUpType.PLASMA_BOMB:
                        AudioHelper.PlaySound(SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE);
                        break;
                    case PowerUpType.BEAM_CANNON:
                        AudioHelper.PlaySound(SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE);
                        break;
                    case PowerUpType.SONIC_BLAST:
                        AudioHelper.PlaySound(SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                AudioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
            }
        }

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public static bool UpdateProjectile(PlayerProjectile projectile)
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
                // move projectile up                
                projectile.MoveY();

                if (projectile.IsPoweredUp)
                {
                    switch (projectile.PowerUpType)
                    {
                        case PowerUpType.PLASMA_BOMB:
                            projectile.Lengthen();
                            break;
                        case PowerUpType.SONIC_BLAST:
                            projectile.Widen();
                            break;
                        default:
                            break;
                    }
                }

                // remove projectile if outside game canvas
                if (projectile.GetY() + projectile.Height < 0)
                {
                    _gameView.AddDestroyableGameObject(projectile);
                    destroyed = true;
                }
            }

            return destroyed;
        }

        /// <summary>
        /// Collides a projectile with destructible game objects.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="score"></param>
        /// <param name="destroyedObject"></param>
        public static (double Score, GameObject DestroyedObject) CheckCollision(PlayerProjectile projectile)
        {
            // get the destructible objects which intersect with the current projectile
            var destructibles = _gameView.GetDestructibles(projectile.GetRect());

            double score = 0;
            GameObject destroyedObject = null;

            foreach (var destructible in destructibles)
            {
                if (destructible.HasExploded || destructible.HasNoHealth)
                    continue;

                LooseHealth(projectile, destructible);

                var (Score, DestroyedObject) = CheckDestruction(destructible);

                score = Score;
                destroyedObject = DestroyedObject;

                if (destroyedObject is not null)
                    break;

            }

            return (score, destroyedObject);
        }      

        /// <summary>
        /// Triggers the powered up state.
        /// </summary>
        public static void PowerUp(PowerUpType powerUpType, Player player)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.BLAZE_BLITZ:
                    {
                        if (player.ProjectileSpawnAfter > 6)
                            player.ProjectileSpawnAfter -= BLAZE_BLITZ_ROUNDS_DELAY; // fast firing rate 

                        player.ProjectileSpeed += BLAZE_BLITZ_ROUNDS_SPEED; // fast projectile
                    }
                    break;
                case PowerUpType.PLASMA_BOMB:
                    {
                        player.ProjectileSpawnAfter += PLASMA_BOMB_ROUNDS_DELAY; // slow firing rate
                        player.ProjectileSpeed -= PLASMA_BOMB_ROUNDS_SPEED; // slow projectile
                    }
                    break;
                case PowerUpType.BEAM_CANNON:
                    {
                        player.ProjectileSpawnAfter += BEAM_CANNON_ROUNDS_DELAY; // slow firing rate
                        player.ProjectileSpeed += BEAM_CANNON_ROUNDS_SPEED; // fast projectile
                    }
                    break;
                case PowerUpType.SONIC_BLAST:
                    {
                        player.ProjectileSpawnAfter += SONIC_BLAST_ROUNDS_DELAY; // medium firing rate
                        player.ProjectileSpeed += SONIC_BLAST_ROUNDS_SPEED; // medium projectile
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Triggers the powered up state down.
        /// </summary>
        public static void PowerDown(PowerUpType powerUpType, Player player)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.BLAZE_BLITZ:
                    {
                        if (player.ProjectileSpawnAfter > 6)
                            player.ProjectileSpawnAfter += BLAZE_BLITZ_ROUNDS_DELAY;

                        player.ProjectileSpeed -= BLAZE_BLITZ_ROUNDS_SPEED;
                    }
                    break;
                case PowerUpType.PLASMA_BOMB:
                    {
                        player.ProjectileSpawnAfter -= PLASMA_BOMB_ROUNDS_DELAY;
                        player.ProjectileSpeed += PLASMA_BOMB_ROUNDS_SPEED;
                    }
                    break;
                case PowerUpType.BEAM_CANNON:
                    {
                        player.ProjectileSpawnAfter -= BEAM_CANNON_ROUNDS_DELAY;
                        player.ProjectileSpeed -= BEAM_CANNON_ROUNDS_SPEED;
                    }
                    break;
                case PowerUpType.SONIC_BLAST:
                    {
                        player.ProjectileSpawnAfter -= SONIC_BLAST_ROUNDS_DELAY;
                        player.ProjectileSpeed -= SONIC_BLAST_ROUNDS_SPEED;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Enforces rage power of the player's ship class.
        /// </summary>
        /// <param name="player"></param>
        public static void RageUp(Player player)
        {
            switch (player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    break;
                case ShipClass.BERSERKER:
                    {
                        player.ProjectileSpawnAfter -= RAGE_UP_ROUNDS_DELAY_DECREASE; // fast firing rate
                        player.ProjectileSpeed += RAGE_UP_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case ShipClass.SPECTRE:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Nullifies rage power of the player's ship class.
        /// </summary>
        /// <param name="player"></param>
        public static void RageDown(Player player)
        {
            switch (player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    break;
                case ShipClass.BERSERKER:
                    {
                        player.ProjectileSpawnAfter += RAGE_UP_ROUNDS_DELAY_DECREASE; // fast firing rate
                        player.ProjectileSpeed -= RAGE_UP_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case ShipClass.SPECTRE:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Levels up projectiles.
        /// </summary>
        public static void LevelUp(Player player)
        {
            if (player.ProjectileSpawnAfter > 14 / 3)
            {
                var scale = _gameView.GameObjectScale;
                player.ProjectileSpawnAfter -= (1.3 * scale);
            }
        }

        #endregion

        #region Private

        private static void LooseHealth(PlayerProjectile projectile, GameObject destructible)
        {
            AudioHelper.PlaySound(SoundType.ROUNDS_HIT);

            // if projectile is powered up then execute over kill               
            switch (projectile.PowerUpType)
            {
                case PowerUpType.BLAZE_BLITZ:
                    {
                        // upon hit with a destructible object remove the projectile
                        projectile.IsDestroyedByCollision = true;
                        destructible.LooseHealth(destructible.HitPoint);
                    }
                    break;
                case PowerUpType.PLASMA_BOMB:
                    {
                        // upon hit with a destructible object remove the projectile
                        projectile.IsDestroyedByCollision = true;

                        // loose 5 times hit point
                        destructible.LooseHealth(destructible.HitPoint * 5);
                    }
                    break;
                case PowerUpType.BEAM_CANNON:
                    {
                        // loose health point but projectile is armor penetrating
                        destructible.LooseHealth(destructible.HitPoint);
                    }
                    break;
                case PowerUpType.SONIC_BLAST:
                    {
                        // loose 1/2 health point but projectile is armor penetrating
                        destructible.LooseHealth(destructible.HitPoint / 2);
                    }
                    break;
                default:
                    {
                        // upon hit with a destructible object remove the projectile                             
                        projectile.IsDestroyedByCollision = true;
                        destructible.LooseHealth();
                    }
                    break;
            }
        }

        private static (double Score, GameObject DestroyedObject) CheckDestruction(GameObject destructible)
        {
            double score = 0;
            GameObject destroyedObject = null;

            switch (destructible.Tag)
            {
                case ElementType.ENEMY:
                    {
                        var enemy = destructible as Enemy;
                        enemy.SetProjectileHitEffect();

                        if (destructible.HasNoHealth)
                        {
                            // bosses cause a score penalty
                            if (enemy.IsPlayerColliding)
                                score += _gameView.IsBossEngaged ? 1 : 3;
                            else if (enemy.IsOverPowered)
                                score += _gameView.IsBossEngaged ? 2 : 4;
                            else if (enemy.IsBoss)
                                score += 6;
                            else
                                score += _gameView.IsBossEngaged ? 1 : 2;

                            destroyedObject = enemy;
                        }
                        else
                        {
                            if (enemy.IsEvading && !enemy.IsEvadingTriggered)
                                enemy.Evade();
                        }
                    }
                    break;
                case ElementType.METEOR:
                    {
                        var meteor = destructible as Meteor;
                        meteor.SetProjectileHitEffect();

                        if (destructible.HasNoHealth)
                        {
                            if (meteor.IsOverPowered)
                                score += _gameView.IsBossEngaged ? 1 : 2;
                            else
                                score++;

                            destroyedObject = meteor;
                        }
                        else
                        {
                            // meteors float away on impact
                            if (!meteor.IsImpactedByProjectile)
                                meteor.Float();
                        }
                    }
                    break;
                default:
                    break;
            }

            return (score, destroyedObject);
        }

        #endregion

        #endregion
    }
}
