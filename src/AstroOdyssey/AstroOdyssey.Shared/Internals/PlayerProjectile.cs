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

        public void SetAttributes(
            double speed,
            GameLevel gameLevel,
            ShipClass shipClass,
            bool isPoweredUp = false,
            PowerUpType powerUpType = PowerUpType.NONE,
            double scale = 1)
        {
            Speed = speed;

            IsPoweredUp = isPoweredUp;
            PowerUpType = powerUpType;

            double height = 0, width = 0;

            if (isPoweredUp) // if power up
            {
                switch (PowerUpType)
                {
                    case PowerUpType.RAPID_SHOT_ROUNDS:
                        {
                            height = 35; width = 10; // medium and faster rounds

                            Background = new SolidColorBrush(Colors.Goldenrod);
                            BorderBrush = new SolidColorBrush(Colors.DarkGoldenrod);

                            BorderThickness = new Thickness(2, 3, 2, 1);
                        }
                        break;
                    case PowerUpType.DEAD_SHOT_ROUNDS:
                        {
                            height = 25; width = 25; // larger and slower rounds

                            Background = new SolidColorBrush(Colors.Cyan);
                            BorderBrush = new SolidColorBrush(Colors.DarkCyan);

                            BorderThickness = new Thickness(3);
                        }
                        break;
                    case PowerUpType.DOOM_SHOT_ROUNDS:
                        {
                            height = 300; width = 20; // larger and longer and faster piercing rounds

                            Background = new SolidColorBrush(Colors.SlateBlue);
                            BorderBrush = new SolidColorBrush(Colors.DarkSlateBlue);

                            BorderThickness = new Thickness(2, 3, 2, 1);
                        }
                        break;
                    case PowerUpType.SONIC_BLAST_ROUNDS:
                        {
                            height = 15; width = 250; // wider and thiner and slower piercing rounds

                            Background = new SolidColorBrush(Colors.Violet);
                            BorderBrush = new SolidColorBrush(Colors.DarkViolet);

                            BorderThickness = new Thickness(2);
                        }
                        break;
                    default:
                        {
                            SetDefaultPlayerProjectileColors(shipClass);
                        }
                        break;
                }
            }
            else // normal shots
            {
                switch (shipClass)
                {
                    case ShipClass.Antimony:
                        height = 20; width = 5; BorderThickness = new Thickness(2, 3, 2, 1);
                        break;
                    case ShipClass.Bismuth:
                        height = 15; width = 15; BorderThickness = new Thickness(1); CornerRadius = new CornerRadius(5);
                        break;
                    case ShipClass.Curium:
                        height = 5; width = 20; BorderThickness = new Thickness(1); CornerRadius = new CornerRadius(5);
                        break;
                    default:
                        break;
                }

                SetDefaultPlayerProjectileColors(shipClass);
            }

            Height = height * scale;
            Width = width * scale;

            HalfWidth = Width / 2;
        }

        private void SetDefaultPlayerProjectileColors(ShipClass shipClass)
        {
            switch (shipClass)
            {
                case ShipClass.Antimony:
                    {
                        Background = new SolidColorBrush(Colors.SkyBlue);
                        BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                    }
                    break;
                case ShipClass.Bismuth:
                    {
                        Background = new SolidColorBrush(Colors.Red);
                        BorderBrush = new SolidColorBrush(Colors.DarkRed);
                    }
                    break;
                case ShipClass.Curium:
                    {
                        Background = new SolidColorBrush(Colors.Pink);
                        BorderBrush = new SolidColorBrush(Colors.DeepPink);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
