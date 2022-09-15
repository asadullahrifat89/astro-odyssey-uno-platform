using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class PowerUp : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public PowerUp()
        {
            Tag = Constants.POWERUP_TAG;
            Height = Constants.PICKUP_OBJECT_SIZE;
            Width = Constants.PICKUP_OBJECT_SIZE;
            Child = content;

            YDirection = YDirection.DOWN;
            PowerUpType = PowerUpType.NONE;

            IsPickup = true;

            //CornerRadius = new Microsoft.UI.Xaml.CornerRadius(100);
            //BorderBrush = new SolidColorBrush(Colors.OrangeRed);
            //BorderThickness = new Microsoft.UI.Xaml.Thickness(0, 0, 3, 3);
            //Padding = new Microsoft.UI.Xaml.Thickness(5);
        }

        #endregion

        #region Properties

        public PowerUpType PowerUpType { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            //TODO: SET IT TO RANDOM
            PowerUpType = (PowerUpType)random.Next(1, Enum.GetNames<PowerUpType>().Length);

            var uri = new Uri("ms-appx:///Assets/Images/powerup.png", UriKind.RelativeOrAbsolute);
            content.Source = new BitmapImage(uri);

            var scaledSize = Constants.PICKUP_OBJECT_SIZE * scale;

            Height = scaledSize;
            Width = scaledSize;

            HalfWidth = Width / 2;
        }

        #endregion
    }

    public enum PowerUpType
    {
        NONE,
        BLAZE_BLITZ,
        PLASMA_BOMB,
        BEAM_CANNON,
        SONIC_BLAST
    }
}
