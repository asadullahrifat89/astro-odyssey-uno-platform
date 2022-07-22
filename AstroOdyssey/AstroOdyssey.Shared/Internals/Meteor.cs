using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Meteor : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Meteor()
        {
            Tag = Constants.METEOR;
            Height = Constants.DefaultGameObjectSize;
            Width = Constants.DefaultGameObjectSize;

            IsDestructible = true;
            Child = content;
            YDirection = YDirection.DOWN;
        }

        #endregion

        #region Properties

        public bool IsFloating { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;
            XDirection = XDirection.NONE;
            IsMarkedForFadedDestruction = false;
            Opacity = 1;

            Rotation = random.NextDouble();

            Uri uri = null;

            var meteorType = random.Next(1, 9);

            switch (meteorType)
            {
                case 1:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_detailedLarge.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
                case 2:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_detailedSmall.png", UriKind.RelativeOrAbsolute);
                    Health = 2;
                    break;
                case 3:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_large.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
                case 4:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_small.png", UriKind.RelativeOrAbsolute);
                    Health = 2;
                    break;
                case 5:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_squareDetailedLarge.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
                case 6:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_squareDetailedSmall.png", UriKind.RelativeOrAbsolute);
                    Health = 1;
                    break;
                case 7:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_squareLarge.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
                case 8:
                    uri = new Uri("ms-appx:///Assets/Images/meteor_squareSmall.png", UriKind.RelativeOrAbsolute);
                    Health = 1;
                    break;
            }

            content.Source = new BitmapImage(uri);

            Height = Constants.DefaultGameObjectSize * scale;
            Width = Constants.DefaultGameObjectSize * scale;

            HalfWidth = Width / 2;
        }

        public void Float()
        {
            if (XDirection == XDirection.NONE)
            {
                YDirection = YDirection.UP;
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed = (Speed / 2); // decrease speed by 50%

                IsFloating = true;
            }
        }

        #endregion
    }
}
