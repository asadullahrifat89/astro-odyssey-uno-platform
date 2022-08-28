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
            Height = Constants.DESTRUCTIBLE_OBJECT_SIZE;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE;

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

            var meteorType = random.Next(1, 10);

            switch (meteorType)
            {
                case 1:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock1.png", UriKind.RelativeOrAbsolute);
                        Health = 1;

                        Height = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 15) * scale;
                        Width = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 15) * scale;
                    }
                    break;
                case 2:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock2.png", UriKind.RelativeOrAbsolute);
                        Health = 1;

                        Height = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 15) * scale;
                        Width = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 15) * scale;
                    }
                    break;
                case 3:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock3.png", UriKind.RelativeOrAbsolute);
                        Health = 2;

                        Height = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 10) * scale;
                        Width = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 10) * scale;
                    }
                    break;
                case 4:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock4.png", UriKind.RelativeOrAbsolute);
                        Health = 2;

                        Height = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 5) * scale;
                        Width = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 5) * scale;
                    }
                    break;
                case 5:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock5.png", UriKind.RelativeOrAbsolute);
                        Health = 3;

                        Height = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
                        Width = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
                    }
                    break;
                case 6:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock6.png", UriKind.RelativeOrAbsolute);
                        Health = 3;

                        Height = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
                        Width = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
                    }
                    break;
                case 7:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock7.png", UriKind.RelativeOrAbsolute);
                        Health = 2;

                        Height = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 10) * scale;
                        Width = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 10) * scale;
                    }
                    break;
                case 8:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock8.png", UriKind.RelativeOrAbsolute);
                        Health = 3;

                        Height = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
                        Width = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
                    }
                    break;
                case 9:
                    {
                        uri = new Uri("ms-appx:///Assets/Images/rock9.png", UriKind.RelativeOrAbsolute);
                        Health = 1;

                        Height = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 15) * scale;
                        Width = (Constants.DESTRUCTIBLE_OBJECT_SIZE - 15) * scale;
                    }
                    break;
            }

            content.Source = new BitmapImage(uri);

            HalfWidth = Width / 2;
        }

        public void Float()
        {
            if (XDirection == XDirection.NONE)
            {
                YDirection = YDirection.UP;
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed = Speed / 2; // decrease speed by 50%

                IsFloating = true;
            }
        }

        public void SetProjectileImpactEffect()
        {
            BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: 0, bottom: 5);
        }

        public void CoolDownProjectileImpactEffect()
        {
            if (BorderThickness.Bottom != 0)
            {
                BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: 0, bottom: BorderThickness.Bottom - 1);
            }
        }

        #endregion
    }
}
