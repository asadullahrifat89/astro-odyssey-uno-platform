using System;

namespace SpaceShooterGame
{
    public class GamePrizeOfTheDay
    {
        public string GameId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public CultureValue[] PrizeDescriptions { get; set; } = Array.Empty<CultureValue>();

        public WinningCriteria WinningCriteria { get; set; } = new();
    }

    public class WinningCriteria
    {
        public WinningCriteriaType CriteriaType { get; set; } = WinningCriteriaType.DailyHighScore;

        public double ScoreThreshold { get; set; }

        public CultureValue[] CriteriaDescriptions { get; set; } = Array.Empty<CultureValue>();

        public CultureValue[] WinningDescriptions { get; set; } = Array.Empty<CultureValue>();
    }

    public enum WinningCriteriaType
    {
        DailyHighScore,
        ScoreThreshold,
    }
}
