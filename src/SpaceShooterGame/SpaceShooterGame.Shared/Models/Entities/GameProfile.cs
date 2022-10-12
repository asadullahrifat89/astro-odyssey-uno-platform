namespace SpaceShooterGame
{
    public class GameProfile : LeaderboardPlacement
    {
        public double PersonalBestScore { get; set; } = 0;

        public double LastGameScore { get; set; } = 0;
    }
}
