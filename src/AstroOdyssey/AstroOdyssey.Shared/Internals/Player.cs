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

        private readonly Grid body = new Grid();


        private readonly Image ship = new Image()
        {
            Stretch = Stretch.Uniform,
        };

        //private readonly Image contentShipBlaze = new Image()
        //{
        //    Stretch = Stretch.Uniform
        //};

        private readonly Border powerGauge = new Border()
        {
            Height = 5,
            Width = 0, // TODO: leave it to 0
            CornerRadius = new Microsoft.UI.Xaml.CornerRadius(50),
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top,
            HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
            Background = new SolidColorBrush(Colors.Goldenrod),
        };

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
            //TODO: Get side kicks which shoot additional projectile, lost on impact with enemy or meteor
            //TODO: Develop shield which protects damage for a certain number of hits
            //TODO: killing enemies fillsup power bar that unleashes a powerful blast damaging all enemies in view

            Tag = PLAYER;

            Height = PLAYER_HEIGHT;
            Width = DESTRUCTIBLE_OBJECT_SIZE;

            Health = 100;
            HitPoint = 10; //TODO: HitPoint is always 10

            // combine power gauge, ship, and blaze
            body = new Grid();
            //content.Children.Add(contentShipBlaze);
            body.Children.Add(ship);
            body.Children.Add(powerGauge);
            //content.Children.Add(contentShipHealthBar);           

            Child = body;

            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#if DEBUG
            //Background = new SolidColorBrush(Colors.White);
#endif      
        }

        #endregion

        #region Properties

        public bool IsInEtherealState { get; set; }

        //public double ExhaustHeight { get; set; } = 50;

        #endregion

        #region Methods

        public void SetAttributes(double speed, Ship ship, double scale = 1)
        {
            Speed = speed;

            this.ship.Source = new BitmapImage(new Uri(ship.ImageUrl, UriKind.RelativeOrAbsolute));

            //var exhaustUri = new Uri("ms-appx:///Assets/Images/effect_purple.png", UriKind.RelativeOrAbsolute);

            //contentShipBlaze.Source = new BitmapImage(exhaustUri);
            //contentShipBlaze.Width = contentShip.Width;

            //contentShipBlaze.Height = ExhaustHeight * scale;
            //contentShipBlaze.Margin = new Microsoft.UI.Xaml.Thickness(0, 50 * scale, 0, 0);

            //contentShipPowerGauge.Margin = new Microsoft.UI.Xaml.Thickness(0, 5 * scale, 0, 0);

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

        public void SetPowerGauge(double powerGauge)
        {
            this.powerGauge.Width = powerGauge * 3;
        }

        public void TriggerPowerUp(PowerUpType powerUpType)
        {
            Speed += 1;
            powerGauge.Width = Width / 2;

            //var exhaustUri = new Uri("ms-appx:///Assets/Images/effect_yellow.png", UriKind.RelativeOrAbsolute);
            //contentShipBlaze.Source = new BitmapImage(exhaustUri);
            //contentShipPowerGauge.Background = Colors.Red;

            //switch (powerUpType)
            //{
            //case PowerUpType.RAPID_SHOT_ROUNDS:
            //    {
            //        var exhaustUri = new Uri("ms-appx:///Assets/Images/effect_yellow.png", UriKind.RelativeOrAbsolute);
            //        contentShipBlaze.Source = new BitmapImage(exhaustUri);
            //        contentShipPowerGauge.Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
            //    }
            //    break;
            //case PowerUpType.DEAD_SHOT_ROUNDS:
            //    contentShipPowerGauge.Background = new SolidColorBrush(SPECIAL_ROUNDS_COLOR);
            //    break;
            //case PowerUpType.DOOM_SHOT_ROUNDS:
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
            powerGauge.Width = 0;
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
