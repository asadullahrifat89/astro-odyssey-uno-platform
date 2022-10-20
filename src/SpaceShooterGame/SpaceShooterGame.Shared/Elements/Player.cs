using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Linq;
using Windows.Foundation;

namespace SpaceShooterGame
{
    public class Player : GameObject
    {
        #region Fields

        private readonly Grid _body = new();

        private readonly Image _ship = new() { Stretch = Stretch.Uniform, };

        private readonly Image _engineThrust = new() { Stretch = Stretch.Uniform };

        #endregion
                
        #region Properties

        public ShipClass ShipClass { get; set; }

        public bool IsRageUp { get; set; } = false;

        public bool IsPoweredUp { get; set; } = false;

        public double Rage { get; set; } = 0;

        public double RageThreashold { get; set; }

        public double ProjectileSpawnCounter { get; set; } = 0;

        public double ProjectileSpawnAfter { get; set; } = 14;

        public double ProjectileSpeed { get; set; } = 18;

        public double ProjectilePower { get; set; } = 0;

        private bool _isRecoveringFromDamage;

        public bool IsRecoveringFromDamage
        {
            get { return _isRecoveringFromDamage; }
            set
            {
                _isRecoveringFromDamage = value;

                if (_isRecoveringFromDamage)
                    Opacity = 0.4d;
                else
                    Opacity = IsCloakUp ? 0.6d : 1;
            }
        }

        private bool _isShieldUp;

        public bool IsShieldUp
        {
            get { return _isShieldUp; }
            set
            {
                _isShieldUp = value;

                if (_isShieldUp)
                {
                    BorderBrush = new SolidColorBrush(Colors.SkyBlue);
                    BorderThickness = new Microsoft.UI.Xaml.Thickness(0, 3, 0, 0);
                    CornerRadius = new Microsoft.UI.Xaml.CornerRadius(10);
                }
                else
                {
                    BorderBrush = new SolidColorBrush(Colors.Transparent);
                    BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                    CornerRadius = new Microsoft.UI.Xaml.CornerRadius(0);
                }
            }
        }

        private bool _isFirePowerUp;

        public bool IsFirePowerUp
        {
            get { return _isFirePowerUp; }
            set
            {
                _isFirePowerUp = value;
            }
        }

        private bool _isCloakUp;

        public bool IsCloakUp
        {
            get { return _isCloakUp; }
            set
            {
                _isCloakUp = value;

                if (_isCloakUp)
                    Opacity = 0.6d;
                else
                    Opacity = 1;
            }
        }

        #endregion

        #region Ctor

        public Player()
        {
            Tag = ElementType.PLAYER;

            Height = Constants.PLAYER_HEIGHT;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE;

            IsPlayer = true;

            Health = 100;

            // combine power gauge, ship, and blaze
            _body = new Grid();
            _body.Children.Add(_engineThrust);
            _body.Children.Add(_ship);

            Child = _body;

            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0);

            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(0);
        }

        #endregion

        #region Methods

        public void SetAttributes(
            double speed,
            PlayerShip ship,
            double scale = 1)
        {
            Speed = speed;

            _ship.Source = new BitmapImage(ship.ImageUrl);
            ShipClass = ship.ShipClass;

            IsRageUp = false;

            var playerShipThrusts = AssetHelper.PLAYER_SHIP_THRUST_TEMPLATES;
            Uri engineThrustUri = playerShipThrusts.FirstOrDefault(x => (int)x.Size == (int)ShipClass).AssetUri;

            switch (ShipClass)
            {
                case ShipClass.DEFENDER:
                    {
                        HitPoint = 100 / 5;
                        RageThreashold = 25;
                    }
                    break;
                case ShipClass.BERSERKER:
                    {
                        HitPoint = 100 / 3;
                        RageThreashold = 15;
                    }
                    break;
                case ShipClass.SPECTRE:
                    {
                        HitPoint = 100 / 4;
                        RageThreashold = 20;
                    }
                    break;
                default:
                    break;
            }

            _engineThrust.Source = new BitmapImage(engineThrustUri);
            _engineThrust.Width = _body.Width;
            _engineThrust.Height = _body.Height;

            Height = Constants.PLAYER_HEIGHT * scale;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;
        }

        public void ReAdjustScale(double scale)
        {
            Height = Constants.PLAYER_HEIGHT * scale;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
        }

        public void PowerUpOn()
        {
            IsPoweredUp = true;
        }

        public void PowerUpOff()
        {
            IsPoweredUp = false;
        }

        /// <summary>
        /// Checks if there is any game object within the left side range of the player.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool AnyObjectsOnTheLeftProximity(GameObject gameObject)
        {
            var left = gameObject.GetX();
            var playerX = GetX();

            return left + gameObject.HalfWidth < playerX && left + gameObject.HalfWidth > playerX - 250;
        }

        /// <summary>
        /// Checks if there is any game object within the right side range of the player.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool AnyObjectsOnTheRightProximity(GameObject gameObject)
        {
            var left = gameObject.GetX();
            var playerX = GetX();

            return left + gameObject.HalfWidth > playerX && left + gameObject.HalfWidth <= playerX + 250;
        }

        public new Rect GetRect()
        {
            return new Rect(x: Canvas.GetLeft(this) + 5, y: Canvas.GetTop(this) + 25, width: Width - 5, height: Height - Height / 2);
        }      

        #endregion
    }
}
