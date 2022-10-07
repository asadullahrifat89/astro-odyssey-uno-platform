using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace AstroOdyssey
{
    public class Collectible : GameObject
    {
        #region Fields

        private readonly Image _content = new Image() { Stretch = Stretch.Uniform };
        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Collectible()
        {
            Tag = Constants.COLLECTIBLE_TAG;
            Height = Constants.COLLECTIBLE_OBJECT_SIZE;
            Width = Constants.COLLECTIBLE_OBJECT_SIZE;
            Child = _content;
            YDirection = YDirection.DOWN;

            IsCollectible = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            var collectibleType = random.Next(0, GameObjectTemplates.COLLECTIBLE_TEMPLATES.Length);
            var uri = GameObjectTemplates.COLLECTIBLE_TEMPLATES[collectibleType];

            _content.Source = new BitmapImage(uri);

            var scaledSize = Constants.COLLECTIBLE_OBJECT_SIZE * scale;
            Height = scaledSize;
            Width = scaledSize;

            _content.Height = this.Height;
            _content.Width = this.Width;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
