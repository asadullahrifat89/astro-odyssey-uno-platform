using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Star : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Star()
        {
            Tag = Constants.STAR;
            Child = content;
            YDirection = YDirection.DOWN;
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            Uri uri = null;

            double size = 0;

            var starType = random.Next(1, 5);

            switch (starType)
            {
                case 1:
                    uri = new Uri("ms-appx:///Assets/Images/star_large.png", UriKind.RelativeOrAbsolute);
                    size = 25;
                    break;
                case 2:
                    uri = new Uri("ms-appx:///Assets/Images/star_medium.png", UriKind.RelativeOrAbsolute);
                    size = 20;
                    break;
                case 3:
                    uri = new Uri("ms-appx:///Assets/Images/star_small.png", UriKind.RelativeOrAbsolute);
                    size = 15;
                    break;
                case 4:
                    uri = new Uri("ms-appx:///Assets/Images/star_tiny.png", UriKind.RelativeOrAbsolute);
                    size = 10;
                    break;
            }

            Height = size * scale;
            Width = size * scale;

            content.Source = new BitmapImage(uri);
        }

        #endregion
    }
}
