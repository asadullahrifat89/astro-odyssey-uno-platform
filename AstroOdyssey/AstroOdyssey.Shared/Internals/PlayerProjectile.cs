using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class PlayerProjectile : GameObject
    {
        #region Ctor

        public PlayerProjectile()
        {
            Tag = PLAYER_PROJECTILE;

            Height = 20;
            Width = 5;

            CornerRadius = new CornerRadius(50);
            YDirection = YDirection.UP;
        }

        #endregion

        #region Properties

        public bool IsPoweredUp { get; set; }

        public PowerUpType PowerUpType { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(double speed, GameLevel gameLevel, bool isPoweredUp = false, PowerUpType powerUpType = PowerUpType.NONE, double scale = 1)
        {
            Speed = speed;

            IsPoweredUp = isPoweredUp;
            PowerUpType = powerUpType;

            double height = 0, width = 0;

            if (isPoweredUp)
            {
                switch (PowerUpType)
                {
                    case PowerUpType.RAPIDSHOT_ROUNDS:
                        {
                            height = 20; width = 10; // smaller but faster rounds
                            //Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
                        }
                        break;
                    case PowerUpType.DEADSHOT_ROUNDS:
                        {
                            height = 25; width = 25; // larger but slower rounds
                            //Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
                        }
                        break;
                    case PowerUpType.SONICSHOT_ROUNDS:
                        {
                            height = 300; width = 20; // larger and longer and faster piercing rounds
                            //Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
                        }
                        break;
                    default:
                        //Background = new SolidColorBrush(Colors.White);
                        break;
                }

                Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                switch (gameLevel)
                {
                    case GameLevel.Level_1:
                        {
                            height = 20; width = 5;
                        }
                        break;
                    case GameLevel.Level_2:
                        {
                            height = 25; width = 6;
                        }
                        break;
                    case GameLevel.Level_3:
                        {
                            height = 30; width = 7;
                        }
                        break;
                    case GameLevel.Level_4:
                        {
                            height = 35; width = 8;
                        }
                        break;
                    case GameLevel.Level_5:
                        {
                            height = 35; width = 9;
                        }
                        break;
                    case GameLevel.Level_6:
                        {
                            height = 35; width = 10;
                        }
                        break;
                    case GameLevel.Level_7:
                        {
                            height = 35; width = 11;
                        }
                        break;
                    case GameLevel.Level_8:
                        {
                            height = 35; width = 12;
                        }
                        break;
                    default:
                        break;
                }

                Background = new SolidColorBrush(Colors.White);
            }

            Height = height * scale;
            Width = width * scale;
        }

        #endregion
    }
}
