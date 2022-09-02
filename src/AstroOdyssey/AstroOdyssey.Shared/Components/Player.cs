using System;
using Windows.Foundation;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class Player : GameObject
    {
        #region Fields

        private readonly Grid _body = new Grid();


        private readonly Image _ship = new Image()
        {
            Stretch = Stretch.Uniform,
        };

        private readonly Image _contentShipBlaze = new Image()
        {
            Stretch = Stretch.Uniform
        };

        //private readonly Border powerGauge = new Border()
        //{
        //    Height = 5,
        //    Width = 0, // TODO: leave it to 0
        //    CornerRadius = new Microsoft.UI.Xaml.CornerRadius(50),
        //    VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top,
        //    HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
        //    Background = new SolidColorBrush(Colors.Goldenrod),
        //};

        //private readonly Border contentShipHealthBar = new Border()
        //{
        //    Height = 5,
        //    CornerRadius = new Microsoft.UI.Xaml.CornerRadius(50),
        //    VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Bottom,
        //    HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
        //    Background = new SolidColorBrush(Colors.Red),
        //};

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Player()
        {
            //TODO: Player: Get side kicks which shoot additional projectile, lost on impact with enemy or meteor

            Tag = PLAYER;

            Height = PLAYER_HEIGHT;
            Width = DESTRUCTIBLE_OBJECT_SIZE;

            Health = 100;
            HitPoint = 10;

            // combine power gauge, ship, and blaze
            _body = new Grid();
            _body.Children.Add(_contentShipBlaze);
            _body.Children.Add(_ship);
            //body.Children.Add(powerGauge);
            //content.Children.Add(contentShipHealthBar);           

            Child = _body;

            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#if DEBUG
            //Background = new SolidColorBrush(Colors.White);
#endif
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(0);
        }

        #endregion

        #region Properties

        public ShipClass ShipClass { get; set; }

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
                    Opacity = 1;
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

        //public double ExhaustHeight { get; set; } = 50;

        #endregion

        #region Methods

        public void SetAttributes(double speed, Ship ship, double scale = 1)
        {
            Speed = speed;

            this._ship.Source = new BitmapImage(new Uri(ship.ImageUrl, UriKind.RelativeOrAbsolute));
            ShipClass = ship.ShipClass;

            Uri exhaustUri = null;

            switch (ShipClass)
            {
                case ShipClass.DEFENDER:
                    exhaustUri = new Uri("ms-appx:///Assets/Images/space_thrust1.png", UriKind.RelativeOrAbsolute);
                    break;
                case ShipClass.BERSERKER:
                    exhaustUri = new Uri("ms-appx:///Assets/Images/space_thrust2.png", UriKind.RelativeOrAbsolute);
                    break;
                case ShipClass.SPECTRE:
                    exhaustUri = new Uri("ms-appx:///Assets/Images/space_thrust3.png", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    break;
            }

            _contentShipBlaze.Source = new BitmapImage(exhaustUri);
            _contentShipBlaze.Width = _body.Width;
            _contentShipBlaze.Height = _body.Height;

            //contentShipBlaze.Margin = new Microsoft.UI.Xaml.Thickness(0, 50 * scale, 0, 0);

            //powerGauge.Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 5 * scale);

            //contentShipHealthBar.Margin = new Microsoft.UI.Xaml.Thickness(0, 75 * scale, 0, 0);
            //contentShipHealthBar.Width = Health / 1.70;

            Height = PLAYER_HEIGHT * scale;
            Width = DESTRUCTIBLE_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;
        }

        public void ReAdjustScale(double scale)
        {
            //contentShipBlaze.Height = ExhaustHeight * scale;
            //contentShipBlaze.Margin = new Microsoft.UI.Xaml.Thickness(0, 50 * scale, 0, 0);

            //contentShipPowerGauge.Margin = new Microsoft.UI.Xaml.Thickness(0, 25 * scale, 0, 0);

            Height = PLAYER_HEIGHT * scale;
            Width = DESTRUCTIBLE_OBJECT_SIZE * scale;
        }

        //public void SetPowerGauge(double powerGauge)
        //{
        //    this.powerGauge.Width = powerGauge * 3;
        //}

        public void TriggerPowerUp(PowerUpType powerUpType)
        {
            Speed += 1;
            //powerGauge.Width = Width / 2;

            //var exhaustUri = new Uri("ms-appx:///Assets/Images/effect_yellow.png", UriKind.RelativeOrAbsolute);
            //contentShipBlaze.Source = new BitmapImage(exhaustUri);
            //contentShipPowerGauge.Background = Colors.Red;

            //switch (powerUpType)
            //{
            //case PowerUpType.BLAZE_BLITZ_ROUNDS:
            //    {
            //        var exhaustUri = new Uri("ms-appx:///Assets/Images/effect_yellow.png", UriKind.RelativeOrAbsolute);
            //        contentShipBlaze.Source = new BitmapImage(exhaustUri);
            //        contentShipPowerGauge.Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
            //    }
            //    break;
            //case PowerUpType.PLASMA_BOMB_ROUNDS:
            //    contentShipPowerGauge.Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
            //    break;
            //case PowerUpType.BEAM_CANNON_ROUNDS:
            //    contentShipPowerGauge.Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
            //    break;
            //default:
            //    break;
            //}
        }

        public void PowerUpCoolDown()
        {
            //var exhaustUri = new Uri("ms-appx:///Assets/Images/effect_purple.png", UriKind.RelativeOrAbsolute);
            //contentShipBlaze.Source = new BitmapImage(exhaustUri);
            Speed -= 1;
            //powerGauge.Width = 0;
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

        //public void SetRecoilEffect()
        //{
        //    BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 4, right: 0, bottom: 0);
        //}

        //public void CoolDownRecoilEffect() 
        //{
        //    if (BorderThickness.Top != 0)
        //    {
        //        BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: BorderThickness.Top - 1, right: 0, bottom: 0);
        //    }
        //}

        #endregion
    }
}
