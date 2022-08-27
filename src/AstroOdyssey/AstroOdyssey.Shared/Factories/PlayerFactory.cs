﻿using Microsoft.UI.Xaml.Controls;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class PlayerFactory
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private int playerDamagedOpacitySpawnCounter;
        private readonly int playerDamagedOpacityDelay = 120;

        private int powerUpTriggerSpawnCounter;
        private readonly int powerUpTriggerDelay = 1050;

        private int playerRageCoolDownCounter;
        private readonly int playerRageCoolDownDelay = 900;

        private double playerSpeed = 12;
        private double accelerationCounter = 0;

        private XDirection xDirectionLast = XDirection.NONE;


        #endregion

        #region Ctor

        public PlayerFactory(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
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
            var top = gameEnvironment.Height - player.Height - 30;

            player.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);

            Canvas.SetZIndex(player, 999);

            return player;
        }

        /// <summary>
        /// Moves the player to last pointer pressed position by x axis.
        /// </summary>
        public /*(double PointerX, double PointerY)*/ double UpdatePlayer(Player player, double pointerX, /*double pointerY,*/ bool moveLeft, bool moveRight/*, bool moveUp, bool moveDown*/)
        {
            var playerX = player.GetX();

            var xDirectionNow = moveLeft ? XDirection.LEFT : XDirection.RIGHT;

            // if direction was changed reset acceleration
            if (xDirectionNow != xDirectionLast)
                accelerationCounter = 0;

            //var playerY = player.GetY();         

            var playerSpeed = accelerationCounter >= player.Speed ? player.Speed : accelerationCounter / 1.3;

            // increase acceleration and stop when player speed is reached
            accelerationCounter++;

            // adjust pointer x as per move direction
            if (moveLeft && playerX > 0)
            {
                pointerX -= playerSpeed;
                xDirectionLast = XDirection.LEFT;
            }

            if (moveRight && playerX + player.Width < gameEnvironment.Width)
            {
                pointerX += playerSpeed;
                xDirectionLast = XDirection.RIGHT;
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
                    SetPlayerX(player: player, left: playerX + playerSpeed);
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

            //return (pointerX, pointerY);

            return pointerX;
        }

        public double UpdateAcceleration(Player player, double pointerX)
        {
            // reset acceleration if was beyond player speed
            if (accelerationCounter > player.Speed)
                accelerationCounter = player.Speed;

            // slowly deaccelerate based on last x axis direction
            if (accelerationCounter > 0)
            {
                var playerX = player.GetX();

                if (playerX + player.Width >= gameEnvironment.Width - 20 || playerX <= 20)
                {
                    accelerationCounter = 0;
                    return pointerX;
                }

                switch (xDirectionLast)
                {
                    case XDirection.NONE:
                        break;
                    case XDirection.LEFT:
                        {
                            pointerX -= accelerationCounter;
                            SetPlayerX(player: player, left: playerX - accelerationCounter);
                        }
                        break;
                    case XDirection.RIGHT:
                        {
                            pointerX += accelerationCounter;
                            SetPlayerX(player: player, left: playerX + accelerationCounter);

                        }
                        break;
                    default:
                        break;
                }

                accelerationCounter--;
            }

            return pointerX;
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
                    {
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            if (player.IsEtherealStateUp)  // only loose health if player is in physical state
                            {
                                return false;
                            }
                            else if (player.IsShieldUp) // if shield is up then player takes no damage but the object gets destroyed
                            {
                                gameObject.IsMarkedForFadedDestruction = true;
                            }
                            else
                            {
                                gameObject.IsMarkedForFadedDestruction = true;
                                PlayerHealthLoss(player);
                            }

                            return true;
                        }
                    }
                    break;
                case ENEMY:
                    {                       
                        if (player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            if (player.IsEtherealStateUp)  // only loose health if player is in physical state
                            {
                                return false;
                            }
                            else if (player.IsShieldUp) // if shield is up then player takes no damage but the object gets destroyed
                            {
                                gameObject.IsMarkedForFadedDestruction = true;
                            }
                            else
                            {
                                if (gameObject is Enemy enemy && !enemy.IsBoss)
                                {
                                    gameObject.IsMarkedForFadedDestruction = true;
                                }

                                PlayerHealthLoss(player);
                            }

                            return true;
                        }
                    }
                    break;
                case ENEMY_PROJECTILE:
                    {                        
                        if (!gameObject.IsMarkedForFadedDestruction && player.GetRect().Intersects(gameObject.GetRect()))
                        {
                            if (player.IsEtherealStateUp) // only loose health if player is in physical state
                            {
                                return false;
                            }
                            else if (player.IsShieldUp) // if shield is up then player takes no damage but the object gets destroyed
                            {
                                gameObject.IsMarkedForFadedDestruction = true;
                            }
                            else
                            {
                                gameObject.IsMarkedForFadedDestruction = true;
                                PlayerHealthLoss(player);
                            }                           

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

            AudioHelper.PlaySound(SoundType.HEALTH_LOSS);

            // enter ethereal state, prevent taking damage for a few milliseconds            
            player.IsEtherealStateUp = true;

            playerDamagedOpacitySpawnCounter = playerDamagedOpacityDelay;
        }

        /// <summary>
        /// Handles the opacity of the player upon taking damage.
        /// </summary>
        public void DamageRecoveryCoolDown(Player player)
        {
            if (player.IsEtherealStateUp)
            {
                playerDamagedOpacitySpawnCounter -= 1;

                if (playerDamagedOpacitySpawnCounter <= 0)
                {
                    player.IsEtherealStateUp = false;
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
            AudioHelper.PlaySound(SoundType.HEALTH_GAIN);
        }

        /// <summary>
        /// Triggers the powered up state on.
        /// </summary>
        private void PowerUp(Player player, PowerUpType powerUpType)
        {
            powerUpTriggerSpawnCounter = powerUpTriggerDelay;

            AudioHelper.PlaySound(SoundType.POWER_UP);
            player.TriggerPowerUp(powerUpType);
        }

        /// <summary>
        /// Triggers the powered up state off.
        /// </summary>
        public (bool PoweredDown, int PowerRemaining) PowerUpCoolDown(Player player)
        {
            powerUpTriggerSpawnCounter -= 1;

            if (powerUpTriggerSpawnCounter <= 0)
            {
                AudioHelper.PlaySound(SoundType.POWER_DOWN);
                player.PowerUpCoolDown();
                return (true, 0);
            }

            return (false, powerUpTriggerSpawnCounter);
        }

        public void RageUp(Player player)
        {
            playerRageCoolDownCounter = playerRageCoolDownDelay;
            switch (player.ShipClass)
            {
                case ShipClass.Antimony:
                    {
                        //TODO: shield up
                        player.IsShieldUp = true;
                    }
                    break;
                case ShipClass.Bismuth:
                    {
                        //TODO: faster shots
                        player.IsRapidFireUp = true;
                    }
                    break;
                case ShipClass.Curium:
                    {
                        //TODO: ethereal state
                        player.IsEtherealStateUp = true;
                    }
                    break;
                default:
                    break;
            }
        }

        public bool RageUpCoolDown(Player player)
        {
            playerRageCoolDownCounter--;

            switch (player.ShipClass)
            {
                case ShipClass.Antimony:
                    {
                        //TODO: shield up
                        if (playerRageCoolDownCounter <= 0)
                            player.IsShieldUp = false;
                    }
                    break;
                case ShipClass.Bismuth:
                    {
                        //TODO: faster shots
                        if (playerRageCoolDownCounter <= 0)
                            player.IsRapidFireUp = false;
                    }
                    break;
                case ShipClass.Curium:
                    {
                        //TODO: ethereal state
                        if (playerRageCoolDownCounter <= 0)
                            player.IsEtherealStateUp = false;
                    }
                    break;
                default:
                    break;
            }

            if (playerRageCoolDownCounter <= 0)
            {
                AudioHelper.PlaySound(SoundType.POWER_DOWN);
                return true;
            }

            return false;
        }

        #endregion
    }
}
