using System;

namespace SpaceShooterGame
{
    public class Company : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string WebSiteUrl { get; set; } = string.Empty;

        public string DefaultLanguage { get; set; } = "en";
    }
}
