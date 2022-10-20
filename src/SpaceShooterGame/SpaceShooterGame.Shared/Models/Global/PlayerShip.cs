using System;

namespace SpaceShooterGame
{
    public class PlayerShip
    {
        public string Name { get; set; }

        public Uri ImageUrl { get; set; }

        public ShipClass ShipClass { get; set; }
    }

    public enum ShipClass
    {
        DEFENDER, // shield generate when enraged
        BERSERKER, // shoots faster shots when enraged
        SPECTRE, // goes into etheral mode when enraged
    }
}
