using System;

namespace SpaceShooterGame
{
    public class GamePlayResult
    {
        public string GameId { get; set; } = string.Empty;

        public string PrizeName { get; set; } = string.Empty;

        public CultureValue[] PrizeDescriptions { get; set; } = Array.Empty<CultureValue>();

        public CultureValue[] WinningDescriptions { get; set; } = Array.Empty<CultureValue>();

        public CultureValue[] PrizeUrls { get; set; } = Array.Empty<CultureValue>();
    }
}
