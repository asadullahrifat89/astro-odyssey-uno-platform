using System;

namespace AstroOdysseyCore
{
    public class AuthToken
    {
        public string Token { get; set; } = string.Empty;

        public DateTime LifeTime { get; set; }
    }
}