using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceShooterGame
{
    public class Health : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        #endregion

        #region Ctor

        public Health()
        {
            Tag = Constants.HEALTH_TAG;
            Height = Constants.PICKUP_OBJECT_SIZE;
            Width = Constants.PICKUP_OBJECT_SIZE;
            Child = content;
            YDirection = YDirection.DOWN;

            IsPickup = true;

            //CornerRadius = new Microsoft.UI.Xaml.CornerRadius(100);
            //BorderBrush = new SolidColorBrush(Colors.Crimson);
            //BorderThickness = new Microsoft.UI.Xaml.Thickness(0, 0, 3, 3);
            //Padding = new Microsoft.UI.Xaml.Thickness(5);
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            var uri = new Uri("ms-appx:///Assets/Images/health.png", UriKind.RelativeOrAbsolute);

            content.Source = new BitmapImage(uri);

            var scaledSize = Constants.PICKUP_OBJECT_SIZE * scale;
            Height = scaledSize;
            Width = scaledSize;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
