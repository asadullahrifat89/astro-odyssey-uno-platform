using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;
using static SpaceShooterGame.Constants;

namespace SpaceShooterGame
{
    public static class PlayerFactory
    {
        #region Fields

        private static GameEnvironment _gameView;

        private static int _playerDamageRecoveryCounter;
        private static readonly int _playerDamageRecoveryAfter = 120;

        private static int _powerUpTriggerSpawnCounter;
        private static readonly int _powerUpTriggerAfter = 1050;

        private static int _playerRageCoolDownCounter;
        private static readonly int _playerRageCoolDownAfter = 1000;

        private static double _accelerationCounter = 0;

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
        /// Spawns the player.
        /// </summary>
        public static Player SpawnPlayer(double pointerX, PlayerShip ship)
        {
            var player = new Player();

            var scale = _gameView.GameObjectScale;

            var speed = PLAYER_SPEED * scale;

#if DEBUG
            Console.WriteLine("Player speed: " + speed);
#endif

            player.SetAttributes(
                speed: speed,
                ship: ship,
                scale: scale);

            var left = pointerX - player.HalfWidth;
            double top = GetOptimalPlayerY(player);

            player.AddToGameEnvironment(top: top, left: left, gameEnvironment: _gameView);

            Canvas.SetZIndex(player, 999);

            return player;
        }

        public static double GetOptimalPlayerY(Player player)
        {
            return _gameView.Height - player.Height - player.HalfWidth - 45;
        }

        /// <summary>
        /// Moves the player to last pointer pressed position by x axis.
        /// </summary>
        public static void UpdatePlayer(Player player, Point pointerPosition, bool moveLeft, bool moveRight, bool isPointerActivated)
        {
            double effectiveSpeed = _accelerationCounter >= player.Speed ? player.Speed : _accelerationCounter / 1.3;

            // increase acceleration and stop when player speed is reached
            if (_accelerationCounter <= player.Speed)
                _accelerationCounter++;

            double left = player.GetX();
            double playerMiddleX = left + player.Width / 2;

            if (isPointerActivated)
            {
                // move left
                if (pointerPosition.X < playerMiddleX - player.Speed && left > 0)
                {
                    player.SetX(left - effectiveSpeed);
                }

                // move right
                if (pointerPosition.X > playerMiddleX + player.Speed && left + player.Width < _gameView.Width)
                {
                    player.SetX(left + effectiveSpeed);
                }
            }
            else
            {
                if (moveLeft && left > 0)
                {
                    player.SetX(left - effectiveSpeed);
                }

                if (moveRight && left + player.Width < _gameView.Width)
                {
                    player.SetX(left + effectiveSpeed);
                }
            }
        }

        public static void UpdateAcceleration()
        {
            if (_accelerationCounter > 0)
                _accelerationCounter--;
        }

        /// <summary>
        /// Triggers the powered up state off.
        /// </summary>
        public static (bool PowerDown, double PowerRemaining) PowerUpCoolDown(Player player)
        {
            _powerUpTriggerSpawnCounter -= 1;

            if (_powerUpTriggerSpawnCounter <= 0)
            {
                AudioHelper.PlaySound(SoundType.POWER_DOWN);
                player.PowerUpOff();
                return (true, 0);
            }

            var remainingPower = (double)((double)_powerUpTriggerSpawnCounter / (double)_powerUpTriggerAfter) * POWER_UP_METER;
            return (false, remainingPower);
        }

        /// <summary>
        /// Triggers player rage.
        /// </summary>
        /// <param name="player"></param>
        public static void RageUp(Player player)
        {
            player.IsRageUp = true;
            _playerRageCoolDownCounter = _playerRageCoolDownAfter;

            switch (player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    {
                        player.IsShieldUp = true;
                    }
                    break;
                case ShipClass.BERSERKER:
                    {
                        player.IsFirePowerUp = true;
                    }
                    break;
                case ShipClass.SPECTRE:
                    {
                        player.IsCloakUp = true;
                    }
                    break;
                default:
                    break;
            }

            AudioHelper.PlaySound(SoundType.RAGE_UP);
        }

        /// <summary>
        /// Cools down player rage.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static (bool RageDown, double RageRemaining) RageUpCoolDown(Player player)
        {
            _playerRageCoolDownCounter--;

            switch (player.ShipClass)
            {
                case ShipClass.DEFENDER:
                    {
                        if (_playerRageCoolDownCounter <= 0)
                            player.IsShieldUp = false;
                    }
                    break;
                case ShipClass.BERSERKER:
                    {
                        if (_playerRageCoolDownCounter <= 0)
                            player.IsFirePowerUp = false;
                    }
                    break;
                case ShipClass.SPECTRE:
                    {
                        if (_playerRageCoolDownCounter <= 0)
                            player.IsCloakUp = false;
                    }
                    break;
                default:
                    break;
            }

            if (_playerRageCoolDownCounter <= 0)
            {
                player.IsRageUp = false;
                player.Rage = 0;

                AudioHelper.PlaySound(SoundType.RAGE_DOWN);
                return (true, 0);
            }

            var remainingRage = (double)(_playerRageCoolDownCounter / (double)_playerRageCoolDownAfter) * player.RageThreashold;

            return (false, remainingRage);
        }

