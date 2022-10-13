using System.Linq;
using System.Threading.Tasks;
using static SpaceShooterGame.Constants;

namespace SpaceShooterGame
{
    public class PlayerProjectileFactory : IPlayerProjectileFactory
    {
        #region Fields

        private GameEnvironment _gameEnvironment;


        private readonly int BLAZE_BLITZ_ROUNDS_DELAY_DECREASE = 2;
        private readonly int BLAZE_BLITZ_ROUNDS_SPEED_INCREASE = 1;

        private readonly int PLASMA_BOMB_ROUNDS_DELAY_INCREASE = 20;
        private readonly int PLASMA_BOMB_ROUNDS_SPEED_DECREASE = 5;

        private readonly int BEAM_CANNON_ROUNDS_DELAY_INCREASE = 25;
        private readonly int BEAM_CANNON_ROUNDS_SPEED_INCREASE = 20;

        private readonly int SONIC_BLAST_ROUNDS_DELAY_INCREASE = 15;
        private readonly int SONIC_BLAST_ROUNDS_SPEED_INCREASE = 3;

        private readonly int RAGE_UP_ROUNDS_DELAY_DECREASE = 2;
        private readonly int RAGE_UP_ROUNDS_SPEED_INCREASE = 3;

        private int _xSide = 15;

        private readonly IAudioHelper _audioHelper;
        #endregion

        #region Ctor

        public PlayerProjectileFactory(IAudioHelper audioHelper)
        {
            _audioHelper = audioHelper;
        }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Sets the game environment.
        /// </summary>
        /// <param name="gameEnvironment"></param>
        public void SetGameEnvironment(GameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }

        /// <summary>
        /// Increases projectile power.
        /// </summary>
        public void IncreaseProjectilePower(Player player)
        {
            if (player.ProjectilePower <= 4)
            {
                player.ProjectilePower += 0.5;
            }
        }

        /// <summary>
        /// Decreases projectile power.
        /// </summary>
        public void DecreaseProjectilePower(Player player)
        {
            if (player.ProjectilePower > 1)
            {
                player.ProjectilePower -= 0.5;
            }
        }

        /// <summary>
        /// Spawns a projectile.
        /// </summary>
        /// <param name="isPoweredUp"></param>
        /// <param name="player"></param>
        /// <param name="gameLevel"></param>
        /// <param name="powerUpType"></param>
        /// 
        public void SpawnProjectile(
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
        public void GenerateProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType)
        {
            var projectile = new PlayerProjectile();

            var scale = _gameEnvironment.GetGameObjectScale();

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
                gameEnvironment: _gameEnvironment);

