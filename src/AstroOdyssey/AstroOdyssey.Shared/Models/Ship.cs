namespace AstroOdyssey
{
    public class Ship
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public ShipClass ShipClass { get; set; }
    }

    public enum ShipClass
    {
        DEFENDER, // shield generate when enraged
        BERSERKER, // shoots faster shots when enraged
        SPECTRE, // goes into etheral mode when enraged
    }
}
