using System;

namespace SpaceShooterGame
{
    public class AuthToken
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime ExpiresOn { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
    }
}