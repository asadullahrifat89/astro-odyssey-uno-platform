using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Linq;

namespace SpaceShooterGame
{
    public class Health : GameObject
    {
        #region Fields

        private readonly Image content = new() { Stretch = Stretch.Uniform };

        #endregion

        #region Ctor

        public Health()
        {
            Tag = ElementType.HEALTH;
            Height = Constants.POWERUP_OBJECT_SIZE;
            Width = Constants.POWERUP_OBJECT_SIZE;
            Child = content;
            YDirection = YDirection.DOWN;

            IsPickup = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            var uri = Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.HEALTH).AssetUri;

            content.Source = new BitmapImage(uri);

            var scaledSize = Constants.POWERUP_OBJECT_SIZE * scale;
            Height = scaledSize;
            Width = scaledSize;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
