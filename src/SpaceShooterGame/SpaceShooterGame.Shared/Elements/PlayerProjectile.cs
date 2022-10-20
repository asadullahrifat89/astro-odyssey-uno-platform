using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace SpaceShooterGame
{
    public class PlayerProjectile : GameObject
    {
        #region Properties

        public bool IsPoweredUp { get; set; }

        public PowerUpType PowerUpType { get; set; }

        #endregion

        #region Ctor

        public PlayerProjectile()
        {
            Tag = ElementType.PLAYER_PROJECTILE;

            CornerRadius = new CornerRadius(50);
            YDirection = YDirection.UP;

            IsProjectile = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(
            double speed,
            GameLevel gameLevel,
            ShipClass shipClass,
            bool isPoweredUp = false,
            double projectilePower = 0,
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
                    case PowerUpType.BLAZE_BLITZ:
                        {
                            height = 40 + projectilePower; 
                            width = 15; // medium and faster rounds

                            Background = new SolidColorBrush(Colors.Goldenrod);
                            BorderBrush = new SolidColorBrush(Colors.DarkGoldenrod);

                            BorderThickness = new Thickness(2, 3, 2, 1);
                        }
                        break;
                    case PowerUpType.PLASMA_BOMB:
                        {
                            height = 30 + projectilePower;
                            width = 30; // larger and slower rounds

                            Background = new SolidColorBrush(Colors.Cyan);
                            BorderBrush = new SolidColorBrush(Colors.DarkCyan);

                            BorderThickness = new Thickness(3);
                        }
                        break;
                    case PowerUpType.BEAM_CANNON:
                        {
                            height = 305; 
                            width = 25 + projectilePower; // larger and longer and faster piercing rounds

                            Background = new SolidColorBrush(Colors.SlateBlue);
                            BorderBrush = new SolidColorBrush(Colors.DarkSlateBlue);

                            BorderThickness = new Thickness(3, 2);

                            CornerRadius = new CornerRadius(20);
                        }
                        break;
                    case PowerUpType.SONIC_BLAST:
                        {
                            height = 5; 
                            width = 85 + projectilePower; // wider and thiner and slower piercing rounds

                            Background = new SolidColorBrush(Colors.SkyBlue);
                            BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);

                            BorderThickness = new Thickness(1);
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
                    case ShipClass.DEFENDER:
                        {
                            height = 25 + projectilePower;
                            width = 10 + projectilePower / 2;
                            BorderThickness = new Thickness(2, 3, 2, 1);
                        }
                        break;
                    case ShipClass.BERSERKER:
                        {
                            height = 20 + projectilePower;
                            width = 20;
                            BorderThickness = new Thickness(1);
                            CornerRadius = new CornerRadius(5);
                        }
                        break;
                    case ShipClass.SPECTRE:
                        {
                            height = 10;
                            width = 25 + projectilePower;
                            BorderThickness = new Thickness(1);
                            CornerRadius = new CornerRadius(5);
                        }
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
                case ShipClass.DEFENDER:
                    {
                        Background = new SolidColorBrush(Colors.SkyBlue);
                        BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                    }
                    break;
                case ShipClass.BERSERKER:
                    {
                        Background = new SolidColorBrush(Colors.Red);
                        BorderBrush = new SolidColorBrush(Colors.DarkRed);
                    }
                    break;
                case ShipClass.SPECTRE:
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
