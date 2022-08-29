using System;

namespace AstroOdyssey
{
    public class DestructibleObjectTemplate
    {
        public DestructibleObjectTemplate(Uri assetUri, double health, double size = Constants.DESTRUCTIBLE_OBJECT_SIZE)
        {
            AssetUri = assetUri;
            Health = health;
            Size = size;
        }

        public Uri AssetUri { get; set; }

        public double Health { get; set; }

        public double Size { get; set; }
    }
}

