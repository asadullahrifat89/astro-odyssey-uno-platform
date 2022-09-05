namespace AstroOdyssey
{
    public class GameScore
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        public double Score { get; set; } = 0;

        public int EnemiesDestroyed { get; set; } = 0;

        public int MeteorsDestroyed { get; set; } = 0;

        public int BossesDestroyed { get; set; } = 0;

        public int GemsCollected { get; set; } = 0;
    }
}
