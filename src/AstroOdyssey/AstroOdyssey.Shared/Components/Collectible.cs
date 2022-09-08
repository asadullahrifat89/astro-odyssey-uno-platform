using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Collectible : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };
        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Collectible()
        {
            Tag = Constants.COLLECTIBLE;
            Height = Constants.COLLECTIBLE_OBJECT_SIZE;
            Width = Constants.COLLECTIBLE_OBJECT_SIZE;
            Child = content;
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

            content.Source = new BitmapImage(uri);

            Height = Constants.COLLECTIBLE_OBJECT_SIZE * scale;
            Width = Constants.COLLECTIBLE_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
