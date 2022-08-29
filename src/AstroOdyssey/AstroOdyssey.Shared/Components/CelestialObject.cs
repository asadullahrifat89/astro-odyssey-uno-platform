using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class CelestialObject : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public CelestialObject()
        {
            Tag = Constants.STAR;
            Child = content;
            YDirection = YDirection.DOWN;
        }

        #endregion

        #region Properties

        public CelestialObjectType CelestialObjectType { get; set; } = CelestialObjectType.Star;

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1, CelestialObjectType celestialObjectType = CelestialObjectType.Star)
        {
            Speed = speed;

            Uri uri = null;

            double size = 0;

            CelestialObjectType = celestialObjectType;

            switch (celestialObjectType)
            {
                case CelestialObjectType.Star:
                    {
                        var starType = random.Next(0, Constants.STAR_TEMPLATES.Length);

                        var starTemplate = Constants.STAR_TEMPLATES[starType];

                        uri = starTemplate.AssetUri;

                        size = starTemplate.Size;

                        if (size > 20)
                            size -= random.Next(1, 15);
                    }
                    break;
                case CelestialObjectType.Planet:
                    {
                        var planetType = random.Next(0, Constants.PLANET_TEMPLATES.Length);

                        var planetTemplate = Constants.PLANET_TEMPLATES[planetType];

                        uri = planetTemplate.AssetUri;

                        size = planetTemplate.Size;

                        size -= random.Next(1, 100);
                    }
                    break;
                default:
                    break;
            }

            content.Source = new BitmapImage(uri);

            Height = size * scale;
            Width = size * scale;

            HalfWidth = Width / 2;
        }

        #endregion      
    }

    public class CelestialObjectTemplate
    {
        public CelestialObjectTemplate(Uri assetUri, double size)
        {
            AssetUri = assetUri;
            Size = size;
        }

        public Uri AssetUri { get; set; }

        public double Size { get; set; }
    }

    public enum CelestialObjectType
    {
        Star,
        Planet
    }
}