            if (projectile.IsPoweredUp)
            {
                switch (powerUpType)
                {
                    case PowerUpType.NONE:
                        _audioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
                        break;
                    case PowerUpType.BLAZE_BLITZ:
                        _audioHelper.PlaySound(SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE);
                        _xSide = _xSide * -1;
                        break;
                    case PowerUpType.PLASMA_BOMB:
                        _audioHelper.PlaySound(SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE);
                        break;
                    case PowerUpType.BEAM_CANNON:
                        _audioHelper.PlaySound(SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE);
                        break;
                    case PowerUpType.SONIC_BLAST:
                        _audioHelper.PlaySound(SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _audioHelper.PlaySound(SoundType.PLAYER_ROUNDS_FIRE);
            }
        }

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public bool UpdateProjectile(PlayerProjectile projectile)
        {
            bool destroyed = false;

            // move projectile up                
            projectile.MoveY();

            if (projectile.IsPoweredUp)
            {
                switch (projectile.PowerUpType)
                {
                    case PowerUpType.NONE:
                        break;
                    case PowerUpType.BLAZE_BLITZ:
                        break;
                    case PowerUpType.PLASMA_BOMB:
                        projectile.Lengthen();
                        break;
                    case PowerUpType.BEAM_CANNON:
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
                _gameEnvironment.AddDestroyableGameObject(projectile);
                destroyed = true;
            }

            return destroyed;
        }

        /// <summary>
        /// Collides a projectile with destructible game objects.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="score"></param>
        /// <param name="destroyedObject"></param>
        public (double Score, GameObject DestroyedObject) CheckCollision(PlayerProjectile projectile)
        {
            double score = 0;
            GameObject destroyedObject = null;

            // get the destructible objects which intersect with the current projectile
            var destructibles = _gameEnvironment.GetDestructibles(projectile.GetRect());

            foreach (var destructible in destructibles)
            //Parallel.ForEach(destructibles, destructible =>            
            {
                // if projectile is powered up then execute over kill
                if (projectile.IsPoweredUp)
                {
                    switch (projectile.PowerUpType)
                    {
                        case PowerUpType.BLAZE_BLITZ:
                            {
                                // upon hit with a destructible object remove the projectile
                                projectile.IsMarkedForFadedDestruction = true;
                                destructible.LooseHealth(destructible.HitPoint);
                            }
                            break;
                        case PowerUpType.PLASMA_BOMB:
                            {
                                // upon hit with a destructible object remove the projectile
                                projectile.IsMarkedForFadedDestruction = true;

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

                _audioHelper.PlaySound(SoundType.ROUNDS_HIT);

                switch (destructible.Tag)
                {
                    case ENEMY_TAG:
                        {
                            var enemy = destructible as Enemy;

                            enemy.SetProjectileImpactEffect();

                            if (destructible.HasNoHealth)
                            {
                                // bosses cause a score penalty
                                if (enemy.IsPlayerColliding)
                                    score += _gameEnvironment.IsBossEngaged ? 1 : 3;
                                else if (enemy.IsOverPowered)
                                    score += _gameEnvironment.IsBossEngaged ? 2 : 4;
                                else if (enemy.IsBoss)
                                    score += 6;
                                else
                                    score += _gameEnvironment.IsBossEngaged ? 1 : 2;

                                destroyedObject = enemy;
                                return (score, destroyedObject);
                            }

                            if (destructible.HasHealth)
                            {
                                if (enemy.IsEvading && !enemy.IsEvadingTriggered)
                                    enemy.Evade();
                            }
                        }
                        break;
                    case METEOR_TAG:
                        {
                            var meteor = destructible as Meteor;

                            meteor.SetProjectileImpactEffect();

                            if (destructible.HasNoHealth)
                            {
                                if (meteor.IsOverPowered)
                                    score += _gameEnvironment.IsBossEngaged ? 1 : 2;
                                else
                                    score++;

                                destroyedObject = meteor;
                                return (score, destroyedObject);
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

            return (score, destroyedObject);
        }

        /// <summary>
        /// Triggers the powered up state.
        /// </summary>
        public void PowerUp(PowerUpType powerUpType, Player player)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.BLAZE_BLITZ:
                    {
                        player.ProjectileSpawnAfter -= BLAZE_BLITZ_ROUNDS_DELAY_DECREASE; // fast firing rate
                        player.ProjectileSpeed += BLAZE_BLITZ_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case PowerUpType.PLASMA_BOMB:
                    {
                        player.ProjectileSpawnAfter += PLASMA_BOMB_ROUNDS_DELAY_INCREASE; // slow firing rate
                        player.ProjectileSpeed -= PLASMA_BOMB_ROUNDS_SPEED_DECREASE; // slow projectile
                    }
                    break;
                case PowerUpType.BEAM_CANNON:
                    {
                        player.ProjectileSpawnAfter += BEAM_CANNON_ROUNDS_DELAY_INCREASE; // slow firing rate
                        player.ProjectileSpeed += BEAM_CANNON_ROUNDS_SPEED_INCREASE; // fast projectile
                    }
                    break;
                case PowerUpType.SONIC_BLAST:
                    {
                        player.ProjectileSpawnAfter += SONIC_BLAST_ROUNDS_DELAY_INCREASE; // medium firing rate
                        player.ProjectileSpeed += SONIC_BLAST_ROUNDS_SPEED_INCREASE; // medium projectile
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Triggers the powered up state down.
        /// </summary>
        public void PowerDown(PowerUpType powerUpType, Player player)
        {
            switch (powerUpType)
            {
                case PowerUpType.NONE:
                    break;
                case PowerUpType.BLAZE_BLITZ:
                    {
                        player.ProjectileSpawnAfter += BLAZE_BLITZ_ROUNDS_DELAY_DECREASE;
                        player.ProjectileSpeed -= BLAZE_BLITZ_ROUNDS_SPEED_INCREASE;
                    }
                    break;
                case PowerUpType.PLASMA_BOMB:
                    {
                        player.ProjectileSpawnAfter -= PLASMA_BOMB_ROUNDS_DELAY_INCREASE;
                        player.ProjectileSpeed += PLASMA_BOMB_ROUNDS_SPEED_DECREASE;
                    }
                    break;
                case PowerUpType.BEAM_CANNON:
                    {
                        player.ProjectileSpawnAfter -= BEAM_CANNON_ROUNDS_DELAY_INCREASE;
                        player.ProjectileSpeed -= BEAM_CANNON_ROUNDS_SPEED_INCREASE;
                    }
                    break;
                case PowerUpType.SONIC_BLAST:
                    {
                        player.ProjectileSpawnAfter -= SONIC_BLAST_ROUNDS_DELAY_INCREASE;
                        player.ProjectileSpeed -= SONIC_BLAST_ROUNDS_SPEED_INCREASE;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Levels up projectiles.
        /// </summary>
        public void LevelUp(Player player)
        {
            var scale = _gameEnvironment.GetGameObjectScale();
            player.ProjectileSpawnAfter -= (1 * scale);
        }

        /// <summary>
        /// Enforces rage power of the player's ship class.
        /// </summary>
        /// <param name="player"></param>
        public void RageUp(Player player)
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
        public void RageDown(Player player)
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

        #endregion

        #endregion
    }
}
