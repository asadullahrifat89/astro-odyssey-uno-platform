using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Meteor : GameObject
    {
        #region Fields

        private Image content = new Image() { Stretch = Stretch.Uniform };

        private double rotation = 0;

        private RotateTransform rotateTransform = new RotateTransform()
        {
            CenterX = 0.5,
            CenterY = 0.5,
        };

        #endregion

        #region Ctor

        public Meteor()
        {
            Tag = Constants.METEOR;
            Height = 70;
            Width = 70;

            IsDestructible = true;
            Child = content;
            YDirection = YDirection.DOWN;

            RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);            
            RenderTransform = rotateTransform;
        } 

        #endregion

        #region Methods

        public void Rotate()
        {
            rotateTransform.Angle += rotation;
        }

        public void SetAttributes(double speed)
        {
            Speed = speed;
            MarkedForFadedRemoval = false;
            Opacity = 1;

            rotation = new Random().NextDouble();
            rotateTransform.Angle = rotation;

            Uri uri = null;

            var rand = new Random();

            var meteorType = rand.Next(1, 9);

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
        } 

        #endregion
    }
}
