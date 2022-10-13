namespace SpaceShooterGame
{
    public interface ICollectibleFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnCollectible(GameLevel gameLevel);

        void GenerateCollectible();

        bool UpdateCollectible(Collectible collectible);

        void LevelUp();

        void Reset();
    }
}
