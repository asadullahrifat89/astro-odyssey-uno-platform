using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Linq;

namespace SpaceShooterGame
{
    public class PowerUp : GameObject
    {
        #region Fields

        private readonly Image content = new() { Stretch = Stretch.Uniform };

        private readonly Random random = new();

        #endregion

        #region Properties

        public PowerUpType PowerUpType { get; set; }

        #endregion

        #region Ctor

        public PowerUp()
        {
            Tag = ElementType.POWERUP;
            Height = Constants.POWERUP_OBJECT_SIZE;
            Width = Constants.POWERUP_OBJECT_SIZE;
            Child = content;

            YDirection = YDirection.DOWN;
            PowerUpType = PowerUpType.NONE;

            IsPickup = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            //TODO: SET IT TO RANDOM
            PowerUpType = (PowerUpType)random.Next(1, Enum.GetNames<PowerUpType>().Length);

            var uri = Constants.IMAGE_TEMPLATES.FirstOrDefault(x => x.ImageType == ImageType.POWERUP).AssetUri;
            content.Source = new BitmapImage(uri);

            var scaledSize = Constants.POWERUP_OBJECT_SIZE * scale;

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
