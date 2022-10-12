using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceShooterGame
{
    public class CelestialObject : GameObject
    {
        #region Fields

        private readonly Image _content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public CelestialObject()
        {
            Tag = Constants.STAR_TAG;
            Child = _content;
            YDirection = YDirection.DOWN;
        }

        #endregion

        #region Properties

        public CelestialObjectType CelestialObjectType { get; set; } = CelestialObjectType.Star;

        #endregion

        #region Methods

        public void SetAttributes(double scale = 1, CelestialObjectType celestialObjectType = CelestialObjectType.Star)
        {
            Uri uri = null;

            double size = 0;

            CelestialObjectType = celestialObjectType;

            switch (celestialObjectType)
            {
                case CelestialObjectType.Star:
                    {
                        var starType = random.Next(0, GameObjectTemplates.STAR_TEMPLATES.Length);
                        var starTemplate = GameObjectTemplates.STAR_TEMPLATES[starType];

                        uri = starTemplate.AssetUri;

                        size = starTemplate.Size;

                        if (size >= 100)
                            size -= random.Next(20, 100);
                        else if (size > 25)
                            size -= random.Next(1, 15);
                    }
                    break;
                case CelestialObjectType.Planet:
                    {
                        var planetType = random.Next(0, GameObjectTemplates.PLANET_TEMPLATES.Length);
                        var planetTemplate = GameObjectTemplates.PLANET_TEMPLATES[planetType];

                        uri = planetTemplate.AssetUri;

                        size = planetTemplate.Size;

                        size -= random.Next(1, 100);
                    }
                    break;
                default:
                    break;
            }

            // add a random rotation
            Rotation = random.Next(0, 360);
            Rotate();

            _content.Source = new BitmapImage(uri);

            var scaledSize = size * scale;
            Height = scaledSize;
            Width = scaledSize;

            _content.Height = this.Height;
            _content.Width = this.Width;

            HalfWidth = Width / 2;
        }

        #endregion      
    }

    public enum CelestialObjectType
    {
        Star,
        Planet
    }
}
