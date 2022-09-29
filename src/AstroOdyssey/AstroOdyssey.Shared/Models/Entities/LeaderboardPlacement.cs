namespace AstroOdyssey
{
    public class LeaderboardPlacement : EntityBase
    {
        public AttachedUser User { get; set; } = new AttachedUser();

        public string GameId { get; set; } = string.Empty;

        public string MedalEmoji { get; set; } = "🏅";

        public string Emoji { get; set; } = string.Empty;

        public string Initials => StringExtensions.GetInitials(User.UserName);

        public string DisplayName => User.UserName + " " + Emoji;
    }
}
