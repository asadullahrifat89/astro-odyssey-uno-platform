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

        public void SetAttributes(double speed, double scale = 1, bool isCelestialObject = false)
        {
            Speed = speed;

            Uri uri = null;

            double size = 0;

            if (isCelestialObject)
            {
                var starType = random.Next(1, 4);

                switch (starType)
                {
                    case 1:
                        uri = new Uri("ms-appx:///Assets/Images/black-hole.png", UriKind.RelativeOrAbsolute);
                        size = 130 - random.Next(1, 15);
                        break;
                    case 2:
                        uri = new Uri("ms-appx:///Assets/Images/nebula.png", UriKind.RelativeOrAbsolute);
                        size = 130 - random.Next(1, 15);
                        break;
                    case 3:
                        uri = new Uri("ms-appx:///Assets/Images/nebula2.png", UriKind.RelativeOrAbsolute);
                        size = 130 - random.Next(1, 15);
                        break;
                }
            }
            else
            {
                var starType = random.Next(1, 13);

                switch (starType)
                {
                    case 1:
                        uri = new Uri("ms-appx:///Assets/Images/star_large.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 2:
                        uri = new Uri("ms-appx:///Assets/Images/star_medium.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 3:
                        uri = new Uri("ms-appx:///Assets/Images/star_small.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 4:
                        uri = new Uri("ms-appx:///Assets/Images/star_tiny.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 5:
                        uri = new Uri("ms-appx:///Assets/Images/star_large2.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 6:
                        uri = new Uri("ms-appx:///Assets/Images/star_medium2.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 7:
                        uri = new Uri("ms-appx:///Assets/Images/star_small2.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 8:
                        uri = new Uri("ms-appx:///Assets/Images/star_tiny2.png", UriKind.RelativeOrAbsolute);
                        size = 20;
                        break;
                    case 9:
                        uri = new Uri("ms-appx:///Assets/Images/gas_star1.png", UriKind.RelativeOrAbsolute);
                        size = 30 - random.Next(1, 15);
                        break;
                    case 10:
                        uri = new Uri("ms-appx:///Assets/Images/gas_star2.png", UriKind.RelativeOrAbsolute);
                        size = 30 - random.Next(1, 15);
                        break;
                    case 11:
                        uri = new Uri("ms-appx:///Assets/Images/gas_star3.png", UriKind.RelativeOrAbsolute);
                        size = 30 - random.Next(1, 15);
                        break;
                    case 12:
                        uri = new Uri("ms-appx:///Assets/Images/gas_star4.png", UriKind.RelativeOrAbsolute);
                        size = 30 - random.Next(1, 15);
                        break;
                }
            }

            content.Source = new BitmapImage(uri);

            Height = size * scale;
            Width = size * scale;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
