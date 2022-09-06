using System;
using Microsoft.UI;
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

            //CornerRadius = new Microsoft.UI.Xaml.CornerRadius(100);
            //Background = new SolidColorBrush(Colors.Goldenrod);
            //BorderBrush = new SolidColorBrush(Colors.DarkGoldenrod);
            //BorderThickness = new Microsoft.UI.Xaml.Thickness(0, 0, 3, 3);
            //Padding = new Microsoft.UI.Xaml.Thickness(5);
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            var uri = Constants.COLLECTIBLE_TEMPLATES[random.Next(0, Constants.COLLECTIBLE_TEMPLATES.Length)];

            content.Source = new BitmapImage(uri);

            Height = Constants.COLLECTIBLE_OBJECT_SIZE * scale;
            Width = Constants.COLLECTIBLE_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;

            //// add a random rotation
            //Rotation = random.Next(0, 180);
            //Rotate();
        }

        #endregion
    }
}
