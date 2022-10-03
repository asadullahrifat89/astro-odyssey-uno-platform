using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace AstroOdyssey
{
    public class Meteor : GameObject
    {
        #region Fields

        private readonly Image _content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Meteor()
        {
            Tag = Constants.METEOR_TAG;

            IsDestructible = true;
            Child = _content;
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
            var meteorType = random.Next(1, GameObjectTemplates.METEOR_TEMPLATES.Length);
            var meteorTemplate = GameObjectTemplates.METEOR_TEMPLATES[meteorType];

            Uri uri = meteorTemplate.AssetUri;
            Health = meteorTemplate.Health;

            var size = meteorTemplate.Size;
            size -= random.Next(1, 15);

            var scaledSize = size * scale;
            Height = scaledSize;
            Width = scaledSize;

            _content.Source = new BitmapImage(uri);

            _content.Height = this.Height;
            _content.Width = this.Width;

            HalfWidth = Width / 2;
        }

        public void Float()
        {
            if (XDirection == XDirection.NONE)
            {
                YDirection = YDirection.UP;
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed = (Speed / 2.3); // decrease speed

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
