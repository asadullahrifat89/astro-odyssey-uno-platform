using System;

namespace SpaceShooterGame
{
    public class LeaderboardPlacement : EntityBase
    {
        public AttachedUser User { get; set; } = new AttachedUser();

        public string GameId { get; set; } = string.Empty;

        public string MedalEmoji { get; set; } = "🏅";

        public string Emoji { get; set; } = string.Empty;

        public string Initials => StringExtensions.GetInitials(User.UserName);

        public string DisplayName => User.UserName + " " + Emoji;

        public string LastPlayTime
        {
            get
            {
                if (ModifiedOn is null)
                    return GetLastPlayTime(CreatedOn);
                else
                    return GetLastPlayTime(ModifiedOn.Value);
            }
        }

        private string GetLastPlayTime(DateTime playTime)
        {
            var now = DateTime.UtcNow;

            var hours = (now - playTime).TotalHours;

            if (hours < 1)
            {
                var minutes = (now - playTime).TotalMinutes;
                return $"{minutes:0}m ago";
            }
            else if (hours < 24)
            {
                return $"{hours:0}h ago";
            }
            else
            {
                var days = (now - playTime).TotalDays;
                return $"{days:0}d ago";
            }
        }
    }
}
