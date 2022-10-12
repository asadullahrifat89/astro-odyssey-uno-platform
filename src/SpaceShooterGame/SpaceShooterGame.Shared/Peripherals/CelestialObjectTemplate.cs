using System;

namespace SpaceShooterGame
{
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
}
