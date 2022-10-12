using System;

namespace SpaceShooterGame
{
    public class Session : EntityBase
    {
        public string SessionId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public DateTime ExpiresOn { get; set; }

        public string GameId { get; set; } = string.Empty;
    }
}
