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

        private int playerDamagedOpacitySpawnCounter;
        private readonly int playerDamagedOpacityDelay = 120;

        private int powerUpTriggerSpawnCounter;
        private readonly int powerUpTriggerDelay = 1000;

        private double playerSpeed = 12;

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
        public Player SpawnPlayer(double pointerX, Ship ship)
        {
            var player = new Player();

            var scale = gameEnvironment.GetGameObjectScale();          

            player.SetAttributes(speed: playerSpeed * scale, ship: ship, scale: scale);

            var left = pointerX - player.HalfWidth;
            var top = gameEnvironment.Height - player.Height - 20;

            player.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            return player;
        }

        /// <summary>
        /// Moves the player to last pointer pressed position by x axis.
        /// </summary>
        public (double PointerX, double PointerY) UpdatePlayer(Player player, double pointerX, double pointerY, bool moveLeft, bool moveRight/*, bool moveUp, bool moveDown*/)
        {
            var playerX = player.GetX();
            //var playerY = player.GetY();

            var playerSpeed = player.Speed /*/ 2*/;

            // adjust pointer x as per move direction
            if (moveLeft && playerX > 0)
            {
                pointerX -= playerSpeed;
            }

            if (moveRight && playerX + player.Width < gameEnvironment.Width)
            {
                pointerX += playerSpeed;
            }

            //if (moveUp && playerY > player.Height)
            //{
            //    pointerY -= playerSpeed;
            //}

            //if (moveDown && playerY < gameEnvironment.Height - player.Height)
            //{
            //    pointerY += playerSpeed;
            //}

            // move right
            if (pointerX - player.HalfWidth > playerX + playerSpeed)
            {
                if (playerX + player.HalfWidth < gameEnvironment.Width)
                {
                    SetPlayerX(player: player, left: playerX + playerSpeed);
                }
            }

            // move left
            if (pointerX - player.HalfWidth < playerX - playerSpeed)
            {
                SetPlayerX(player: player, left: playerX - playerSpeed);
            }

            //// move up
            //if (pointerY - player.Height / 2 < playerY - playerSpeed)
            //{
            //    SetPlayerY(player: player, top: playerY - playerSpeed);
            //}

            //// move down
            //if (pointerY - player.Height / 2 > playerY + playerSpeed)
            //{
            //    if (playerY + player.Height / 2 < gameEnvironment.Height)
            //    {
            //        SetPlayerY(player: player, top: playerY + playerSpeed);
            //    }
            //}

            return (pointerX, pointerY);
        }

        /// <summary>
        /// Sets the x axis position of the player on game canvas.
        /// </summary>
        /// <param name="left"></param>
        private void SetPlayerX(Player player, double left)
        {
            player.SetX(left);
        }

        /// <summary>
        /// Sets the y axis position of the player on game canvas.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="top"></param>
        private void SetPlayerY(Player player, double top)
        {
            player.SetY(top);
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
                            if (gameObject is not Enemy enemy || !enemy.IsBoss)
                            {
                                gameEnvironment.AddDestroyableGameObject(gameObject);
                            }

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
                            PowerUp(player: player, powerUpType: (gameObject as PowerUp).PowerUpType);

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

            playerDamagedOpacitySpawnCounter = playerDamagedOpacityDelay;
        }

        /// <summary>
        /// Handles the opacity of the player upon taking damage.
        /// </summary>
        public void HandleDamageRecovery(Player player)
        {
            if (player.IsInEtherealState)
            {
                playerDamagedOpacitySpawnCounter -= 1;

                if (playerDamagedOpacitySpawnCounter <= 0)
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
            powerUpTriggerSpawnCounter = powerUpTriggerDelay;

            App.PlaySound(baseUrl, SoundType.POWER_UP);
            player.TriggerPowerUp(powerUpType);
        }

        /// <summary>
        /// Triggers the powered up state off.
        /// </summary>
        public bool PowerDown(Player player)
        {
            powerUpTriggerSpawnCounter -= 1;

            var powerGauge = ((powerUpTriggerSpawnCounter / 100) + 1) * gameEnvironment.GetGameObjectScale();

            player.SetPowerGauge(powerGauge);

            if (powerUpTriggerSpawnCounter <= 0)
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

