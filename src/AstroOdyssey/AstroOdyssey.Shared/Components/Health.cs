using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Health : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        #endregion

        #region Ctor

        public Health()
        {
            Tag = Constants.HEALTH;
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
            Health = 10;

            content.Source = new BitmapImage(uri);

            Height = Constants.PICKUP_OBJECT_SIZE * scale;
            Width = Constants.PICKUP_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
