using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceShooterGame
{
    public class Collectible : GameObject
    {
        #region Fields

        private readonly Image _content = new() { Stretch = Stretch.Uniform };
        private readonly Random random = new();

        #endregion

        #region Ctor

        public Collectible()
        {
            Tag = ElementType.COLLECTIBLE;
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

            var collectibles = AssetHelper.COLLECTIBLE_TEMPLATES;
            var collectibleType = random.Next(0, collectibles.Length);
            var collectibleTemplate = collectibles[collectibleType];

            _content.Source = new BitmapImage(collectibleTemplate.AssetUri);

            var scaledSize = collectibleTemplate.Size * scale;
            Height = scaledSize;
            Width = scaledSize;

            _content.Height = this.Height;
            _content.Width = this.Width;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
