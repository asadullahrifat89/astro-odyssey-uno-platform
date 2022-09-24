namespace AstroOdyssey
{
    public class PlayerShipTemplate
    {
        public PlayerShipTemplate(string name, string assetUri, ShipClass shipClass)
        {
            Name = name;
            AssetUri = assetUri;
            ShipClass = shipClass;
        }

        public string Name { get; set; }

        public string AssetUri { get; set; }

        public ShipClass ShipClass { get; set; }
    }
}

