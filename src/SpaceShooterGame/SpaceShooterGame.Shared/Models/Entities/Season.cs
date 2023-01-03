using System;

namespace SpaceShooterGame
{
    public class Season : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string CompanyId { get; set; } = string.Empty;

        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public DateTime EndTime { get; set; } = DateTime.UtcNow.AddDays(1);

        public CultureValue[] SeasonDescriptions { get; set; } = Array.Empty<CultureValue>();

        public SeasonTheme Theme { get; set; } = new();

        public CultureValue[] PrizeDescriptions { get; set; } = Array.Empty<CultureValue>();

        public CultureValue[] TermsAndConditionsUrls { get; set; } = Array.Empty<CultureValue>();
    }

    public class SeasonTheme
    {
        public string BackgroundColor { get; set; } = string.Empty;

        public string BubbleType1BackgroundColor { get; set; } = string.Empty;

        public string BubbleType2BackgroundColor { get; set; } = string.Empty;

        public string BubbleType3BackgroundColor { get; set; } = string.Empty;

        public string CardBackgroundColor { get; set; } = string.Empty;

        public string CardBorder { get; set; } = string.Empty;

        public string SeasonDeadlineColor { get; set; } = string.Empty;

        public string OfferDetailsColor { get; set; } = string.Empty;

        public string WinningCriteriaColor { get; set; } = string.Empty;

        public string RequiredPointsColor { get; set; } = string.Empty;

        public string OfferColor { get; set; } = string.Empty;

        public string PlayButtonBackgroundColor { get; set; } = string.Empty;

        public string PlayButtonBorderColor { get; set; } = string.Empty;

        public string PlayButtonTextColor { get; set; } = string.Empty;
    }
}