        /// <summary>
        /// Checks and performs player collision.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool CheckCollision(Player player, GameObject gameObject)
        {
            var tag = gameObject.Tag;

            switch (tag)
            {
                case ElementType.METEOR:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            if (player.IsCloakUp || player.IsRecoveringFromDamage)  // only loose health if player is in physical state
                            {
                                return false;
                            }
                            else if (player.IsShieldUp) // if shield is up then player takes no damage but the object gets destroyed
                            {
                                gameObject.IsDestroyedByCollision = true;
                                AudioHelper.PlaySound(SoundType.ROUNDS_HIT);
                            }
                            else
                            {
                                gameObject.IsDestroyedByCollision = true;
                                PlayerHealthLoss(player);
                            }

                            return true;
                        }
                    }
                    break;
                case ElementType.ENEMY:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            if (player.IsCloakUp || player.IsRecoveringFromDamage)  // only loose health if player is in physical state
                            {
                                return false;
                            }
                            else if (player.IsShieldUp) // if shield is up then player takes no damage but the object gets destroyed
                            {
                                gameObject.IsDestroyedByCollision = true;
                                AudioHelper.PlaySound(SoundType.ROUNDS_HIT);
                            }
                            else
                            {
                                if (gameObject is Enemy enemy && !enemy.IsBoss)
                                {
                                    gameObject.IsDestroyedByCollision = true;
                                }

                                PlayerHealthLoss(player);
                            }

                            return true;
                        }
                    }
                    break;
                case ElementType.ENEMY_PROJECTILE:
                    {
                        if (!gameObject.IsDestroyedByCollision && player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            if (player.IsCloakUp || player.IsRecoveringFromDamage) // only loose health if player is in physical state
                            {
                                return false;
                            }
                            else if (player.IsShieldUp) // if shield is up then player takes no damage but the object gets destroyed
                            {
                                gameObject.IsDestroyedByCollision = true;
                                AudioHelper.PlaySound(SoundType.ROUNDS_HIT);
                            }
                            else
                            {
                                gameObject.IsDestroyedByCollision = true;
                                PlayerHealthLoss(player);
                            }

                            return true;
                        }
                    }
                    break;
                case ElementType.HEALTH:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            _gameView.AddDestroyableGameObject(gameObject);
                            PlayerHealthGain(player);

                            return true;
                        }
                    }
                    break;
                case ElementType.COLLECTIBLE:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            _gameView.AddDestroyableGameObject(gameObject);
                            PlayerCollectibleCollected();

                            return true;
                        }
                    }
                    break;
                case ElementType.POWERUP:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            _gameView.AddDestroyableGameObject(gameObject);
                            PowerUp(player: player);

                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Handles the opacity of the player upon taking damage.
        /// </summary>
        public static void DamageRecoveryCoolDown(Player player)
        {
            if (player.IsRecoveringFromDamage)
            {
                _playerDamageRecoveryCounter -= 1;

                if (_playerDamageRecoveryCounter <= 0)
                {
                    player.IsRecoveringFromDamage = false;
                }
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Sets the x axis position of the player on game canvas.
        /// </summary>
        /// <param name="left"></param>
        private static void SetPlayerX(Player player, double left)
        {
            player.SetX(left);
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="top"></param>
        private static void SetPlayerY(Player player, double top)
        {
            player.SetY(top);
        }

        /// <summary>
        /// Makes the player loose health.
        /// </summary>
        private static void PlayerHealthLoss(Player player)
        {
            player.LooseHealth();

            AudioHelper.PlaySound(SoundType.HEALTH_LOSS);

            // enter damage recovery state, prevent taking damage for a few milliseconds            
            player.IsRecoveringFromDamage = true;

            _playerDamageRecoveryCounter = _playerDamageRecoveryAfter;
        }

        /// <summary>
        /// Makes the player gain health.
        /// </summary>
        /// <param name="player"></param>
        private static void PlayerHealthGain(Player player)
        {
            player.GainHealth(player.HitPoint);
            AudioHelper.PlaySound(SoundType.HEALTH_GAIN);
        }

        /// <summary>
        /// Makes the player collect collectible.
        /// </summary>
        private static void PlayerCollectibleCollected()
        {
            AudioHelper.PlaySound(SoundType.COLLECTIBLE_COLLECTED);
        }

        /// <summary>
        /// Triggers the powered up state on.
        /// </summary>
        private static void PowerUp(Player player)
        {
            _powerUpTriggerSpawnCounter = _powerUpTriggerAfter;

            AudioHelper.PlaySound(SoundType.POWER_UP);
            player.PowerUpOn();
        }

        #endregion

        #endregion
    }
}

