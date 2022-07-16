using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Enemy : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Enemy()
        {
            Tag = Constants.ENEMY;
            Height = Constants.DefaultGameObjectSize;
            Width = Constants.DefaultGameObjectSize;

            IsDestructible = true;
            Child = content;
            YDirection = YDirection.DOWN;
        }

        #endregion

        #region Properties

        public bool IsEvading { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;
            XDirection = XDirection.NONE;
            MarkedForFadedRemoval = false;
            Opacity = 1;

            var rand = new Random();

            Uri uri = null;
            var enemyShipType = rand.Next(1, 6);

            switch (enemyShipType)
            {
                case 1:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_A.png", UriKind.RelativeOrAbsolute);
                    Health = 2;
                    break;
                case 2:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_B.png", UriKind.RelativeOrAbsolute);
                    Health = 2;
                    break;
                case 3:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_C.png", UriKind.RelativeOrAbsolute);
                    Health = 1;
                    break;
                case 4:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_D.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
                case 5:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_E.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
            }

            content.Source = new BitmapImage(uri);

            Height = Constants.DefaultGameObjectSize * scale;
            Width = Constants.DefaultGameObjectSize * scale;
        }

        public void Evade()
        {
            if (!IsEvading && XDirection == XDirection.NONE)
            {
                XDirection = (XDirection)new Random().Next(1, 3);
                Speed = Speed - 1;

                IsEvading = true;
            }
        }

        #endregion
    }
}
