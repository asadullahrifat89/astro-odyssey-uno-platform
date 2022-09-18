namespace AstroOdyssey
{
    public class GameScore : EntityBase
    {
        public AttachedUser User { get; set; } = new AttachedUser();

        public double Score { get; set; } = 0;

        public string GameId { get; set; } = string.Empty;

        public string Emoji { get; set; }

        public string Initials => StringExtensions.GetInitials(User.UserName);

        public string DisplayName => User.UserName + " " + Emoji;
    }
}
