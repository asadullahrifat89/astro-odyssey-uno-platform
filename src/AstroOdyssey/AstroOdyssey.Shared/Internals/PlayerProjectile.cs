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

            IsProjectile = true;
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
                    case PowerUpType.RAPID_SHOT_ROUNDS:
                        {
                            height = 35; width = 10; // medium and faster rounds

                            Background = new SolidColorBrush(Colors.Goldenrod);
                            BorderBrush = new SolidColorBrush(Colors.DarkGoldenrod);
                        }
                        break;
                    case PowerUpType.DEAD_SHOT_ROUNDS:
                        {
                            height = 25; width = 25; // larger and slower rounds

                            Background = new SolidColorBrush(Colors.Cyan);
                            BorderBrush = new SolidColorBrush(Colors.DarkCyan);
                        }
                        break;
                    case PowerUpType.DOOM_SHOT_ROUNDS:
                        {
                            height = 300; width = 20; // larger and longer and faster piercing rounds

                            Background = new SolidColorBrush(Colors.SlateBlue);
                            BorderBrush = new SolidColorBrush(Colors.DarkSlateBlue);
                        }
                        break;
                    default:
                        {
                            Background = new SolidColorBrush(Colors.Red);
                            BorderBrush = new SolidColorBrush(Colors.DarkRed);
                        }
                        break;
                }
            }
            else
            {
                switch (gameLevel)
                {
                    case GameLevel.Level_1:
                        {
                            height = 20; width = 10;
                        }
                        break;
                    case GameLevel.Level_2:
                        {
                            height = 25; width = 11;
                        }
                        break;
                    case GameLevel.Level_3:
                        {
                            height = 30; width = 12;
                        }
                        break;
                    case GameLevel.Level_4:
                        {
                            height = 35; width = 13;
                        }
                        break;
                    case GameLevel.Level_5:
                        {
                            height = 35; width = 14;
                        }
                        break;
                    case GameLevel.Level_6:
                        {
                            height = 35; width = 15;
                        }
                        break;
                    case GameLevel.Level_7:
                        {
                            height = 35; width = 16;
                        }
                        break;
                    case GameLevel.Level_8:
                        {
                            height = 35; width = 17;
                        }
                        break;
                    case GameLevel.Level_9:
                        {
                            height = 35; width = 18;
                        }
                        break;
                    case GameLevel.Level_10:
                        {
                            height = 35; width = 19;
                        }
                        break;
                    default:
                        break;
                }

                Background = new SolidColorBrush(Colors.Red);
                BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }

            if (PowerUpType == PowerUpType.DEAD_SHOT_ROUNDS)
            {
                BorderThickness = new Thickness(3);
            }
            else
            {
                BorderThickness = new Thickness(2, 3, 2, 3);
            }

            Height = height * scale;
            Width = width * scale;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
