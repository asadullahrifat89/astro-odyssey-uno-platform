namespace SpaceShooterGame
{
    public interface IHealthFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnHealth(Player player);

        void GenerateHealth();

        bool UpdateHealth(Health health);

        void LevelUp();

        void Reset();
    }
}
