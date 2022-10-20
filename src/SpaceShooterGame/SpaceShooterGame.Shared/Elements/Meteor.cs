using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceShooterGame
{
    public class Meteor : GameObject
    {
        #region Fields

        private readonly Image _content = new() { Stretch = Stretch.Uniform };

        private readonly Random random = new();

        #endregion

        #region Properties

        public bool IsImpactedByProjectile { get; set; }

        #endregion

        #region Ctor

        public Meteor()
        {
            Tag = ElementType.METEOR;

            IsDestructible = true;
            Child = _content;
            YDirection = YDirection.DOWN;
        }

        #endregion

        #region Methods

        public void SetAttributes(
            double speed,
            double scale = 1,
            bool recycle = false)
        {
            Opacity = 1;
            Speed = speed;
            Rotation = random.NextDouble();

            YDirection = YDirection.DOWN;
            XDirection = XDirection.NONE;

            IsDestroyedByCollision = false;
            IsImpactedByProjectile = false;
            
            Health = random.Next(1, 4);

            if (!recycle)
            {
                var meteors = AssetHelper.METEOR_TEMPLATES;
                var meteorType = random.Next(0, meteors.Length);
                var meteorTemplate = meteors[meteorType];

                var size = meteorTemplate.Size;
                size -= random.Next(1, 15);

                var scaledSize = size * scale;
                Height = scaledSize;
                Width = scaledSize;

                Uri uri = meteorTemplate.AssetUri;
                _content.Source = new BitmapImage(uri);

                HalfWidth = Width / 2; 
            }
        }

        public void Float()
        {
            if (XDirection == XDirection.NONE)
            {
                YDirection = YDirection.UP;
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed /= 2.3; // decrease speed

                IsImpactedByProjectile = true;
            }
        }

        public void SetProjectileHitEffect()
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
