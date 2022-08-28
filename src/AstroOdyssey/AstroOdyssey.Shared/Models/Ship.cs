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
        Antimony, // shield generate when enraged
        Bismuth, // shoots faster shots when enraged
        Curium, // goes into etheral mode when enraged
    }
}
