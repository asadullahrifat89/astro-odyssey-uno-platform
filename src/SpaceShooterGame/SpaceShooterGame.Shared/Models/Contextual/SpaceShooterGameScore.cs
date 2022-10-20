namespace SpaceShooterGame
{
    public class SpaceShooterGameScore : PlayerScore
    {
        public int EnemiesDestroyed { get; set; } = 0;

        public int MeteorsDestroyed { get; set; } = 0;

        public int BossesDestroyed { get; set; } = 0;

        public int CollectiblesCollected { get; set; } = 0;
    }
}
