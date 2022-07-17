using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class PlayerHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;
        private readonly string baseUrl;

        private readonly Random random = new Random();

        private int playerDamagedOpacityCounter;
        private readonly int playerDamagedOpacityLimit = 120;

        private int powerUpTriggerCounter;
        private readonly int powerUpTriggerLimit = 1000;

        #endregion

        #region Ctor

        public PlayerHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns the player.
        /// </summary>
        public Player SpawnPlayer(double pointerX, double playerSpeed)
        {
            var player = new Player();

            var scale = gameEnvironment.GetGameObjectScale();

            var left = pointerX - 35;
            var top = gameEnvironment.Height - player.Height - 20;

            player.SetAttributes(speed: playerSpeed * scale, scale: scale);

            player.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            return player;
        }

        /// <summary>
        /// Moves the player to last pointer pressed position by x axis.
        /// </summary>
        public double MovePlayer(Player player, double pointerX, bool moveLeft, bool moveRight)
        {
            var playerX = player.GetX();
            var playerWidthHalf = player.Width / 2;

            if (moveLeft && playerX > 0)
                pointerX -= player.Speed;

            if (moveRight && playerX + player.Width < gameEnvironment.Width)
                pointerX += player.Speed;

            // move right
            if (pointerX - playerWidthHalf > playerX + player.Speed)
            {
                if (playerX + playerWidthHalf < gameEnvironment.Width)
                {
                    SetPlayerX(player, playerX + player.Speed);
                }
            }

            // move left
            if (pointerX - playerWidthHalf < playerX - player.Speed)
            {
                SetPlayerX(player, playerX - player.Speed);
            }

            return pointerX;
        }

        /// <summary>
        /// Sets the x axis position of the player on game canvas.
        /// </summary>
        /// <param name="x"></param>
        private void SetPlayerX(Player player, double x)
        {
            player.SetX(x);
        }

        /// <summary>
        /// Checks and performs player collision.
        /// </summary>
        /// <param name="gameObject"></param>
        /// 
        /// <returns></returns>
        public bool PlayerCollision(Player player, GameObject gameObject)
        {
            var tag = gameObject.Tag;

            switch (tag)
            {
                case METEOR:
                case ENEMY:
                case ENEMY_PROJECTILE:
                    {
                        // only loose health if player is now in physical state
                        if (!player.IsInEtherealState && player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            gameEnvironment.AddDestroyableGameObject(gameObject);
                            PlayerHealthLoss(player);

                            return true;
                        }
                    }
                    break;
                case HEALTH:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            gameEnvironment.AddDestroyableGameObject(gameObject);
                            PlayerHealthGain(player, gameObject as Health);

                            return true;
                        }
                    }
                    break;
                case POWERUP:
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            gameEnvironment.AddDestroyableGameObject(gameObject);
                            PowerUp(player, (gameObject as PowerUp).PowerUpType);

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
        /// Makes the player loose health.
        /// </summary>
        private void PlayerHealthLoss(Player player)
        {
            player.LooseHealth();

            App.PlaySound(baseUrl, SoundType.HEALTH_LOSS);

            // enter ethereal state, prevent taking damage for a few milliseconds
            player.Opacity = 0.4d;

            player.IsInEtherealState = true;

            playerDamagedOpacityCounter = playerDamagedOpacityLimit;
        }

        /// <summary>
        /// Handles the opacity of the player upon taking damage.
        /// </summary>
        public void HandleDamageRecovery(Player player)
        {
            if (player.IsInEtherealState)
            {
                playerDamagedOpacityCounter -= 1;

                if (playerDamagedOpacityCounter <= 0)
                {
                    player.Opacity = 1;
                    player.IsInEtherealState = false;
                }
            }
        }

        /// <summary>
        /// Makes the player gain health.
        /// </summary>
        /// <param name="health"></param>
        private void PlayerHealthGain(Player player, Health health)
        {
            player.GainHealth(health.Health);
            App.PlaySound(baseUrl, SoundType.HEALTH_GAIN);
        }

        /// <summary>
        /// Triggers the powered up state on.
        /// </summary>
        private void PowerUp(Player player, PowerUpType powerUpType)
        {
            powerUpTriggerCounter = powerUpTriggerLimit;

            App.PlaySound(baseUrl, SoundType.POWER_UP);
            player.TriggerPowerUp(powerUpType);
        }

        /// <summary>
        /// Triggers the powered up state off.
        /// </summary>
        public bool PowerDown(Player player)
        {
            powerUpTriggerCounter -= 1;

            var powerGauge = ((powerUpTriggerCounter / 100) + 1) * gameEnvironment.GetGameObjectScale();

            player.SetPowerGauge(powerGauge);

            if (powerUpTriggerCounter <= 0)
            {
                App.PlaySound(baseUrl, SoundType.POWER_DOWN);
                player.TriggerPowerDown();
                return true;
            }

            return false;
        }

        #endregion
    }
}

