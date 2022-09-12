namespace AstroOdyssey
{
    public interface IHealthFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnHealth(Player player);

        void GenerateHealth();

        void UpdateHealth(Health health, out bool destroyed);

        void LevelUp();
    }
}
