using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceShooterGame
{
    public class CelestialObject : GameObject
    {
        #region Fields

        private readonly Image _content = new() { Stretch = Stretch.Uniform };

        private readonly Random random = new();

        #endregion

        #region Properties

        public CelestialObjectType CelestialObjectType { get; set; } = CelestialObjectType.Star;

        #endregion

        #region Ctor

        public CelestialObject()
        {
            Tag = ElementType.CELESTIAL_OBJECT;
            Child = _content;
            YDirection = YDirection.DOWN;
        }

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
                        var stars = AssetHelper.STAR_TEMPLATES;
                        var starType = random.Next(0, stars.Length);
                        var starTemplate = stars[starType];

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
                        var planets = AssetHelper.PLANET_TEMPLATES;
                        var planetType = random.Next(0, planets.Length);
                        var planetTemplate = planets[planetType];

                        uri = planetTemplate.AssetUri;

                        size = planetTemplate.Size;

                        size -= random.Next(1, 100);
                    }
                    break;
                default:
                    break;
            }          

            _content.Source = new BitmapImage(uri);

            var scaledSize = size * scale;
            Height = scaledSize;
            Width = scaledSize;

            _content.Height = this.Height;
            _content.Width = this.Width;

            HalfWidth = Width / 2;

            // add a random rotation
            Rotation = random.Next(0, 360);
            Rotate();
        }

        #endregion      
    }

    public enum CelestialObjectType
    {
        Star,
        Planet
    }
}
